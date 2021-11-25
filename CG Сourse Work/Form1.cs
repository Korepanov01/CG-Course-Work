using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CG_Сourse_Work
{
    public sealed partial class Form1 : Form
    {
        private const string ModelFileName = "cat.obj";

        private readonly List<Vector[]> _originalModelPolygons = new List<Vector[]>();
        private List<Vector[]> _transformedModelPolygons = new List<Vector[]>();

        private Matrix _transformationMatrix;

        private int _rotationAngle;

        public Form1()
        {
            InitializeComponent();

            Width = 1200;
            Height = 800;
            BackColor = Color.AntiqueWhite;

            ReadModelPolygons();
            FillTransformMatrix();

            KeyDown += OnKeyDown;
            Paint += OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            TransformModel();
            DrawModel(e.Graphics);
        }

        private void TransformModel()
        {
            var rotationMatrix = Matrix.CreateYRotationMatrix(_rotationAngle);

            _transformedModelPolygons = _originalModelPolygons.Select(vectors => new[]
                {
                    _transformationMatrix * rotationMatrix * vectors[0],
                    _transformationMatrix * rotationMatrix * vectors[1],
                    _transformationMatrix * rotationMatrix * vectors[2]
                })
                .OrderBy(vectors => vectors[0].Z + vectors[1].Z + vectors[2].Z).ToList();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
                case Keys.Right:
                    _rotationAngle -= 10;
                    Refresh();
                    break;
                case Keys.Left:
                    _rotationAngle += 10;
                    Refresh();
                    break;
            }
        }

        private void ReadModelPolygons()
        {
            var vectors = new List<Vector>();
            foreach (var line in File.ReadAllLines(ModelFileName))
            {
                var words = line.Replace('.', ',').Split(' ');
                if (words[0] == "v")
                {
                    vectors.Add(new Vector(double.Parse(words[1]), double.Parse(words[2]), double.Parse(words[3])));
                }
                else if (words[0] == "f")
                {
                    var firstPointIndex = int.Parse(words[1].Split('/')[0]) - 1;
                    var secondPointIndex = int.Parse(words[2].Split('/')[0]) - 1;
                    var thirdPointIndex = int.Parse(words[3].Split('/')[0]) - 1;

                    _originalModelPolygons.Add(new[]
                        {vectors[firstPointIndex], vectors[secondPointIndex], vectors[thirdPointIndex]});
                }
            }
        }

        private void FillTransformMatrix()
        {
            var minX = _originalModelPolygons.Min(polygon => polygon.Min(vector => vector.X));
            var maxX = _originalModelPolygons.Max(polygon => polygon.Max(vector => vector.X));
            var minY = _originalModelPolygons.Min(polygon => polygon.Min(vector => vector.Y));
            var maxY = _originalModelPolygons.Max(polygon => polygon.Max(vector => vector.Y));
            var minZ = _originalModelPolygons.Min(polygon => polygon.Min(vector => vector.Z));
            var maxZ = _originalModelPolygons.Max(polygon => polygon.Max(vector => vector.Z));

            var viewportMatrix = Matrix.CreateViewportMatrix(-170, -150, Width, Height, maxZ - minZ);

            var projectionMatrix = Matrix.CreateProjectionMatrix(minX, maxX, minY, maxY, maxZ, minZ);

            var cameraPosition = new Vector(0.4, 0.4, 1);
            var center = new Vector(0, 0, 0);
            var z = cameraPosition - center;
            var x = Vector.ScalarMultiplication(new Vector(0, 1, 0), z);
            var y = Vector.ScalarMultiplication(z, x);
            var lookAtMatrix = Matrix.CreateLookAtMatrix(x, y, z, cameraPosition);

            var scaleMatrix = Matrix.CreateScaleMatrix(0.6, 0.5, 0.8);

            _transformationMatrix = viewportMatrix * projectionMatrix * lookAtMatrix * scaleMatrix;
        }

        private void DrawModel(Graphics graphics)
        {
            var colorCoefficientStep = 1.0 / (_transformedModelPolygons.Count + 2000);
            var colorCoefficient = colorCoefficientStep * 1950;

            foreach (var vectors in _transformedModelPolygons)
            {
                var points = new[]
                {
                    new Point((int) vectors[0].X, (int) vectors[0].Y),
                    new Point((int) vectors[1].X, (int) vectors[1].Y),
                    new Point((int) vectors[2].X, (int) vectors[2].Y)
                };

                colorCoefficient += colorCoefficientStep;
                var brush = new SolidBrush(Color.FromArgb((int) (255 * colorCoefficient),
                    (int) (255 * colorCoefficient),
                    (int) (255 * colorCoefficient)));
                graphics.FillPolygon(brush, points);
            }
        }
    }
}