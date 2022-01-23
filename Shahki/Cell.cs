using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Shahki
{
    [Serializable]
    public class Cell
    {
        public int color;
        public int x, y;
        public Figure item;
        public Cell(int x, int y, int color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }
        public Cell()
        {

        }

        public void Show(Graphics g)
        {
            SolidBrush Pen = new SolidBrush(Color.Red);
            System.Drawing.Color colors = Color.Red;
            switch (color)
            {
                case 0:
                    colors = Color.White;
                    break;
                case 1:
                    colors = Color.Brown;
                    break;
                case 2:
                    colors = Color.Red;
                    break;
                case 3:
                    colors = Color.Green;
                    break;
            }
            Pen.Color = colors;
            g.FillRectangle(Pen, x, y, 70, 70);
            Pen blackPen = new Pen(Color.Black, 3);
            g.DrawRectangle(blackPen, x, y, 70, 70);
            if (item != null)
            {
                item.Show(g);
            }
        }
    }
}
