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

namespace Medical.Patient_Packages
{
    public partial class Patient_Analysis_Form : Form
    {
        private DataSet ds;
        private Patient_Normal_Class patientClass;
        public Patient_Analysis_Form()
        {
            InitializeComponent();
        }

        private void RefForm()
        {
            patientClass = new Patient_Normal_Class();
            txt_Precenteg.Text = "0";

            button1.Enabled = button2.Enabled = button3.Enabled = true;

            txt_date.Text = String.Format("{0:HH:mm:ss  dd/MM/yyyy}", DateTime.Now);

            object o = Ezzat.ExecutedScalar("Analysis_selectID");
            int Analysis_ID = (int)o;
            txt_Number.Text = (Analysis_ID + 1) + "";

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
                combo_Patient.DataSource = ds.Tables["X"];
                combo_Patient.DisplayMember = "اسم المريض";
                combo_Patient.ValueMember = "الرقم المسلسل";
                combo_Patient.Text = "";
                combo_Patient.SelectedText = "اختار اسم المريض";
            }

            using (ds = Ezzat.GetDataSet("Analysis_Select_All", "X"))
            {
                combo_Analysis.DataSource = ds.Tables["X"];
                combo_Analysis.DisplayMember = "اسم التحليل";
                combo_Analysis.ValueMember = "الرقم المسلسل";
                combo_Analysis.Text = "";
                combo_Analysis.SelectedText = "اختار اسم التحليل";
            }

        }

        private void Patient_Analysis_Form_Load(object sender, EventArgs e)
        {
            RefForm();
        }

        private void txt_Precenteg_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedClass.KeyPress(txt_Precenteg, e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedClass.KeyPress(txt_Prise, e);
        }

        private void ShowDetailsPatient(int selectedValue)
        {
            SqlConnection con;
            SqlDataReader dataReader = Ezzat.GetDataReader("Patient_Select_All_BYID",
                out con, new SqlParameter("@Patient_ID", selectedValue));


            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    string patient_Age = dataReader["Patient_Age"].ToString();
                    string patient_Gender = dataReader["Patient_Gender"].ToString();
                    patientClass.set_patientAge(int.Parse(patient_Age));
                    patientClass.set_patientGender(bool.Parse(patient_Gender));
                }
            }
            con.Close();
        }

        private void getPatientPeriod(int patientAge)
        {
            SqlConnection con;
            SqlDataReader dataReader = Ezzat.GetDataReader("Period_Select_ByAge",
                out con, new SqlParameter("@Patient_Age", patientAge));

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    patientClass.set_patientPeriod((int)dataReader[0]);
                }
            }
            con.Close();
        }

        private void showDetailsNormals(int analysisID, int periodID)
        {
            SqlConnection con;
            string periodNormal = "";

            if (patientClass.get_PatientGender())
            {
                SqlDataReader dataReader = Ezzat.GetDataReader("Patient_Male_Normal",
                out con, new SqlParameter("@analysisID", analysisID),
                    new SqlParameter("@periodID", periodID));

                MessageBox.Show(analysisID + "     " + periodID);

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        periodNormal = dataReader["Male_Normal"].ToString();
                        patientClass.set_patientNormal(periodNormal);
                    }
                }

            }
            else
            {
                SqlDataReader dataReader = Ezzat.GetDataReader("Patient_FeMale_Normal",
                out con, new SqlParameter("@analysisID", analysisID),
                    new SqlParameter("@periodID", periodID));

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        periodNormal = dataReader[0].ToString();
                        patientClass.set_patientNormal(periodNormal);
                    }
                }
            }
            con.Close();
        }

        private void combo_Analysis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combo_Analysis.Focused)
            {
                SqlConnection con;

                SqlDataReader dataReader = Ezzat.GetDataReader("Analysis_Select_All_BYID",
                  out con, new SqlParameter("@Analysis_ID", (int)combo_Analysis.SelectedValue));

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        patientClass.set_analysisUnit(dataReader["Analysis_Unit"].ToString());
                    }
                }

                con.Close();
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (combo_Analysis.SelectedIndex >= 0 && combo_Doctor.SelectedIndex >= 0 && combo_Patient.SelectedIndex >= 0)
                AddRow();
            else
                MessageBox.Show(SharedClass.Check_Message);
        }

        private void AddRow()
        {
            ShowDetailsPatient((int)combo_Patient.SelectedValue);
            getPatientPeriod(patientClass.get_PatientAge());
            showDetailsNormals((int)combo_Analysis.SelectedValue, patientClass.get_PatientPeriod());

            dataGridView1.Rows.Add();
            dataGridView1[0, dataGridView1.Rows.Count - 1].Value = dataGridView1.Rows.Count + 1;
            dataGridView1[1, dataGridView1.Rows.Count - 1].Value = combo_Analysis.Text;
            dataGridView1[2, dataGridView1.Rows.Count - 1].Value = patientClass.get_PatientNormal();
            dataGridView1[3, dataGridView1.Rows.Count - 1].Value = patientClass.get_analysisUnit();
            dataGridView1[4, dataGridView1.Rows.Count - 1].Value = txt_Precenteg.Text;
            dataGridView1[5, dataGridView1.Rows.Count - 1].Value = combo_Analysis.SelectedValue;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = button2.Enabled=button3.Enabled = false;
            insert_Analysis();
            insert_Analysis_Details();
            MessageBox.Show(SharedClass.Successful_Message);
            RefForm();
        }

        private void insert_Analysis()
        {
            Ezzat.ExecutedNoneQuery("AnalysisPatient_Insert"
                , new SqlParameter("@Analysis_ID",int.Parse(txt_Number.Text))
                ,new SqlParameter("@Analysis_Date",DateTime.Parse(txt_date.Text))
                ,new SqlParameter("@Analysis_Prise",double.Parse(txt_Prise.Text))
                ,new SqlParameter("@Patient_ID",combo_Patient.SelectedValue)
                ,new SqlParameter("@Doctor_ID",combo_Doctor.SelectedValue)
                );
        }

        private void insert_Analysis_Details()
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                Ezzat.ExecutedNoneQuery("AnalysisDetails_Insert"
                , new SqlParameter("@Analysis_ID", int.Parse(txt_Number.Text))
                , new SqlParameter("@Analysis_Type", true)
                , new SqlParameter("@Test_ID", int.Parse(item.Cells[5].Value.ToString()))
                , new SqlParameter("@Test_Normal", item.Cells[2].Value.ToString())
                , new SqlParameter("@Test_Unit", item.Cells[3].Value.ToString())
                , new SqlParameter("@Patient_Percantage", item.Cells[4].Value.ToString())
                );
            }
        }
    }
}
