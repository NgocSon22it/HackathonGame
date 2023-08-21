using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;

public static class Account_DAO
{
    static string ConnectionStr = HocVuiConnect.GetConnectHocVui();

    public static void CreateAccount(string UserID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                SqlCommand cmd = connection.CreateCommand();
               // cmd.CommandText = "INSERT INTO [dbo].[Account] ([ID]) VALUES (@UserID)";
                cmd.Parameters.AddWithValue("@UserID", UserID);
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

    public static void CheckUsername(string Username)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                

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
