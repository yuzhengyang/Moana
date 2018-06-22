using SQLite.CodeFirst;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DBTest.DatabaseEngine
{
    [DbConfigurationType(typeof(MyConfiguration))]
    public class SuperDb : DbContext
    {
        public SuperDb() : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Configurations.AddFromAssembly(typeof(SuperDb).Assembly);

            MyConfiguration.Configuer(modelBuilder);
            Database.SetInitializer(new MyDbInitializer(Database.Connection.ConnectionString, modelBuilder));
        }

        public class MyDbInitializer : SqliteCreateDatabaseIfNotExists<SuperDb>//SqliteDropCreateDatabaseAlways
        {
            public MyDbInitializer(string connectionString, DbModelBuilder modelBuilder)
                : base(modelBuilder)
            {
            }

            protected override void Seed(SuperDb context)
            {
                //context.Set<Files>().Add(new Files { FileName = "123" });
                base.Seed(context);
            }
        }
    }
}