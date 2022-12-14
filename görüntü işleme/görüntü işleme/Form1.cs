using System.Collections;

namespace görüntü_işleme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            pictureBox2.Visible = false;
            textBox1.Visible= false;
        }


        public void ResmiKaydet() {
            SaveFileDialog savefiledialog1 = new SaveFileDialog();
            savefiledialog1.Filter = "JpegResmi|jpg|Bitmap Resmi|.bmp|Gif Resmi|*.gif";
            savefiledialog1.Title="ResmiKaydet";
            savefiledialog1.ShowDialog();
            if(savefiledialog1.FileName != "")
            {
                FileStream DosyaAkisi = (FileStream)savefiledialog1.OpenFile();
                switch(savefiledialog1.FilterIndex)
                {
                    case 1:
                        pictureBox2.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 2:
                        pictureBox2.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        pictureBox2.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Gif);
                        break;

                }
                DosyaAkisi.Close();
            }
        }

        public Image SiyahBeyaz(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen İşlem Yapılacak Resmi Ekleyin");
            }
            else
            {
                for (int i = 0; i < bm.Width; i++)
                {
                    for (int j = 0; j < bm.Height; j++)
                    {
                        int rgb = (bm.GetPixel(i, j).R + bm.GetPixel(i, j).G + bm.GetPixel(i, j).B) / 3;
                        bm.SetPixel(i, j, Color.FromArgb(rgb, rgb, rgb));
                    }

                }
            }
                return bm;
            
        }
        public Image negatif(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen İşlem Yapılacak Resmi Ekleyin");
            }
            else
            {
                for (int i = 0; i < bm.Width; i++)
                {
                    for (int j = 0; j < bm.Height; j++)
                    {
                        int R = 255 - (bm.GetPixel(i, j).R);
                        int G = 255 - (bm.GetPixel(i, j).G);
                        int B = 255 - (bm.GetPixel(i, j).B);
                        bm.SetPixel(i, j, Color.FromArgb(R, G, B));
                    }

                }
            }
            return bm;
        }
        public Image esıkleme(Bitmap bm)
        {
           
                int EsiklemeDegeri = Convert.ToInt32(textBox2.Text);
                for (int i = 0; i < bm.Width; i++)
                {
                    for (int j = 0; j < bm.Height; j++)
                    {
                        int R = 255 - (bm.GetPixel(i, j).R);
                        int B = 255 - (bm.GetPixel(i, j).G);
                        int G = 255 - (bm.GetPixel(i, j).B);

                        if (R >= EsiklemeDegeri)
                            R = 255;
                        else
                            R = 0;
                        if (G >= EsiklemeDegeri)
                            G = 255;
                        else
                            G = 0;
                        if (B >= EsiklemeDegeri)
                            B = 255;
                        else
                            B = 0;

                        bm.SetPixel(i, j, Color.FromArgb(R, G, B));
                    }
                }
            
                return bm;
            
        }
        
        public Image Histogramciz(Bitmap bm)
        {
            ArrayList DiziPiksel = new ArrayList();
            for (int i = 0; i < bm.Width; i++)
            {
                for (int j = 0; j < bm.Height; j++)
                {

                    int rgb = (bm.GetPixel(i, j).R + bm.GetPixel(i, j).G + bm.GetPixel(i, j).B) / 3;
                    bm.SetPixel(i, j, Color.FromArgb(rgb, rgb, rgb));
                    DiziPiksel.Add(rgb);
                }

            }
            

            int[] DiziPikselSayilari = new int[256];
            for (int r = 0; r <= 255; r++) //256 tane renk tonu için dönecek.
            {
                int PikselSayisi = 0;
                for (int s = 0; s < DiziPiksel.Count; s++) //resimdeki piksel sayısınca dönecek.
                {
                    if (r == Convert.ToInt16(DiziPiksel[s]))
                        PikselSayisi++;
                }
                DiziPikselSayilari[r] = PikselSayisi;
            }
            //Değerleri listbox'a ekliyor.
            int RenkMaksPikselSayisi = 0; //Grafikte y eksenini ölçeklerken kullanılacak.
            for (int k = 0; k <= 255; k++)
            {
                listBox1.Items.Add("Renk:" + k + "=" + DiziPikselSayilari[k]);
                //Maksimum piksel sayısını bulmaya çalışıyor.
                if (DiziPikselSayilari[k] > RenkMaksPikselSayisi)
                {
                    RenkMaksPikselSayisi = DiziPikselSayilari[k];
                }
            }
            Graphics CizimAlani;
            Pen Kalem1 = new Pen(System.Drawing.Color.Yellow, 1);
            Pen Kalem2 = new Pen(System.Drawing.Color.White, 1);
            CizimAlani = pictureBox3.CreateGraphics();
            pictureBox3.Refresh();
            int GrafikYuksekligi = 250;
            double OlcekY = RenkMaksPikselSayisi / GrafikYuksekligi;
            double OlcekX = 1.5;
            int X_kaydirma = 10;
            for (int x = 0; x <= 255; x++)
            {
                if (x % 50 == 0)
                    CizimAlani.DrawLine(Kalem2, (int)(X_kaydirma + x * OlcekX),
                   GrafikYuksekligi, (int)(X_kaydirma + x * OlcekX), 0);
                CizimAlani.DrawLine(Kalem1, (int)(X_kaydirma + x * OlcekX), GrafikYuksekligi,
               (int)(X_kaydirma + x * OlcekX), (GrafikYuksekligi - (int)(DiziPikselSayilari[x] / OlcekY)));
                //Dikey kırmızı çizgiler.

            }
            label5.Text = "Maks.Piks=" + RenkMaksPikselSayisi.ToString();
            return bm;

    }


      


          public Image Dondurme(Bitmap bm)
          {
              if (textd.Text == "")
              {
                  MessageBox.Show("Döndürme açısı değerini giriniz");
                  textd.Focus();
                  textd.BackColor = Color.Red;

              }
              else
              {

                    Bitmap GirisResmi, CikisResmi;
                    GirisResmi = new Bitmap(pictureBox1.Image);
                    int ResimGenisligi = GirisResmi.Width;
                    int ResimYuksekligi = GirisResmi.Height;
                    CikisResmi = new Bitmap(bm.Width, bm.Height);
                int Aci = Convert.ToInt16(textd.Text);
                  double RadyanAci = Aci * 2 * Math.PI / 360;

                  int x0 = bm.Width / 2;
                  int y0 = bm.Height / 2;
                  for (int i = 0; i < (bm.Width); i++)
                  {
                      for (int j = 0; j < (bm.Height); j++)
                      {

                         double x2 = Math.Cos(RadyanAci) * (i - x0) - Math.Sin(RadyanAci) * (j - y0) + x0;
                         double y2 = Math.Sin(RadyanAci) * (i - x0) + Math.Cos(RadyanAci) * (j - y0) + y0;
                          if (x2 > 0 && x2 < bm.Width && y2 > 0 && y2 <bm.Height)
                            CikisResmi.SetPixel((int)x2, (int)y2, bm.GetPixel(i, j));
                      }
                  }
                pictureBox2.Image = CikisResmi;
            }

              
            return bm;

          }



        public Image KontrastGerme(Bitmap bm)
        {
            
            int X1 = Convert.ToInt16(textBox3.Text);
            int X2 = Convert.ToInt16(textBox4.Text);
            int Y1 = Convert.ToInt16(textBox5.Text);
            int Y2 = Convert.ToInt16(textBox6.Text);
            for (int i = 0; i < bm.Width; i++)
            {
                for (int j = 0; j < bm.Height; j++)
                {

                    int R = bm.GetPixel(i, j).R;
                    int G = bm.GetPixel(i, j).G;
                    int B = bm.GetPixel(i, j).B;
                    int gri = (R + G + B) / 3;
                    int X = gri;
                    int Y = (((X - X1) * (Y2 - Y1)) / (X2 - X1)) + Y1;
                    if (Y > 255) Y = 255;
                    if (Y < 0) Y = 0;
                    bm.SetPixel(i, j, Color.FromArgb(Y, Y, Y));
                }
        
                
            }
            return bm;
        }



                private void button1_Click(object sender, EventArgs e)
        {
            
            Bitmap bmp = (Bitmap)pictureBox2.Image;
            SiyahBeyaz(bmp);
            pictureBox2.Visible = true;
            Refresh();
            
        }

       
       public Image aynalama(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {

                if (textd.Text == "")
                {
                    MessageBox.Show("Açı değeri giriniz!..");
                    textd.Focus();
                    textd.BackColor = Color.Red;
                }
                else
                {
                    textd.BackColor = Color.White;
                    Color OkunanRenk;
                    Bitmap GirisResmi, CikisResmi;
                    GirisResmi = new Bitmap(pictureBox1.Image);
                    int ResimGenisligi = GirisResmi.Width;
                    int ResimYuksekligi = GirisResmi.Height;
                    CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                    double Aci = Convert.ToDouble(textd.Text);
                    double RadyanAci = Aci * 2 * Math.PI / 360;
                    double x2 = 0, y2 = 0;
                     
                    int x0 = ResimGenisligi / 2;
                    int y0 = ResimYuksekligi / 2;
                    for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                    {
                        for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x1, y1);
                            double Delta = (x1 - x0) * Math.Sin(RadyanAci) - (y1 - y0) * Math.Cos(RadyanAci);
                            x2 = Convert.ToInt16(x1 + 2 * Delta * (-Math.Sin(RadyanAci)));
                            y2 = Convert.ToInt16(y1 + 2 * Delta * (Math.Cos(RadyanAci)));
                            if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                                CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                        }
                    }
                    pictureBox2.Image = CikisResmi;

                }

            }



            return bm;
        }

        public void meanFiltresi(Bitmap bm)
        {
            if (textm.Text == "")
            {
                MessageBox.Show("Şablon boyutu giriniz...");
                textm.Focus();
                textm.BackColor = Color.Red;
            }
            else
            {
                textm.BackColor = Color.White;
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = Convert.ToInt32(textm.Text); //şablon boyutu 3 den büyük tek rakam  olmalıdır(3, 5, 7 gibi).
                int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        toplamR = 0;
                        toplamG = 0;
                        toplamB = 0;
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                toplamR = toplamR + OkunanRenk.R;
                                toplamG = toplamG + OkunanRenk.G;
                                toplamB = toplamB + OkunanRenk.B;
                            }
                        }
                        ortalamaR = toplamR / (SablonBoyutu * SablonBoyutu);
                        ortalamaG = toplamG / (SablonBoyutu * SablonBoyutu);
                        ortalamaB = toplamB / (SablonBoyutu * SablonBoyutu);
                        CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
          
        }

        public void meadian(Bitmap bm)
        {
            if (textm.Text == "")
            {
                MessageBox.Show("Şablon boyutu giriniz...");
                textm.Focus();
                textm.BackColor = Color.Red;
            }
            else
            {
                textm.BackColor = Color.White;
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = Convert.ToInt32(textm.Text); 
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int[] R = new int[ElemanSayisi];
                int[] G = new int[ElemanSayisi];
                int[] B = new int[ElemanSayisi];
                int[] Gri = new int[ElemanSayisi];
                int x, y, i, j;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        
                        int k = 0;
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                R[k] = OkunanRenk.R;
                                G[k] = OkunanRenk.G;
                                B[k] = OkunanRenk.B;
                                Gri[k] = Convert.ToInt16(R[k] * 0.299 + G[k] * 0.587 + B[k] * 0.114); 
                                k++;
                            }
                        }
                       
                        int GeciciSayi = 0;
                        for (i = 0; i < ElemanSayisi; i++)
                        {
                            for (j = i + 1; j < ElemanSayisi; j++)
                            {
                                if (Gri[j] < Gri[i])
                                {
                                    GeciciSayi = Gri[i];
                                    Gri[i] = Gri[j];
                                    Gri[j] = GeciciSayi;
                                    GeciciSayi = R[i];
                                    R[i] = R[j];
                                    R[j] = GeciciSayi;
                                    GeciciSayi = G[i];
                                    G[i] = G[j];
                                    G[j] = GeciciSayi;
                                    GeciciSayi = B[i];
                                    B[i] = B[j];
                                    B[j] = GeciciSayi;
                                }
                            }
                        }
                        
                        CikisResmi.SetPixel(x, y, Color.FromArgb(R[(ElemanSayisi - 1) / 2], G[(ElemanSayisi - 1) /
                       2], B[(ElemanSayisi - 1) / 2]));
                    }
                }
                pictureBox2.Image = CikisResmi;
            }

        }
        public void gauss(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = 5; 
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;
                int[] Matris = { 1, 4, 7, 4, 1, 4, 20, 33, 20, 4, 7, 33, 55, 33, 7, 4, 20, 33, 20, 4, 1, 4, 7, 4, 1 };
                int MatrisToplami = 1 + 4 + 7 + 4 + 1 + 4 + 20 + 33 + 20 + 4 + 7 + 33 + 55 + 33 + 7 + 4 + 20 +
               33 + 20 + 4 + 1 + 4 + 7 + 4 + 1;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) 
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        toplamR = 0;
                        toplamG = 0;
                        toplamB = 0;
                        
                        int k = 0; 
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                toplamR = toplamR + OkunanRenk.R * Matris[k];
                                toplamG = toplamG + OkunanRenk.G * Matris[k];
                                toplamB = toplamB + OkunanRenk.B * Matris[k];
                                k++;
                            }
                            ortalamaR = toplamR / MatrisToplami;
                            ortalamaG = toplamG / MatrisToplami;
                            ortalamaB = toplamB / MatrisToplami;
                            CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                        }
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }
        public void sobel(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Bitmap GirisResmi, CikisResmiXY, CikisResmiX, CikisResmiY;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmiX = new Bitmap(ResimGenisligi, ResimYuksekligi);
                CikisResmiY = new Bitmap(ResimGenisligi, ResimYuksekligi);
                CikisResmiXY = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = 3;
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y;
                Color Renk;
                int P1, P2, P3, P4, P5, P6, P7, P8, P9;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) 
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        Renk = GirisResmi.GetPixel(x - 1, y - 1);
                        P1 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y - 1);
                        P2 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y - 1);
                        P3 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y);
                        P4 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y);
                        P5 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y);
                        P6 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y + 1);
                        P7 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y + 1);
                        P8 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y + 1);
                        P9 = (Renk.R + Renk.G + Renk.B) / 3;
                        
                        int Gx = Math.Abs(-P1 + P3 - 2 * P4 + 2 * P6 - P7 + P9); 
                        int Gy = Math.Abs(P1 + 2 * P2 + P3 - P7 - 2 * P8 - P9); 
                        int Gxy = Gx + Gy;
                       
                        if (Gx > 255) Gx = 255;
                        if (Gy > 255) Gy = 255;
                        if (Gxy > 255) Gxy = 255;
                      
                        CikisResmiX.SetPixel(x, y, Color.FromArgb(Gx, Gx, Gx));
                        CikisResmiY.SetPixel(x, y, Color.FromArgb(Gy, Gy, Gy));
                        CikisResmiXY.SetPixel(x, y, Color.FromArgb(Gxy, Gxy, Gxy));
                    }
                }
                pictureBox2.Image = CikisResmiXY;
               

            }
        }
        public void egme(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);                
                double EgmeKatsayisi = 0.2;
                double x2 = 0, y2 = 0;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);                      
                        x2 = x1;
                        y2 = -EgmeKatsayisi * x1 + y1;

                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;

            }
        }

        public void Perspektif(Bitmap bm)
        {
            double x1 = Convert.ToDouble(txt_x1.Text);
            double y1 = Convert.ToDouble(txt_y1.Text);
            double x2 = Convert.ToDouble(txt_x2.Text);
            double y2 = Convert.ToDouble(txt_y2.Text);
            double x3 = Convert.ToDouble(txt_x3.Text);
            double y3 = Convert.ToDouble(txt_y3.Text);
            double x4 = Convert.ToDouble(txt_x4.Text);
            double y4 = Convert.ToDouble(txt_y4.Text);
            double X1 = Convert.ToDouble(txt_xx1.Text);
            double Y1 = Convert.ToDouble(txt_yy1.Text);
            double X2 = Convert.ToDouble(txt_xx2.Text);
            double Y2 = Convert.ToDouble(txt_yy2.Text);
            double X3 = Convert.ToDouble(txt_xx3.Text);
            double Y3 = Convert.ToDouble(txt_yy3.Text);
            double X4 = Convert.ToDouble(txt_xx4.Text);
            double Y4 = Convert.ToDouble(txt_yy4.Text);
            double[,] GirisMatrisi = new double[8, 8];
            // { x1, y1, 1, 0, 0, 0, -x1 * X1, -y1 * X1 }
            GirisMatrisi[0, 0] = x1;
            GirisMatrisi[0, 1] = y1;
            GirisMatrisi[0, 2] = 1;
            GirisMatrisi[0, 3] = 0;
            GirisMatrisi[0, 4] = 0;
            GirisMatrisi[0, 5] = 0;
            GirisMatrisi[0, 6] = -x1 * X1;
            GirisMatrisi[0, 7] = -y1 * X1;
            //{ 0, 0, 0, x1, y1, 1, -x1 * Y1, -y1 * Y1 }
            GirisMatrisi[1, 0] = 0;
            GirisMatrisi[1, 1] = 0;
            GirisMatrisi[1, 2] = 0;
            GirisMatrisi[1, 3] = x1;
            GirisMatrisi[1, 4] = y1;
            GirisMatrisi[1, 5] = 1;
            GirisMatrisi[1, 6] = -x1 * Y1;
            GirisMatrisi[1, 7] = -y1 * Y1;
            //{ x2, y2, 1, 0, 0, 0, -x2 * X2, -y2 * X2 }
            GirisMatrisi[2, 0] = x2;
            GirisMatrisi[2, 1] = y2;
            GirisMatrisi[2, 2] = 1;
            GirisMatrisi[2, 3] = 0;
            GirisMatrisi[2, 4] = 0;
            GirisMatrisi[2, 5] = 0;
            GirisMatrisi[2, 6] = -x2 * X2;
            GirisMatrisi[2, 7] = -y2 * X2;
            //{ 0, 0, 0, x2, y2, 1, -x2 * Y2, -y2 * Y2 }
            GirisMatrisi[3, 0] = 0;
            GirisMatrisi[3, 1] = 0;
            GirisMatrisi[3, 2] = 0;
            GirisMatrisi[3, 3] = x2;
            GirisMatrisi[3, 4] = y2;
            GirisMatrisi[3, 5] = 1;
            GirisMatrisi[3, 6] = -x2 * Y2;
            GirisMatrisi[3, 7] = -y2 * Y2;
            //{ x3, y3, 1, 0, 0, 0, -x3 * X3, -y3 * X3 }
            GirisMatrisi[4, 0] = x3;
            GirisMatrisi[4, 1] = y3;
            GirisMatrisi[4, 2] = 1;
            GirisMatrisi[4, 3] = 0;
            GirisMatrisi[4, 4] = 0;
            GirisMatrisi[4, 5] = 0;
            GirisMatrisi[4, 6] = -x3 * X3;
            GirisMatrisi[4, 7] = -y3 * X3;
            //{ 0, 0, 0, x3, y3, 1, -x3 * Y3, -y3 * Y3 }
            GirisMatrisi[5, 0] = 0;
            GirisMatrisi[5, 1] = 0;
            GirisMatrisi[5, 2] = 0;
            GirisMatrisi[5, 3] = x3;
            GirisMatrisi[5, 4] = y3;
            GirisMatrisi[5, 5] = 1;
            GirisMatrisi[5, 6] = -x3 * Y3;
            GirisMatrisi[5, 7] = -y3 * Y3;
            //{ x4, y4, 1, 0, 0, 0, -x4 * X4, -y4 * X4 }
            GirisMatrisi[6, 0] = x4;
            GirisMatrisi[6, 1] = y4;
            GirisMatrisi[6, 2] = 1;
            GirisMatrisi[6, 3] = 0;
            GirisMatrisi[6, 4] = 0;
            GirisMatrisi[6, 5] = 0;
            GirisMatrisi[6, 6] = -x4 * X4;
            GirisMatrisi[6, 7] = -y4 * X4;
            //{ 0, 0, 0, x4, y4, 1, -x4 * Y4, -y4 * Y4 }
            GirisMatrisi[7, 0] = 0;
            GirisMatrisi[7, 1] = 0;
            GirisMatrisi[7, 2] = 0;
            GirisMatrisi[7, 3] = x4;
            GirisMatrisi[7, 4] = y4;
            GirisMatrisi[7, 5] = 1;
            GirisMatrisi[7, 6] = -x4 * Y4;
            GirisMatrisi[7, 7] = -y4 * Y4;
            //------------------------------------------------------------------
            double[,] matrisBTersi = MatrisTersiniAl(GirisMatrisi);
            //-------------------- A Dönüşüm Matrisi (3x3) -----------------
            double a00 = 0, a01 = 0, a02 = 0, a10 = 0, a11 = 0, a12 = 0, a20 = 0, a21 = 0, a22 = 0;
            a00 = matrisBTersi[0, 0] * X1 + matrisBTersi[0, 1] * Y1 + matrisBTersi[0, 2] *
            X2 + matrisBTersi[0, 3] * Y2 + matrisBTersi[0, 4] * X3 + matrisBTersi[0, 5] * Y3 +
            matrisBTersi[0, 6] * X4 + matrisBTersi[0, 7] * Y4;
            a01 = matrisBTersi[1, 0] * X1 + matrisBTersi[1, 1] * Y1 + matrisBTersi[1, 2] *
            X2 + matrisBTersi[1, 3] * Y2 + matrisBTersi[1, 4] * X3 + matrisBTersi[1, 5] * Y3 +
            matrisBTersi[1, 6] * X4 + matrisBTersi[1, 7] * Y4;
            a02 = matrisBTersi[2, 0] * X1 + matrisBTersi[2, 1] * Y1 + matrisBTersi[2, 2] *
            X2 + matrisBTersi[2, 3] * Y2 + matrisBTersi[2, 4] * X3 + matrisBTersi[2, 5] * Y3 +
            matrisBTersi[2, 6] * X4 + matrisBTersi[2, 7] * Y4;
            a10 = matrisBTersi[3, 0] * X1 + matrisBTersi[3, 1] * Y1 + matrisBTersi[3, 2] *
            X2 + matrisBTersi[3, 3] * Y2 + matrisBTersi[3, 4] * X3 + matrisBTersi[3, 5] * Y3 +
            matrisBTersi[3, 6] * X4 + matrisBTersi[3, 7] * Y4;
            a11 = matrisBTersi[4, 0] * X1 + matrisBTersi[4, 1] * Y1 + matrisBTersi[4, 2] *
            X2 + matrisBTersi[4, 3] * Y2 + matrisBTersi[4, 4] * X3 + matrisBTersi[4, 5] * Y3 +
            matrisBTersi[4, 6] * X4 + matrisBTersi[4, 7] * Y4;
            a12 = matrisBTersi[5, 0] * X1 + matrisBTersi[5, 1] * Y1 + matrisBTersi[5, 2] *
            X2 + matrisBTersi[5, 3] * Y2 + matrisBTersi[5, 4] * X3 + matrisBTersi[5, 5] * Y3 +
            matrisBTersi[5, 6] * X4 + matrisBTersi[5, 7] * Y4;
            a20 = matrisBTersi[6, 0] * X1 + matrisBTersi[6, 1] * Y1 + matrisBTersi[6, 2] *
            X2 + matrisBTersi[6, 3] * Y2 + matrisBTersi[6, 4] * X3 + matrisBTersi[6, 5] * Y3 +
            matrisBTersi[6, 6] * X4 + matrisBTersi[6, 7] * Y4;
            a21 = matrisBTersi[7, 0] * X1 + matrisBTersi[7, 1] * Y1 + matrisBTersi[7, 2] * X2 + matrisBTersi[7, 3] * Y2 + matrisBTersi[7, 4] * X3 + matrisBTersi[7, 5] * Y3 + matrisBTersi[7, 6] * X4 + matrisBTersi[7, 7] * Y4;
            a22 = 1;
            //------------------------- Perspektif düzeltme işlemi ------------------------
            PerspektifDuzelt(a00, a01, a02, a10, a11, a12, a20, a21, a22);
        }

        public double[,] MatrisTersiniAl(double[,] GirisMatrisi)
        {
            int MatrisBoyutu = Convert.ToInt16(Math.Sqrt(GirisMatrisi.Length));
            //matris boyutu içindeki eleman sayısı olduğu için kare matrisde
            //karekökü matris boyutu olur.
            double[,] CikisMatrisi = new double[MatrisBoyutu, MatrisBoyutu]; //A nın
                                                                             //tersi alındığında bu matris içinde tutulacak
                                                                             //--I Birim matrisin içeriğini dolduruyor
            for (int i = 0; i < MatrisBoyutu; i++)
            {
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (i == j)
                        CikisMatrisi[i, j] = 1;
                    else
                        CikisMatrisi[i, j] = 0;
                }
            }
            //--Matris Tersini alma işlemi---------
            double d, k;
            for (int i = 0; i < MatrisBoyutu; i++)
            {
                d = GirisMatrisi[i, i];
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (d == 0)
                    {
                        d = 0.0001; //0 bölme hata veriyordu.
                    }
                    GirisMatrisi[i, j] = GirisMatrisi[i, j] / d;
                    CikisMatrisi[i, j] = CikisMatrisi[i, j] / d;
                }
                for (int x = 0; x < MatrisBoyutu; x++)
                {
                    if (x != i)
                    {
                        k = GirisMatrisi[x, i];
                        for (int j = 0; j < MatrisBoyutu; j++)
                        {
                            GirisMatrisi[x, j] = GirisMatrisi[x, j] - GirisMatrisi[i, j] * k;
                            CikisMatrisi[x, j] = CikisMatrisi[x, j] - CikisMatrisi[i, j] * k;
                        }
                    }
                }
            }
            return CikisMatrisi;
        }

        
        public void PerspektifDuzelt(double a00, double a01, double a02, double a10, double a11, double a12, double a20, double a21, double a22)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen İşlem Yapılacak Resmi Ekleyin");
            }
            else
            {
                Bitmap GirisResmi, CikisResmi;
                Color OkunanRenk;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                double X, Y, z;
                for (int x = 0; x < (ResimGenisligi); x++)
                {
                    for (int y = 0; y < (ResimYuksekligi); y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        z = a20 * x + a21 * y + 1;
                        X = (a00 * x + a01 * y + a02) / z;
                        Y = (a10 * x + a11 * y + a12) / z;
                        if (X > 0 && X < ResimGenisligi && Y > 0 && Y < ResimYuksekligi)

                            CikisResmi.SetPixel((int)X, (int)Y, OkunanRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }
        public void asindirma(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen İşlem Yapılacak Resmi Ekleyin");
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                Bitmap girisresmi = new Bitmap(pictureBox1.Image);
                int resimGenisligi = girisresmi.Width;
                int resimYuksekligi = girisresmi.Height;
                CikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);
                int SablonBoyutu = 7;
                bool beyazMi = true;
                for (int x = (SablonBoyutu - 1) / 2; x < resimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (int y = (SablonBoyutu - 1) / 2; y < resimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        beyazMi = false;
                        for (int i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (int j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = girisresmi.GetPixel(x + i, y + j);
                                if (OkunanRenk.R == 255)
                                    beyazMi = true;
                                else
                                {
                                    beyazMi = false;
                                    break;
                                }
                            }
                        }
                        if (beyazMi == true)
                        {
                            CikisResmi.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            CikisResmi.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        }


                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }
        public void yayma(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen İşlem Yapılacak Resmi Ekleyin");
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                Bitmap girisresmi = new Bitmap(pictureBox1.Image);
                int resimGenisligi = girisresmi.Width;
                int resimYuksekligi = girisresmi.Height;
                CikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);
                int SablonBoyutu = 7;
                bool beyazMi = true;

                for (int x = (SablonBoyutu - 1) / 2; x < resimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (int y = (SablonBoyutu - 1) / 2; y < resimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        beyazMi = false;
                        for (int i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (int j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = girisresmi.GetPixel(x + i, y + j);
                                if (OkunanRenk.R == 255)
                                    beyazMi = true;
                            }
                        }
                        if (beyazMi == true)
                        {
                            CikisResmi.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            CikisResmi.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        }


                    }
                }
                pictureBox2.Image = CikisResmi;
            }
           
        }

        


        public void prewitt(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = 3;
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y;
                Color Renk;
                int P1, P2, P3, P4, P5, P6, P7, P8, P9;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) 
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        Renk = GirisResmi.GetPixel(x - 1, y - 1);
                        P1 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y - 1);
                        P2 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y - 1);
                        P3 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y);
                        P4 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y);
                        P5 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y);
                        P6 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y + 1);
                        P7 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y + 1);
                        P8 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y + 1);
                        P9 = (Renk.R + Renk.G + Renk.B) / 3;
                        int Gx = Math.Abs(-P1 + P3 - P4 + P6 - P7 + P9); 
                        int Gy = Math.Abs(P1 + P2 + P3 - P7 - P8 - P9); 
                        int PrewittDegeri = 0;
                        PrewittDegeri = Gx;
                        PrewittDegeri = Gy;
                        PrewittDegeri = Gx + Gy; 
                        if (PrewittDegeri > 255) PrewittDegeri = 255;
                        
                        CikisResmi.SetPixel(x, y, Color.FromArgb(PrewittDegeri, PrewittDegeri, PrewittDegeri));
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        
        public void RobertCross(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int x, y;
                Color Renk;
                int P1, P2, P3, P4;
                for (x = 0; x < ResimGenisligi - 1; x++) 
                {
                    for (y = 0; y < ResimYuksekligi - 1; y++)
                    {
                        Renk = GirisResmi.GetPixel(x, y);
                        P1 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y);
                        P2 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y + 1);
                        P3 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y + 1);
                        P4 = (Renk.R + Renk.G + Renk.B) / 3;
                        int Gx = Math.Abs(P1 - P4); 
                        int Gy = Math.Abs(P2 - P3); 
                        int RobertCrossDegeri = 0;
                        RobertCrossDegeri = Gx;
                        RobertCrossDegeri = Gy;
                        RobertCrossDegeri = Gx + Gy; 
                        if (RobertCrossDegeri > 255) RobertCrossDegeri = 255; 
                                                                            
                        CikisResmi.SetPixel(x, y, Color.FromArgb(RobertCrossDegeri, RobertCrossDegeri,
                       RobertCrossDegeri));
                    }
                }
                pictureBox2.Image = CikisResmi;
            }

        }

       

            public void boyutlandirma(Bitmap bm,int KucultmeKatsayisi)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }

            else
            {
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int x2 = 0, y2 = 0;
                
                for (int x1 = 0; x1 < ResimGenisligi; x1 = x1 + KucultmeKatsayisi)
                {
                    y2 = 0;
                    for (int y1 = 0; y1 < ResimYuksekligi; y1 = y1 + KucultmeKatsayisi)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);
                        DonusenRenk = OkunanRenk;
                        CikisResmi.SetPixel(x2, y2, DonusenRenk);
                        y2++;
                    }
                    x2++;
                }
                pictureBox2.Image = CikisResmi;
            }
        }
        public void tasima(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen İşlem Yapılacak Resmi Ekleyin");
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                double x2 = 0, y2 = 0;

                int Tx = 70;
                int Ty = 50;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);
                        x2 = x1 + Tx;
                        y2 = y1 + Ty;
                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }
        public void netlestir(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen İşlem Yapılacak Resmi Ekleyin");
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = 3;
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y, i, j, toplamR, toplamG, toplamB;
                int R, G, B;
                int[] Matris = { 0, -2, 0, -2, 11, -2, 0, -2, 0 };
                int MatrisToplami = 0 + -2 + 0 + -2 + 11 + -2 + 0 + -2 + 0;

                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        toplamR = 0;
                        toplamG = 0;
                        toplamB = 0;

                        int k = 0;
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                toplamR = toplamR + OkunanRenk.R * Matris[k];
                                toplamG = toplamG + OkunanRenk.G * Matris[k];
                                toplamB = toplamB + OkunanRenk.B * Matris[k];
                                k++;
                            }
                        }
                        R = toplamR / MatrisToplami;
                        G = toplamG / MatrisToplami;
                        B = toplamB / MatrisToplami;

                        if (R > 255) R = 255;
                        if (G > 255) G = 255;
                        if (B > 255) B = 255;
                        if (R < 0) R = 0;
                        if (G < 0) G = 0;
                        if (B < 0) B = 0;

                        CikisResmi.SetPixel(x, y, Color.FromArgb(R, G, B));
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyası |*.jpg;*.nef;*.png |  Tüm Dosyalar |*.*";
            dosya.ShowDialog();  
            String dosyayolu=dosya.FileName;
            textBox1.Text = dosyayolu;
            pictureBox1.ImageLocation = dosya.FileName;
            pictureBox2.ImageLocation = dosya.FileName;
            
        }

      

        public void Parlaklik(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen İşlem Yapılacak Resmi Ekleyin");
            }
            else
            {
                int R = 0, G = 0, B = 0;
            Color OkunanRenk, DonusenRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                if (text_p.Text.Length == 0)
                {
                    MessageBox.Show("Lütfen Parlaklık Değeri Giriniz");

                }
                else
                {
                    int parlaklıkdegeri = int.Parse(text_p.Text);

                    int i = 0, j = 0;
                    for (int x = 0; x < ResimGenisligi; x++)
                    {
                        j = 0;
                        for (int y = 0; y < ResimYuksekligi; y++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x, y);
                            R = OkunanRenk.R + parlaklıkdegeri;
                            G = OkunanRenk.G + parlaklıkdegeri;
                            B = OkunanRenk.B + parlaklıkdegeri;
                            if (R > 255) R = 255;
                            if (G > 255) G = 255;
                            if (B > 255) B = 255;
                            DonusenRenk = Color.FromArgb(R, G, B);
                            CikisResmi.SetPixel(i, j, DonusenRenk); j++;
                        }

                        i++;
                    }
                    pictureBox2.Image = CikisResmi;
                }
            }
        }
        

        private void Negatif_Click(object sender, EventArgs e)
        {
            
            Bitmap bmp = (Bitmap)pictureBox2.Image;
            negatif(bmp);
            pictureBox2.Visible = true;
            Refresh();

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (pictureBox1.ImageLocation == null)
            {
                MessageBox.Show("Resim Ekleyiniz");
            }
            else
            {
                if (textBox2.Text.Length == 0)
                {
                    MessageBox.Show("Eşikleme Değeri giriniz");
                }
                else
                {
                   
                    Bitmap bmp = (Bitmap)pictureBox2.Image;
                    esıkleme(bmp);
                    Refresh();
                }
            }
            
            
        }

        public void kontrast(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen İşlem Yapılacak Resmi Ekleyin");
            }
            else
            {
                int R = 0, G = 0, B = 0;
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);

                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                if (txtk.Text.Length == 0)
                {
                    MessageBox.Show("Lütfen Değer Giriniz");
                }
                else
                {
                    double C_KontrastSeviyesi = Convert.ToInt32(txtk.Text);
                    double F_KontrastFaktoru = (259 * (C_KontrastSeviyesi + 255)) / (255 * (259 - C_KontrastSeviyesi));

                    for (int x = 0; x < ResimGenisligi; x++)
                    {
                        for (int y = 0; y < ResimYuksekligi; y++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x, y);
                            R = OkunanRenk.R;
                            G = OkunanRenk.G;
                            B = OkunanRenk.B;
                            R = (int)((F_KontrastFaktoru * (R - 128)) + 128);
                            G = (int)((F_KontrastFaktoru * (G - 128)) + 128);
                            B = (int)((F_KontrastFaktoru * (B - 128)) + 128);

                            if (R > 255) R = 255;
                            if (G > 255) G = 255;
                            if (B > 255) B = 255;
                            if (R < 0) R = 0;
                            if (G < 0) G = 0;
                            if (B < 0) B = 0;
                            DonusenRenk = Color.FromArgb(R, G, B);

                            CikisResmi.SetPixel(x, y, DonusenRenk);
                        }
                    }
                    pictureBox2.Image = CikisResmi;
                }
            }
        }
        public void histogramesitleme(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen İşlem Yapılacak Resmi Ekleyin");
            }
            else
            {
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int pixels = (int)GirisResmi.Height * (int)GirisResmi.Width;
                decimal Const = 255 / (decimal)pixels;
                int R, G, B;
                int[] red = new int[256], green = new int[256], blue = new int[256];
                for (int i = 0; i < GirisResmi.Width; i++)
                {
                    for (int j = 0; j < GirisResmi.Height; j++)
                    {
                        var piksel = GirisResmi.GetPixel(i, j);
                        red[(int)piksel.R]++;
                        green[(int)piksel.G]++;
                        blue[(int)piksel.B]++;
                    }
                }
                for (int r = 1; r <= 255; r++)
                {
                    red[r] = red[r] + red[r - 1];
                    green[r] = green[r] + green[r - 1];
                    blue[r] = blue[r] + blue[r - 1];
                }
                for (int y = 0; y < GirisResmi.Height; y++)
                {
                    for (int x = 0; x < GirisResmi.Width; x++)
                    {
                        Color pixelColor = GirisResmi.GetPixel(x, y);
                        R = (int)((decimal)red[pixelColor.R] * Const);
                        G = (int)((decimal)green[pixelColor.G] * Const);
                        B = (int)((decimal)blue[pixelColor.B] * Const);
                        Color newColor = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, newColor);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        public void yakinlastirma(Bitmap bm)
        {

            Color OkunanRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int BuyutmeKatsayisi = Convert.ToInt32(textb.Text);
            int x2 = 0, y2 = 0;

            for (int x1 = 0; x1 < ResimGenisligi; x1++)
            {
                for (int y1 = 0; y1 < ResimYuksekligi; y1++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x1, y1);
                    for (int i = 0; i < BuyutmeKatsayisi; i++)
                    {
                        for (int j = 0; j < BuyutmeKatsayisi; j++)
                        {
                            x2 = x1 * BuyutmeKatsayisi + i;
                            y2 = y1 * BuyutmeKatsayisi + j;
                            if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                                CikisResmi.SetPixel(x2, y2, OkunanRenk);
                        }
                    }

                }
            }
            pictureBox2.Image = CikisResmi;
        }
      public void laplass(Bitmap bm)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen İşlem Yapılacak Resmi Ekleyin");
            }
            else
            {
                Bitmap img = new Bitmap(pictureBox1.Image);
                Bitmap image = new Bitmap(img);
                for (int x = 1; x < image.Width - 1; x++)
                {
                    for (int y = 1; y < image.Height - 1; y++)
                    {
                        Color color2, color4, color5, color6, color8;
                        color2 = img.GetPixel(x, y - 1);
                        color4 = img.GetPixel(x - 1, y);
                        color5 = img.GetPixel(x, y);
                        color6 = img.GetPixel(x + 1, y);
                        color8 = img.GetPixel(x, y + 1);
                        int r = color2.R + color4.R + color5.R * (-4) + color6.R + color8.R;
                        int g = color2.G + color4.G + color5.G * (-4) + color6.G + color8.G;
                        int b = color2.B + color4.B + color5.B * (-4) + color6.B + color8.B;




                        int avg = (r + g + b) / 3;
                        if (avg > 255) avg = 255;
                        if (avg < 0) avg = 0;
                        image.SetPixel(x, y, Color.FromArgb(avg, avg, avg));
                    }
                }
                pictureBox2.Image = image;
            }
        }
      
        private void button1_Click_2(object sender, EventArgs e)
        {
            
            pictureBox2.ImageLocation = textBox1.Text;            
            pictureBox3.Refresh();
            pictureBox2.Visible = true;
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            listBox1.Items.Clear();          
            label5.Text = " ";
        }

        private void histogram_Click(object sender, EventArgs e)
        {
            if (pictureBox1.ImageLocation == null)
            {
                MessageBox.Show("Resim Ekleyiniz");
            }
            else
            {
                Bitmap bmp = (Bitmap)pictureBox2.Image;
                Histogramciz(bmp);
                
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = " ";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.ImageLocation!=null)
            {
                if (textBox3.Text.Length != 0 && textBox4.Text.Length != 0 && textBox5.Text.Length != 0 && textBox6.Text.Length != 0)
                {
                    Bitmap bmp = (Bitmap)pictureBox2.Image;
                    KontrastGerme(bmp);
                    Refresh();
                }
                else
                    MessageBox.Show("Bütün Değerleri Doldurunuz");
            }
            else
                MessageBox.Show("Resim Ekleyiniz");
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
                    }

        private void button5_Click(object sender, EventArgs e)
        {
            
            Bitmap bmp = (Bitmap)pictureBox2.Image;
            Dondurme(bmp);
            Refresh();
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox2.Image;
            aynalama(bmp);
            Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox2.Image;
            meanFiltresi(bmp);
            Refresh();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox2.Image;
            
            if (textb.Text.Length == 0)
            {
                MessageBox.Show("Lütfen Küçültme Kat Sayısı Giriniz");
            }
            else
            {
                int KucultmeKatsayisi = int.Parse(textb.Text);
                boyutlandirma(bmp, KucultmeKatsayisi);
            }
            Refresh();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox2.Image;
            netlestir(bmp);
            Refresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox2.Image;
            meadian(bmp);
            Refresh();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox2.Image;
            gauss(bmp);
            Refresh();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Bitmap bmp=(Bitmap)pictureBox2.Image;
            sobel(bmp);
            Refresh();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox2.Image;
            prewitt(bmp);
            Refresh();
        }

      

        private void button13_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox2.Image;
            tasima(bmp);
            Refresh();
        }

        private void resimEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyası |*.jpg;*.nef;*.png |  Tüm Dosyalar |*.*";
            dosya.ShowDialog();
            String dosyayolu = dosya.FileName;
            textBox1.Text = dosyayolu;
            pictureBox1.ImageLocation = dosya.FileName;
            pictureBox2.ImageLocation = dosya.FileName;
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResmiKaydet();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            egme(bmp);
            Refresh();
        }

     

        private void button16_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            RobertCross(bmp);
            Refresh();

        }

        private void button15_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            Parlaklik(bmp);
            Refresh();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            yakinlastirma(bmp);
            Refresh();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            laplass(bmp);
            Refresh();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            histogramesitleme(bmp);
            Refresh();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            kontrast(bmp);
            Refresh();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            Perspektif(bmp);
            Refresh();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            yayma(bmp);
            Refresh();
        }


        private void button22_Click_1(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            asindirma(bmp);
            Refresh();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox2.Image;
        }
    }
}