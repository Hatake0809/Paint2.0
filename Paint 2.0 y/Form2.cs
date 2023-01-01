using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint_2._0
{
    public partial class form2 : Form
    {

        Bitmap bm;
        Graphics g;
        int index;
        int x, y, sX, sY, cX, cY;

        

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1.saved = true;
            Application.Exit();
            
            
        }

        Color new_color;

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public form2()
        {
            InitializeComponent();
        }
        public form2(Bitmap bm)
        {
            InitializeComponent();

            this.bm = bm;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.jpg)|*.jpg|(*.*|*.*;";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bm.Save(sfd.FileName, ImageFormat.Jpeg);
            }//kaydetme eventı
            this.Close();
            Form1.saved = true;
            Application.Exit();
        }
    }
}
