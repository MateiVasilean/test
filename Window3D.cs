using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System;
using System.Drawing;

namespace OpenTK3_CLI_template {
    internal class Window3D : GameWindow {
        private KeyboardState previousKeyboard;
        private MouseState previousMouse;

        private readonly Axes ax;
        private Camera cam;
        private Tetraedru3D piramida;
        private Triangle3D tri;
        private readonly Color DEFAULT_BKG_COLOR = Color.FromArgb(49, 50, 51);

        public Window3D() : base(800, 800, new GraphicsMode(32, 24, 0, 8)) {
            VSync = VSyncMode.On;
            Title = "Vasilean Matei - 3133B";
            // inits
            ax = new Axes();
            piramida = new Tetraedru3D();
            tri = new Triangle3D();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
                       
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            // set background
            GL.ClearColor(DEFAULT_BKG_COLOR);

            // set viewport
            GL.Viewport(0, 0, this.Width, this.Height);

            // set perspective
            Matrix4 perspectiva = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)this.Width / (float)this.Height, 1, 1024);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiva);

            // set the eye
            cam = new Camera();
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);

            // LOGIC CODE
            KeyboardState currentKeyboard = Keyboard.GetState();
            MouseState currentMouse = Mouse.GetState();

            if (currentKeyboard[Key.Escape]) {
                Exit();
            }

            if (currentMouse[MouseButton.Right] && !previousMouse[MouseButton.Right])
            {
                piramida.ToggleVisibility();
            }

            if (currentKeyboard[Key.H] && !previousKeyboard[Key.H])
            {
                DisplayHelp();
            }
            if (currentKeyboard[Key.F] && !previousKeyboard[Key.F])
            {
                piramida.TogglePolygonMode();
            }

            if (currentKeyboard[Key.B] && !previousKeyboard[Key.B])
            {
                GL.ClearColor(piramida.GenerateColor());
            }
            if (currentKeyboard[Key.W] && previousKeyboard[Key.S])
                tri.RandomizeColor();

            if (currentKeyboard[Key.W])
            {
                piramida.Translate(-Vector3.UnitZ);
                tri.Translate(Vector3.UnitY);
            }
            if (currentKeyboard[Key.S])
            {
                piramida.Translate(Vector3.UnitZ);
                tri.Translate(-Vector3.UnitY);
            }
            if (currentKeyboard[Key.A])
            {
                piramida.Translate(-Vector3.UnitX);
                tri.Translate(Vector3.UnitY);
            }
            if (currentKeyboard[Key.D])
            {
                piramida.Translate(Vector3.UnitX);
                tri.Translate(-Vector3.UnitY);
            }
            previousKeyboard = currentKeyboard;
            previousMouse = currentMouse;
            // END logic code

        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            // RENDER CODE
            ax.Draw();

            piramida.TetraedruFromFile();
            piramida.Draw();
            
            //tri.Draw();

            // END render code

            SwapBuffers();
        }
        private void DisplayHelp()
        {
            Console.WriteLine("\nMENU");
            Console.WriteLine("H - meniu ajutor");
            Console.WriteLine("ESC - parasire aplicatie");
            Console.WriteLine("W - Deplasare triunghuri in sus");
            Console.WriteLine("S - Deplasare triunghuri in jos");
            Console.WriteLine("W+S - Schimbare culoare triunghuri aleatorie");
            Console.WriteLine("B - schimbare culoare de fundal");
            Console.WriteLine("ClickDreapta - afiseaza/ascunde triunghiurile");
            Console.WriteLine("F - Afisare piramida in mod contur");
        }

    }
}
