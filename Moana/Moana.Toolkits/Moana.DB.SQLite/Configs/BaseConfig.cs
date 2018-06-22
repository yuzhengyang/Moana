using Moana.WakaTime.Models.WebModels;
using System.Data.Entity;

namespace Moana.DB.SQLite.Configs
{
    public class BaseConfig : DbConfiguration 
    {
        public static void Configuer(DbModelBuilder modelBuilder)
        {
            ConfiguerUserEntity(modelBuilder);
        }
        private static void ConfiguerUserEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WakaTimeDatas>().HasKey(x => x.ID);
        }
    }
}
