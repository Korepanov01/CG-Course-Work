using System;

namespace CG_Сourse_Work
{
    public class Vector
    {
        private readonly double[] _array = new double[4];
        
        public double X { get => _array[0]; set => _array[0] = value; }
        public double Y { get => _array[1]; set => _array[1] = value; }
        public double Z { get => _array[2]; set => _array[2] = value; }
        public double W { get => _array[3]; set => _array[3] = value; }

        public Vector() { }

        public Vector(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
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

        public double this[int i]
        {
            get => _array[i];
            set => _array[i] = value;
        }

        public static Vector ScalarMultiplication(Vector firstVector, Vector secondVector)
        {
            return new Vector
            {
                X = firstVector.Y * secondVector.Z - firstVector.Z * secondVector.Y,
                Y = firstVector.Z * secondVector.X - firstVector.X * secondVector.Z,
                Z = firstVector.X * secondVector.Y - firstVector.Y * secondVector.X
            };
        }

        public static Vector Clone(Vector vector)
        {
            return new Vector
            {
                X = vector.X,
                Y = vector.Y,
                Z = vector.Z
            };
        }
        
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector
            {
                X = v1.X - v2.X,
                Y = v1.Y - v2.Y,
                Z = v1.Z - v2.Z
            };
        }

        public double Length() => Math.Sqrt(X * X + Y * Y + Z * Z);

        public static Vector Normalize(Vector vector)
        {
            return new Vector()
            {
                X = vector.X * (1.0 / vector.Length()),
                Y = vector.Y * (1.0 / vector.Length()),
                Z = vector.Z * (1.0 / vector.Length())
            };
        }
    }
}