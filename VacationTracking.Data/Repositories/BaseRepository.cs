using Microsoft.Extensions.Configuration;
using Npgsql;

namespace VacationTracking.Data.Repositories
{
    public class BaseRepository 
    {
        protected NpgsqlConnection DbConnection;
        protected string ConnectionString;
        
        public BaseRepository(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("MyConnection");
            DbConnection = new NpgsqlConnection(ConnectionString);
        }
    }
}
