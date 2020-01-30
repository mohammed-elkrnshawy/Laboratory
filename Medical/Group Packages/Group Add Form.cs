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

namespace Medical.Group_Packages
{
    public partial class Group_Add_Form : Form
    {
        private DataSet ds;
        private int Group_ID;

        public Group_Add_Form()
        {
            InitializeComponent();
        }

        private void RefForm()
        {
            txt_Name.Text = "";

            bt_save.Enabled = true;
            bt_edit.Enabled = false;

            using (ds = Ezzat.GetDataSet("Group_Select_All", "X"))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }

            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void Group_Add_Form_Load(object sender, EventArgs e)
        {
            RefForm();
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            ChechValid();
        }

        private void ChechValid()
        {
            if (SharedClass.ValidText(txt_Name))
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
            Ezzat.ExecutedNoneQuery("Group_Insert",
                    new SqlParameter("@Group_Name", txt_Name.Text)
                    );

            MessageBox.Show(SharedClass.Successful_Message);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            using (ds = Ezzat.GetDataSet("Group_Select_All_Search", "X", new SqlParameter("@Group_Name", textBox6.Text)))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Group_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            ShowDetails(Group_ID);
        }

        private void ShowDetails(int group_ID)
        {
            bt_edit.Enabled = true;
            bt_save.Enabled = false;

            SqlConnection con;
            SqlDataReader dataReader = Ezzat.GetDataReader("Group_Select_All_BYID",
                out con, new SqlParameter("@Group_ID", group_ID));


            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    txt_Name.Text = dataReader["Group_Name"].ToString();
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
            if (SharedClass.ValidText(txt_Name))
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
            Ezzat.ExecutedNoneQuery("Group_Edit"
                , new SqlParameter("@Group_ID", Group_ID)
                , new SqlParameter("@Group_Name", txt_Name.Text)
                );
            MessageBox.Show(SharedClass.Edit_Message);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefForm();
        }
    }
}
