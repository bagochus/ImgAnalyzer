using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    public class SmoothImageViewer : Form
    {
        private readonly PictureBox pictureBox;
        private Bitmap originalImage;
        private Bitmap displayImage;
        private PointF imagePosition = new PointF(0, 0);
        private Point lastMousePos;
        private float zoomFactor = 1.0f;
        private bool isPanning;

        public SmoothImageViewer(string imagePath)
        {
            // Загрузка изображения с блокировкой в памяти
            originalImage = new Bitmap(imagePath);
            displayImage = new Bitmap(originalImage);

            // Настройка PictureBox
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.AutoSize,
                BackColor = Color.Gray
            };

            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
            pictureBox.MouseWheel += PictureBox_MouseWheel;
            pictureBox.Paint += PictureBox_Paint;

            // Двойная буферизация для устранения мерцания
            this.DoubleBuffered = true;
           // pictureBox.DoubleBuffered = true;

            Controls.Add(pictureBox);
            ClientSize = new Size(800, 600);
            Text = "Оптимизированный просмотр изображения";
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (displayImage == null) return;

            // Отрисовка с учетом позиции и масштаба
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImage(displayImage, imagePosition.X, imagePosition.Y,
                               displayImage.Width * zoomFactor,
                               displayImage.Height * zoomFactor);
         
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPanning = true;
                lastMousePos = e.Location;
                Cursor = Cursors.Hand;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPanning)
            {
                // Плавное перемещение с коэффициентом сглаживания
                float smoothFactor = 0.7f;
                imagePosition.X += (e.X - lastMousePos.X) * smoothFactor;
                imagePosition.Y += (e.Y - lastMousePos.Y) * smoothFactor;

                lastMousePos = e.Location;
                pictureBox.Invalidate();
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPanning = false;
                Cursor = Cursors.Default;
            }
        }

        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                float zoom = e.Delta > 0 ? 1.1f : 1 / 1.1f;
                zoomFactor *= zoom;
                pictureBox.Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            // Освобождение ресурсов
            if (disposing)
            {
                originalImage?.Dispose();
                displayImage?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
