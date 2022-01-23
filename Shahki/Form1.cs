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
    public partial class Form1 : Form
    {
        //public static Setting setting;
        public event OnKlick_event OnKlick;
        Field field;
        private bool over = false, draw = false;
        public Form1()
        {
            InitializeComponent();
            Program.setting = new Setting();
            BinaryFormatter formatter = new BinaryFormatter();
            if (File.Exists("set.st"))
            {
                using (FileStream fs = new FileStream("set.st", FileMode.Open))
                {
                    Program.setting = (Setting)formatter.Deserialize(fs);
                }
            }
            else
            {
                Program.setting.Set(0, 0, 0, 0, 0, 0, 2);
                using (FileStream fs = new FileStream("set.st", FileMode.Create))
                {
                    formatter.Serialize(fs, Program.setting);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void hotseatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw = false;
            if (field != null)
            {
                field.Over -= Form1_Over;
                field.Delete(this);
            }
            over = false;
            Graphics g = pictureBox1.CreateGraphics();
            field = null;
            field = new Field(0, 0, 1, g, this);
            field.Over += Form1_Over;
            field.Mes += Form1_Mes;
            label2.Text = "Ход белых";
            saveToolStripMenuItem.Enabled = true;
            сдатьсяToolStripMenuItem.Enabled = true;
            предложитьНичьюToolStripMenuItem.Enabled = true;
        }

        private void Form1_Mes(string mes)
        {
            label2.Text = mes;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (field != null && !over)
            {
                OnKlick?.Invoke(e.X, e.Y, draw);
                draw = false;
            }
        }

        private void Form1_Over()
        {
            over = true;
            saveToolStripMenuItem.Enabled = false;
            сдатьсяToolStripMenuItem.Enabled = false;
            предложитьНичьюToolStripMenuItem.Enabled = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "save files (*.sav)|*.sav";
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Сохранения"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Сохранения");
            }
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Сохранения";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                if (filename != "")
                {
                    //Field.Save(filename, field);
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream fs = new FileStream(filename, FileMode.Create))
                    {
                        formatter.Serialize(fs, field);
                    }
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "save files (*.sav)|*.sav";
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Сохранения"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Сохранения");
            }
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Сохранения";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                if (filename != "")
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream fs = new FileStream(filename, FileMode.Open))
                    {
                        if (field != null)
                        {
                            field.Over -= Form1_Over;
                            field.Delete(this);
                        }
                        over = false;
                        draw = false;
                        if (field != null)
                        {
                            field.Over -= Form1_Over;
                            field.Mes -= Form1_Mes;
                        }
                        field = null;
                        field = (Field)formatter.Deserialize(fs);
                        field.G(pictureBox1.CreateGraphics(), this);
                        field.Over += Form1_Over;
                        field.Mes += Form1_Mes;
                        field.Show();
                        if (field.player == 1)
                        {
                            label2.Text = "Ход белых";
                        }
                        else
                        {
                            label2.Text = "Ход черных";
                        }
                    }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void предложитьНичьюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw = true;
        }

        private void botToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw = false;
            if (field != null)
            {
                field.Over -= Form1_Over;
                field.Delete(this);
            }
            over = false;
            Graphics g = pictureBox1.CreateGraphics();
            field = null;
            field = new Field(0, 0, 2, g, this);
            field.Over += Form1_Over;
            field.Mes += Form1_Mes;
            label2.Text = "Ход белых";
            saveToolStripMenuItem.Enabled = true;
            сдатьсяToolStripMenuItem.Enabled = true;
            //предложитьНичьюToolStripMenuItem.Enabled = true;
            DialogResult result = MessageBox.Show("Выбранная цвет - белый?", "Выберите цвет шашки", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                field.Bot_turn();
            }
        }

        private void сдатьсяToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            over = true;
            if (field.player == 2)
            {
                label2.Text = "Победили черные";
            }
            else
            {
                label2.Text = "Победили белые";
            }
        }
    }
}
