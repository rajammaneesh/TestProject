using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Data
{
    public class Class1
    {
        public void Test()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DCode"].ToString());
            string queryString = "SELECT * FROM dbo.Tasks";
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

            DataSet customers = new DataSet();
            adapter.Fill(customers, "Customers");
        }
    }
}
