using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Shahki
{
    [Serializable]
    public class King : Figure
    {
        public King(int x, int y, int color, int i, int j, Field owner)
        {
            this.x = x;
            this.y = y;
            this.i = i;
            this.j = j;
            this.color = color;
            this.owner = owner;
        }
        public King()
        {

        }
        public override void Show(Graphics g)
        {
                if (color == 2)
                {
                    g.FillEllipse(new SolidBrush(Color.Black), x + 5, y + 5, 60, 60);
                }
                else
                {
                    g.FillEllipse(new SolidBrush(Color.White), x + 5, y + 5, 60, 60);
                }
                g.FillEllipse(new SolidBrush(Color.Red), x + 15, y + 15, 40, 40);
        }
        public override bool Make_turn(int i2, int j2, Graphics g)
        {
            owner.cells[i2, j2].item = owner.cells[i, j].item;
            owner.cells[i, j].item = null;
            owner.cells[i, j].color = 3;
            owner.cells[i, j].Show(g);
            i = i2;
            j = j2;
            x = 70 * j;
            y = 70 * i;
            owner.cells[i, j].color = 3;
            owner.cells[i, j].Show(g);
            Show(g);
            return false;
        }
        public override bool Check_eating()
        {
            Clear_turns();
            int s = 0, si, sj, bi1 = 0, bj1 = 0, bi2 = 0, bj2 = 0;
            bool l = true, r = true;
            if (i != 7)
            {
                si = 1;
                sj = 1;
                while (i + si < 8)
                {
                    if (j + sj < 8 && r)
                    {
                        if (owner.cells[i + si, j + sj].item != null && owner.cells[i + si, j + sj].item.color != color)
                        {
                            if (bi1 == 0 && bj1 == 0)
                            {
                                bi1 = i + si;
                                bj1 = j + sj;
                            }
                            else
                            {
                                r = false;
                            }
                        }
                        if (owner.cells[i + si, j + sj].item != null && owner.cells[i + si, j + sj].item.color == color)
                        {
                            r = false;
                        }
                        if (owner.cells[i + si, j + sj].item == null && bi1 != 0 && bj1 != 0)
                        {
                            turn[s] = bi1 * 1000 + bj1 * 100 + (i + si) * 10 + j + sj;
                            s++;
                        }
                    }
                    if (j - sj > -1 && l)
                    {
                        if (owner.cells[i + si, j - sj].item != null && owner.cells[i + si, j - sj].item.color != color)
                        {
                            if (bi2 == 0 && bj2 == 0)
                            {
                                bi2 = i + si;
                                bj2 = j - sj;
                            }
                            else
                            {
                                l = false;
                            }
                        }
                        if (owner.cells[i + si, j - sj].item != null && owner.cells[i + si, j - sj].item.color == color)
                        {
                            l = false;
                        }
                        if (owner.cells[i + si, j - sj].item == null && bi2 != 0 && bj2 != 0)
                        {
                            turn[s] = bi2 * 1000 + bj2 * 100 + (i + si) * 10 + j - sj;
                            s++;
                        }
                    }
                    si++;
                    sj++;
                }
            }
            bi1 = 0;
            bj1 = 0;
            bi2 = 0;
            bj2 = 0;
            l = true;
            r = true;
            if (i != 0)
            {
                si = 1;
                sj = 1;
                while (i - si > -1)
                {
                    if (j + sj < 8 && r)
                    {
                        if (owner.cells[i - si, j + sj].item != null && owner.cells[i - si, j + sj].item.color != color)
                        {
                            if (bi1 == 0 && bj1 == 0)
                            {
                                bi1 = i - si;
                                bj1 = j + sj;
                            }
                            else
                            {
                                r = false;
                            }
                        }
                        if (owner.cells[i - si, j + sj].item != null && owner.cells[i - si, j + sj].item.color == color)
                        {
                            r = false;
                        }
                        if (owner.cells[i - si, j + sj].item == null && bi1 != 0 && bj1 != 0)
                        {
                            turn[s] = bi1 * 1000 + bj1 * 100 + (i - si) * 10 + j + sj;
                            s++;
                        }
                    }
                    if (j - sj > -1 && l)
                    {
                        if (owner.cells[i - si, j - sj].item != null && owner.cells[i - si, j - sj].item.color != color)
                        {
                            if (bi2 == 0 && bj2 == 0)
                            {
                                bi2 = i - si;
                                bj2 = j - sj;
                            }
                            else
                            {
                                l = false;
                            }
                        }
                        if (owner.cells[i - si, j - sj].item != null && owner.cells[i - si, j - sj].item.color == color)
                        {
                            l = false;
                        }
                        if (owner.cells[i - si, j - sj].item == null && bi2 != 0 && bj2 != 0)
                        {
                            turn[s] = bi2 * 1000 + bj2 * 100 + (i - si) * 10 + j - sj;
                            s++;
                        }
                    }
                    si++;
                    sj++;
                }
            }
            if (turn[0] != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Check_moving()
        {
            Clear_turns();
            int s = 0, si, sj, bi, bj;
            bool l = true, r = true;
            if (i != 7)
            {
                si = 1;
                sj = 1;
                while (i + si < 8 && (r || l)) // оба раза вышло
                {
                    if (j + sj < 8 && r)
                    {
                        bi = i + si;
                        bj = j + sj;
                        if (owner.cells[bi, bj].item == null)
                        {
                            turn[s] = bi * 10 + bj;
                            s++;
                        }
                        else
                        {
                            r = false;
                        }
                    }
                    if (j - sj > -1 && l) 
                    {
                        bi = i + si;
                        bj = j - sj;
                        if (owner.cells[bi, bj].item == null)
                        {
                            turn[s] = bi * 10 + bj;
                            s++;
                        }
                        else
                        {
                            l = false;
                        }
                    }
                    si++;
                    sj++;
                }
            }
            l = true;
            r = true;
            if (i != 0)
            {
                si = 1;
                sj = 1;
                while (i - si > -1 && (r || l))
                {
                    if (j + sj < 8 && r)
                    {
                        bi = i - si;
                        bj = j + sj;
                        if (owner.cells[bi, bj].item == null)
                        {
                            turn[s] = bi * 10 + bj;
                            s++;
                        }
                        else
                        {
                            r = false;
                        }
                    }
                    if (j - sj > -1 && l)
                    {
                        bi = i - si;
                        bj = j - sj;
                        if (owner.cells[bi, bj].item == null)
                        {
                            turn[s] = bi * 10 + bj;
                            s++;
                        }
                        else
                        {
                            l = false;
                        }
                    }
                    si++;
                    sj++;
                }
            }
            if (turn[0] != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Bot_make_turn(int i2, int j2)
        {
            owner.cells[i2, j2].item = owner.cells[i, j].item;
            owner.cells[i, j].item = null;
            owner.cells[i, j].color = 3;
            i = i2;
            j = j2;
            x = 70 * j;
            y = 70 * i;
            owner.cells[i, j].color = 3;
            return false;
        }
    }
}
