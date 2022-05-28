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
    public partial class Form1 : Form
    {
        private int ids;
        int idp;
        NpgsqlConnection con = new NpgsqlConnection("Server=localhost;Port=5432; User Id = postgres; Password=123; Database=UOR;");

        public void set_id(int id)
        {
            this.ids = id;
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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Load_Groups();
            Load_Kindsport();
            Load_Trainers();
            con.Open();
            NpgsqlCommand a = new NpgsqlCommand("SELECT DISTINCT \"students\".\"idp\", \"people\".\"fio\", \"people\".\"bdate\"," +
                "\"people\".\"phone\", \"people\".\"passport\",\"people\".\"snils\",\"people\".\"inn\",\"students\".\"course\"," +
                   " \"students\".\"city\", \"students\".\"school\", \"students\".\"category\", \"a1\".\"trainer\"," +
                   "\"kindSport\".name_kind,\"group\".name_group, \"students\".\"result\", " +
                   "students.ssm,students.vsm,students.tsm " +
                   "FROM \"students\"" +
                   "INNER JOIN \"people\"" +
                   "ON \"people\".\"idp\" = \"students\".\"idp\" " +
                   "INNER JOIN " +
                   "(SELECT \"students\".\"idt\", \"people\".\"fio\" AS \"trainer\" FROM \"students\",\"people\", \"trainers\"" +
                   "WHERE" +
                   "\"students\".\"idt\" = \"trainers\".\"idt\" AND \"trainers\".\"idp\" = \"people\".\"idp\") AS \"a1\"" +
                   "ON \"a1\".\"idt\" = \"students\".\"idt\" " +
                   "INNER JOIN \"kindSport\" " +
                   "ON students.idk = \"kindSport\".idk " +
                   "INNER JOIN \"group\" " +
                   "ON students.idg = \"group\".idg " +
                   "WHERE \"students\".\"ids\" = @id", con);
            a.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer).Value = ids;
            NpgsqlDataReader reader = a.ExecuteReader();
            while (reader.Read())
            {
                idp = Convert.ToInt32(reader[0]);
                textBox1.Text = reader[1].ToString();
                textBox2.Text = reader[3].ToString();
                textBox3.Text = reader[4].ToString();
                textBox4.Text = reader[5].ToString();
                textBox5.Text = reader[6].ToString();
                textBox6.Text = reader[8].ToString();
                textBox7.Text = reader[7].ToString();
                textBox8.Text = reader[9].ToString();
                textBox9.Text = reader[10].ToString();
                comboBox2.Text = reader[11].ToString();
                comboBox1.Text = reader[12].ToString();
                comboBox3.Text = reader[13].ToString();
                richTextBox1.Text = reader[14].ToString();
                if (Convert.ToBoolean(reader[15]))
                    checkBox1.Checked = true;
                if (Convert.ToBoolean(reader[16]))
                    checkBox2.Checked = true;
                if (Convert.ToBoolean(reader[17]))
                    checkBox3.Checked = true;
                dateTimePicker1.Value = Convert.ToDateTime(reader[2]);

            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            NpgsqlCommand a = new NpgsqlCommand("UPDATE people set fio = @fio, bdate = @bdate, phone = @phone, passport = @passport, snils = @snils, inn = @inn WHERE idp = @idp", con);
            a.Parameters.Add("fio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox1.Text;
            a.Parameters.Add("bdate", NpgsqlTypes.NpgsqlDbType.Date).Value = dateTimePicker1.Value;
            a.Parameters.Add("phone", NpgsqlTypes.NpgsqlDbType.Numeric).Value = Convert.ToInt64(textBox2.Text);
            a.Parameters.Add("passport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox3.Text;
            a.Parameters.Add("snils", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox4.Text;
            a.Parameters.Add("inn", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox5.Text;
            a.Parameters.Add("idp", NpgsqlTypes.NpgsqlDbType.Integer).Value = idp;
            a.ExecuteNonQuery();
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
            a = new NpgsqlCommand("UPDATE students set idg = @idg, idt = @idt, idk = @idk, city = @city," +
                " course = @course, school = @school, category = @category, result = @result, ssm = @ssm, vsm = @vsm, tsm = @tsm" +
                " WHERE idp = @idp and ids = @ids", con);
            a.Parameters.Add("idg", NpgsqlTypes.NpgsqlDbType.Integer).Value = idg;
            a.Parameters.Add("idt", NpgsqlTypes.NpgsqlDbType.Integer).Value = idt;
            a.Parameters.Add("idk", NpgsqlTypes.NpgsqlDbType.Integer).Value = idk;
            a.Parameters.Add("idp", NpgsqlTypes.NpgsqlDbType.Integer).Value = idp;
            a.Parameters.Add("ids", NpgsqlTypes.NpgsqlDbType.Integer).Value = ids;
            a.Parameters.Add("city", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox6.Text;
            a.Parameters.Add("course", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox7.Text;
            a.Parameters.Add("school", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox8.Text;
            a.Parameters.Add("category", NpgsqlTypes.NpgsqlDbType.Varchar).Value = textBox9.Text;
            a.Parameters.Add("result", NpgsqlTypes.NpgsqlDbType.Varchar).Value = richTextBox1.Text;
            a.Parameters.Add("ssm", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(checkBox1.Checked);
            a.Parameters.Add("vsm", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(checkBox2.Checked);
            a.Parameters.Add("tsm", NpgsqlTypes.NpgsqlDbType.Integer).Value = Convert.ToInt32(checkBox3.Checked);
            a.ExecuteNonQuery();
            con.Close();
            if (System.Windows.Forms.Application.OpenForms["UOR"] != null)
            {
                (System.Windows.Forms.Application.OpenForms["UOR"] as UOR).Load_Students();
            }
            this.Close();
        }
    }
}
