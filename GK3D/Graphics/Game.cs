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

namespace GK3D.Graphics
{
    public class Game : GameWindow
    {
        public SceneController SceneController { get; private set; }

        private FrameManager _frameManeger;
        private Vector2 _lastMousePos;
        private bool isMouseDown;

        public Game() : base(512, 512, new GraphicsMode(32, 24, 0, 4), "Game")
        {
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //_scene = new MainScene();
            SceneController = new TestSceneController(new TestSceneLoader(), new TestSceneScenario());
             _frameManeger = new FrameManager();
             _frameManeger.Collection = SceneController.Collection;
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

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _frameManeger.RenderFrame(ClientSize.Width / (float)ClientSize.Height);

            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            SceneController.Scenario.Process((float)e.Time);
            UpdateActiveCamera();
        }
        private void UpdateActiveCamera()
        {
            if (Focused && isMouseDown)
            {
                Vector2 delta = _lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                _lastMousePos += delta;

                SceneController.Collection.ActiveCamera.AddRotation(delta.X, delta.Y);
                ResetCursor();
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            var camm = SceneController.Collection.ActiveCamera;
            base.OnKeyPress(e);
            switch (e.KeyChar)
            {
                case 'w': camm.MoveWithSeparatedY(0f, 0f, 0.1f); break;
                case 'a': camm.MoveWithSeparatedY(0.1f, 0f, 0f); break;
                case 's': camm.MoveWithSeparatedY(0f, 0f, -0.1f); break;
                case 'd': camm.MoveWithSeparatedY(-0.1f, 0f, 0f); break;
                case 'q': camm.MoveWithSeparatedY(0f, 0.1f, 0f); break;
                case 'e': camm.MoveWithSeparatedY(0f, -0.1f, 0f); break;
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
