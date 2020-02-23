using System.Data;

namespace VacationTracking.Data.Repositories
{
    public abstract class BaseRepository
    {
        protected IDbConnection Connection { get; private set; }

        public BaseRepository(IDbConnection connection)
        {
            Connection = connection;
        }

        //protected NpgsqlConnection DbConnection;
        //protected string ConnectionString;

        //public BaseRepository(IConfiguration configuration)
        //{
        //    ConnectionString = configuration.GetConnectionString("MyConnection");
        //    DbConnection = new NpgsqlConnection(ConnectionString);
        //}
    }
}
