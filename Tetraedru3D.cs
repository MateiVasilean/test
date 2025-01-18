using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK3_CLI_template
{
    internal class Tetraedru3D
    {

        private Color[] color;
        private PolygonMode polygonMode = PolygonMode.Fill;
        private bool visible = true;
        private Random random = new Random();
        private Vector3[] vertices;
        private Vector3 position;


        public Tetraedru3D()
        {
            position = new Vector3();
            color = new Color[4];
            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                color[i] = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            }
        }

        public void TetraedruFromFile()
        {
            vertices = new Vector3[4];

            ReadVerticesFromFile("tetraedru.txt");
        }

        private void ReadVerticesFromFile(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                for (int i = 0; i < 4; i++)
                {
                    string[] parts = lines[i].Split(' ');
                    float x = float.Parse(parts[0]);
                    float y = float.Parse(parts[1]);
                    float z = float.Parse(parts[2]);

                    vertices[i] = new Vector3(x, y, z);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Eroare la citirea fisierului: " + ex.Message);
            }
        }

        public void Draw()
        {
            if (visible)
            {
                GL.PushMatrix();
                GL.Translate(position);
                GL.PolygonMode(MaterialFace.FrontAndBack, polygonMode);

                GL.Begin(PrimitiveType.Triangles);

                GL.Color3(color[0]);
                GL.Vertex3(vertices[0]);
                GL.Vertex3(vertices[1]);
                GL.Vertex3(vertices[2]);

                GL.Color3(color[1]);
                GL.Vertex3(vertices[0]);
                GL.Vertex3(vertices[2]);
                GL.Vertex3(vertices[3]);

                GL.Color3(color[2]);
                GL.Vertex3(vertices[0]);
                GL.Vertex3(vertices[3]);
                GL.Vertex3(vertices[1]);

                GL.Color3(color[3]);
                GL.Vertex3(vertices[1]);
                GL.Vertex3(vertices[3]);
                GL.Vertex3(vertices[2]);

                GL.End();

                GL.PopMatrix();
            }
        }

        public void TogglePolygonMode()
        {
            if (polygonMode == PolygonMode.Fill)
                polygonMode = PolygonMode.Line;
            else if (polygonMode == PolygonMode.Line)
                polygonMode = PolygonMode.Fill;
        }
        public void ToggleVisibility()
        {
            visible = !visible;
        }

        public Color GenerateColor()
        {

            int genR = random.Next(0, 256);
            int genG = random.Next(0, 256);
            int genB = random.Next(0, 256);

            Color col = Color.FromArgb(genR, genG, genB);

            return col;
        }
        public void Translate(Vector3 direction)
        {
            position += direction;
        }
    }
}
