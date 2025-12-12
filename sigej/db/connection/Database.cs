using Npgsql;

namespace sigej.db.connection
{
    public static class Database
    {
        private const string ConnectionString = "Host=localhost;Username=sigej_admin;Password=sigejadmin123;Database=sigej_db";
        public static NpgsqlConnection GetConnection()
        {
            var connection = new NpgsqlConnection(ConnectionString);
            connection.Open(); 
            return connection;
        }
    }
}