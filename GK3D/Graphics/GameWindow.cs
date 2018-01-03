using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace GK3D.Graphics
{
    class Game : GameWindow
    {
        public Game() : base(512, 512, new GraphicsMode(32, 24, 0, 4),"Game")
        {
        }
    }
}
