using DBTest.Models;
using System.Data.Entity;

namespace DBTest.DatabaseEngine
{
    public class MyConfiguration : DbConfiguration 
    {
        public static void Configuer(DbModelBuilder modelBuilder)
        {
            ConfiguerUserEntity(modelBuilder);
        }
        private static void ConfiguerUserEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pigs>().HasKey(x => x.Id);
        }
    }
}
