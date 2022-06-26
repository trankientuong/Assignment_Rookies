using eCommerce.BackEndAPI.Models;
using eCommerce.BackEndAPI.Models.DTOs.AuthService;
using eCommerce.BackEndAPI.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using CryptographyHelper.HashAlgorithms;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly eCommerceDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private static readonly object _lock = new object();
        private static readonly Dictionary<string, RefreshToken> _refreshTokens = new Dictionary<string, RefreshToken>();
        private readonly IConfiguration _configuration;

        public AuthController(eCommerceDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("Users/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string userName)
        {
            //Check if the user exist
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The user with {userName} does not exist" });
            }

            // return the roles
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);

        }

        [HttpPost]
        [Route("Roles/Create")]
        public async Task<IActionResult> CreateRole(string name)
        {
            //Check exist
            var roleExists = await _roleManager.RoleExistsAsync(name);
            if (!roleExists)
            {
                var newRole = await _roleManager.CreateAsync(new IdentityRole(name));
                if (newRole.Succeeded) // Check if create successful
                {
                    return Ok(new Response { Status = "Success", Message = $"The role {name} has been created" });
                }
                else
                {
                    return BadRequest(new Response { Status = "Failed", Message = $"The role {name} has not been created" });
                }
            }
            return BadRequest(new Response { Status = "Failed", Message = "Role already exist" });
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string userName, string roleName)
        {
            //Check if the user exist
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The user with {userName} does not exist" });
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The role with {roleName} does not exist" });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                       new Claim(ClaimTypes.Role, roleName),
                    });
            if (result.Succeeded)
            {
                return Ok(new Response { Status = "Success", Message = $"The user has been added to the {roleName}" });
            }
            else
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The user has not been added to the {roleName}" });
            }

        }

        [HttpPost]
        [Route("RemoveUserRole")]
        public async Task<IActionResult> RemoveUserRole(string userName, string roleName)
        {
            //Check if the user exist
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The user with {userName} does not exist" });
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The role with {roleName} does not exist" });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new Response { Status = "Success", Message = $"User {userName} has been remove from role {roleName}" });
            }
            else
            {
                return BadRequest(new Response { Status = "Failed", Message = $"Can not remove user {userName} from role {roleName}" });
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto user)
        {
            if (ModelState.IsValid)
            {
                var emailExist = await _userManager.FindByEmailAsync(user.Email);
                if (emailExist != null) // Ktra email
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return BadRequest(ModelState);
                }

                var usernameExist = await _userManager.FindByNameAsync(user.UserName);
                if (usernameExist != null) // Ktra username
                {
                    ModelState.AddModelError("UserName", "UserName already in use!");
                }

                var NewUser = new IdentityUser { Email = user.Email, UserName = user.UserName };
                var result = await _userManager.CreateAsync(NewUser, user.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(NewUser, "User");
                    await _userManager.AddClaimsAsync(NewUser, new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim(ClaimTypes.Role,"User"),
                    });
                    var token = await CreateToken(NewUser);
                    var refreshTokenPart1 = GenerateRefreshToken();
                    var refreshTokenPart2 = GenerateRefreshToken();
                    var refreshToken = $"{refreshTokenPart1}.{refreshTokenPart2}";

                    lock (_lock)
                    {
                        _refreshTokens.Add(refreshTokenPart1, new RefreshToken
                        {
                            UserName = user.UserName,
                            Expiration = DateTimeOffset.UtcNow.AddHours(24),
                            TokenHash = refreshToken.UseSha256().ComputeHashedString()
                        });
                    }
                    return Ok(new
                    {
                        UserName = user.UserName,
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = refreshToken,
                        Expiration = token.ValidTo
                    });
                }
            }
            return BadRequest();
        }

        
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    ModelState.AddModelError("Email", "Email Not Exist");
                    return BadRequest(ModelState);
                }

                var result = await _userManager.CheckPasswordAsync(existingUser, user.Password);
                if(result == false)
                {
                    return BadRequest("Invalid Login Request");
                }
                var token = await CreateToken(existingUser);
                var refreshTokenPart1 = GenerateRefreshToken();
                var refreshTokenPart2 = GenerateRefreshToken();
                var refreshToken = $"{refreshTokenPart1}.{refreshTokenPart2}";

                lock (_lock)
                {
                    _refreshTokens.Add(refreshTokenPart1, new RefreshToken
                    {
                        UserName = existingUser.UserName,
                        Expiration = DateTimeOffset.UtcNow.AddHours(24),
                        TokenHash = refreshToken.UseSha256().ComputeHashedString()
                    });
                }
                return Ok(new
                {
                    UserName = existingUser.UserName,
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
            return BadRequest();
        }

        private async Task<JwtSecurityToken> CreateToken(IdentityUser user)
        {
            var authClaims = await GetAllValidClaims(user);
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SymmetricKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return randomNumber.UseSha256().ComputeHashedString();
        }

        private async Task<List<Claim>> GetAllValidClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToString()),
                //new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Getting the claims that we have assigned to the user
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // Get the user role and add it to the claims
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);

                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            return claims;
        }
    }
}
