using AutoMapper;
using eCommerce.BackEndAPI.Models;
using eCommerce.BackEndAPI.Models.Entities;
using eCommerce.BackEndAPI.Repository.Services;
using eCommerce.UnitTest.MockData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.UnitTest.Systems.Services
{
    public class TestProductService : IDisposable
    {
        private readonly eCommerceDbContext _db;
        private readonly IMapper _mapper;

        public TestProductService(eCommerceDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            var options = new DbContextOptionsBuilder<eCommerceDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            _db.Database.EnsureCreated();
        }


        public void Dispose()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
        }
    }
}
