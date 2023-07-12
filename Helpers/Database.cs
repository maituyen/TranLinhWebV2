using System;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper; 
namespace MyProject.Helpers
{
	public class Database
	{
        private string connectionString = @"Data Source=112.78.2.73;Initial Catalog=iam08265_suadienthoaihaiphong;User ID=iam08265_suadienthoaihaiphong; Password=Dkt49dh3@;TrustServerCertificate=true";
        public Database()
        {
        }
        public Database(string database)
        {
            this.connectionString = connectionString = @"Data Source=112.78.2.73;Initial Catalog=" + database + ";User ID=iam08265_suadienthoaihaiphong; Password=Dkt49dh3@";
        }
        public IEnumerable<T> Query<T>(string storedProcName, object param = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<T>(storedProcName, param, commandType: CommandType.StoredProcedure);
            }
        }
        public int Execute(string storedProcName, object param = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Execute(storedProcName, param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}

