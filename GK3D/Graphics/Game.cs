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
            SceneController.Scenario.Process(SceneController.Collection.SceneObjects, (float)e.Time);
            UpdateActiveCamera();
            UpdateCarPosition();
        }

        private void UpdateCarPosition()
        {
            var car = SceneController.Collection.SceneObjects.GetComplexObjectsWiThGlobalModelMatrices().FirstOrDefault(x => x.Object.Name == "Car");
            if (car != null)
            {
                float moved = 0;
                var keyboardState = OpenTK.Input.Keyboard.GetState();
                if (keyboardState.IsKeyDown(Key.W))
                {
                    MoveCar(car, 0.1f);
                    moved = 1;
                }
                if (keyboardState.IsKeyDown(Key.S))
                {
                    MoveCar(car, -0.03f);
                    moved = -1;
                }

                if (keyboardState.IsKeyDown(Key.D))
                    car.Object.Rotation += new Vector3(0, -0.03f * moved, 0);

                if (keyboardState.IsKeyDown(Key.A))
                    car.Object.Rotation += new Vector3(0, 0.03f * moved, 0);
            }
        }

        private void MoveCar(CollectionItem<ComplexObject> car, float distance)
        {
            var direction = (-Vector3.UnitZ).ApplyOnVector(car.GlobalModelMatrix);
            direction.NormalizeFast();

            car.Object.Position += direction * distance;
        }

        private void UpdateActiveCamera()
        {
            var camm = SceneController.Collection.ActiveCamera.Object;

            if (Focused && isMouseDown)
            {
                Vector2 delta = _lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                _lastMousePos += delta;

                if (camm.Name == "StaticCamera")
                    camm.AddRotation(delta.X, delta.Y);
                ResetCursor();
            }

            // var keyboardState = OpenTK.Input.Keyboard.GetState();
            //if (camm.Name == "StaticCamera")
            //{
            //    if (keyboardState.IsKeyDown(Key.W))
            //        camm.MoveWithSeparatedY(0f, 0f, 0.1f);
            //    if (keyboardState.IsKeyDown(Key.A))
            //        camm.MoveWithSeparatedY(0.1f, 0f, 0f);
            //    if (keyboardState.IsKeyDown(Key.S))
            //        camm.MoveWithSeparatedY(0f, 0f, -0.1f);
            //    if (keyboardState.IsKeyDown(Key.D))
            //        camm.MoveWithSeparatedY(-0.1f, 0f, 0f);
            //    if (keyboardState.IsKeyDown(Key.Q))
            //        camm.MoveWithSeparatedY(0f, 0.1f, 0f);
            //    if (keyboardState.IsKeyDown(Key.E))
            //        camm.MoveWithSeparatedY(0f, -0.1f, 0f);
            //}

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
