namespace eCommerce.BackEndAPI.Models.DTOs
{
    public class ApiResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public string DisplayMessage { get; set; }
    }
}
