using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Shahki
{
    [Serializable]
    public class Checker : Figure
    {
        public Checker(int x, int y, int color, int i, int j, Field owner)
        {
            this.x = x;
            this.y = y;
            this.i = i;
            this.j = j;
            this.color = color;
            this.owner = owner;
        }
        public Checker()
        {

        }
        public override void Show(Graphics g)
        {
            if (color == 2)
            {
                SolidBrush brush = new SolidBrush(Color.Black);
                g.FillEllipse(brush, x + 5, y + 5, 60, 60);
            }
            else
            {
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, x + 5, y + 5, 60, 60);
            }
        }
        public override bool Make_turn(int i2, int j2, Graphics g)
        {
            int bi = i, bj = j;
            i = i2;
            j = j2;
            x = 70 * j;
            y = 70 * i;
            owner.cells[i2, j2].item = owner.cells[bi, bj].item;
            owner.cells[bi, bj].item = null;
            owner.cells[bi, bj].color = 3;
            owner.cells[bi, bj].Show(g);
            owner.cells[i, j].color = 3;
            owner.cells[i, j].Show(g);
            //Show(g);
            if (color == 1 && i == 0)
            {
                Become_King(g);
                return true;
            }
            if (color == 2 && i == 7)
            {
                Become_King(g);
                return true;
            }
            return false;
        }

        public override void Become_King(Graphics g)
        {
            if (color == 1)
            {
                owner.cells[i, j].item = new King(x, y, 1, i, j, owner);
                owner.cells[i, j].item.Show(g);
            }
            else
            {
                owner.cells[i, j].item = new King(x, y, 2, i, j, owner);
                owner.cells[i, j].item.Show(g);
            }
        }
        public override bool Check_eating()
        {
            Clear_turns();
            int s = 0;
            if (i > 1)
            {
                if (j > 1)
                {
                    if (owner.cells[i - 1, j - 1].item != null && owner.cells[i - 1, j - 1].item.color != color && owner.cells[i - 2, j - 2].item == null)
                    {
                        turn[s] = (i - 1) * 1000 + (j - 1) * 100 + (i - 2) * 10 + (j - 2);
                        s++;
                    }
                }
                if (j < 6)
                {
                    if (owner.cells[i - 1, j + 1].item != null && owner.cells[i - 1, j + 1].item.color != color && owner.cells[i - 2, j + 2].item == null)
                    {
                        turn[s] = (i - 1) * 1000 + (j + 1) * 100 + (i - 2) * 10 + (j + 2);
                        s++;
                    }
                }
            }
            if (i < 6)
            {
                if (j > 1)
                {
                    if (owner.cells[i + 1, j - 1].item != null && owner.cells[i + 1, j - 1].item.color != color && owner.cells[i + 2, j - 2].item == null)
                    {
                        turn[s] = (i + 1) * 1000 + (j - 1) * 100 + (i + 2) * 10 + (j - 2);
                        s++;
                    }
                }
                if (j < 6)
                {
                    if (owner.cells[i + 1, j + 1].item != null && owner.cells[i + 1, j + 1].item.color != color && owner.cells[i + 2, j + 2].item == null)
                    {
                        turn[s] = (i + 1) * 1000 + (j + 1) * 100 + (i + 2) * 10 + (j + 2);
                        s++;
                    }
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
            if (color == 2)
            {
                si = 1;
            }
            else
            {
                si = -1;
            }
            if (j != 7)
            {
                sj = 1;
                bi = i + si;
                bj = j + sj;
                if (owner.cells[bi,bj].item == null)
                {
                    turn[s] = bi * 10 + bj;
                    s++;
                }
            }
            if (j != 0)
            {
                sj = -1;
                bi = i + si;
                bj = j + sj;
                if (owner.cells[bi, bj].item == null)
                {
                    turn[s] = bi * 10 + bj;
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
        //public override int Find_eat(int ci, int cj) // не используется
        //{
        //    int res = 0;
        //    if (ci > i)
        //    {
        //        res = ci - 1;
        //    }
        //    else
        //    {
        //        res = ci + 1;
        //    }
        //    if (cj > j)
        //    {
        //        res += 1; 
        //    }
        //    return 0;
        //}
        public override bool Bot_make_turn(int i2, int j2)
        {
            int bi = i, bj = j;
            i = i2;
            j = j2;
            x = 70 * j;
            y = 70 * i;
            owner.cells[i2, j2].item = owner.cells[bi, bj].item;
            owner.cells[bi, bj].item = null;
            if (color == 1 && i == 0)
            {
                Bot_become_king();
                return true;
            }
            if (color == 2 && i == 7)
            {
                Bot_become_king();
                return true;
            }
            return false;
        }
        public override void Bot_become_king()
        {
            if (color == 1)
            {
                owner.cells[i, j].item = new King(x, y, 1, i, j, owner);
            }
            else
            {
                owner.cells[i, j].item = new King(x, y, 2, i, j, owner);
            }
        }
    }
}
