using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using Microsoft.Win32;

namespace Paint_2._0
{
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;
        bool paint=false;
        Point px, py;
        static float kalınlık = 5;
        static float skalınlık = 40;
        Pen p = new Pen(Color.Black,kalınlık);
        Pen eraser =new Pen(Color.White,skalınlık);
        Pen kesik_cizgi = new Pen(Color.Black, 5);
        int index;
        public static bool saved = false;
        int x, y, sX, sY, cX, cY;

        Image dosya;

        ColorDialog cd=new ColorDialog();
        Color new_color;

        private static bool buttonClicked = false;

        public Form1()
        {
            InitializeComponent();

            this.Width= 1280;
            this.Height= 720;
            p.EndCap = p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            eraser.EndCap = eraser.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            kesik_cizgi.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            bm = new Bitmap(resim.Width,resim.Height);
            g = Graphics.FromImage(bm);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            resim.Image= bm;
            
            this.FormClosing += Form1_FormClosing;
        }


        private void resim_MouseDown(object sender, MouseEventArgs e)
        {
            paint= true;
            py= e.Location;

            cX= e.X;
            cY= e.Y;

        }//boyamaya başlama işlemi ve başladığı pixelin konumu

        

        private void resim_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {

                if (index == 1)
                {
                    px = e.Location;
                    g.DrawLine(p,px,py);
                    py = px;
                }//mouseun üzerinden geçtiği her pixeli onların konumlarını kullanarak boyama işlemi yapıyor

                if (index == 2)
                {
                    px = e.Location;
                    g.DrawLine(eraser, px, py);
                    py = px;
                }//yukarı işlemin aynısı ama tek farkı beyaza boyaması ve "silmesi"
                if (index == 9)
                {
                    px = e.Location;
                    g.DrawLine(kesik_cizgi, px, py);
                    py = px;
                }
            }
            resim.Refresh();

