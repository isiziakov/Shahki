using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Shahki
{
    public delegate void OnKlick_event(int i, int j, bool draw); 
    public delegate void Game_over_event();
    public delegate void Mes_event(string mes);
    [Serializable]
    public class Field
    {
        [field: NonSerializedAttribute()]
        public event Game_over_event Over;
        [field: NonSerializedAttribute()]
        public event Mes_event Mes;
        public int ci, cj;
        public int x, y;
        private bool draw;
        [NonSerialized]private Graphics g;
        public int player;
        public int type;
        public bool eating, have_turn;
        public Cell[,] cells = new Cell[8,8];
        private II ii;
        public Field()
        {

        }
        public Field(int x, int y, int type, Graphics g, Form1 form1)
        {
            ii = new II();
            this.x = x;
            this.y = y;
            this.type = type;
            this.player = 1;
            this.eating = false;
            this.have_turn = false;
            this.draw = false;
            this.g = g;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        cells[i,j] = new Cell(70 * j, 70 * i, 1);
                    }
                    else
                    {
                        cells[i, j] = new Cell(70 * j, 70 * i, 0);
                    }
                }
            Set_checkers();
            Show();
            form1.OnKlick += Field_OnKlick;
            ci = 0;
            cj = 0;
        }

        private void Field_OnKlick(int x, int y, bool draw)
        {
            if (draw)
            {
                this.draw = draw;
            }
            HotClick(x, y);
        }

        public void Change_player()
        {
            eating = false;
            have_turn = false;
            player = player + 1;
            if (player == 3)
            {
                player -= 2;
            }
            Check_eating();
            if (!eating)
            {
                Check_moving(); // замена шашки на дамку событием, сообщения на форму
            }
            if (!have_turn)
            {
                Game_over(false);
                return;
            }
            else
            {
                eating = false;
                have_turn = false;
            }
            if (draw)
            {
                if (type == 1)
                {
                    DialogResult result = MessageBox.Show("Ничья?", "Вы согласны на ничью?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Game_over(true);
                    }
                    draw = false;
                    return;
                }
            }
            if (player == 1)
            {
                Mes.Invoke("Ход белых");
            }
            else 
            {
                Mes.Invoke("Ход черных");
            }
        }
        public void Check_eating()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (cells[i, j].item != null)
                    {
                        if (cells[i, j].item.color == player)
                        {
                            if (cells[i, j].item.Check_eating())
                            {
                                eating = true;
                                have_turn = true;
                            }
                        }
                    }
                }
            }
        }
        public void Check_moving()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (cells[i, j].item != null)
                    {
                        if (cells[i, j].item.color == player)
                        {
                            if (cells[i, j].item.Check_moving())
                            {
                                have_turn = true;
                            }
                        }
                    }
                }
            }
        }
        public void Clear_turns()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (cells[i,j].item != null)
                    {
                        if (cells[i, j].item.color == player)
                        {
                            cells[i, j].item.Clear_turns();
                        }
                    }
                }
            }
        }
        public void Set_checkers()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        cells[i, j].item = new Checker(cells[i, j].x, cells[i, j].y, 2, i, j, this);
                    }
                }
            }
            for (int i = 5; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        cells[i, j].item = new Checker(cells[i, j].x, cells[i, j].y, 1, i, j, this);
                    }
                }
            }
            //cells[1, 4].item = new Checker(cells[1, 4].x, cells[1, 4].y, 2, 1, 4, this);
            //cells[3, 2].item = new Checker(cells[3, 2].x, cells[3, 2].y, 2, 3, 2, this);
            //cells[4, 1].item = new Checker(cells[4, 1].x, cells[4, 1].y, 1, 4, 1, this);
        }
        public void Delete(Form1 form1)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (cells[i, j].item != null)
                    {
                        cells[i, j].item.owner = null;
                    }
                    cells[i, j].item = null;
                    cells[i, j] = null;
                }
            }
            form1.OnKlick -= Field_OnKlick;
        }
        public void Show()
        {
            g.Clear(Color.White);
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    cells[i, j].Show(g);
                }
        }
        public void HotClick(int x, int y)
        {
            int i, j;
            i = y / 70;
            j = x / 70;
            if (i > 7)
            {
                i = 7;
            }
            if (j > 7)
            {
                j = 7;
            }
            if (ci == 0 && cj == 0)
            {
                if (have_turn == false && eating == false)
                {
                    Check_eating();
                    if (!eating)
                    {
                        Check_moving(); // замена шашки на дамку событием, сообщения на форму
                    }
                    if (!have_turn)
                    {
                        Game_over(false);
                        return;
                    }
                }
                if (cells[i,j].item != null && cells[i, j].item.color == player && cells[i, j].item.turn[0] != 0)
                {
                    ci = i;
                    cj = j;
                    cells[ci, cj].color = 2;
                    for (int v = 0; v < 15; v++)
                    {
                        if (cells[ci, cj].item.turn[v] == 0)
                        {
                            break;
                        }
                        y = cells[ci, cj].item.turn[v] % 100 / 10;
                        x = cells[ci, cj].item.turn[v] % 10;
                        cells[y, x].color = 2;
                    }
                }
                for (int bi = 0; bi < 8; bi++)
                {
                    for (int bj = 0; bj < 8; bj++)
                    {
                        if (cells[bi, bj].color == 3)
                        {
                            cells[bi, bj].color = 1;
                        }
                    }
                }
                Show();
            }
            else
            {
                if (cells[i, j].color == 2 && ci != i && cj != j)
                {
                    if (!eating)
                    {
                        cells[ci, cj].item.Make_turn(i, j, g);
                        End_turn();
                    }
                    else
                    {
                        int res = cells[ci, cj].item.Find(/*ci, cj, */i, j);
                        cells[ci, cj].item.Make_eat(i, j, res / 10, res % 10, g);
                        for (int w = 0; w < 8; w++)
                        {
                            for (int v = 0; v < 8; v++)
                            {
                                if (cells[w, v].color == 2)
                                {
                                    cells[w, v].color = 1;
                                    cells[w, v].Show(g);
                                }
                            }
                        }
                        if (cells[i, j].item.Check_eating())
                        {
                            ci = i;
                            cj = j;
                            cells[ci, cj].color = 2;
                            for (int v = 0; v < 15; v++)
                            {
                                if (cells[ci, cj].item.turn[v] == 0)
                                {
                                    break;
                                }
                                y = cells[ci, cj].item.turn[v] % 100 / 10;
                                x = cells[ci, cj].item.turn[v] % 10;
                                cells[y, x].color = 2;
                                cells[y, x].Show(g);
                            }
                        }
                        else
                        {
                            End_turn();
                        }
                    }
                }
            }
        }
        public void End_turn()
        {
            Clear_turns();
            eating = false;
            have_turn = false;
            ci = 0;
            cj = 0;
            for (int bi = 0; bi < 8; bi++)
            {
                for (int bj = 0; bj < 8; bj++)
                {
                    if (cells[bi, bj].color == 2)
                    {
                        cells[bi, bj].color = 1;
                    }
                }
            }
            Change_player();
            if (type == 2)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (cells[i, j].color == 3)
                        {
                            cells[i, j].color = 1;
                        }
                    }
                }
                Bot_turn();
            }
            Show();
        }

        public void Game_over(bool draw)
        {
            if (draw)
            {
                Mes.Invoke("Ничья");
                Over?.Invoke();
            }
            else
            {
                if (player == 1)
                {
                    Mes.Invoke("Победили черные");
                }
                else
                {
                    Mes.Invoke("Победили белые");
                }
                Over?.Invoke();
            }
        }

        //public int CI()
        //{
        //    return ci;
        //}
        //public int CJ()
        //{
        //    return cj;
        //}
        //public int X()
        //{
        //    return x;
        //}
        //public int Y()
        //{
        //    return y;
        //}
        //public int Player()
        //{
        //    return player;
        //}
        //public int Type()
        //{
        //    return type;
        //}
        //public bool Eating()
        //{
        //    return eating;
        //}
        //public bool Have_turn()
        //{
        //    return have_turn;
        //}
        public void G(Graphics g, Form1 form1)
        {
            this.g = g;
            form1.OnKlick += Field_OnKlick;
        }

        public void Bot_turn()
        {
            ii.Bot_turn_analisis(this, 1);
            Check_eating();
            for (int i = 0; !(ii.best_turn[0,i] == 0 && ii.best_turn[1,i] == 0); i++)
            {
                if (eating)
                {
                    int food = cells[ii.best_turn[0, i] % 100 / 10, ii.best_turn[0, i] % 10].item.Find(ii.best_turn[1, i] % 100 / 10, ii.best_turn[1, i] % 10);
                    cells[ii.best_turn[0, i] % 100 / 10, ii.best_turn[0, i] % 10].item.Make_eat(ii.best_turn[1, i] % 100 / 10, ii.best_turn[1, i] % 10, food % 100 / 10, food % 10, g);
                }
                else
                {
                    cells[ii.best_turn[0, i] % 100 / 10, ii.best_turn[0, i] % 10].item.Make_turn(ii.best_turn[1, i] % 100 / 10, ii.best_turn[1, i] % 10, g);
                }
                ii.best_turn[0, i] = 0;
                ii.best_turn[1, i] = 0;
            }
            Change_player();
        }

        public Field(Field field)
        {
            this.eating = field.eating;
            this.have_turn = field.have_turn;
            this.player = field.player;
            this.type = field.type;
            this.x = field.x;
            this.y = field.y;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        cells[i, j] = new Cell(70 * j, 70 * i, 1);
                    }
                    else
                    {
                        cells[i, j] = new Cell(70 * j, 70 * i, 0);
                    }
                    if (field.cells[i, j].item != null)
                    {
                        if (field.cells[i, j].item is Checker)
                        {
                            cells[i, j].item = new Checker();
                        }
                        else
                        {
                            cells[i, j].item = new King();
                        }
                        cells[i, j].item.Clone(field.cells[i, j].item, this);
                    }
                }
            }
        }
    }
}
