using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Medical.Medical_Tests_Packages
{
    public partial class Normals_Add_Form : Form
    {
        private DataSet ds;
        private int Test_ID, Period_ID;
        private float Male_Value,Female_Value;
        public Normals_Add_Form()
        {
            InitializeComponent();
        }

        private void RefForm()
        {
            txt_Name.Text = "";

            bt_save.Enabled = true;
            bt_edit.Enabled = false;

            dataGridView2.Rows.Clear();

            SqlConnection con;
            SqlDataReader dataReader = Ezzat.GetDataReader("Period_Select_All", out con);


            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = dataReader["الرقم المسلسل"].ToString();
                    row.Cells[1].Value = dataReader["اسم المرحلة"].ToString();
                    row.Cells[2].Value = 0;
                    row.Cells[3].Value = 0;
                    dataGridView2.Rows.Add(row);
                }
            }
            con.Close();

            using (ds = Ezzat.GetDataSet("Test_Select_All", "X"))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void Normals_Form_Load(object sender, EventArgs e)
        {
            RefForm();
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            ValidateAdd();
            RefForm();
        }

        private void ValidateAdd()
        {
            if (SharedClass.ValidText(txt_Name))
            {
                SaveData();
            }
            else
                MessageBox.Show(SharedClass.Check_Message);
        }

        private void SaveData()
        {
            object obj = Ezzat.ExecutedScalar("Test_Insert", new SqlParameter("@Test_Name", txt_Name.Text));
            Test_ID = int.Parse(obj + "");

            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                if (item.Cells[0].Value != null)
                {
                    Period_ID = int.Parse(item.Cells[0].Value.ToString());
                    Male_Value = float.Parse(item.Cells[2].Value.ToString());
                    Female_Value = float.Parse(item.Cells[3].Value.ToString());
                    
                    Ezzat.ExecutedNoneQuery("Normal_Insert"
                        , new SqlParameter("@Test_ID", Test_ID)
                        , new SqlParameter("@Period_ID", Period_ID)
                        , new SqlParameter("@Male_Value", Male_Value)
                        , new SqlParameter("@Female_Value", Female_Value)
                        );
                }
            }

            MessageBox.Show(SharedClass.Successful_Message);
        }
    }
}
