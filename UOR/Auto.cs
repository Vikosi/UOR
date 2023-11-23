using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;

namespace UOR
{
    public partial class Auto : Form
    {
        NpgsqlConnection con = new NpgsqlConnection("Server=localhost;Port=5432; User Id =; Password=; Database=UOR1;");

        public Auto()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                try
                {
                    con.Open();
                    NpgsqlCommand a = new NpgsqlCommand("SELECT * FROM users WHERE email=@email AND password=@password", con);
                    a.Parameters.AddWithValue("@email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox1.Text;
                    a.Parameters.AddWithValue("@password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox2.Text;
                    NpgsqlDataReader result = a.ExecuteReader();
                if (result.HasRows)
                {
                    result.Read();
                    int roleId = Convert.ToInt32(result[5]);
                    con.Close();
                    if (roleId==2) 
                    {
                        UOR FormMain = new UOR();
                        FormMain.Show();
                    }
                    else
                    {
                        UOR1 OtherMain = new UOR1();
                        OtherMain.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Неверные данные");
                    con.Close();
                }
                }
                catch (Exception ex)
                {
                    con.Close();
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            
        }
    }
}

