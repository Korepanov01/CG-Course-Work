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

        private readonly List<Vector[]> _originalModel = new List<Vector[]>();
        private List<Vector[]> _transformedModel = new List<Vector[]>();

        private readonly Matrix _transformationMatrix;

        private int _rotationAngle;

        public Form1()
        {
            InitializeComponent();
            Width = 1000;
            Height = 500;
            BackColor = Color.Aqua;
            ReadModel(ModelFileName);
            _transformationMatrix = CreateTransformMatrix();
            
            KeyDown += OnKeyDown;
            Paint += OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            RotateModel(Matrix.CreateYRotationMatrix(_rotationAngle));
            DrawModel(graphics);
        }

        private void RotateModel(Matrix rotationMatrix)
        {
            _transformedModel = _originalModel.Select(vectors => new[]
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
                    _rotationAngle += 10;
                    Refresh();
                    break;
                case Keys.Left:
                    _rotationAngle -= 10;
                    Refresh();
                    break;
            }
        }

        private void ReadModel(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var vectors = new List<Vector>();
            foreach (var line in lines)
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

                    _originalModel.Add(new[]
                        {vectors[firstPointIndex], vectors[secondPointIndex], vectors[thirdPointIndex]});
                }
            }
        }

        private Matrix CreateTransformMatrix()
        {
            var minX = _originalModel.Min(polygon => polygon.Min(vector => vector.X));
            var maxX = _originalModel.Max(polygon => polygon.Max(vector => vector.X));
            var minY = _originalModel.Min(polygon => polygon.Min(vector => vector.Y));
            var maxY = _originalModel.Max(polygon => polygon.Max(vector => vector.Y));
            var minZ = _originalModel.Min(polygon => polygon.Min(vector => vector.Z));
            var maxZ = _originalModel.Max(polygon => polygon.Max(vector => vector.Z));

            var viewportMatrix = Matrix.CreateViewportMatrix(-50, 0, Width, Height, maxZ - minZ);

            var projectionMatrix = Matrix.CreateProjectionMatrix(minX, maxX, minY, maxY, maxZ, minZ);

            return viewportMatrix * projectionMatrix;
        }

        private void DrawModel(Graphics graphics)
        {
            var colorStep = 1.0 / (_transformedModel.Count + 100);
            var currentColor = colorStep * 50;
            
            foreach (var vectors in _transformedModel)
            {
                var points = new[]
                {
                    new Point((int) vectors[0].X, (int) vectors[0].Y),
                    new Point((int) vectors[1].X, (int) vectors[1].Y),
                    new Point((int) vectors[2].X, (int) vectors[2].Y)
                };

                currentColor += colorStep;
                var currentRgb = (int) (255 * currentColor);
                var brush = new SolidBrush(Color.FromArgb(currentRgb, currentRgb, currentRgb));
                graphics.FillPolygon(brush, points);
            }
        }
    }
}