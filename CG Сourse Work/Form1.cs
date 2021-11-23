using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CG_Сourse_Work
{
    public partial class Form1 : Form
    {
        private const string ModelFileName = "cat.obj";

        private readonly List<Vector[]> _model = new List<Vector[]>();

        public Form1()
        {
            InitializeComponent();
            ReadModel(ModelFileName);
            KeyDown += OnKeyDown;
            Paint += OnPaint;

            Width = 1000;
            Height = 1000;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            DrawModel(graphics, CreateTransformMatrix());
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

                    _model.Add(new[]
                        {vectors[firstPointIndex], vectors[secondPointIndex], vectors[thirdPointIndex]});
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
                case Keys.Right:
                    Refresh();
                    break;
                case Keys.Left:
                    Refresh();
                    break;
            }
        }

        private Matrix CreateTransformMatrix()
        {
            return Matrix.CreateUnitMatrix();
        }

        private void DrawModel(Graphics graphics, Matrix transformationMatrix)
        {
            var colorStep = 1.0 / (_model.Count + 100);
            var currentColor = colorStep * 50;
            
            var newModel = _model.Select(vectors => new[]
                {
                    transformationMatrix * vectors[0],
                    transformationMatrix * vectors[1],
                    transformationMatrix * vectors[2]
                })
                .OrderBy(vectors => vectors[0].Z + vectors[1].Z + vectors[2].Z).ToList();

            foreach (var vectors in newModel)
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