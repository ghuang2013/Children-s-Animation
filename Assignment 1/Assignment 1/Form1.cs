using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Media;

namespace Assignment_1 {
    enum Time_of_day {
        DAYTIME, NIGHTTIME
    }

    public partial class MainForm : Form {
        private SpeechSynthesizer synth;
        private List<Car> cars;
        private Bunny bunny;
        private Rainbow rainbow;
        private Snow snow;
        private Traffic traffic;
        private Sun sun;
        private Moon moon;

        private int magnitude = 10;
        private float opacity = 1;
        private bool forward = true;
        private Time_of_day time = Time_of_day.DAYTIME;

        private SoundPlayer carSound;
        private SoundPlayer windSound;
        private SoundPlayer brake;
        Dictionary<Char, String> message;

        public MainForm() {
            InitializeComponent();
            message = new Dictionary<char, string>() {
                {'+',"increase screen opacity"},
                {'-',"decrease screen opacity"},
                {'1',"change color"},
                {'2',"change color"},
                {'3',"red light on"},
                {'4',"green light on"},
                {'5',"switch to night time"},
                {'6',"switch to day time"},
                {'7',"make snows"},
                {'8',"show rainbow"},
                {'9',"show bunny"},
                {'0',"make the bunny jump"}
            };

            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();

            cars = new List<Car>();
            bunny = new Bunny(new Point(20, 20));
            rainbow = new Rainbow(new Point(0, 0));
            snow = new Snow(new Point(0, 0));
            traffic = new Traffic();

            sun = new Sun(new Point(Size.Width - 250, 10));
            moon = new Moon(new Point(Size.Width - 250, 10));

            carSound = new SoundPlayer(@"carSounds.wav");
            windSound = new SoundPlayer(@"Wind.wav");
            brake = new SoundPlayer(@"car-brake.wav");
        }

        private void MainForm_Load(object sender, EventArgs e) {
            carSound.Play();
            cars.Add(
                    new Car(new Rectangle(100, 200, 400, 100),
                    Color.Indigo, Color.Blue, Color.Brown
                ));
            cars.Add(
                  new Car(new Rectangle(200, 500, 400, 100),
                  Color.Orange, Color.Aqua, Color.Brown
                  ));
        }

        private Color getRndColor() {
            Random rnd = new Random();
            return Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e) {
            char key = Char.ToLower(e.KeyChar);

            switch (key) {
                case '+':
                    if (Opacity < 1)
                        Opacity += 0.05;
                    break;
                case '-':
                    if (Opacity > 0.6)
                        Opacity -= 0.05;
                    break;
                case '1':
                    foreach (var car in cars) {
                        Color color = getRndColor();
                        car.changeColorOfCarBody(color);
                    }
                    break;
                case '2':
                    foreach (var car in cars) {
                        car.changeColorOfWheels(getRndColor());
                    }
                    break;
                case '3':
                    traffic.TrafficLight = Light.RED;
                    brake.Play();
                    break;
                case '4':
                    traffic.TrafficLight = Light.GREEN;
                    carSound.PlayLooping();
                    break;
                case '5':
                    time = Time_of_day.NIGHTTIME;
                    BackgroundImage = Image.FromFile("night.jpg");
                    BackgroundImageLayout = ImageLayout.Stretch;
                    break;
                case '6':
                    time = Time_of_day.DAYTIME;
                    BackgroundImage = Image.FromFile("background.png");
                    BackgroundImageLayout = ImageLayout.Stretch;
                    break;
                case '7':
                    if (snow.Show) {
                        snow.Show = false;
                        windSound.Stop();
                        carSound.PlayLooping();
                    } else {
                        snow.Show = true;
                        windSound.Play();
                    }
                    break;
                case '8':
                    rainbow.Show = rainbow.Show ? false : true;
                    break;
                case '9':
                    bunny.Show = bunny.Show ? false : true;
                    break;
                case '0':
                    bunny.MakeMeJump = bunny.MakeMeJump ? false : true;
                    break;
            }

            try {
                label1.Text = "You pressed " + key + ": " + message[key];
            } catch (KeyNotFoundException) {
                label1.Text = "Please try a different key :)";
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e) {
            Keys key = e.KeyCode;
            switch (key) {
                case Keys.Up:
                    if (forward) {
                        magnitude += 5;
                    } else {
                        magnitude -= 5;
                    }
                    break;
                case Keys.Down:
                    if (forward && magnitude > 5) {
                        magnitude -= 5;
                    } else if (!forward && magnitude < -5) {
                        magnitude += 5;
                    }
                    break;
                case Keys.Left:
                    forward = false;
                    magnitude = -10;
                    break;
                case Keys.Right:
                    forward = true;
                    magnitude = 10;
                    break;
            }
        }
        private void MainForm_Paint(object sender, PaintEventArgs e) {
            if (rainbow.Show) {
                rainbow.draw(e.Graphics, Size.Width / 2, Size.Height / 3, opacity);
            }

            if (time == Time_of_day.DAYTIME) {
                sun.draw(e.Graphics, 200, 200, opacity);
            } else {
                moon.draw(e.Graphics, 500, 500, opacity);
            }

            foreach (var car in cars) {
                car.draw(e.Graphics);
            }
            if (bunny.Show) {
                bunny.draw(e.Graphics, 150, 200);
            }
            traffic.draw(e.Graphics, traffic.TrafficLight);
            if (snow.Show) {
                snow.draw(e.Graphics, Size.Width, Size.Height);
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (traffic.TrafficLight == Light.GREEN) {
                foreach (var car in cars) {
                    car.move(magnitude);
                    if (forward) {
                        if (!check_left_boundary(car.Body.DefineShape.X)) {
                            car.move(-Size.Width);
                        }
                    } else {
                        if (!check_right_boundary(car.Body.DefineShape.X)) {
                            car.move(Size.Width);
                        }
                    }
                }
            }
            Invalidate();
        }

        private bool check_left_boundary(int x) {
            return x < Size.Width;
        }

        private bool check_right_boundary(int x) {
            return x > 0;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e) {
            if (bunny.MoveMe) {
                bunny.move(new Point(e.X, e.Y));
            }
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e) {
            if (bunny.Show) {
                bunny.MoveMe = bunny.MoveMe ? false : true;
            }
        }

        private void bunny_timer_Tick(object sender, EventArgs e) {
            if (bunny.MakeMeJump) {
                bunny.jump(200);
                Invalidate();
            }
        }

        private void snow_timer_Tick(object sender, EventArgs e) {
            if (snow.Show) {
                if (snow.Position.Y > Size.Height) {
                    snow.Position = new Point(snow.Position.X, 0);
                }
                snow.Position = new Point(snow.Position.X, snow.Position.Y + 10);
                Invalidate();
            }
        }

        private void shining_rainbow_Tick(object sender, EventArgs e) {
            Random rnd = new Random();
            opacity -= 0.1f;
            if (opacity < 0.3) {
                opacity = 1;
            }
        }
    }
}