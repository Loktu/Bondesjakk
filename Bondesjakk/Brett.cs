using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Bondesjakk
{
    class Brett
    {
        private class Kombination
        {
            int verdi;
            public int Verdi { get { return verdi; } set { verdi = value; } }
            public int[] x = new int[5];
            public int[] y = new int[5];

            public Kombination()
            {
                verdi = 0;
            }
        }

        private int[,] brett;
        private int[,] values;
        private Kombination[] kombinations;

        public int NoColumns
        {
            get { return brett.GetLength(0); }
        }
        public int NoRows
        {
            get { return brett.GetLength(1); }
        }
        public int this[int x, int y]
        { 
            get
            {
                if (x >= brett.GetLength(0))
                    return 0;
                if (y >= brett.GetLength(1))
                    return 0;
                return brett[x, y];
            }
            set
            {
                try
                {
                    brett[x, y] = value;
                }
                catch
                {
                }
            }
        }

        public Brett(int columns, int rows)
        {
            brett = new int[columns, rows];
            values = new int[columns, rows];
            int nKomb = 4 * rows * columns - 12 * rows - 12 * columns + 32;
            kombinations = new Kombination[nKomb];
            int n = 0;
            for (int j = 0; j < rows; ++j)
            {
                for (int i = 0; i < columns - 4; ++i)
                {
                    kombinations[n] = new Kombination();
                    for (int k = 0; k < 5; ++k)
                    {
                        kombinations[n].x[k] = i + k;
                        kombinations[n].y[k] = j;
                    }
                    ++n;
                }
            }
            for (int j = 0; j < rows - 4; ++j)
            {
                for (int i = 0; i < columns; ++i)
                {
                    kombinations[n] = new Kombination();
                    for (int k = 0; k < 5; ++k)
                    {
                        kombinations[n].x[k] = i;
                        kombinations[n].y[k] = j + k;
                    }
                    ++n;
                }
            }
            for (int j = 0; j < rows - 4; ++j)
            {
                for (int i = 0; i < columns - 4; ++i)
                {
                    kombinations[n] = new Kombination();
                    for (int k = 0; k < 5; ++k)
                    {
                        kombinations[n].x[k] = i + k;
                        kombinations[n].y[k] = j + k;
                    }
                    ++n;
                    kombinations[n] = new Kombination();
                    for (int k = 0; k < 5; ++k)
                    {
                        kombinations[n].x[k] = i - k + 4;
                        kombinations[n].y[k] = j + k;
                    }
                    ++n;
                }
            }
            Clear();
        }

        public int CalculateValues(int who)
        {
            int winner = 0;
            ClearValues();
            foreach(Kombination k in kombinations)
            {
                int v = 0;
                bool noValue = false;
                for (int p = 0; p < 5; ++p)
                {
                    int x = k.x[p];
                    int y = k.y[p];
                    int b = brett[x,y];
                    if (b * v < 0)
                    {
                        // Different marks, no value
                        v = 0;
                        noValue = true;
                        break;
                    }
                    else
                    {
                        // No of equal marks
                        v += b;
                    }
                }
                if (noValue)
                    continue;

                // Do we have a winner?
                if (Math.Abs(v) > 4)
                    winner = Math.Sign(v);

                int verdi = (int)Math.Pow(4, Math.Abs(v));
                if (v * who > 0)
                    verdi *= 2;
                
                if (v * who > 0)
                {
                    if (v == 1) verdi = 3;
                    if (v == 2) verdi = 30;
                    if (v == 3) verdi = 80;
                    if (v == 4) verdi = 20000;
                }
                else if (v * who < 0)
                {
                    if (v == 1) verdi = 2;
                    if (v == 2) verdi = 40;
                    if (v == 3) verdi = 70;
                    if (v == 4) verdi = 1000;
                }
                else
                {
                    verdi = 1;
                }
                k.Verdi = verdi;

                // The value of a move is the sum of all the combunations it is a member of
                for (int p = 0; p < 5; ++p)
                {
                    if (brett[k.x[p], k.y[p]] == 0)
                        values[k.x[p], k.y[p]] += k.Verdi;
                }
            }
            return winner;
        }

        public void ClearValues()
        {
            for (int row = 0; row < NoRows; ++row)
            {
                for (int col = 0; col < NoColumns; ++col)
                {
                    values[col, row] = 0;
                }
            }
        }

        public void Clear()
        {
            for (int row = 0; row < NoRows; ++row)
            {
                for (int col = 0; col < NoColumns; ++col)
                {
                    brett[col, row] = 0;
                    values[col, row] = 0;
                }
            }
        }

        public bool SetBestMove(out int lastx, out int lasty)
        {
            int bestRow = 0;
            int bestCol = 0;
            int bestVal = -1;
            for (int row = 0; row < NoRows; ++row)
            {
                for (int col = 0; col < NoColumns; ++col)
                {
                    if (brett[col, row] == 0)
                    {
                        int v = Math.Abs(values[col, row]);
                        if (v > bestVal)
                        {
                            bestVal = v;
                            bestRow = row;
                            bestCol = col;
                        }
                    }
                }
            }
            if (bestVal > -1)
            {
                brett[bestCol, bestRow] = -1;
                lastx = bestCol;
                lasty = bestRow;
                return true;
            }
            lastx = -1;
            lasty = -1;
            return false;
        }
    }
}
