using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetris
{
    public partial class Form1 : Form
    {
        TetrisGame tetris = new TetrisGame(20, 10, 20);

        public Form1()
        {
            InitializeComponent();            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            tetris.DrawShape();
            tetris.Draw(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.tetris.Update();
            label1.Text = tetris.points.ToString();
            this.pictureBox1.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            this.tetris.Move(e.KeyCode);
            this.pictureBox1.Refresh();
        }
    }
}