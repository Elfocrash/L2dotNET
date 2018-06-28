using System.Data.SqlClient;
using PeregrineDb;
using PeregrineDb.Databases;

namespace L2dotNET.Repositories.Utils
{
    static class ConnectionFactory
    {
        public static IDatabase Open()
        {
            // TODO: Add configuration here
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            return new DefaultDatabase(connection, PeregrineConfig.SqlServer2012);
        }
    }
}
