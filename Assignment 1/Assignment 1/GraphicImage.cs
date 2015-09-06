using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Assignment_1 {
    abstract class GraphicImage {
        public GraphicImage(Point p, string filename) {
            colorMatx = new ColorMatrix();
            attributes = new ImageAttributes();
            selfie = Image.FromFile(filename);
            position = p;
            show = false;
        }
        public bool Show {
            get { return show; }
            set { show = value; }
        }
        public Point Position {
            get { return position; }
            set { position = value; }
        }
        public void draw(Graphics g, int width, int height, float opacity) {
            colorMatx.Matrix33 = opacity;
            attributes.SetColorMatrix(colorMatx, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            g.DrawImage(
                selfie,
                new Rectangle(position.X, position.Y, width, height),
                0, 0, 1000, 1000,
                GraphicsUnit.Pixel, attributes
                ); 
        }
        public void draw(Graphics g, int width, int height) {
            g.DrawImage(
                selfie,
                position.X, position.Y, width, height
                );
        }

        private ImageAttributes attributes;
        private ColorMatrix colorMatx;
        protected Image selfie;
        protected Point position;
        protected bool show;
    }
}
