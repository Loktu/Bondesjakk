using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Bondesjakk
{
    public partial class BrettControl : UserControl
    {
        private Brett brett;
        private Color brettColor = Color.Black;
        private Color xColor = Color.Red;
        private Color oColor = Color.Blue;
        private Color lastColor = Color.Black;

        int size = 10;
        int xMin = 0;
        int yMin = 0;
        int xMax = 300;
        int yMax = 300;

        int lastx = -1;
        int lasty = -1;

        public BrettControl()
        {
            InitializeComponent();
            brett = new Brett(20, 20);
        }

        public void Start()
        {
            Clear();
            int x = brett.NoColumns / 2;
            int y = brett.NoRows / 2;
            brett[x, y] = -1;
            Invalidate();
        }

        public void Clear()
        {
            brett.Clear();
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Rectangle r = ClientRectangle;
            Graphics g = e.Graphics;
            Pen brettPen = new Pen(brettColor, 1);

            {
                int sizeX = r.Width / brett.NoColumns;
                int sizeY = r.Height / brett.NoRows;
                size = Math.Min(sizeX, sizeY);
            }

            xMin = 0;
            yMin = 0;
            xMax = brett.NoColumns * size;
            yMax = brett.NoRows * size;

            for (int row = 0; row <= brett.NoRows; ++row)
            {
                int y = yMin + size * row;
                g.DrawLine(brettPen, xMin, y, xMax, y);
            }
            for (int col = 0; col <= brett.NoColumns; ++col)
            {
                int x = xMin + size * col;
                g.DrawLine(brettPen, x, yMin, x, yMax);
            }

            for (int row = 0; row < brett.NoRows; ++row)
            {
                int y = yMin + size * row;
                for (int col = 0; col < brett.NoColumns; ++col)
                {
                    int x = xMin + size * col;
                    bool last = (col == lastx && row == lasty) ;
                    DrawValue(g, x, y, size, brett[col, row], last);
                }
            }
        }
        public void DrawValue(Graphics g, int x, int y, int size, int value, bool last)
        {
            if (value == 0) return;
            float lineSize = size / 10.0f; // just for fun
            x += 2;
            y += 2;
            size -=4;

            Pen pen;
            if (value > 0)
            {
                if (last)
                    pen = new Pen(lastColor, lineSize);
                else
                    pen = new Pen(xColor, lineSize);
                // Draw X
                g.DrawLine(pen, x, y, x + size, y + size);
                g.DrawLine(pen, x, y + size, x + size, y);
            }
            else if (value < 0)
            {
                if (last)
                    pen = new Pen(lastColor, lineSize);
                else
                    pen = new Pen(this.oColor, lineSize);
                // Draw X
                g.DrawEllipse(pen, x, y, size, size);
            }
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / size;
            int y = e.Y / size;

            if (brett[x, y] != 0)
            {
                return;
            }
            brett[x, y] = 1;
            lastx = x;
            lasty = y;
            Invalidate();
            int w = brett.CalculateValues(-1);
            //brett.Dump();

            if (w == 0)
            {
                brett.SetBestMove(out lastx, out lasty);
                Invalidate();
                w = brett.CalculateValues(-1);
            }
            if (w != 0)
            {
                if (w>0)
                    MessageBox.Show("You win");
                else if (w<0)
                    MessageBox.Show("Computer win!");
                Clear();
            }
        }
    }
}
