using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK3_CLI_template
{
    internal class Triangle3D
    {
        private Vector3[] point;
        private Color[] color;
        private Vector3 position;
        private bool visibility;
     
        Random random = new Random();
        private PolygonMode polygonMode= PolygonMode.Line;
        public Triangle3D()
        {
            position = new Vector3();
            point=new Vector3[4];
            point[0] = new Vector3(-40, 10, 10);
            point[1] = new Vector3(-40, 10, 30);
            point[2] = new Vector3(-40, 30, 10);
            point[3] = new Vector3(-40, 30, 30);
            color = new Color[2];
            Random rand = new Random();
            for (int i = 0; i < 2; i++)
            {
                color[i] = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            }
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(position);
            GL.PolygonMode(MaterialFace.FrontAndBack, polygonMode);
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(color[0]);
            GL.Vertex3(point[0]);
            GL.Vertex3(point[1]);
            GL.Vertex3(point[2]);
            GL.Color3(color[1]);
            GL.Vertex3(point[1]);
            GL.Vertex3(point[2]);
            GL.Vertex3(point[3]);
            GL.End();
            GL.PopMatrix();
        }
        public void RandomizeColor()
        {
            for (int i = 0; i < 2; i++)
            {
                color[i] = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }
        }
        public void Translate(Vector3 direction)
        {
            position += direction;
        }
    }
}
