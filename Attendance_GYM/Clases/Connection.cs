using Npgsql;
using System;
using System.Windows.Forms;

namespace Attendance_GYM.Clases
{
    public class Connection
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server = localhost; User Id = postgres; Password = admin123; Database = test");

        public NpgsqlConnection connection()
        {
            try
            {
                Console.WriteLine("Se logró conecctar a la base de datos...");
            } catch (NpgsqlException ex)
            {
                MessageBox.Show("No se logró conectar a la base de datos, error: " + ex.Message);
            }
            return conn;
        }

    }
}
