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

namespace Medical.Doctor_Packages
{
    public partial class Doctor_Add_Form : Form
    {
        private DataSet ds;
        private int Doctor_ID;
        public Doctor_Add_Form()
        {
            InitializeComponent();
        }

        private void RefForm()
        {
            txt_Name.Text = txt_special.Text=txt_Phone.Text=txt_Clinic.Text=txt_Address.Text = "";

            bt_save.Enabled = true;
            bt_edit.Enabled = false;

            using (ds = Ezzat.GetDataSet("Doctor_Select_All", "X"))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }

            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void Doctor_Add_Form_Load(object sender, EventArgs e)
        {
            RefForm();
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            ChechValid();
        }

        private void ChechValid()
        {
            if (SharedClass.ValidText(txt_Name)&& SharedClass.ValidText(txt_Address) && SharedClass.ValidText(txt_Clinic) && SharedClass.ValidText(txt_Phone) && SharedClass.ValidText(txt_special))
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
            Ezzat.ExecutedNoneQuery("Doctor_Insert",
                    new SqlParameter("@Doctor_Name", txt_Name.Text)
                    ,new SqlParameter("@Doctor_Special", txt_special.Text)
                    ,new SqlParameter("@Doctor_Address", txt_Address.Text)
                    ,new SqlParameter("@Doctor_Phone", txt_Phone.Text)
                    ,new SqlParameter("@Doctor_Clinic", txt_Clinic.Text)
                    );

            MessageBox.Show(SharedClass.Successful_Message);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            using (ds = Ezzat.GetDataSet("Doctor_Select_All_Search", "X", new SqlParameter("@Doctor_Name", textBox6.Text)))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Doctor_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            ShowDetails(Doctor_ID);
        }

        private void ShowDetails(int doctor_ID)
        {
            bt_edit.Enabled = true;
            bt_save.Enabled = false;

            SqlConnection con;
            SqlDataReader dataReader = Ezzat.GetDataReader("Doctor_Select_All_BYID",
                out con, new SqlParameter("@Doctor_ID", doctor_ID));


            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    txt_Name.Text = dataReader["Doctor_Name"].ToString();
                    txt_Address.Text = dataReader["Doctor_Address"].ToString();
                    txt_special.Text = dataReader["Doctor_Spic"].ToString();
                    txt_Clinic.Text = dataReader["Doctor_Clinic"].ToString();
                    txt_Phone.Text = dataReader["Doctor_Phone"].ToString();
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
            if (SharedClass.ValidText(txt_Name) && SharedClass.ValidText(txt_Address) && SharedClass.ValidText(txt_special) && SharedClass.ValidText(txt_Clinic) && SharedClass.ValidText(txt_Phone))
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
            Ezzat.ExecutedNoneQuery("Doctor_Edit"
                , new SqlParameter("@Doctor_ID", Doctor_ID)
                , new SqlParameter("@Doctor_Name", txt_Name.Text)
                , new SqlParameter("@Doctor_Address", txt_Address.Text)
                , new SqlParameter("@Doctor_Special", txt_special.Text)
                , new SqlParameter("@Doctor_Phone", txt_Phone.Text)
                , new SqlParameter("@Doctor_Clinic", txt_Clinic.Text)
                );
            MessageBox.Show(SharedClass.Edit_Message);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefForm();
        }
    }
}
