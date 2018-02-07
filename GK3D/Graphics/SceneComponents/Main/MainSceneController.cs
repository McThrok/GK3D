using GK3D.Graphics.SceneComponents.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;
using GK3D.Graphics.Common;
using GK3D.Graphics.Objects.Renderable;

namespace GK3D.Graphics.SceneComponents.Main
{
    public class MainSceneController : SceneController
    {
        private Vector2 _lastMousePos;
        private bool _focused = true;
        private bool isMouseDown;
        public MainSceneController(SceneLoader loader = null) : base(loader)
        { }

        public override void ChangeCamera()
        {
            if (Collection.ActiveCamera != null)
            {
                var cameraList = Collection.SceneObjects.GetCamerasWiThGlobalModelMatrices();
                var activeCam = cameraList.SingleOrDefault(x => x.Object == Collection.ActiveCamera.Object);
                if (activeCam != null)
                {
                    Collection.ActiveCamera = cameraList[(cameraList.IndexOf(activeCam) + 1) % cameraList.Count];
                }
            }
        }
        public override void ChangeShading()
        {
            switch (Collection.ActiveShader)
            {
                case "phong_phong":
                    Collection.ActiveShader = "gouraud_phong";
                    break;

                case "gouraud_phong":
                    Collection.ActiveShader = "phong_phong";
                    break;

                case "phong_blinn":
                    Collection.ActiveShader = "gouraud_blinn";
                    break;

                case "gouraud_blinn":
                    Collection.ActiveShader = "phong_blinn";
                    break;
            }
        }
        public override void ChangeLighting()
        {
            switch (Collection.ActiveShader)
            {
                case "phong_phong":
                    Collection.ActiveShader = "phong_blinn";
                    break;

                case "gouraud_phong":
                    Collection.ActiveShader = "gouraud_blinn";
                    break;

                case "phong_blinn":
                    Collection.ActiveShader = "phong_phong";
                    break;

                case "gouraud_blinn":
                    Collection.ActiveShader = "gouraud_phong";
                    break;
            }

        }

        public override void HandleInput(KeyboardState keyboardState, MouseState mouseState)
        {
            HandleKeyboard(keyboardState);
            HandleMouse(mouseState);
        }

        private void HandleMouse(MouseState mouseState)
        {
            if (isMouseDown)
            {
                var camm = Collection.ActiveCamera.Object;

                Vector2 delta = _lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                _lastMousePos += delta;

                if (camm.Name == "StaticCamera")
                    camm.AddRotation(delta.X, delta.Y);
            }
            if (_focused && mouseState.IsButtonDown(MouseButton.Left))
            {
                isMouseDown = true;
                _lastMousePos = new Vector2(mouseState.X, mouseState.Y);
            }
            if (mouseState.IsButtonUp(MouseButton.Left))
            {
                isMouseDown = false;
            }
        }
        public override void HandleFocusChange(bool focused)
        {
            _focused = focused;
            if (!_focused)
                isMouseDown = false;
        }

        private void HandleKeyboard(KeyboardState keyboardState)
        {
            var car = Collection.SceneObjects.GetComplexObjectsWiThGlobalModelMatrices().FirstOrDefault(x => x.Object.Name == "Car");
            if (car != null)
            {
                float moved = 0;
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
                    car.Object.Rotation += new Vector3(0, -0.05f * moved, 0);

                if (keyboardState.IsKeyDown(Key.A))
                    car.Object.Rotation += new Vector3(0, 0.05f * moved, 0);
            }
        }

        private void MoveCar(CollectionItem<ComplexObject> car, float distance)
        {
            var direction = (-Vector3.UnitZ).ApplyOnVector(car.GlobalModelMatrix);
            direction.NormalizeFast();

            car.Object.Position += direction * distance;
        }

    }
}
