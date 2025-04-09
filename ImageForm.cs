using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer
{


    public class ImageForm : Form
    {
        private Image _image;
        public event Action<Point> ImageClicked; // Событие для передачи координат

        public ImageForm(Image image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            _image = image;

            // Настройка формы
            //this.FormBorderStyle = FormBorderStyle.None; // Убираем рамку
            this.BackgroundImage = _image;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = _image.Size; // Размер формы = размеру изображения
            this.DoubleBuffered = true; // Уменьшаем мерцание

            // Обработчик клика
            this.MouseClick += (sender, e) =>
            {
                // Отправляем координаты относительно изображения
                float scaleX = (float)_image.Width / this.ClientSize.Width;
                float scaleY = (float)_image.Height / this.ClientSize.Height;
                Point unscaledPoint = new Point((int)(e.X * scaleX), (int)(e.Y * scaleY));
                ImageClicked?.Invoke(unscaledPoint);
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(_image, this.ClientRectangle);
        }
    }
}