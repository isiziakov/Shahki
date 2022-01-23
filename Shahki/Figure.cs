using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Shahki
{
    [Serializable]
    public class Figure
    {
        protected int x, y;
        protected int i, j;
        public int color;
        public int[] turn = new int[15];
        public Field owner;

        public virtual void Show(Graphics g)
        {

        }
        public virtual bool Make_turn(int i2, int j2, Graphics g)
        {
            return false;
        }
        public virtual bool Check_eating()
        {
            return false;
        }
        public virtual bool Check_moving()
        {
            return false;
        }
        public void Clear_turns()
        {
            for (int i = 0; i < 15 && turn[i] != 0; i++)
            {
                turn[i] = 0;
            }
        }
        public virtual void Make_eat(int i2, int j2, int i0, int j0, Graphics g)
        {
            Make_turn(i2, j2, g);
            owner.cells[i0, j0].item = null;
            owner.cells[i0, j0].Show(g);
        }
        public virtual void Become_King(Graphics g)
        {

        }

        public int Find(/*int ci, int cj, */int ei, int ej)
        {
            int result = 0;
            int bi, bj;

            if (ei > /*c*/i)
            {
                bi = 1;
            }
            else
            {
                bi = -1;
            }
            if (ej > /*c*/j)
            {
                bj = 1;
            }
            else
            {
                bj = -1;
            }
            while (owner.cells[/*c*/i + bi, /*c*/j + bj].item == null)
            {
                if (bi > 0)
                {
                    bi++;
                }
                else
                {
                    bi--;
                }
                if (bj > 0)
                {
                    bj++;
                }
                else
                {
                    bj--;
                }
            }
            result = (/*c*/i + bi) * 10 + /*c*/j + bj;
            return result;
        }
        public virtual bool Bot_make_turn(int i2, int j2)
        {
            return false;
        }
        public virtual void Bot_become_king()
        {

        }
        public virtual bool Bot_make_eat(int i2, int j2, int i0, int j0)
        {
            owner.cells[i0, j0].item = null;
            return Bot_make_turn(i2, j2);
        }

        public void Clone(Figure item, Field field)
        {
            this.i = item.i;
            this.j = item.j;
            this.color = item.color;
            this.owner = field;
            this.x = item.x;
            this.y = item.y;
            for (int i = 0; i < 15 && item.turn[i] != 0; i++)
            {
                this.turn[i] = item.turn[i];
            }
        }
    }
}
