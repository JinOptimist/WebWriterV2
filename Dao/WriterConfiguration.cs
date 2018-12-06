using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Dal
{
    public class WriterConfiguration : DbConfiguration
    {
        public WriterConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}