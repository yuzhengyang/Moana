using Moana.DB.SQLite.Configs;
using SQLite.CodeFirst;
using System.Data.Entity;

namespace Moana.DB.SQLite.Engine
{
    [DbConfigurationType(typeof(BaseConfig))]
    public class SuperDB : DbContext
    {
        public SuperDB() : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Configurations.AddFromAssembly(typeof(SuperDb).Assembly);

            BaseConfig.Configuer(modelBuilder);
            Database.SetInitializer(new MyDbInitializer(Database.Connection.ConnectionString, modelBuilder));
        }
        public class MyDbInitializer : SqliteCreateDatabaseIfNotExists<SuperDB>//SqliteDropCreateDatabaseAlways
        {
            public MyDbInitializer(string connectionString, DbModelBuilder modelBuilder)
                : base(modelBuilder)
            {
            }

            protected override void Seed(SuperDB context)
            {
                //context.Set<Files>().Add(new Files { FileName = "123" });
                base.Seed(context);
            }
        }
    }
}