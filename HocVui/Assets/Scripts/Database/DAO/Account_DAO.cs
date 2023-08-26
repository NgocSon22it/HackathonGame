using Assets.Scripts.Database.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking.Types;
using static UnityEditor.ShaderData;

public static class Account_DAO
{
    static string ConnectionStr = HocVuiConnect.GetConnectHocVui();

    public static void CreateAccount(string Username, string Pass, int RoleID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO [dbo].[Account]  ([Username], [Pass], [RoleID])  " +
                                     "VALUES (@Username, @Pass, @RoleID)";
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Pass", References.Hash(Pass).ToLower());
                cmd.Parameters.AddWithValue("@RoleID", RoleID);
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

    public static Account_Entity Login(string Username, string Pass)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM [dbo].[Account] where Username = @Username and Pass = @Pass";
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Pass", References.Hash(Pass).ToLower());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    var acc = new Account_Entity
                    {
                        Id = Convert.ToInt32(row["ID"]),
                        Username = row["Username"].ToString(),
                        Name = row["Name"].ToString(),
                        Password = row["Pass"].ToString(),
                        RoleID = Convert.ToInt32(row["RoleID"]),
                        EyeID = row["EyeID"].ToString(),
                        HairID = row["HairID"].ToString(),
                        SkinID = row["SkinID"].ToString(),
                        MouthID = row["MouthID"].ToString(),
                        IsFirst = Convert.ToBoolean(row["IsFirst"])
                    };

                    return acc;
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
        return null;
    }

    public static bool CheckUsername(string Username)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT [Username] FROM [dbo].[Account] where Username = @Username";
                cmd.Parameters.AddWithValue("@Username", Username);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0) { return false; }
                else { return true; }
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
        return false;
    }

    public static bool CheckName(string Name, string Username)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT [Name] FROM [dbo].[Account] where Name = @Name and Username != @Username ";
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Username", Username);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0) { return false; }
                else { return true; }
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
        return false;
    }

    public static void UpdateLayout(string Username, string Name, string HairID, string EyeID, string MouthID, string SkinID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE [dbo].[Account]  " +
                                    "SET[Name] = @Name " +
                                        ",[HairID] = @HairID " +
                                        ",[EyeID] = @EyeID" +
                                        ",[SkinID] = @SkinID " +
                                        ",[MouthID] = @MouthID " +
                                        ",[IsFirst] = 0 " +
                                    "WHERE Username = @Username";
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@HairID", HairID);
                cmd.Parameters.AddWithValue("@EyeID", EyeID);
                cmd.Parameters.AddWithValue("@SkinID", SkinID);
                cmd.Parameters.AddWithValue("@MouthID", MouthID);
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
