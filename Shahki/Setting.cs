using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahki
{
    [Serializable]
    public class Setting
    {
        public int attack, defense, pos, e_king, m_king, c_king, turns;
        public Setting()
        {

        }
        public void Set(int attack, int defense, int m_pos, int c_pos, int m_king, int c_king, int turns)
        {
            this.attack = attack;
            this.defense = defense;
            this.pos = m_pos;
            this.e_king = c_pos;
            this.m_king = m_king;
            this.c_king = c_king;
            this.turns = turns;
        }
    }
}
