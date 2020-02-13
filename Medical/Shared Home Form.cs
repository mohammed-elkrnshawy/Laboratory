using Medical.Chemicals_Packages;
using Medical.Doctor_Packages;
using Medical.Group_Packages;
using Medical.Medical_Tests_Packages;
using Medical.Patient;
using Medical.Patient_Company;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Medical
{
    public partial class Shared_Home_Form : Form
    {
        private Image closeImage, closeImageAct;

        public Shared_Home_Form()
        {
            InitializeComponent();
        }

        private void Shared_Home_Form_Load(object sender, EventArgs e)
        {
            Size mysize = new System.Drawing.Size(20, 20); // co anh chen vao
            Bitmap bt = new Bitmap(Properties.Resources.closeBlack);
            // anh nay ban dau minh da them vao
            Bitmap btm = new Bitmap(bt, mysize);
            closeImageAct = btm;
            //
            //
            Bitmap bt2 = new Bitmap(Properties.Resources.closeBlack);
            // anh nay ban dau minh da them vao
            Bitmap btm2 = new Bitmap(bt2, mysize);
            closeImage = btm2;
            tabControl1.Padding = new Point(30);
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // minh viet san, khoi mat thoi gian
            Rectangle rect = tabControl1.GetTabRect(e.Index);
            Rectangle imageRec = new Rectangle(rect.Right - closeImage.Width,
                rect.Top + (rect.Height - closeImage.Height) / 2,
                closeImage.Width, closeImage.Height);
            // tang size rect
            rect.Size = new Size(rect.Width + 20, 38);

            Font f;
            Brush br = Brushes.Black;
            StringFormat strF = new StringFormat(StringFormat.GenericDefault);
            // neu tab dang duoc chon
            if (tabControl1.SelectedTab == tabControl1.TabPages[e.Index])
            {
                // hinh mau do, hinh nay them tu properti
                e.Graphics.DrawImage(closeImageAct, imageRec);
                f = new Font("Arial", 10, FontStyle.Bold);
                // Ten tabPage
                e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text,
                    f, br, rect, strF);
            }
            else
            {
                // Tap dang mo, nhung ko dc chon, hinh mau den
                e.Graphics.DrawImage(closeImage, imageRec);
                f = new Font("Arial", 9, FontStyle.Regular);
                // Ten tabPage
                e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text,
                    f, br, rect, strF);
            }
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            // Su kien click dong tabpage
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                // giong o DrawItem
                Rectangle rect = tabControl1.GetTabRect(i);
                Rectangle imageRec = new Rectangle(rect.Right - closeImage.Width,
                    rect.Top + (rect.Height - closeImage.Height) / 2,
                    closeImage.Width, closeImage.Height);

                if (imageRec.Contains(e.Location) && i != 0)
                    tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
        }

        private void تسديدالىموردToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Tab("اضافة و تعديل طبيب", new Doctor_Add_Form());
        }

        private void حركاتالخزنةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Tab("اضافة و تعديل الكيماويات", new Chemicals_Add_Form());
        }

        private void اضافةالاصنافToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Tab("اضافة و تعديل المجموعات", new Group_Add_Form());
        }

        private void خردالمخازنToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Tab("اضافة و تعديل الشركات", new Company_Add_Form());
        }

        private void اضافةوتعديلمريضToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Tab("اضافة و تعديل المرضى", new Patient_Add_Form());
        }

        private void اضافةوتعديلمريضToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Add_Tab("اضافة و تعديل المرضى", new Patient_Company_Add_Form());
        }

        private void اضافةفتراتالعمرToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Tab("اضافة و تعديل فنرات الاعمار", new Age_Periods_Add_Form());
        }

        private void اضافةوتعديلالتحاليلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Tab("اضافة و تعديل التحاليل", new Normals_Add_Form());
        }

        private void Add_Tab(string Name, Form form)
        {
            TabPage tp = new TabPage(Name);


            form.TopLevel = false;
            tp.Controls.Add(form);
            form.Dock = DockStyle.Fill;
            form.Show();

            tabControl1.TabPages.Add(tp);
            tabControl1.SelectedIndex = tabControl1.Controls.Count - 1;
        }


    }
}
