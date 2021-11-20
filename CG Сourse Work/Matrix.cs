using System;

namespace CG_Сourse_Work
{
    public class Matrix
    {
        private readonly double[,] _matrix= new double[4, 4];

        public Matrix() { }
        
        public double this[int i, int j]
        {
            get => _matrix[i, j];
            set => _matrix[i, j] = value;
        }

        public static Matrix CreateUnitMatrix()
        {
            var matrix = new Matrix();
            
            for (var i = 0; i < 4; i++)
            {
                matrix[i, i] = 1.0;
            }

            return matrix;
        }

        public static Matrix operator *(Matrix firstMatrix, Matrix secondMatrix)
        {
            var resultMatrix = new Matrix();

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    for (var k = 0; k < 4; k++)
                    {
                        resultMatrix[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                    }
                }
            }

            return resultMatrix;
        }

        public static Matrix CreateViewport(int x, int y, int w, int h, int depth = 255)
        {
            return new Matrix
            {
                [0, 0] = w / 2.0,
                [1, 1] = h / -2.0,
                [2, 2] = depth / 2.0,
                [3, 3] = 1,
                    
                [0, 3] = x + w / 2.0,
                [1, 3] = y + h / 2.0,
                [2, 3] = depth / 2.0
            };
        }

        public static Matrix CreateProjOrto(double l, double r, double b, double t, double n, double f)
        {
            return new Matrix
            {
                [0, 0] = 2.0 / (r - l),
                [1, 1] = 2.0 / (t - b),
                [2, 2] = -2.0 / (f - n),
                [3, 3] = 1,
                
                [0, 3] = -(r + l) / (r - l),
                [1, 3] = -(t + b) / (t - b),
                [2, 3] = -(f + n) / (f - n)
            };
        }

        public static Matrix CreateLookAt(Vector eye, Vector center, Vector up)
        {
            var z = Vector.Normalize(eye - center);
            var x = Vector.Normalize(Vector.ScalarMultiplication(up, z));
            var y = Vector.Normalize(Vector.ScalarMultiplication(z, x));

            return new Matrix
            {
                [0, 0] = x.X,
                [0, 1] = x.Y,
                [0, 2] = x.Z,


                [1, 0] = y.X,
                [1, 1] = y.Y,
                [1, 2] = y.Z,

                [2, 0] = z.X,
                [2, 1] = z.Y,
                [2, 2] = z.Z,

                [3, 3] = 1,

                [0, 3] = -center.X,
                [1, 3] = -center.Y,
                [2, 3] = -center.Z
            };
        }

        public static Matrix CreateShiftMatrix(double tx, double ty, double tz)
        {
            return new Matrix
            {
                [0, 0] = 1,
                [1, 1] = 1,
                [2, 2] = 1,
                [3, 3] = 1,
                    
                [0, 3] = tx,
                [1, 3] = ty,
                [2, 3] = tz
            };
        }

        public static Matrix CreateScaleMatrix(double kx, double ky, double kz)
        {
            return new Matrix
            {
                [0, 0] = kx,
                [1, 1] = ky,
                [2, 2] = kz,
                [3, 3] = 1
            };
        }
        
        public static Matrix CreateScaleMatrix(double k)
        {
            return new Matrix
            {
                [0, 0] = 1,
                [1, 1] = 1,
                [2, 2] = 1,
                [3, 3] = k
            };
        }

        public static Matrix CreateRotateMatrix(double rotationAngleInDegrees, Rotation rotation)
        {
            var rotateMatrix = new Matrix()
            {
                [0, 0] = 1,
                [1, 1] = 1,
                [2, 2] = 1,
                [3, 3] = 1
            };
            
            var rotationAngleInRadians = rotationAngleInDegrees * 0.0174533;
            switch (rotation)
            {
                case Rotation.X:
                    rotateMatrix[0, 0] = Math.Cos(rotationAngleInRadians);
                    rotateMatrix[1, 0] = Math.Sin(rotationAngleInRadians);
                    rotateMatrix[0, 1] = -Math.Sin(rotationAngleInRadians);
                    rotateMatrix[1, 1] = Math.Cos(rotationAngleInRadians);
                    break;
                
                case Rotation.Y:
                    rotateMatrix[1, 1] = Math.Cos(rotationAngleInRadians);
                    rotateMatrix[2, 1] = Math.Sin(rotationAngleInRadians);
                    rotateMatrix[1, 2] = -Math.Sin(rotationAngleInRadians);
                    rotateMatrix[2, 2] = Math.Cos(rotationAngleInRadians);
                    break;
                
                case Rotation.Z:
                    rotateMatrix[0, 0] = Math.Cos(rotationAngleInRadians);
                    rotateMatrix[2, 0] = Math.Sin(rotationAngleInRadians);
                    rotateMatrix[0, 2] = -Math.Sin(rotationAngleInRadians);
                    rotateMatrix[2, 2] = Math.Cos(rotationAngleInRadians);
                    break;
            }

            return rotateMatrix;
        }

        public enum Rotation
        {
            X,
            Y,
            Z
        }
    }
}