using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Attendance_GYM.Clases
{
    public class SearchMember
    {
        public void searchMember(TextBox txtKey, TextBox txtName)
        {
            Connection objectConn = new Connection();

            NpgsqlConnection conn = objectConn.connection();

            try
            {
                conn.Open();

                string query = "SELECT members.name FROM members WHERE members.key = '" + txtKey.Text + "';";

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txtName.Text = reader[0].ToString();
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
}
