using Newtonsoft.Json;
using Npgsql;
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace Attendance_GYM.Clases
{
    public class SearchMember
    {
        private const string apiURL = "http://localhost:8080/visits_woa/visits";

        public async void searchMember(TextBox txtKey, TextBox txtName)
        {
            Connection objectConn = new Connection();

            NpgsqlConnection conn = objectConn.connection();

            try
            {
                conn.Open();

                string query = "SELECT members.name, members.id, members.\"tenantId\" FROM members WHERE members.key = '" + txtKey.Text + "';";

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txtName.Text = reader[0].ToString();
                        int id = Convert.ToInt32(reader[1]);
                        string uniqueTenant = reader[2].ToString();

                        var visitData = new
                        {
                            visitDate = DateTime.Now,
                            tenantId = uniqueTenant,
                            memberId = id
                        };

                        Uri RequestUri = new Uri(apiURL);

                        var client = new HttpClient();
                        var json = JsonConvert.SerializeObject(visitData);
                        var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(RequestUri, contentJson);

                        Console.WriteLine(response);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Visita registrada");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ErrorMessage errorMessage = JsonConvert.DeserializeObject<ErrorMessage>(errorContent);

                            MessageBox.Show(errorMessage.message);
                        }
                    }
                } else
                {
                    MessageBox.Show("El miembro no existe o no se encontró en la base de datos.");
                }

            } catch (Exception ex)
            {
                MessageBox.Show("No se logró realizar la búsqueda, error: " + ex.Message);
            }
        }
    }

    public class ErrorMessage
    {
        public string message { get; set; }
    }

}
