using ADPLabs_DeliverIT_Test.API.Model;
using Microsoft.EntityFrameworkCore;


namespace ADPLabs_DeliverIT_Test.API.EF
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring
       (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "CalcTaskDb");
        }
        public DbSet<CalcTask> CalcTasks { get; set; }
        public DbSet<RequestPostCalc> RequestPostCalcs { get; set; }
    }
}
