using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.IO;
using System.Drawing;
using OpenTK.Input;
using System.Drawing.Imaging;
using GK3D.Graphics.Objects;
using GK3D.Graphics.SceneComponents;
using GK3D.Graphics.SceneComponents.Test;
using GK3D.Graphics.SceneComponents.Base;
using GK3D.Graphics.Common;
using GK3D.Graphics.SceneComponents.Main;
using GK3D.Graphics.Objects.Renderable;

namespace GK3D.Graphics
{
    public class Game : GameWindow
    {
        public SceneController SceneController { get; private set; }
        public SceneScenario SceneScenario { get; private set; }

        private FrameManager _frameManeger;

        public Game() : base(512, 512, new GraphicsMode(32, 24, 0, 4), "Game")
        {
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SceneController = new MainSceneController(new MainSceneLoader());
            SceneScenario = new MainSceneScenario();
            _frameManeger = new FrameManager();
            _frameManeger.Collection = SceneController.Collection;

            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _frameManeger.RenderFrame(ClientSize.Width / (float)ClientSize.Height);

            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            SceneScenario.Process(SceneController.Collection.SceneObjects, (float)e.Time);
            SceneController.HandleInput(Keyboard.GetState(), Mouse.GetState());
        }
        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);
            SceneController.HandleFocusChange(Focused);
        }
    }
}
