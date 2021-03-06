﻿using System.Windows.Forms;
using System;
using Graphics3D.Geometry;

namespace Graphics3D
{
    public partial class Form1 : Form
    {
        private Mesh CurrentMesh
        {
            get
            {
                return sceneView4.Mesh;
            }
            set
            {
                sceneView1.Mesh = value;
                sceneView2.Mesh = value;
                sceneView3.Mesh = value;
                sceneView4.Mesh = value;
                RefreshScenes();
            }
        }

        private Camera camera;

        public Form1()
        {
            InitializeComponent();
            CurrentMesh = new Tetrahedron(0.5);
            sceneView1.ViewCamera = new Camera(new Vertex(0, 0, 0), 0, 0,
                Transformations.OrthogonalProjection());//XOY
            sceneView2.ViewCamera = new Camera(new Vertex(0, 0, 0), 0, 0,
                Transformations.RotateY(-Math.PI / 2)
                * Transformations.OrthogonalProjection());//YOZ
            sceneView3.ViewCamera = new Camera(new Vertex(0, 0, 0), 0, 0,
                Transformations.RotateX(Math.PI / 2)
                * Transformations.OrthogonalProjection());//XOZ
            camera = new Camera(new Vertex(0, 0, 0), Math.PI / 4, -Math.PI / 4,
                        Transformations.OrthogonalProjection());//Изометрическая
            sceneView4.ViewCamera = camera;
        }

        private static double DegToRad(double deg)
        {
            return deg / 180 * Math.PI;
        }

        private void RefreshScenes()
        {
            sceneView1.Refresh();
            sceneView2.Refresh();
            sceneView3.Refresh();
            sceneView4.Refresh();
        }

        private void Scale(object sender, EventArgs e)
        {
            double scalingX = (double)numericUpDown1.Value;
            double scalingY = (double)numericUpDown2.Value;
            double scalingZ = (double)numericUpDown3.Value;
            CurrentMesh.Apply(Transformations.Scale(scalingX, scalingY, scalingZ));
            RefreshScenes();
        }

        private void Rotate(object sender, EventArgs e)
        {
            double rotatingX = DegToRad((double)numericUpDown4.Value);
            double rotatingY = DegToRad((double)numericUpDown5.Value);
            double rotatingZ = DegToRad((double)numericUpDown6.Value);
            CurrentMesh.Apply(Transformations.RotateX(rotatingX)
                * Transformations.RotateY(rotatingY)
                * Transformations.RotateZ(rotatingZ));
            RefreshScenes();
        }

        private void Translate(object sender, EventArgs e)
        {
            double translatingX = (double)numericUpDown7.Value;
            double translatingY = (double)numericUpDown8.Value;
            double translatingZ = (double)numericUpDown9.Value;
            CurrentMesh.Apply(Transformations.Translate(translatingX, translatingY, translatingZ));
            RefreshScenes();
        }

        private void Reflect(object sender, EventArgs e)
        {
            Matrix reflection;
            if (radioButton1.Checked)
                reflection = Transformations.ReflectX();
            else if (radioButton2.Checked)
                reflection = Transformations.ReflectY();
            else if (radioButton3.Checked)
                reflection = Transformations.ReflectZ();
            else throw new Exception("Unreachable statement");
            CurrentMesh.Apply(reflection);
            RefreshScenes();
        }

        private void RotateAroundCenter(object sender, EventArgs e)
        {
            double angleX = DegToRad((double)numericUpDown10.Value);
            double angleY = DegToRad((double)numericUpDown11.Value);
            double angleZ = DegToRad((double)numericUpDown12.Value);
            var p = CurrentMesh.Center;
            CurrentMesh.Apply(Transformations.RotateAroundPoint(p, angleX, angleY, angleZ));
            RefreshScenes();
        }

        private void RotateAroundLine(object sender, EventArgs e)
        {
            Vertex a = new Vertex(
                (double)numericUpDownPoint1X.Value,
                (double)numericUpDownPoint1Y.Value,
                (double)numericUpDownPoint1Z.Value);
            Vertex b = new Vertex(
                (double)numericUpDownPoint2X.Value,
                (double)numericUpDownPoint2Y.Value,
                (double)numericUpDownPoint2Z.Value);
            var angle = DegToRad((double)numericUpDownAngle.Value);
            CurrentMesh.Apply(Transformations.RotateAroundLine(a, b, angle));
            RefreshScenes();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            double delta = 0.3;
			double kTranslate = 0.1;
            switch (keyData)
            {
                case Keys.Oemplus: camera.Position *= Transformations.Translate(kTranslate * camera.Forward); break;
                case Keys.OemMinus: camera.Position *= Transformations.Translate(kTranslate * camera.Backward); break;
                case Keys.S: camera.Position *= Transformations.Translate(kTranslate * camera.Up); break;
                case Keys.D: camera.Position *= Transformations.Translate(kTranslate * camera.Left); break;
                case Keys.W: camera.Position *= Transformations.Translate(kTranslate * camera.Down); break;
                case Keys.A: camera.Position *= Transformations.Translate(kTranslate * camera.Right); break;
                case Keys.Left: camera.AngleY -= delta; break;
                case Keys.Right: camera.AngleY += delta; break;
                case Keys.Up: camera.AngleX += delta; break;
                case Keys.Down: camera.AngleX -= delta; break;
            }
            RefreshScenes();
            return base.ProcessCmdKey(ref msg, keyData);
        }
    

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rB = (RadioButton)sender;
            if(rB.Checked && rB == radioButtonIzPr)
            {
                radioButtonPerPr.Checked = false;
                camera = new Camera(new Vertex(0, 0, 0), Math.PI / 4, -Math.PI / 4,
                        Transformations.OrthogonalProjection());
                sceneView4.ViewCamera = camera;
                RefreshScenes();
            }
            if (rB.Checked && rB == radioButtonPerPr)
            {
                radioButtonIzPr.Checked = false;
                Matrix projection = Transformations.PerspectiveProjection(-0.1, 0.1, -0.1, 0.1, 0.1, 20);
                camera = new Camera(new Vertex(1, 1, 1), Math.PI / 4, -Math.PI / 4, projection);
                sceneView4.ViewCamera = camera;
                RefreshScenes();
            }
        }

        private void radioButtonPolyh_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rB = (RadioButton)sender;
            if (rB.Checked && rB == radioButtonTetrah)
            {
                radioButtonIcosah.Checked = false;
                CurrentMesh = new Tetrahedron(0.5);
                RefreshScenes();
            }
            if (rB.Checked && rB == radioButtonIcosah)
            {
                radioButtonTetrah.Checked = false;
                CurrentMesh = new Icosahedron(0.5);
                RefreshScenes();
            }
        }
    }
}
