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

namespace GK3D.Graphics
{
    //buid 86x!!! - avoid exception test
    public class Game : GameWindow
    {
        public SceneController SceneController { get; private set; }

        private Scene _scene;
        private FrameManager _frameManeger;
        private Vector2 _lastMousePos;
        private float _time = 0.0f;
        private bool isMouseDown;

        public Game() : base(512, 512, new GraphicsMode(32, 24, 0, 4), "Game")
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //_scene = new MainScene();
            var testScene = new TestScene();
            _scene = testScene;
            _scene.Load();
            SceneController = new TestSceneController(testScene);
            _frameManeger = new FrameManager();
            _frameManeger.Scene = _scene;
            _lastMousePos = new Vector2(Mouse.X, Mouse.Y);


            Mouse.ButtonUp += (s, ee) => isMouseDown = false;
            Mouse.ButtonDown += (s, ee) =>
            {
                ResetCursor();
                isMouseDown = true;
            };

            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);

            _frameManeger.RenderFrame();

            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _time += (float)e.Time;
            _scene.Process((float)e.Time);
            UpdateActiveCamera();
            _frameManeger.UpdateFrame(ClientSize.Width / (float)ClientSize.Height);
        }
        private void UpdateActiveCamera()
        {
            if (Focused && isMouseDown)
            {
                Vector2 delta = _lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                _lastMousePos += delta;

                _scene.ActiveCamera.AddRotation(delta.X, delta.Y);
                ResetCursor();
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            var camm = _scene.ActiveCamera;
            base.OnKeyPress(e);
            switch (e.KeyChar)
            {
                case 'w':
                    camm.MoveWithSeparatedY(0f, 0f, 0.1f);
                    break;
                case 'a':
                    camm.MoveWithSeparatedY(0.1f, 0f, 0f);
                    break;
                case 's':
                    camm.MoveWithSeparatedY(0f, 0f, -0.1f);
                    break;
                case 'd':
                    camm.MoveWithSeparatedY(-0.1f, 0f, 0f);
                    break;
                case 'q':
                    camm.MoveWithSeparatedY(0f, 0.1f, 0f);
                    break;
                case 'e':
                    camm.MoveWithSeparatedY(0f, -0.1f, 0f);
                    break;
            }
        }
        void ResetCursor()
        {
            _lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }
        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);

            if (Focused)
            {
                ResetCursor();
            }
        }
    }
}
