using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shahki
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            trackBar1.Value = Program.setting.attack;
            trackBar2.Value = Program.setting.defense;
            trackBar3.Value = Program.setting.pos;
            trackBar4.Value = Program.setting.e_king;
            trackBar5.Value = Program.setting.m_king;
            trackBar6.Value = Program.setting.c_king;
            trackBar7.Value = Program.setting.turns;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.setting.attack = trackBar1.Value;
            Program.setting.defense = trackBar2.Value;
            Program.setting.pos = trackBar3.Value;
            Program.setting.e_king = trackBar4.Value;
            Program.setting.m_king = trackBar5.Value;
            Program.setting.c_king = trackBar6.Value;
            Program.setting.turns = trackBar7.Value;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("set.st", FileMode.Create))
            {
                formatter.Serialize(fs, Program.setting);
            }
            this.Close();
        }
    }
}
