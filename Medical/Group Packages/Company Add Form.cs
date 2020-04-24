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
    public partial class Company_Add_Form : Form
    {
        private DataSet ds;
        private int Company_ID,Group_ID;

        public Company_Add_Form()
        {
            InitializeComponent();
        }

        private void RefForm()
        {
            txt_Name.Text = txt_Adress.Text= txt_Phone.Text = "";
            txt_Money.Text=txt_Discount.Text = "0";

            txt_Money.Enabled = true;
            bt_save.Enabled = true;
            bt_edit.Enabled = false;

            using (ds = Ezzat.GetDataSet("Group_Select_All", "X"))
            {
                combo_Group.DataSource = ds.Tables["X"];
                combo_Group.DisplayMember = "اسم المجموعة";
                combo_Group.ValueMember = "الرقم المسلسل";
                combo_Group.Text = "";
                combo_Group.SelectedText = "اختار اسم المؤسسة";
            }

            using (ds = Ezzat.GetDataSet("Company_Select_All", "X"))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }

            //dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void Company_Add_Form_Load(object sender, EventArgs e)
        {
            RefForm();
        }

        private void txt_Money_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedClass.KeyPress(txt_Money, e);
        }

        private void txt_Discount_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedClass.KeyPress(txt_Discount, e);
        }

        private void combo_Group_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combo_Group.Focused)
            {

                Group_ID = (int)combo_Group.SelectedValue;
            }
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            ChechValid();
        }

        private void ChechValid()
        {
            if (combo_Group.SelectedIndex>=0 && SharedClass.ValidText(txt_Name) && SharedClass.ValidText(txt_Adress) && SharedClass.ValidText(txt_Phone))
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
            Ezzat.ExecutedNoneQuery("Company_Insert",
                    new SqlParameter("@Company_Name", txt_Name.Text),
                    new SqlParameter("@Company_Adress", txt_Adress.Text),
                    new SqlParameter("@Company_Phone", txt_Phone.Text),
                    new SqlParameter("@Company_Money", double.Parse(txt_Money.Text)),
                    new SqlParameter("@Company_Payment", int.Parse("0")),
                    new SqlParameter("@Company_Discount", double.Parse(txt_Discount.Text)),
                    new SqlParameter("@Group_ID", Group_ID)
                    );

            MessageBox.Show(SharedClass.Successful_Message);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_Money.Enabled = false;
            Company_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            ShowDetails(Company_ID);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            using (ds = Ezzat.GetDataSet("Company_Select_All_Search", "X", new SqlParameter("@Company_Name", textBox6.Text)))
            {
                dataGridView1.DataSource = ds.Tables["X"];
            }
        }

        private void ShowDetails(int company_ID)
        {
            bt_edit.Enabled = true;
            bt_save.Enabled = false;

            SqlConnection con;
            SqlDataReader dataReader = Ezzat.GetDataReader("Company_Select_All_BYID",
                out con, new SqlParameter("@Company_ID", company_ID));


            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    txt_Name.Text = dataReader["Company_Name"].ToString();
                    txt_Adress.Text = dataReader["Company_Adress"].ToString();
                    txt_Discount.Text = dataReader["Company_Discount"].ToString();
                    txt_Money.Text = dataReader["Company_Money"].ToString();
                    txt_Phone.Text = dataReader["Company_Phone"].ToString();
                    int value = (int)dataReader["Group_ID"];
                    combo_Group.SelectedValue = value;
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
            if (combo_Group.SelectedIndex >= 0 && SharedClass.ValidText(txt_Name) && SharedClass.ValidText(txt_Adress) && SharedClass.ValidText(txt_Phone))
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

            Ezzat.ExecutedNoneQuery("Company_Edit",
                    new SqlParameter("@Company_Name", txt_Name.Text),
                    new SqlParameter("@Company_Adress", txt_Adress.Text),
                    new SqlParameter("@Company_Phone", txt_Phone.Text),
                    new SqlParameter("@Company_ID", Company_ID),
                    new SqlParameter("@Group_ID", (int)combo_Group.SelectedValue)
                    );

            MessageBox.Show(SharedClass.Edit_Message);
        }
    }
}
