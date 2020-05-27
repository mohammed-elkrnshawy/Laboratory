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

namespace Medical.Patient
{
    public partial class Patient_Add_Form : Form
    {
        private DataSet ds;
        private int Doctor_ID, Patient_ID;

        public Patient_Add_Form()
        {
            InitializeComponent();
        }

        private void RefForm()
        {
            txt_Name.Text = txt_Phone.Text = "";
            txt_Age.Text = "0";
            radio_Male.Checked = true;
            radio_Female.Checked = false;

            bt_save.Enabled = true;
            bt_edit.Enabled = false;

            using (ds = Ezzat.GetDataSet("Doctor_Select_All", "X"))
            {
                combo_Doctor.DataSource = ds.Tables["X"];
                combo_Doctor.DisplayMember = "اسم الدكتور";
                combo_Doctor.ValueMember = "الرقم المسلسل";
                combo_Doctor.Text = "";
                combo_Doctor.SelectedText = "اختار اسم الطبيب";
            }

            using (ds = Ezzat.GetDataSet("Patient_Select_All", "X"))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }

        }

        private void txt_Age_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedClass.KeyPress(txt_Age, e);
        }

        private void combo_Doctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combo_Doctor.Focused)
            {

                Doctor_ID = (int)combo_Doctor.SelectedValue;
            }
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            ChechValid();
        }

        private void Patient_Add_Form_Load(object sender, EventArgs e)
        {
            RefForm();
        }

        private void ChechValid()
        {
            if (combo_Doctor.SelectedIndex >= 0 && SharedClass.ValidText(txt_Name) && SharedClass.ValidText(txt_Age) && SharedClass.ValidText(txt_Phone))
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
            Ezzat.ExecutedNoneQuery("Patient_Insert",
                    new SqlParameter("@Patient_Name", txt_Name.Text),
                    new SqlParameter("@Patient_Phone", txt_Phone.Text),
                    new SqlParameter("@Patient_Age", int.Parse(txt_Age.Text)),
                    new SqlParameter("@Patient_Gender", radio_Male.Checked),
                    new SqlParameter("@Doctor_ID", Doctor_ID)
                    );

            MessageBox.Show(SharedClass.Successful_Message);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Patient_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            ShowDetails(Patient_ID);
        }

        private void ShowDetails(int patient_ID)
        {
            bt_edit.Enabled = true;
            bt_save.Enabled = false;

            SqlConnection con;
            SqlDataReader dataReader = Ezzat.GetDataReader("Patient_Select_All_BYID",
                out con, new SqlParameter("@Patient_ID", patient_ID));


            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    txt_Name.Text = dataReader["Patient_Name"].ToString();
                    txt_Phone.Text = dataReader["Patient_Phone"].ToString();
                    txt_Age.Text = dataReader["Patient_Age"].ToString();
                    int value = (int)dataReader["Doctor_ID"];
                    combo_Doctor.SelectedValue = value;
                    bool bo = (bool)dataReader["Patient_Gender"];
                    radio_Male.Checked = bo;
                    radio_Female.Checked = !bo;

                }
            }
            con.Close();


        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            using (ds = Ezzat.GetDataSet("Patient_Select_All_Search", "X", new SqlParameter("@Patient_Name", textBox6.Text)))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }
        }

        private void bt_edit_Click(object sender, EventArgs e)
        {
            ChechValidEdit();
        }

        private void ChechValidEdit()
        {
            if (combo_Doctor.SelectedIndex >= 0 && SharedClass.ValidText(txt_Name) && SharedClass.ValidText(txt_Age) && SharedClass.ValidText(txt_Phone))
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

            Ezzat.ExecutedNoneQuery("Patient_Edit",
                    new SqlParameter("@Patient_ID", Patient_ID),
                    new SqlParameter("@Patient_Name", txt_Name.Text),
                    new SqlParameter("@Patient_Phone", txt_Phone.Text),
                    new SqlParameter("@Patient_Age", int.Parse(txt_Age.Text)),
                    new SqlParameter("@Patient_Gender", radio_Male.Checked),
                    new SqlParameter("@Doctor_ID", combo_Doctor.SelectedValue)
                    );

            MessageBox.Show(SharedClass.Edit_Message);
        }
    }
}
