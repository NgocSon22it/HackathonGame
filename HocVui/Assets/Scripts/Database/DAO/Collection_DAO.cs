﻿using Assets.Scripts.Database.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.DAO
{
    public class Collection_DAO
    {
        static string ConnectionStr = HocVuiConnect.GetConnectHocVui();

        public static List<Collection_Entity> GetAllbyUserID()
        {
            var list = new List<Collection_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[Collection] where ID = @UserID";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Collection_Entity
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            AccountID = Convert.ToInt32(dr["AccountID"]),
                            AmountQuestion = Convert.ToInt32(dr["AmountQuestion"]),
                            Name = dr["Name"].ToString(),
                            LinkVideo = dr["LinkVideo"].ToString()
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

        public static void SaveCollection(string AccountID, string Name, int AmountQuestion, string LinkVideo)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO [dbo].[Collection] ([AccountID] ,[Name] ,[AmountQuestion] ,[LinkVideo])" +
                                         "VALUES (@AccountID ,@Name ,@AmountQuestion ,@LinkVideo)";
                    cmd.Parameters.AddWithValue("@AccountID", AccountID);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@AmountQuestion", AmountQuestion);
                    cmd.Parameters.AddWithValue("@LinkVideo", LinkVideo);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}