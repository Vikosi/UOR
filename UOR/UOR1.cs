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

namespace UOR
{
    public partial class UOR1 : Form
    {
        NpgsqlConnection con = new NpgsqlConnection("Server=localhost;Port=5432; User Id = postgres; Password=dbrfnbif23; Database=UOR1;");
        public UOR1()
        {
            InitializeComponent();
        }

        private void Load_Kindsport()
        {
            con.Open();
            NpgsqlCommand a = new NpgsqlCommand("SELECT \"name_kind\" from \"kindSport\"", con);
            NpgsqlDataReader reader = a.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString());
            }
            con.Close();
        }

        private void Load_Trainers()
        {
            con.Open();
            NpgsqlCommand a = new NpgsqlCommand("SELECT \"people\".\"fio\" from \"people\",\"trainers\" WHERE \"people\".\"idp\" = \"trainers\".\"idp\"", con);
            NpgsqlDataReader reader = a.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader[0].ToString());
            }
            con.Close();
        }

        private void Load_Groups()
        {
            con.Open();
            NpgsqlCommand a = new NpgsqlCommand("SELECT \"name_group\" from \"group\"", con);
            NpgsqlDataReader reader = a.ExecuteReader();
            while (reader.Read())
            {
                comboBox3.Items.Add(reader[0].ToString());
            }
            con.Close();
        }

        private void UOR1_Load(object sender, EventArgs e)
        {
            Load_Kindsport();
            Load_Trainers();
            Load_Groups();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            int id = 0;
            
            try
            {
                NpgsqlCommand a = new NpgsqlCommand("INSERT INTO \"people\" (\"fio\",\"bdate\",\"phone\",\"passport\",\"snils\",\"inn\") " +
                "VALUES (@fio,@bdate,@phone,@passport,@snils,@inn) RETURNING idp;", con);
                a.Parameters.Add("fio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox1.Text;
                a.Parameters.Add("bdate", NpgsqlTypes.NpgsqlDbType.Date).Value = dateTimePicker1.Value;
                a.Parameters.Add("phone", NpgsqlTypes.NpgsqlDbType.Numeric).Value = Convert.ToInt64(textBox2.Text);
                a.Parameters.Add("passport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox3.Text;
                a.Parameters.Add("snils", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox4.Text;
                a.Parameters.Add("inn", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox5.Text;
                id = Convert.ToInt32(a.ExecuteScalar());
                a = new NpgsqlCommand("SELECT \"idg\" from \"group\" WHERE \"name_group\" = @idg", con);
                a.Parameters.Add("idg", NpgsqlTypes.NpgsqlDbType.Varchar).Value = comboBox3.Text;
                int idg = Convert.ToInt32(a.ExecuteScalar());
                a = new NpgsqlCommand("SELECT DISTINCT \"trainers\".\"idt\" " +
                        "FROM \"trainers\"" +
                        "INNER JOIN \"people\"" +
                        "ON \"trainers\".\"idp\" = \"people\".\"idp\"" +
                        "WHERE \"people\".\"fio\" = @fio", con);
                a.Parameters.Add("fio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = comboBox2.Text;
                int idt = Convert.ToInt32(a.ExecuteScalar());
                a = new NpgsqlCommand("SELECT \"idk\" from \"kindSport\" WHERE \"name_kind\" = @idk", con);
                a.Parameters.Add("idk", NpgsqlTypes.NpgsqlDbType.Varchar).Value = comboBox1.Text;
                int idk = Convert.ToInt32(a.ExecuteScalar());
                a = new NpgsqlCommand("INSERT INTO \"students\" (\"idg\",\"idt\",\"idk\",\"idp\",\"city\",\"course\", " +
                    "\"school\", \"category\",\"result\", \"ssm\",\"vsm\",\"tsm\") " +
                   "VALUES (@idg,@idt,@idk,@idp,@city,@course,@school,@category,@result,@ssm,@vsm,@tsm) RETURNING idp;", con);
                a.Parameters.Add("idg", NpgsqlTypes.NpgsqlDbType.Integer).Value = idg;
                a.Parameters.Add("idt", NpgsqlTypes.NpgsqlDbType.Integer).Value = idt;
                a.Parameters.Add("idk", NpgsqlTypes.NpgsqlDbType.Integer).Value = idk;
                a.Parameters.Add("idp", NpgsqlTypes.NpgsqlDbType.Integer).Value = id;
                a.Parameters.Add("city", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox6.Text;
                a.Parameters.Add("course", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox7.Text;
                a.Parameters.Add("school", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox8.Text;
                a.Parameters.Add("category", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox9.Text;
                a.Parameters.Add("result", NpgsqlTypes.NpgsqlDbType.Varchar).Value = richTextBox1.Text;
                a.Parameters.Add("ssm", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(checkBox1.Checked);
                a.Parameters.Add("vsm", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(checkBox2.Checked);
                a.Parameters.Add("tsm", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(checkBox3.Checked);
                a.ExecuteNonQuery();
                if (System.Windows.Forms.Application.OpenForms["UOR"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["UOR"] as UOR).Load_Students();
                }
            }
            catch (Exception)
            {
                if(id != 0)
                {
                    NpgsqlCommand a = new NpgsqlCommand("DELETE FROM people WHERE idp = @id", con);
                    a.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer).Value = id;
                    a.ExecuteNonQuery();
                }
            }      
            con.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
