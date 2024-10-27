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
            // Orijinal resmi gri tonlamaya çevir
            griBitmap = ConvertTogriscale(orijinalBitmap);
            pictureBox.Image = griBitmap;
        }
        private Bitmap ConvertTogriscale(Bitmap source)
        {
            if (source == null)
            {
                MessageBox.Show("Önce bir resim yüklemelisiniz.");
                return null;
            }

            Bitmap griBitmap = new Bitmap(source.Width, source.Height); // Gri bitmap'i oluþtur

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

            return griBitmap; // Gri bitmap'i geri döndür
        }


        private void btnApplyOtsu_Click(object sender, EventArgs e)
        {
            if (griBitmap == null)
            {
                MessageBox.Show("Lütfen önce gri tonlamaya çevrilmiþ bir resim oluþturun.");
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

            // Otsu'nun eþik deðerini bul
            int threshold = OtsuThreshold(histogram, griBitmap.Width * griBitmap.Height);
            label1.Text = threshold.ToString();

            // Otsu görüntüsünü oluþtur
            Bitmap otsuBitmap = new Bitmap(griBitmap.Width, griBitmap.Height); // Yeni bir bitmap oluþtur

            for (int y = 0; y < griBitmap.Height; y++)
            {
                for (int x = 0; x < griBitmap.Width; x++)
                {
                    int pixelValue = griBitmap.GetPixel(x, y).R;
                    Color color = (pixelValue >= threshold) ? Color.White : Color.Black;
                    otsuBitmap.SetPixel(x, y, color);
                }
            }

            // Otsu görüntüyü göster
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
                MessageBox.Show("Lütfen önce bir görüntü yükleyin.");
                return;
            }

            int k;
            if (!int.TryParse(textBox1.Text, out k) || k <= 0)
            {
                MessageBox.Show("Lütfen k için geçerli bir deðer girin (pozitif bir tamsayý).");
                return;
            }

            segmenteEdilmisBitmap = GörüntüyüBölgele(orijinalBitmap, k);
            pictureBox1.Image = segmenteEdilmisBitmap;
        }
        private Bitmap GörüntüyüBölgele(Bitmap orijinalGörüntü, int k)
        {
            Bitmap segmenteEdilmisBitmap = new Bitmap(orijinalGörüntü.Width, orijinalGörüntü.Height);

            // Piksellerin renk bilgilerini al
            List<Color> pikseller = new List<Color>();
            for (int y = 0; y < orijinalGörüntü.Height; y++)
            {
                for (int x = 0; x < orijinalGörüntü.Width; x++)
                {
                    pikseller.Add(orijinalGörüntü.GetPixel(x, y));
                }
            }

            // Rasgele k pozisyondaki piksellerin renk bilgisini küme merkezi olarak seç
            Random rasgele = new Random();
            List<Color> kümeMerkezleri = new List<Color>();
            for (int i = 0; i < k; i++)
            {
                int indeks = rasgele.Next(pikseller.Count);
                kümeMerkezleri.Add(pikseller[indeks]);
            }

            // Kümeleme
            bool merkezlerDeðiþti = true;
            while (merkezlerDeðiþti)
            {
                merkezlerDeðiþti = false;

                // Her pikselin en yakýn küme merkezini bul
                Dictionary<Color, List<Color>> kümeler = new Dictionary<Color, List<Color>>();
                foreach (Color merkez in kümeMerkezleri)
                {
                    kümeler[merkez] = new List<Color>();
                }

                foreach (Color piksel in pikseller)
                {
                    Color enYakýnMerkez = EnYakýnMerkeziBul(piksel, kümeMerkezleri);
                    kümeler[enYakýnMerkez].Add(piksel);
                }

                // Yeni küme merkezlerini hesapla
                List<Color> yeniMerkezler = new List<Color>();
                foreach (KeyValuePair<Color, List<Color>> küme in kümeler)
                {
                    if (küme.Value.Count == 0)
                    {
                        yeniMerkezler.Add(küme.Key);
                        continue;
                    }

                    int r = 0, g = 0, b = 0;
                    foreach (Color piksel in küme.Value)
                    {
                        r += piksel.R;
                        g += piksel.G;
                        b += piksel.B;
                    }
                    int say = küme.Value.Count;
                    Color yeniMerkez = Color.FromArgb(r / say, g / say, b / say);
                    yeniMerkezler.Add(yeniMerkez);

                    // Eski ve yeni küme merkezlerini karþýlaþtýr
                    if (yeniMerkez != küme.Key)
                    {
                        merkezlerDeðiþti = true;
                    }
                }

                kümeMerkezleri = yeniMerkezler;
            }

            // Segmentasyon yapýlmýþ görüntüyü oluþtur
            int pikselIndeksi = 0;
            for (int y = 0; y < orijinalGörüntü.Height; y++)
            {
                for (int x = 0; x < orijinalGörüntü.Width; x++)
                {
                    Color piksel = pikseller[pikselIndeksi];
                    Color enYakýnMerkez = EnYakýnMerkeziBul(piksel, kümeMerkezleri);
                    segmenteEdilmisBitmap.SetPixel(x, y, enYakýnMerkez);
                    pikselIndeksi++;
                }
            }

            return segmenteEdilmisBitmap;
        }

        private Color EnYakýnMerkeziBul(Color piksel, List<Color> merkezler)
        {
            double minUzaklýk = double.MaxValue;
            Color enYakýnMerkez = merkezler[0];
            foreach (Color merkez in merkezler)
            {
                double uzaklýk = UzaklýkHesapla(piksel, merkez);
                if (uzaklýk < minUzaklýk)
                {
                    minUzaklýk = uzaklýk;
                    enYakýnMerkez = merkez;
                }
            }
            return enYakýnMerkez;
        }

        private double UzaklýkHesapla(Color renk1, Color renk2)
        {
            double dr = renk1.R - renk2.R;
            double dg = renk1.G - renk2.G;
            double db = renk1.B - renk2.B;
            return Math.Sqrt(dr * dr + dg * dg + db * db);
        }
    }
}
    


