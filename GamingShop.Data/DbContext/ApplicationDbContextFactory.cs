using GamingShop.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace GamingShop.Data.DbContext
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private string _connectionString;

        public ApplicationDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public  ApplicationDbContext CreateDbContext(string[] args = null)
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connectionString);

            return new ApplicationDbContext(dbOptions.Options);
        }
    }
}
