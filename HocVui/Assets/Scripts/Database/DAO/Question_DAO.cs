using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Database.DAO
{
    public class Question_DAO
    {
        static string ConnectionStr = HocVuiConnect.GetConnectHocVui();
        public static void SaveQuestion(int CollectionID, List<Question_Entity> List)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO [dbo].[Question] ([CollectionID] ,[Content] ,[Time] ,[Scores] ,[Answer] ,[LinkImage])" +
                                         "VALUES (@CollectionID ,@Content ,0 ,0 ,@Answer ,@LinkImage)";
                    cmd.Parameters.Add("@CollectionID", SqlDbType.Int).Value = CollectionID;
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@Answer", SqlDbType.Int);
                    cmd.Parameters.Add("@LinkImage", SqlDbType.NVarChar);
                    connection.Open();

                    foreach (var i in List)
                    {
                        var json = JsonUtility.ToJson(i);
                        Debug.Log("Serialized JSON: " + json);

                        cmd.Parameters["@Content"].Value = json;
                        cmd.Parameters["@Answer"].Value = i.correctAnswerIndex;
                        cmd.Parameters["@LinkImage"].Value = i.LinkImage;
                        cmd.ExecuteNonQuery();
                    }
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
