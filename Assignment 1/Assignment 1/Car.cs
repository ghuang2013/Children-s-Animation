using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;

namespace Assignment_1 {
    class Car {
        public Car(Rectangle carPosition, Color bodyColor, Color windowColor, Color wheelColor) {
            carLeftWindow = new Window(
                windowColor,
                new Point(carPosition.X + 100, carPosition.Y - 50), 
                new Point(carPosition.X + 50, carPosition.Y - 5),
                new Point(carPosition.X + 200, carPosition.Y - 5),
                new Point(carPosition.X + 230, carPosition.Y - 50)
                );
            leftWheel = new Wheel(
                wheelColor,
                new Rectangle(carPosition.X + 50, carPosition.Y + 80, 50, 50)
                );
            rightWheel = new Wheel(
                wheelColor,
                new Rectangle(carPosition.X + 300, carPosition.Y + 80, 50, 50)
                );
            body = new CarBody(
                bodyColor,
                carPosition 
                );
            carRightWindow = new Window(
                windowColor,
                 new Point(carPosition.X + 200, carPosition.Y - 5),
                 new Point(carPosition.X + 230, carPosition.Y - 50),
                 new Point(carPosition.X + 300, carPosition.Y - 50),
                 new Point(carPosition.X + 350, carPosition.Y - 5)
                );
        }
        public void draw(Graphics g) {
            body.draw(g);
            leftWheel.draw(g);
            rightWheel.draw(g);
            carLeftWindow.draw(g);
            carRightWindow.draw(g);
        }
        public void move(int intensity) {
            body.move(intensity);
            leftWheel.move(intensity);
            rightWheel.move(intensity);
            carLeftWindow.move(intensity);
            carRightWindow.move(intensity);
        }
        public void changeColorOfCarBody(Color bodyColor) {
            body.Fill_Color = bodyColor;
        }
        public void changeColorOfWheels(Color wheelColor) {
            leftWheel.Fill_Color = wheelColor;
            rightWheel.Fill_Color = wheelColor;
        }
        public CarBody Body{
            get { return body; }
        }
        private Window carLeftWindow;
        private Window carRightWindow;
        private Wheel leftWheel;
        private Wheel rightWheel;
        private CarBody body;
    }

    class Window:CarParts {
        public Window(Color windowColor, params Point[] shape):base(windowColor) {
            fill_with_paint = new SolidBrush(windowColor);
            plotPoints = new List<Point>(shape);
        }
        public override void draw(Graphics g) {
            g.DrawPolygon(border, plotPoints.ToArray());
            g.FillPolygon(fill_with_paint, plotPoints.ToArray());
        }
        public override void move(int intensity) {
            for (int i = 0; i < plotPoints.Count(); ++i) {
                plotPoints[i] = new Point(
                    plotPoints[i].X + intensity,
                    plotPoints[i].Y
                    );
            }
        }
        private List<Point> plotPoints;
    }

    class Wheel:CarParts {
        public Wheel(Color wheelColor, Rectangle shape):base(wheelColor) {
            fill_color = wheelColor;
            defineShape = shape;
            pointList = new List<PointF>();
            degree = 0.0;
        }
        public override void draw(Graphics g) {
            fill_with_paint = new SolidBrush(fill_color);
            g.DrawEllipse(border, defineShape);
            pointList.Clear();
            double radius = defineShape.Width / 2;
            for (int i = 0; i < 10; ++i) {
                degree += (4.0 * Math.PI) / 10;

                PointF point = new PointF(
                    (float)(radius * Math.Cos(degree) + defineShape.X + radius),
                    (float)(radius * Math.Sin(degree) + defineShape.Y + radius)
                    );

                pointList.Add(point);
            }
            g.FillPolygon(fill_with_paint, pointList.ToArray(), FillMode.Winding);
        }
        public override void move(int intensity) {
            defineShape.X += intensity;
            degree += Math.PI / intensity;
        }
        private double degree;
        private Rectangle defineShape;
        private List<PointF> pointList;
    }

    class CarBody:CarParts {
        public CarBody(Color bodyColor, Rectangle shape):base(bodyColor) {
            fill_color = bodyColor;
            defineShape = shape;
        }
        public override void draw(Graphics g) {
            fill_with_paint = new SolidBrush(fill_color);
            g.DrawRectangle(border, defineShape);
            g.FillRectangle(fill_with_paint, defineShape);
        }
        public override void move(int intensity) {
            defineShape.X += intensity;
        }
        public Rectangle DefineShape{
            get { return defineShape; }
        }
        private Rectangle defineShape;
    }

    abstract class CarParts {
        public CarParts(Color color) {
            border = new Pen(borderColor);
            border.Width = width;
            fill_color = color;
        }

        public abstract void draw(Graphics g);
        public abstract void move(int intensity);

        public Color Fill_Color {
            get { return fill_color; }
            set { fill_color = value; }
        }

        protected Brush fill_with_paint;
        protected Color fill_color;
        protected Pen border;

        private Color borderColor = Color.AliceBlue;
        private const int width = 10;
    }
}
