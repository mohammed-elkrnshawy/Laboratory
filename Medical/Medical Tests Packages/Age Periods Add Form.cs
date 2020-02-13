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
    public partial class Age_Periods_Add_Form : Form
    {
        private DataSet ds;
        private int Period_ID, Period_Start, Period_End;
        private string Period_Name;

        public Age_Periods_Add_Form()
        {
            InitializeComponent();
        }

        private void RefForm()
        {
            txt_Name.Text = "";
            txt_Start.Text = "0";
            txt_End.Text = "0";
          
            bt_save.Enabled = true;
            bt_edit.Enabled = false;

            using (ds = Ezzat.GetDataSet("Period_Select_All", "X"))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }

            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private void Age_Periods_Add_Form_Load(object sender, EventArgs e)
        {
            RefForm();
        }
        private void bt_save_Click(object sender, EventArgs e)
        {
            ChechValid(); 
        }
        private void ChechValid()
        {
            if (SharedClass.ValidText(txt_Name) && SharedClass.ValidText(txt_Start) && SharedClass.ValidText(txt_End))
            {
                SaveData();
                RefForm();
            }
            else
            {
                MessageBox.Show(SharedClass.Check_Message);
            }
        }
        private void txt_Start_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedClass.KeyPress(txt_Start, e);
        }
        private void txt_End_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedClass.KeyPress(txt_End, e);
        }
        private void SaveData()
        {
            Ezzat.ExecutedNoneQuery("Periods_Insert"
                , new SqlParameter("@Periods_Name", txt_Name.Text)
                , new SqlParameter("@Periods_Start", int.Parse(txt_Start.Text))
                , new SqlParameter("@Periods_End", int.Parse(txt_End.Text))
                );

            MessageBox.Show(SharedClass.Successful_Message);
        }
        private void bt_edit_Click(object sender, EventArgs e)
        {
            if (SharedClass.ValidText(txt_Name) && SharedClass.ValidText(txt_Start) && SharedClass.ValidText(txt_End))
            {
                EditData();
                RefForm();
            }
            else
            {
                MessageBox.Show(SharedClass.Check_Message);
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Period_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            Period_Name = dataGridView1.CurrentRow.Cells[01].Value.ToString();
            Period_Start = (int)dataGridView1.CurrentRow.Cells[2].Value;
            Period_End = (int)dataGridView1.CurrentRow.Cells[3].Value;
            ShowDetails(Period_ID,Period_Name,Period_Start,Period_End);
        }
        private void ShowDetails(int period_ID, string period_Name, int period_Start, int period_End)
        {
            bt_save.Enabled = false;
            bt_edit.Enabled = true;
            txt_Name.Text = period_Name;
            txt_Start.Text = Period_Start+"";
            txt_End.Text = Period_End + "";
        }
        private void EditData()
        {
            Ezzat.ExecutedNoneQuery("Periods_Edit"
                , new SqlParameter("@Periods_ID", Period_ID)
                , new SqlParameter("@Periods_Name", txt_Name.Text)
                , new SqlParameter("@Periods_Start", int.Parse(txt_Start.Text))
                , new SqlParameter("@Periods_End", int.Parse(txt_End.Text))
                );

            MessageBox.Show(SharedClass.Edit_Message);
        }
    }
}
