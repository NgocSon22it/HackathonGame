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
                    cmd.CommandText = "INSERT INTO [dbo].[Question] ([CollectionID] ,[Content] ,[Time] ,[Score] ,[Answer] ,[LinkImage])" +
                                         "VALUES (@CollectionID ,@Content ,0 ,0 ,@Answer ,@LinkImage)";
                    cmd.Parameters.Add("@CollectionID", SqlDbType.Int).Value = CollectionID;
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@Answer", SqlDbType.Int);
                    cmd.Parameters.Add("@LinkImage", SqlDbType.NVarChar);
                    connection.Open();

                    foreach (var i in List)
                    {
                        i.rawImage = null;
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
                    Debug.Log("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.Log("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static List<Question_Entity> GetAllbyCollectionID(int CollectionID)
        {
            var list = new List<Question_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[Question] where CollectionID = @CollectionID";
                    cmd.Parameters.AddWithValue("@CollectionID", CollectionID);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var Question = new Question_Entity_DB
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            CollectionID = Convert.ToInt32(dr["CollectionID"]),
                            Content = dr["Content"].ToString(),
                            Time = Convert.ToInt32(dr["Time"]),
                            Score = Convert.ToInt32(dr["Score"]),
                            Answer = Convert.ToInt32(dr["Answer"]),
                            LinkImage = dr["LinkImage"].ToString()
                        };

                        Question_Entity obj = JsonUtility.FromJson<Question_Entity>(Question.Content);

                        Debug.Log("obj.questionText: " + obj.questionText);

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
}
