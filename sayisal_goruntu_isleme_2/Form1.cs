using System;
using System.Drawing;
using System.Windows.Forms;

namespace sayisal_goruntu_isleme_2
{
    public partial class Form1 : Form
    {
        private Bitmap orijinalBitmap;
        private Bitmap griBitmap;
        private Bitmap otsuBitmap;
        private Bitmap segmenteEdilmisBitmap;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    orijinalBitmap = new Bitmap(openFileDialog.FileName);
                    pictureBox.Image = orijinalBitmap;
                    pictureBox1.Image = orijinalBitmap;
                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Orijinal resmi gri tonlamaya �evir
            griBitmap = ConvertTogriscale(orijinalBitmap);
            pictureBox.Image = griBitmap;
        }
        private Bitmap ConvertTogriscale(Bitmap source)
        {
            if (source == null)
            {
                MessageBox.Show("�nce bir resim y�klemelisiniz.");
                return null;
            }

            Bitmap griBitmap = new Bitmap(source.Width, source.Height); // Gri bitmap'i olu�tur

            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    Color orijinalColor = source.GetPixel(x, y);
                    int griScale = (int)(orijinalColor.R * 0.3 + orijinalColor.G * 0.59 + orijinalColor.B * 0.11);
                    Color griColor = Color.FromArgb(griScale, griScale, griScale);
                    griBitmap.SetPixel(x, y, griColor);
                }
            }

            return griBitmap; // Gri bitmap'i geri d�nd�r
        }


        private void btnApplyOtsu_Click(object sender, EventArgs e)
        {
            if (griBitmap == null)
            {
                MessageBox.Show("L�tfen �nce gri tonlamaya �evrilmi� bir resim olu�turun.");
                return;
            }

            // Histogram hesapla
            int[] histogram = new int[256];
            for (int y = 0; y < griBitmap.Height; y++)
            {
                for (int x = 0; x < griBitmap.Width; x++)
                {
                    int pixelValue = griBitmap.GetPixel(x, y).R;
                    histogram[pixelValue]++;
                }
            }

            // Otsu'nun e�ik de�erini bul
            int threshold = OtsuThreshold(histogram, griBitmap.Width * griBitmap.Height);
            label1.Text = threshold.ToString();

            // Otsu g�r�nt�s�n� olu�tur
            Bitmap otsuBitmap = new Bitmap(griBitmap.Width, griBitmap.Height); // Yeni bir bitmap olu�tur

            for (int y = 0; y < griBitmap.Height; y++)
            {
                for (int x = 0; x < griBitmap.Width; x++)
                {
                    int pixelValue = griBitmap.GetPixel(x, y).R;
                    Color color = (pixelValue >= threshold) ? Color.White : Color.Black;
                    otsuBitmap.SetPixel(x, y, color);
                }
            }

            // Otsu g�r�nt�y� g�ster
            pictureBox.Image = otsuBitmap;
        }


        private int OtsuThreshold(int[] histogram, int totalPixels)
        {
            float sum = 0;
            for (int i = 0; i < 256; i++) sum += i * histogram[i];

            float sumB = 0;
            int wB = 0;
            int wF = 0;

            float varMax = 0;
            int threshold = 0;

            for (int t = 0; t < 256; t++)
            {
                wB += histogram[t];
                if (wB == 0) continue;

                wF = totalPixels - wB;
                if (wF == 0) break;

                sumB += (float)(t * histogram[t]);

                float mB = sumB / wB;
                float mF = (sum - sumB) / wF;

                float varBetween = (float)wB * (float)wF * (mB - mF) * (mB - mF);

                if (varBetween > varMax)
                {
                    varMax = varBetween;
                    threshold = t;
                }
            }

            return threshold;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (orijinalBitmap == null)
            {
                MessageBox.Show("L�tfen �nce bir g�r�nt� y�kleyin.");
                return;
            }

            int k;
            if (!int.TryParse(textBox1.Text, out k) || k <= 0)
            {
                MessageBox.Show("L�tfen k i�in ge�erli bir de�er girin (pozitif bir tamsay�).");
                return;
            }

            segmenteEdilmisBitmap = G�r�nt�y�B�lgele(orijinalBitmap, k);
            pictureBox1.Image = segmenteEdilmisBitmap;
        }
        private Bitmap G�r�nt�y�B�lgele(Bitmap orijinalG�r�nt�, int k)
        {
            Bitmap segmenteEdilmisBitmap = new Bitmap(orijinalG�r�nt�.Width, orijinalG�r�nt�.Height);

            // Piksellerin renk bilgilerini al
            List<Color> pikseller = new List<Color>();
            for (int y = 0; y < orijinalG�r�nt�.Height; y++)
            {
                for (int x = 0; x < orijinalG�r�nt�.Width; x++)
                {
                    pikseller.Add(orijinalG�r�nt�.GetPixel(x, y));
                }
            }

            // Rasgele k pozisyondaki piksellerin renk bilgisini k�me merkezi olarak se�
            Random rasgele = new Random();
            List<Color> k�meMerkezleri = new List<Color>();
            for (int i = 0; i < k; i++)
            {
                int indeks = rasgele.Next(pikseller.Count);
                k�meMerkezleri.Add(pikseller[indeks]);
            }

            // K�meleme
            bool merkezlerDe�i�ti = true;
            while (merkezlerDe�i�ti)
            {
                merkezlerDe�i�ti = false;

                // Her pikselin en yak�n k�me merkezini bul
                Dictionary<Color, List<Color>> k�meler = new Dictionary<Color, List<Color>>();
                foreach (Color merkez in k�meMerkezleri)
                {
                    k�meler[merkez] = new List<Color>();
                }

                foreach (Color piksel in pikseller)
                {
                    Color enYak�nMerkez = EnYak�nMerkeziBul(piksel, k�meMerkezleri);
                    k�meler[enYak�nMerkez].Add(piksel);
                }

                // Yeni k�me merkezlerini hesapla
                List<Color> yeniMerkezler = new List<Color>();
                foreach (KeyValuePair<Color, List<Color>> k�me in k�meler)
                {
                    if (k�me.Value.Count == 0)
                    {
                        yeniMerkezler.Add(k�me.Key);
                        continue;
                    }

                    int r = 0, g = 0, b = 0;
                    foreach (Color piksel in k�me.Value)
                    {
                        r += piksel.R;
                        g += piksel.G;
                        b += piksel.B;
                    }
                    int say = k�me.Value.Count;
                    Color yeniMerkez = Color.FromArgb(r / say, g / say, b / say);
                    yeniMerkezler.Add(yeniMerkez);

                    // Eski ve yeni k�me merkezlerini kar��la�t�r
                    if (yeniMerkez != k�me.Key)
                    {
                        merkezlerDe�i�ti = true;
                    }
                }

                k�meMerkezleri = yeniMerkezler;
            }

            // Segmentasyon yap�lm�� g�r�nt�y� olu�tur
            int pikselIndeksi = 0;
            for (int y = 0; y < orijinalG�r�nt�.Height; y++)
            {
                for (int x = 0; x < orijinalG�r�nt�.Width; x++)
                {
                    Color piksel = pikseller[pikselIndeksi];
                    Color enYak�nMerkez = EnYak�nMerkeziBul(piksel, k�meMerkezleri);
                    segmenteEdilmisBitmap.SetPixel(x, y, enYak�nMerkez);
                    pikselIndeksi++;
                }
            }

            return segmenteEdilmisBitmap;
        }

        private Color EnYak�nMerkeziBul(Color piksel, List<Color> merkezler)
        {
            double minUzakl�k = double.MaxValue;
            Color enYak�nMerkez = merkezler[0];
            foreach (Color merkez in merkezler)
            {
                double uzakl�k = Uzakl�kHesapla(piksel, merkez);
                if (uzakl�k < minUzakl�k)
                {
                    minUzakl�k = uzakl�k;
                    enYak�nMerkez = merkez;
                }
            }
            return enYak�nMerkez;
        }

        private double Uzakl�kHesapla(Color renk1, Color renk2)
        {
            double dr = renk1.R - renk2.R;
            double dg = renk1.G - renk2.G;
            double db = renk1.B - renk2.B;
            return Math.Sqrt(dr * dr + dg * dg + db * db);
        }
    }
}
    


