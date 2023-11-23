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
    public partial class UOR : Form
    {
        public struct trainers
        {
            int id;
            string name;
        }

        int rowIndex = -1;
        private string sql;
        private NpgsqlCommand b;
        private NpgsqlCommand c;
        private NpgsqlCommand d;
        private DataTable tt;
        private DataTable cc;
        private DataTable dd;
        NpgsqlConnection con = new NpgsqlConnection("Server=localhost;Port=5432; User Id = ; Password=; Database=UOR1;");

        public UOR()
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

        public void Load_Students()
        {
            dataGridView1.DataSource = null;
            NpgsqlCommand a = new NpgsqlCommand("SELECT DISTINCT \"students\".\"ids\", \"people\".\"fio\", \"people\".\"bdate\", \"students\".\"course\"," +
                    " \"students\".\"city\", \"students\".\"school\", \"students\".\"category\", \"a1\".\"trainer\", \"students\".\"result\" " +
                    "FROM \"students\"" +
                    "INNER JOIN \"people\"" +
                    "ON \"people\".\"idp\" = \"students\".\"idp\" " +
                    "INNER JOIN " +
                    "(SELECT \"students\".\"idt\", \"people\".\"fio\" AS \"trainer\" FROM \"students\",\"people\", \"trainers\"" +
                    "WHERE" +
                    "\"students\".\"idt\" = \"trainers\".\"idt\" AND \"trainers\".\"idp\" = \"people\".\"idp\") AS \"a1\"" +
                    "ON \"a1\".\"idt\" = \"students\".\"idt\"", con);

            con.Open();
            DataTable dt = new DataTable();
            dt.Load(a.ExecuteReader());

            con.Close();
            
            dataGridView1.DataSource = dt;
        }

        private void UOR_Load(object sender, EventArgs e)
        {
      
            con = new NpgsqlConnection("Server=localhost;Port=5432; User Id = ; Password=; Database=UOR1;");
            Select();
            Load_Kindsport();
        }
        private void Select()
        {
            try
            {
                Load_Students();
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            UOR1 OtherMain = new UOR1();
            OtherMain.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.Red;
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            con.Open();
            NpgsqlCommand a = new NpgsqlCommand("SELECT idp FROM \"students\" WHERE \"ids\" = @id", con);
            a.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer).Value = dataGridView1.SelectedRows[0].Cells[0].Value;
            int idp = Convert.ToInt32(a.ExecuteScalar());
            a = new NpgsqlCommand("DELETE FROM \"students\" WHERE \"ids\" = @id", con);
            a.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer).Value = dataGridView1.SelectedRows[0].Cells[0].Value;
            a = new NpgsqlCommand("DELETE FROM people WHERE idp = @id", con);
            a.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer).Value = idp;
            a.ExecuteNonQuery();
            con.Close();
            Load_Students();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            con.Open();
            NpgsqlCommand a = new NpgsqlCommand("SELECT \"idk\" from \"kindSport\" WHERE name_kind = @name", con);
            a.Parameters.AddWithValue("@name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = comboBox1.Text;
            int id =Convert.ToInt32(a.ExecuteScalar());
            a = new NpgsqlCommand("SELECT DISTINCT \"students\".\"ids\", \"people\".\"fio\", \"people\".\"bdate\", \"students\".\"course\"," +
                  " \"students\".\"city\", \"students\".\"school\", \"students\".\"category\", \"a1\".\"trainer\", \"students\".\"result\" " +
                  "FROM \"students\"" +
                  "INNER JOIN \"people\"" +
                  "ON \"people\".\"idp\" = \"students\".\"idp\" " +
                  "INNER JOIN " +
                  "(SELECT \"students\".\"idt\", \"people\".\"fio\" AS \"trainer\" FROM \"students\",\"people\", \"trainers\"" +
                  "WHERE" +
                  "\"students\".\"idt\" = \"trainers\".\"idt\" AND \"trainers\".\"idp\" = \"people\".\"idp\") AS \"a1\"" +
                  "ON \"a1\".\"idt\" = \"students\".\"idt\"" +
                  "WHERE \"students\".\"idk\" = @id", con);
            a.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer).Value = id;
            DataTable dt = new DataTable();
            dt.Load(a.ExecuteReader());
            con.Close();
            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 OtherMain = new Form1();
            OtherMain.set_id(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));
            OtherMain.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
