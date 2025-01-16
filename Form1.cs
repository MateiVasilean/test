using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using OpenTK3_StandardTemplate_WinForms.helpers;
using OpenTK3_StandardTemplate_WinForms.objects;

namespace OpenTK3_StandardTemplate_WinForms
{
    public partial class MainForm : Form
    {
        private Axes mainAxis;
        private Camera cam;
        private Scene scene;

        private Point mousePosition;
        private Color4 originalColor;
        private Color4 currentColor;
        private bool lineStatus = false;
        private OpenTK.Vector3 lightPosition = new OpenTK.Vector3(-20, 22, 15);

        public MainForm()
        {   
            // general init
            InitializeComponent();

            originalColor = ColorRandomizer();
            currentColor = originalColor;

            // init VIEWPORT
            scene = new Scene();

            scene.GetViewport().Load += new EventHandler(this.mainViewport_Load);
            scene.GetViewport().Paint += new PaintEventHandler(this.mainViewport_Paint);
            scene.GetViewport().MouseMove += new MouseEventHandler(this.mainViewport_MouseMove);

            this.Controls.Add(scene.GetViewport());
            Text = "Vasilean Matei, 3113B";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // init RNG
            Randomizer.Init();

            // init CAMERA/EYE
            cam = new Camera(scene.GetViewport());

            // init AXES
            mainAxis = new Axes(showAxes.Checked);
        }

        private void showAxes_CheckedChanged(object sender, EventArgs e)
        {
            mainAxis.SetVisibility(showAxes.Checked);

            scene.Invalidate();
        }

        private void changeBackground_Click(object sender, EventArgs e)
        {
            GL.ClearColor(Randomizer.GetRandomColor());

            scene.Invalidate();
        }

        private void resetScene_Click(object sender, EventArgs e)
        {
            showAxes.Checked = true;
            mainAxis.SetVisibility(showAxes.Checked);
            scene.Reset();
            cam.Reset();

            lightPosition = new OpenTK.Vector3(-20, 22, 15);
            currentColor = originalColor;

            iluminationColorCheckBox.Checked = false;

            trackBarX.Value = -20;
            trackBarY.Value = 22;
            trackBarZ.Value = 15;

            scene.Invalidate();
        }

        private void mainViewport_Load(object sender, EventArgs e)
        {
            scene.Reset();
        }

        private void mainViewport_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = new Point(e.X, e.Y);
            scene.Invalidate();
        }

        private void mainViewport_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            cam.SetView();

            if (enableRotation.Checked == true)
            {
                // Doar după axa Ox.
                GL.Rotate(Math.Max(mousePosition.X, mousePosition.Y), 1, 1, 1);
            }

            // GRAPHICS PAYLOAD
            mainAxis.Draw();

            if (enableObjectRotation.Checked == true)
            {
                // Rotatie a obiectului
                GL.PushMatrix();
                GL.Rotate(Math.Max(mousePosition.X, mousePosition.Y), 1, 1, 1);
                DrawWireframe();
                GL.PopMatrix();
            } else
            {
                DrawWireframe();
            }

            if (illuminationCheckBox.Checked == true)
            {
                DrawLightSource();
            }

            if (lineStatus) DrawFromLightSourceToOrigin();

            scene.GetViewport().SwapBuffers();
        }

        private void DrawWireframe()
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(Color.White);

            OpenTK.Vector3[] vertices = new OpenTK.Vector3[]
            {
                new OpenTK.Vector3(-5, 5, 5),
                new OpenTK.Vector3(-5, 5, 25),
                new OpenTK.Vector3(-35, 5, 25),
                new OpenTK.Vector3(-35, 5, 5),
                new OpenTK.Vector3(-20, 40, 15)
            };

            GL.Vertex3(vertices[0]); GL.Vertex3(vertices[1]);
            GL.Vertex3(vertices[1]); GL.Vertex3(vertices[2]);
            GL.Vertex3(vertices[2]); GL.Vertex3(vertices[3]);
            GL.Vertex3(vertices[3]); GL.Vertex3(vertices[0]);

            GL.Vertex3(vertices[0]); GL.Vertex3(vertices[4]);
            GL.Vertex3(vertices[1]); GL.Vertex3(vertices[4]);
            GL.Vertex3(vertices[2]); GL.Vertex3(vertices[4]);
            GL.Vertex3(vertices[3]); GL.Vertex3(vertices[4]);

            GL.End();
        }

        private void DrawLightSource()
        {
            if (iluminationColorCheckBox.Checked == true)
            {
                currentColor = ColorRandomizer();
            }

            float[] lightPositionArray = { lightPosition.X, lightPosition.Y, lightPosition.Z, 1.0f };
            float[] lightColorArray = { currentColor.R, currentColor.G, currentColor.B, currentColor.A };

            if (illuminationCheckBox.Checked)
            {
                GL.Enable(EnableCap.Lighting);
                GL.Enable(EnableCap.Light0);
                GL.Light(LightName.Light0, LightParameter.Position, lightPositionArray);
                GL.Light(LightName.Light0, LightParameter.Diffuse, lightColorArray);
                GL.Light(LightName.Light0, LightParameter.Ambient, lightColorArray);
            }
            else
            {
                GL.Disable(EnableCap.Lighting);
            }

            GL.Color4(currentColor);
            GL.PointSize(15);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(lightPosition);
            GL.End();
        }

        private Color4 ColorRandomizer()
        {
            Random rand = new Random();
            return new Color4(
                (float)rand.NextDouble(),
                (float)rand.NextDouble(),
                (float)rand.NextDouble(),
                1.0f
            );
        }

        private void DrawFromLightSourceToOrigin()
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(Color.Black);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(lightPosition);
            GL.End();
        }

        private void illuminationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            scene.Invalidate();
        }

        private void iluminationColorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            scene.Invalidate();
        }

        private void lineONRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (lineONRadioButton.Checked) {
                lineStatus = true;
                scene.Invalidate();
            }
        }

        private void lineOFFRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (lineOFFRadioButton.Checked)
            {
                lineStatus = false;
                scene.Invalidate();
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            float X = trackBarX.Value;
            float Y = trackBarY.Value;
            float Z = trackBarZ.Value;

            UpdateLightPosition(X, Y, Z);

            currentColor = ColorRandomizer();

            scene.Invalidate();
        }

        private void UpdateLightPosition(float X, float Y, float Z)
        {
            lightPosition = new OpenTK.Vector3(X, Y, Z);
        }
    }
}
