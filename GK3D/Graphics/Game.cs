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

    public class Game : GameWindow
    {
        Scene scene;
        FrameManager frameManeger;

        int ibo_elements;
        Vector2 lastMousePos = new Vector2();
       // Matrix4 view;

        float time = 0.0f;
        public Game() : base(512, 512, new GraphicsMode(32, 24, 0, 4),"Game")
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            scene = new MainScene();
            frameManeger = new FrameManager();
            lastMousePos = new Vector2(Mouse.X, Mouse.Y);
           // GL.GenBuffers(1, out ibo_elements);

            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
        }
      
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);

            frameManeger.RenderFrame(scene);

            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            time += (float)e.Time;
            // Reset mouse position
            if (Focused)
            {
                Vector2 delta = lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                lastMousePos += delta;

                scene.ActiveCamera.AddRotation(delta.X, delta.Y);
                ResetCursor();
            }

           frameManeger.UpdateFrame(scene, ClientSize.Width / (float)ClientSize.Height);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            var camm = scene.ActiveCamera;
            base.OnKeyPress(e);
            switch (e.KeyChar)
            {
                case 'w':
                    camm.Move(0f, 0.1f, 0f);
                    break;
                case 'a':
                    camm.Move(-0.1f, 0f, 0f);
                    break;
                case 's':
                    camm.Move(0f, -0.1f, 0f);
                    break;
                case 'd':
                    camm.Move(0.1f, 0f, 0f);
                    break;
                case 'q':
                    camm.Move(0f, 0f, 0.1f);
                    break;
                case 'e':
                    camm.Move(0f, 0f, -0.1f);
                    break;
            }
        }
        void ResetCursor()
        {
            lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
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
