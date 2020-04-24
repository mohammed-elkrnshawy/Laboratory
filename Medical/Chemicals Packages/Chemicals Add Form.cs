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

namespace Medical.Chemicals_Packages
{
    public partial class Chemicals_Add_Form : Form
    {
        private DataSet ds;
        private int Chemical_ID;

        public Chemicals_Add_Form()
        {
            InitializeComponent();
        }

        private void txt_Quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedClass.KeyPress(txt_Quantity, e);
        }

        private void RefForm()
        {
            txt_Name.Text = txt_Unit.Text = "";
            txt_Quantity.Text = "0";

            txt_Quantity.Enabled = true;
            bt_save.Enabled = true;
            bt_edit.Enabled = false;

            using (ds = Ezzat.GetDataSet("Chemical_Select_All", "X"))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }

            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void Chemicals_Add_Form_Load(object sender, EventArgs e)
        {
            RefForm();
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            ChechValid();
        }

        private void ChechValid()
        {
            if (SharedClass.ValidText(txt_Name) && SharedClass.ValidText(txt_Unit) && SharedClass.ValidText(txt_Quantity))
            {
                SaveData();
                RefForm();
            }
            else
            {
                MessageBox.Show(SharedClass.Check_Message);
            }
        }

        private void SaveData()
        {
            Ezzat.ExecutedNoneQuery("Chemical_Insert",
                    new SqlParameter("@Chemical_Name", txt_Name.Text),
                    new SqlParameter("@Chemical_Quantity", int.Parse(txt_Quantity.Text)),
                    new SqlParameter("@Chemical_Unit", txt_Unit.Text)
                    );

            MessageBox.Show(SharedClass.Successful_Message);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            using (ds = Ezzat.GetDataSet("Chemical_Select_All_Search", "X", 
                new SqlParameter("@Chemical_Name", textBox6.Text)))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_Quantity.Enabled = false;
            Chemical_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            ShowDetails(Chemical_ID);
        }

        private void ShowDetails(int chemical_ID)
        {
            bt_edit.Enabled = true;
            bt_save.Enabled = false;

            SqlConnection con;
            SqlDataReader dataReader = Ezzat.GetDataReader("Chemical_Select_All_BYID",
                out con, new SqlParameter("@Chemical_ID", chemical_ID));


            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    txt_Name.Text = dataReader["Chemical_Name"].ToString();
                    txt_Quantity.Text = dataReader["Chemical_Quantity"].ToString();
                    txt_Unit.Text = dataReader["Chemical_Unit"].ToString();
                }
            }
            con.Close();


        }

        private void bt_edit_Click(object sender, EventArgs e)
        {
            ChechValidEdit();
        }

        private void ChechValidEdit()
        {
            if (SharedClass.ValidText(txt_Name) && SharedClass.ValidText(txt_Unit))
            {
                EditData();
                RefForm();
            }
            else
            {
                MessageBox.Show(SharedClass.Check_Message);
            }
        }

        private void EditData()
        {
            Ezzat.ExecutedNoneQuery("Chemical_Edit"
                , new SqlParameter("@Chemical_ID", Chemical_ID)
                , new SqlParameter("@Chemical_Name",txt_Name.Text)
                , new SqlParameter("@Chemical_Unit", txt_Unit.Text)
                );
            MessageBox.Show(SharedClass.Edit_Message);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefForm();
        }
    }
}
