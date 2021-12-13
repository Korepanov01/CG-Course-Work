﻿using System;

namespace CG_Сourse_Work
{
    public class Matrix
    {
        private readonly double[,] _matrix = new double[4, 4];

        private Matrix()
        {
        }

        public double this[int i, int j]
        {
            get => _matrix[i, j];
            private set => _matrix[i, j] = value;
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

        public static Matrix CreateYRotationMatrix(double rotationAngleInDegrees)
        {
            var rotationAngleInRadians = rotationAngleInDegrees * 0.0174533;

            return new Matrix()
            {
                [0, 0] = Math.Cos(rotationAngleInRadians),
                [0, 2] = Math.Sin(rotationAngleInRadians),
                [2, 0] = -Math.Sin(rotationAngleInRadians),
                [2, 2] = Math.Cos(rotationAngleInRadians),
                [1, 1] = 1,
                [3, 3] = 1
            };
        }

        public static Matrix CreateProjectionMatrix(double l, double r, double b, double t, double n, double f)
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

        public static Matrix CreateViewportMatrix(double x, double y, double w, double h, double depth)
        {
            return new Matrix
            {
                [0, 0] = w / 2.0,
                [1, 1] = -h / 2.0,
                [2, 2] = depth / 2.0,
                [3, 3] = 1,

                [0, 3] = x + w / 2.0,
                [1, 3] = y + h / 2.0,
                [2, 3] = depth / 2.0
            };
        }

        public static Matrix CreateLookAtMatrix(Vector rightVector, Vector upVector, Vector forwardVector,
            Vector cameraPosition)
        {
            return new Matrix
            {
                [0, 0] = rightVector.X,
                [0, 1] = rightVector.Y,
                [0, 2] = rightVector.Z,

                [1, 0] = upVector.X,
                [1, 1] = upVector.Y,
                [1, 2] = upVector.Z,

                [2, 0] = forwardVector.X,
                [2, 1] = forwardVector.Y,
                [2, 2] = forwardVector.Z,

                [0, 3] = -cameraPosition.X,
                [1, 3] = -cameraPosition.Y,
                [2, 3] = -cameraPosition.Z,

                [3, 3] = 1
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
    }
}