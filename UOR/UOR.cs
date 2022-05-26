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
        int rowIndex = -1;
        private string sql;
        private NpgsqlCommand b;
        private NpgsqlCommand c;
        private NpgsqlCommand d;
        private DataTable tt;
        private DataTable cc;
        private DataTable dd;
        NpgsqlConnection con = new NpgsqlConnection("Server=localhost;Port=5432; User Id = postgres; Password=dbrfnbif23; Database=UOR;");

        public UOR()
        {
            InitializeComponent();
        }

        private void UOR_Load(object sender, EventArgs e)
        {
            NpgsqlConnection con = new NpgsqlConnection("Server=localhost;Port=5432; User Id = postgres; Password=dbrfnbif23; Database=UOR;");
            Select();
        }
        private void Select()
        {
            try
            {

                NpgsqlCommand a = new NpgsqlCommand("SELECT * FROM public.students", con);

                con.Open();
                DataTable dt = new DataTable();
                dt.Load(a.ExecuteReader());
                con.Close();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;

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
    }
}
