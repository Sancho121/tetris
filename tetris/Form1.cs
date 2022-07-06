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
        TetrisGame tetrisGame = new TetrisGame(20, 10, 20);

        public Form1()
        {
            InitializeComponent();
            tetrisGame.Restart();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {            
            tetrisGame.Draw(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.tetrisGame.Update();
            label1.Text = tetrisGame.Points.ToString();
            this.pictureBox1.Refresh();            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            this.tetrisGame.Move(e.KeyCode);
            this.pictureBox1.Refresh();
        }
    }
}