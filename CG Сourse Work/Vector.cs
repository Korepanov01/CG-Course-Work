using System;

namespace CG_Сourse_Work
{
    public class Vector
    {
        private readonly double[] _array = { 0, 0, 0, 1};

        public double X
        {
            get => _array[0];
            private set => _array[0] = value;
        }

        public double Y
        {
            get => _array[1];
            private set => _array[1] = value;
        }

        public double Z
        {
            get => _array[2];
            private set => _array[2] = value;
        }

        private double Length { get; }

        public Vector()
        {
        }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
            Length = Math.Sqrt(x * x + y * y + z * z);
        }

        private double this[int i]
        {
            get => _array[i];
            set => _array[i] = value;
        }

        public Vector Normalized()
        {
            if (Length == 0)
            {
                return new Vector();
            }

            return new Vector(X / Length, Y / Length, Z / Length);
        }

        public static Vector operator -(Vector firstVector, Vector secondVector)
        {
            return new Vector
            {
                X = firstVector.X - secondVector.X,
                Y = firstVector.Y - secondVector.Y,
                Z = firstVector.Z - secondVector.Z
            };
        }

        public static Vector operator +(Vector firstVector, Vector secondVector)
        {
            return new Vector
            {
                X = firstVector.X + secondVector.X,
                Y = firstVector.Y + secondVector.Y,
                Z = firstVector.Z + secondVector.Z
            };
        }

        public static Vector operator *(Matrix matrix, Vector vector)
        {
            var resultVector = new Vector();

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    resultVector[i] += matrix[i, j] * vector[j];
                }
            }

            return resultVector;
        }

        public static Vector ScalarMultiplication(Vector firstVector, Vector secondVector)
        {
            return new Vector
            (
                firstVector.Y * secondVector.Z - firstVector.Z * secondVector.Y,
                firstVector.Z * secondVector.X - firstVector.X * secondVector.Z,
                firstVector.X * secondVector.Y - firstVector.Y * secondVector.X
            );
        }
    }
}