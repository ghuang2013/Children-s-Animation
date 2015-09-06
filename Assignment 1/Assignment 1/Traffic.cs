using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1 {

    public enum Light { 
        RED, GREEN 
    };

    class Traffic {
        public Traffic() {
            trafficLight = Light.GREEN;
            topLight = new SolidBrush(Color.Gray);
            buttomLight = new SolidBrush(Color.Gray);
            back = new SolidBrush(Color.Black);
        }

        public void draw(Graphics g, Light color) {
            if (color == Light.GREEN) {
                topLight = new SolidBrush(Color.Green);
                buttomLight = new SolidBrush(Color.Gray);
            } else {
                topLight = new SolidBrush(Color.Gray);
                buttomLight = new SolidBrush(Color.Red);
            }
            g.FillRectangle(back, new Rectangle(40, 10, 70, 130));
            g.FillRectangle(back, new Rectangle(70, 130, 10, 200));
            g.FillEllipse(topLight, new Rectangle(50, 20, 50, 50));
            g.FillEllipse(buttomLight, new Rectangle(50, 80, 50, 50));
        }

        public Light TrafficLight {
            get {
                return trafficLight;
            }
            set {
                trafficLight = value;
            }
        }

        private Brush topLight;
        private Brush buttomLight;
        private Brush back;
        private Light trafficLight;
    }
}
