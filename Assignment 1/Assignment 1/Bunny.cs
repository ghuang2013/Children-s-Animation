using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assignment_1 {
    class Bunny : GraphicImage {
        public Bunny(Point p)
            : base(p, "rabbit.png") {
            moveMe = true;
            jumpUp = true;
            makeMeJump = false;
        }
        public void move(Point p) {
            this.position = p;
        }
        public bool MoveMe {
            get { return moveMe; }
            set { moveMe = value; }
        }
        public bool MakeMeJump {
            get { return makeMeJump; }
            set { makeMeJump = value; }
        }
        public void jump(int height) {
            if (jumpUp) {
                this.position.Y += height;
                jumpUp = false;
            } else {
                this.position.Y -= height;
                jumpUp = true;
            }
        }
        private bool makeMeJump;
        private bool moveMe;
        private bool jumpUp;
    }
}
