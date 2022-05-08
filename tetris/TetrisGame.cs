using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetris
{
    class TetrisGame
    {
        private readonly int gameFieldHeightInCells;
        private readonly int gameFieldWidthInCells;
        private readonly int cellSize;
        private int[,] map = new int[20, 10]; // 1 - заполненная клетка, 0 - пустая

        private int startPositionX = 4;
        private int startPositionY = 0;        
        
        private int[,] shape = new int[3, 3]
        {
            {0, 0, 0},
            {0, 1, 0},
            {0, 0, 0}
        };

        public TetrisGame(int gameFieldHeightInCells, int gameFieldWidthInCells, int cellSize)
        {
            this.gameFieldHeightInCells = gameFieldHeightInCells;
            this.gameFieldWidthInCells = gameFieldWidthInCells;
            this.cellSize = cellSize;
        }

        public void DrawShape()
        {
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] == 1)
                        map[startPositionY + i, startPositionX + j] = 1;
                }
            }
        }

        public void Draw(Graphics graphics)
        {           
            for (int i = 0; i < gameFieldHeightInCells; i++)
            {
                for (int j = 0; j < gameFieldWidthInCells; j++)
                {
                    if (map[i, j] == 1)
                    {
                        graphics.FillRectangle(Brushes.BlueViolet, j * cellSize, i * cellSize, cellSize, cellSize);
                    }
                }
            }

            for (int i = 0; i <= gameFieldHeightInCells; i++)
            {
                graphics.DrawLine(Pens.Black, 0, cellSize * i, cellSize * gameFieldWidthInCells, cellSize * i);
            }

            for (int j = 0; j <= gameFieldWidthInCells; j++)
            {
                graphics.DrawLine(Pens.Black, cellSize * j, 0, cellSize * j, cellSize * gameFieldHeightInCells);
            }
        }

        public void Update()
        {            
            ClearArea();         
            if (CheckBottomBorderMap())
            {
                DrawShape();               
                startPositionX = 4;
                startPositionY = 0;
            }
            else
            {
                startPositionY++;
            }           
        }

        public void Move(Keys direction)
        {
            switch (direction)
            {
                case Keys.Left:
                    ClearArea();
                    if (CheckLeftBorder())
                    {
                        startPositionX--;
                    }
                    break;
                case Keys.Right:
                    ClearArea();
                    startPositionX++;
                    break;
                case Keys.Down:
                    ClearArea();
                    startPositionY++;
                    break;
                //case Keys.Space:
                //    ClearArea();
                //    startPositionY = gameFieldHeightInCells - 2;
                //    break;
            }
        }

        private void ClearArea()
        {
            for (int y = startPositionY; y < startPositionY + 3; y++)
            {
                for (int x = startPositionX; x < startPositionX + 3; x++)
                {                   
                    if (x >= 0 && y >= 0 && x < gameFieldWidthInCells && y < gameFieldHeightInCells)
                    {
                        if(shape[y - startPositionY, x - startPositionX] == 1)
                        {
                            map[y, x] = 0;
                        }
                    }
                }
            }
        }

        private bool CheckBottomBorderMap()
        {
            for(int y = startPositionY + 2; y >= startPositionY; y--)
            {
                for(int x = startPositionX; x < startPositionX + 3; x++)
                {
                    if (shape[y - startPositionY, x - startPositionX] == 1 && gameFieldHeightInCells == y + 1)                   
                        return true;
                }
            }
            return false;
        }


        private bool CheckLeftBorder()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    switch (x)
                    {
                        case 0:
                            if (shape[y, x] == 1 && startPositionX > 0)
                            {
                                return true;
                            }
                            break;
                        case 1:
                            if (shape[y, x] == 1 && startPositionX + 1 > 0 && CheckColumnOne())
                            {
                                return true;
                            }
                            break;
                        case 2:
                            if (shape[y, x] == 1 && startPositionX + 2 > 0 && CheckColumnTwo())
                            {
                                return true;
                            }
                            break;
                    }
                }
            }
            return false;
        }

        private bool CheckColumnOne()
        {
            return shape[0, 0] == 0 && 
                   shape[1, 0] == 0 && 
                   shape[2, 0] == 0;
        }

        private bool CheckColumnTwo()
        {
            return shape[0, 1] == 0 &&
                   shape[1, 1] == 0 &&
                   shape[2, 1] == 0 &&
                   CheckColumnOne();
        }
    }
}