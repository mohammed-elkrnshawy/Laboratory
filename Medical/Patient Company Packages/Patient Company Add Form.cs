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

namespace Medical.Patient_Company
{
    public partial class Patient_Company_Add_Form : Form
    {
        private DataSet ds;
        private int Company_ID, Patient_ID;
        public Patient_Company_Add_Form()
        {
            InitializeComponent();
        }

        private void RefForm()
        {
            txt_Name.Text = txt_Number.Text = txt_Phone.Text = "";
            txt_Age.Text = "0";
            radio_Male.Checked = true;
            radio_Female.Checked = false;

            bt_save.Enabled = true;
            bt_edit.Enabled = false;

            using (ds = Ezzat.GetDataSet("Company_Select_All", "X"))
            {
                combo_Company.DataSource = ds.Tables["X"];
                combo_Company.DisplayMember = "اسم الشركة";
                combo_Company.ValueMember = "الرقم المسلسل";
                combo_Company.Text = "";
                combo_Company.SelectedText = "اختار اسم الشركة";
            }

            using (ds = Ezzat.GetDataSet("PatientCompany_Select_All", "X"))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }

        }

        private void txt_Age_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedClass.KeyPress(txt_Age, e);
        }

        private void txt_Phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedClass.KeyPress(txt_Phone, e);
        }

        private void txt_Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedClass.KeyPress(txt_Number, e);
        }

        private void combo_Company_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combo_Company.Focused)
            {

                Company_ID = (int)combo_Company.SelectedValue;
            }
        }

        private void Patient_Company_Add_Form_Load(object sender, EventArgs e)
        {
            RefForm();
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            ChechValid();
        }

        private void ChechValid()
        {
            if (combo_Company.SelectedIndex >= 0 && SharedClass.ValidText(txt_Name) && SharedClass.ValidText(txt_Age) && SharedClass.ValidText(txt_Phone) && SharedClass.ValidText(txt_Number))
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
            Ezzat.ExecutedNoneQuery("PatientCompany_Insert",
                    new SqlParameter("@Patient_Name", txt_Name.Text),
                    new SqlParameter("@Patient_Phone", txt_Phone.Text),
                    new SqlParameter("@Patient_Age", int.Parse(txt_Age.Text)),
                    new SqlParameter("@Patient_Gender", radio_Male.Checked),
                    new SqlParameter("@Company_ID", Company_ID),
                    new SqlParameter("@Patient_Number", txt_Number.Text)
                    );

            MessageBox.Show(SharedClass.Successful_Message);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Patient_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            ShowDetails(Patient_ID);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            using (ds = Ezzat.GetDataSet("PatientCompany_Select_All_Search", "X", new SqlParameter("@Patient_Name", textBox6.Text)))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }
        }

        private void bt_edit_Click(object sender, EventArgs e)
        {
            ChechValidEdit();
        }

        private void ShowDetails(int patient_ID)
        {
            bt_edit.Enabled = true;
            bt_save.Enabled = false;

            SqlConnection con;
            SqlDataReader dataReader = Ezzat.GetDataReader("PatientCompany_Select_All_BYID",
                out con, new SqlParameter("@Patient_ID", patient_ID));


            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    txt_Name.Text = dataReader["Patient_Name"].ToString();
                    txt_Phone.Text = dataReader["Patient_Phone"].ToString();
                    txt_Age.Text = dataReader["Patient_Age"].ToString();
                    txt_Number.Text = dataReader["Patient_Number"].ToString();
                    int value = (int)dataReader["Company_ID"];
                    combo_Company.SelectedValue = value;
                    bool bo = (bool)dataReader["Patient_Gender"];
                    radio_Male.Checked = bo;
                    radio_Female.Checked = !bo;

                }
            }
            con.Close();


        }

        private void ChechValidEdit()
        {
            if (combo_Company.SelectedIndex >= 0 && SharedClass.ValidText(txt_Name) && SharedClass.ValidText(txt_Age) && SharedClass.ValidText(txt_Phone) && SharedClass.ValidText(txt_Number))
            {
                EditData();
                RefForm();
            }
            else
            {
                MessageBox.Show(SharedClass.Check_Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefForm();
        }
        
        private void EditData()
        {
            Ezzat.ExecutedNoneQuery("PatientCompany_Edit",
                    new SqlParameter("@Patient_ID", Patient_ID),
                    new SqlParameter("@Patient_Name", txt_Name.Text),
                    new SqlParameter("@Patient_Phone", txt_Phone.Text),
                    new SqlParameter("@Patient_Age", int.Parse(txt_Age.Text)),
                    new SqlParameter("@Patient_Gender", radio_Male.Checked),
                    new SqlParameter("@Patient_Number", txt_Number.Text),
                    new SqlParameter("@Company_ID", combo_Company.SelectedValue)
                    );

            MessageBox.Show(SharedClass.Edit_Message);
        }
    }
}
