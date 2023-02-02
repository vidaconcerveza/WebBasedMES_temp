using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Utils
{
    public class RawQueryHelper
    {
        private string connectionConst;
        
        public RawQueryHelper(string conn)
        {
            connectionConst= conn;
        }

        public async Task<List<dynamic>> ExecuteRawQuery(string query)
        {
            List<dynamic> list = new List<dynamic>();
            using(SqlConnection conn = new SqlConnection(connectionConst))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            dynamic d = reader;
                            list.Add(d);
                        }

                        await reader.CloseAsync();
                        await conn.CloseAsync();
                        return list;
                    }catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        await conn.CloseAsync();
                        return list;
                    }
                }
            }
        }
    }
}
