namespace sayisal_goruntu_isleme_2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pictureBox = new PictureBox();
            button_gri = new Button();
            button1 = new Button();
            button2 = new Button();
            textBox1 = new TextBox();
            pictureBox1 = new PictureBox();
            button3 = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(12, 15);
            pictureBox.Margin = new Padding(3, 4, 3, 4);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(512, 512);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            // 
            // button_gri
            // 
            button_gri.Location = new Point(12, 556);
            button_gri.Margin = new Padding(3, 4, 3, 4);
            button_gri.Name = "button_gri";
            button_gri.Size = new Size(137, 29);
            button_gri.TabIndex = 1;
            button_gri.Text = "Griye Çevir";
            button_gri.UseVisualStyleBackColor = true;
            button_gri.Click += button1_Click;
            // 
            // button1
            // 
            button1.Location = new Point(581, 185);
            button1.Name = "button1";
            button1.Size = new Size(124, 58);
            button1.TabIndex = 2;
            button1.Text = "Resim Seç";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // button2
            // 
            button2.Location = new Point(289, 556);
            button2.Name = "button2";
            button2.Size = new Size(136, 29);
            button2.TabIndex = 3;
            button2.Text = "Otsu";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnApplyOtsu_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(794, 557);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 4;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(757, 15);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(512, 512);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // button3
            // 
            button3.Location = new Point(1100, 549);
            button3.Name = "button3";
            button3.Size = new Size(121, 29);
            button3.TabIndex = 6;
            button3.Text = "K means";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(194, 568);
            label1.Name = "label1";
            label1.Size = new Size(0, 20);
            label1.TabIndex = 8;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1412, 597);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(pictureBox1);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(button_gri);
            Controls.Add(pictureBox);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Sayısal Görüntü İşleme Ödev 2";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button button_gri;
        private Button button1;
        private Button button2;
        private TextBox textBox1;
        private PictureBox pictureBox1;
        private Button button3;
        private Label label1;
    }
}
