﻿using System.Diagnostics;

namespace Graphics3D.Geometry
{
    public class Matrix
    {
        private double[,] data;

        public double this[int i, int j]
        {
            get { return data[i, j]; }
            set { data[i, j] = value; }
        }

        public Matrix()
        {
            data = new double[,] {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
        }

        public Matrix(double[,] data)
        {
            Debug.Assert(4 == data.GetLength(0));
            Debug.Assert(4 == data.GetLength(1));
            this.data = data;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            double[,] data = new double[4, 4];
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    data[i, j] = 0;
                    for (int k = 0; k < 4; ++k)
                        data[i, j] += m1[i, k] * m2[k, j];
                }
            return new Matrix(data);
        }
    }
}
