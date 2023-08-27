using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using UnityEngine;

public static class Eye_DAO 
{
    static string ConnectionStr = HocVuiConnect.GetConnectHocVui();

    public static List<Eye_Enity> GetAll()
    {
        var list = new List<Eye_Enity>();
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Select * from Eye";
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);


                foreach (DataRow dr in dataTable.Rows)
                {
                    var obj = new Eye_Enity
                    {
                        ID = dr["ID"].ToString(),
                        Name = dr["Name"].ToString(),
                        Link = dr["Link"].ToString()
                    };
                    list.Add(obj);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Exception: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return list;
    }
}