            x = e.X;
            y = e.Y;
            sX = e.X - cX;
            sY = e.Y - cY; //boyutu ayarlamak için şeklin sol üst ve sağ alt köşe konumlarının değiştiği event
        }

        

        private void resim_MouseUp(object sender, MouseEventArgs e)
        {
            paint= false;

            sX = x - cX;
            sY = y - cY; // tıklamayı bıraktığımızzda şekli çizmeyi bitiren değerler

            if (index == 3)
            {
                g.DrawEllipse(p, cX, cY, sX, sY);
            }

            if (index == 4)
            {
                g.DrawRectangle(p, cX,cY, sX, sY);
            }

            if (index == 5)
            {
                g.DrawLine(p, cX, cY, x, y);
            }

        }//mouseu kaldırınca boyama işlemini bitiren event(genel anlamda boyamaktan bahsediyorum, şekillerde burada) 

        private void alayinisil_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            resim.Image = bm;
            index = 0;
            // bütün resimi beyaza boyayan event
        }

        private void kalem_Click(object sender, EventArgs e)
        {
            index = 1;
        }//düz kalem


        private void silgi_Click(object sender, EventArgs e)
        {
            index = 2;
        }//silgi

        private void elips_Click(object sender, EventArgs e)
        {
            index = 3;
        }//elips


        private void kare_Click(object sender, EventArgs e)
        {
            index = 4;
        }//kare

        private void cizgi_Click(object sender, EventArgs e)
        {
            index= 5;
        }//düz çizgi
        private void button6_Click(object sender, EventArgs e)
        {//kesik çizgi
            index = 9;
        }
        private void resim_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            kesik_cizgi.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;


            if (paint)
            {
                if (index == 3)
                {
                    g.DrawEllipse(p, cX, cY, sX, sY);
                }

                if (index == 4)
                {
                    g.DrawRectangle(p, cX, cY, sX, sY);
                }

                if (index == 5)
                {
                    g.DrawLine(p, cX, cY, x, y);
                }
            }// şeklin boyutunu ayarlarken her harekette ne kadar değiştiğiini gösteren method

        }

        

        private void renk_paleti2_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();
            new_color= cd.Color;
            kalem_rengi.BackColor= new_color;
            p.Color = cd.Color;
            //colordialog kullanarak kalem rengini değiştiriyoruz
        }


        static Point set_point(PictureBox pb, Point pt)
        {
            float pX = 1f * pb.Image.Width / pb.Width;
            float pY = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }//resimdeki tıklanan pixelin rengini algılayan method


        private void renk_paleti1_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = set_point(renk_paleti1, e.Location);
            kalem_rengi.BackColor= ((Bitmap)renk_paleti1.Image).GetPixel(point.X, point.Y);
            new_color = kalem_rengi.BackColor;
            p.Color = kalem_rengi.BackColor;
        }//yukarıda algılanan pixelin rengini hem kalemin hem de renk kutusunun rengine eşitleyen event

        

        private void validate(Bitmap bm, Stack<Point>sp,int x, int y,Color eski_renk,Color new_color)
        {
            Color cx = bm.GetPixel(x, y);
            if (cx == eski_renk)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);

            }//stuck kullanarak şeklin çevresini buluyoruz bunu şeklin rengine eski renk diyerek yapıyoruz
        }

        

        public void Doldur(Bitmap bm, int x, int y, Color new_clr)
        {
            Color eski_renk= bm.GetPixel(x, y);
            Stack<Point>pixel= new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x,y, new_clr);
            if (eski_renk == new_clr)
                return;
            //eski renge gelene kadar her pixeli yeni renge boyamak için 

            while(pixel.Count > 0)
            {
                Point pt = (Point)pixel.Pop();
                if(pt.X>0 && pt.Y>0 && pt.X<bm.Width-1 && pt.Y<bm.Height-1) 
                {
                    validate(bm, pixel, pt.X-1, pt.Y, eski_renk, new_clr);
                    validate(bm, pixel, pt.X, pt.Y-1, eski_renk, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y, eski_renk, new_clr);
                    validate(bm, pixel, pt.X, pt.Y+1, eski_renk, new_clr);

                }//her pixelin cevresini taradığımız method
            }
        }

        

        private void resim_MouseClick(object sender, MouseEventArgs e)
        {
            if (index == 7)
            {
                Point point = set_point(resim, e.Location);
                Doldur(bm, point.X, point.Y, new_color);
            }
             
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            p.Width = trackBar1.Value*5;
            eraser.Width = trackBar1.Value*5;
        }

        private void dolgu_Click(object sender, EventArgs e)
        {
            index = 7;
        }

        protected void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.jpg)|*.jpg|(*.*|*.*;";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, resim.Width, resim.Height), bm.PixelFormat);
                btm.Save(sfd.FileName, ImageFormat.Jpeg);
                MessageBox.Show("Başarıyla Kaydedilmiştir.");
            }//kaydetme eventı
                
        }

        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printBarCode();
        }

        private void printBarCode()
        {
            PrintDialog pd = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += Doc_PrintPage;
            pd.Document = doc;
            if (pd.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }
        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(resim.Width, resim.Height);
            resim.DrawToBitmap(bm, new Rectangle(0, 0, resim.Width, resim.Height));
            e.Graphics.DrawImage(bm, 0, 0);
            bm.Dispose();

        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog a = new OpenFileDialog();
            a.Filter = "JPG(*.JPG)|*.jpg";
            if (a.ShowDialog() == DialogResult.OK)
            {
                dosya = Image.FromFile(a.FileName);

                resim.Image = dosya;
            }
        }

        private void jpgOlarakKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();

            save.Filter = "JPG(*.JPG)|*.jpg";

            if (save.ShowDialog() == DialogResult.OK)
            {
                dosya.Save(save.FileName);

                MessageBox.Show("Başarıyla Kaydedilmiştir.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (buttonClicked)
            {
                // İlk kere tıklandığında yapılacak işlemler
                SoundPlayer sesss = new SoundPlayer(@"C:\\Users\\ASUS\\Desktop\\Paint 2.0 y\\RelaxingMusic1.wav");
                buttonClicked = false;
                sesss.Stop();
                
            }
            
            else
            {
                // İkinci kere tıklandığında yapılacak işlemler
                //nesne tanımımız
                SoundPlayer sesss = new SoundPlayer(@"C:\\Users\\ASUS\\Desktop\\Paint 2.0 y\\RelaxingMusic1.wav");
                //çaldırma fonksiyonu hazır olarak mevcut
                sesss.Play();
                buttonClicked = true;
                //diğer adımlarlarda aynısına sahip
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (buttonClicked)
            {
                SoundPlayer sesss = new SoundPlayer(@"C:\\Users\\ASUS\\Desktop\\Paint 2.0 y\\RelaxingMusic2.wav");
                //çaldırma fonksiyonu hazır olarak mevcut
                sesss.Play();
                buttonClicked = true;
            }
            else
            {
                SoundPlayer sesss = new SoundPlayer(@"C:\\Users\\ASUS\\Desktop\\Paint 2.0 y\\RelaxingMusic2.wav");
                sesss.Play();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (buttonClicked)
            {
                SoundPlayer sesss = new SoundPlayer(@"C:\\Users\\ASUS\\Desktop\\Paint 2.0 y\\RelaxingMusic3.wav");
                sesss.Play();
                buttonClicked = true;
            }
            else
            {
                SoundPlayer sesss = new SoundPlayer(@"C:\\Users\\ASUS\\Desktop\\Paint 2.0 y\\RelaxingMusic3.wav");
                sesss.Play();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (buttonClicked)
            {
                SoundPlayer sesss = new SoundPlayer(@"C:\\Users\\ASUS\\Desktop\\Paint 2.0 y\\RelaxingMusic4.wav");
                sesss.Play();
                buttonClicked = true;
            }
            else
            {
                SoundPlayer sesss = new SoundPlayer(@"C:\\Users\\ASUS\\Desktop\\Paint 2.0 y\\RelaxingMusic4.wav");
                sesss.Play();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (buttonClicked)
            {
                SoundPlayer sesss = new SoundPlayer(@"C:\\Users\\ASUS\\Desktop\\Paint 2.0 y\\RelaxingMusic5.wav");
                sesss.Play();
                buttonClicked = true;
            }
            else
            {
                SoundPlayer sesss = new SoundPlayer(@"C:\\Users\\ASUS\\Desktop\\Paint 2.0 y\\RelaxingMusic5.wav");
                sesss.Play();
            }
        }

        

        public Bitmap getBitmap()
        {
            return bm;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void resim_Click(object sender, EventArgs e)
        {

        }

        private void kalem_rengi_Click(object sender, EventArgs e)
        {

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saved)
            {
                form2 form2 = new form2(bm);
                // Form 1'i kapatmayı iptal edin
                e.Cancel = true;

                // Form 2'yi gösterin
                form2.Show();
            }
            else
                Application.Exit();
            
            
        }

    }
}
/* genel notlar 
         * 
         * -Bitmap: Grafik görüntüsünün piksel verilerinden ve özniteliklerinden oluşan GDI+ bit eşlemini kapsüller. A Bitmap,
         * piksel verileriyle tanımlanan görüntülerle çalışmak için kullanılan bir nesnedir.
         * 
         * -Stack: ilk giren son çıkar işleyişine sahip bir koleksiyondur. Diğer bir deyişle; ilk eklenen elemanın
         * koleksiyondan en son çıkarıldığı ve en son eklenen elemanında ilk çıkarıldığı bir veri yapısıdır
         * 
         * -Push(): Stack’in en üstüne bir nesne ekler.
         
         
         
         
         */
