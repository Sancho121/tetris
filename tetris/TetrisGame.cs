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
        private int[,] gameField = new int[20, 10]; // 1 - заполненная клетка, 0 - пустая

        private int shapePositionX = 4;
        private int shapePositionY = 0;
        private int numberShape;

        Figure figure;

        private Random random = new Random();

        private List<int> filledCellsInLine = new List<int>();
        //private List<int[,]> shapes = new List<int[,]>()
        //{
        //    new int[3, 3]
        //    {
        //        {0, 1, 0},
        //        {0, 0, 0},
        //        {0, 0, 0}
        //    },

        //    new int[3, 3]
        //    {
        //        {1, 1, 1},
        //        {0, 1, 0},
        //        {0, 0, 0}
        //    },

        //    new int[2, 2]
        //    {
        //        {1, 1},
        //        {1, 1}
        //    },

        //    new int[3, 3]
        //    {
        //        {1, 1, 0},
        //        {0, 1, 1},
        //        {0, 0, 0}
        //    },

        //    new int[3, 3]
        //    {
        //        {0, 1, 0},
        //        {0, 1, 0},
        //        {0, 1, 1}
        //    },

        //    new int[3, 3]
        //    {
        //        {0, 1, 0},
        //        {0, 1, 0},
        //        {1, 1, 0}
        //    },

        //    new int[3, 3]
        //    {
        //        {0, 1, 1},
        //        {1, 1, 0},
        //        {0, 0, 0}
        //    },

        //    new int[4, 4]
        //    {
        //        {1, 1, 1, 1},
        //        {0, 0, 0, 0},
        //        {0, 0, 0, 0},
        //        {0, 0, 0, 0}
        //    }
        //};

        public int Points { get; set; }
    
        public TetrisGame(int gameFieldHeightInCells, int gameFieldWidthInCells, int cellSize)
        {
            this.gameFieldHeightInCells = gameFieldHeightInCells;
            this.gameFieldWidthInCells = gameFieldWidthInCells;
            this.cellSize = cellSize;
            numberShape = random.Next(0, 7);
        }

        public Figure CreateNewFigure(FigureType type, int[,] a)
        {
            return new Figure(type, a);
        }

        public void Draw(Graphics graphics)
        {
            PreparationGameFieldForDrawShape();
            for (int i = 0; i < gameFieldHeightInCells; i++)
            {
                for (int j = 0; j < gameFieldWidthInCells; j++)
                {
                    if (gameField[i, j] == 1)
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
        public void Restart()
        {   
            Points = 0;
            Array.Clear(gameField, 0, gameField.Length);
            figure = CreateNewFigure((FigureType)random.Next(0, 7), new int[4, 4]);
        }

        public void Update()
        {
            ClearArea();
            if (CheckBottomBorderMap() || CheckFigureMapBelowShape())
            {
                PreparationGameFieldForDrawShape();
                ClearFullLines();
                shapePositionX = 4;
                shapePositionY = 0;
                numberShape = random.Next(0, 7);
            }
            else
            {
                shapePositionY++;
            }

            if (GameOver())
            {
                Restart();
                //MessageBox.Show("gg wp");               
            }
        }

        public void Move(Keys direction)
        {          
            switch (direction)
            {
                case Keys.Left:
                    ClearArea();
                    if (CheckLeftBorderMap() && CheckFigureMapLeftShape())
                    {
                        shapePositionX--;
                    }
                    break;
                case Keys.Right:
                    ClearArea();
                    if (CheckRightBorderMap() && CheckFigureMapRightShape())
                    {
                        shapePositionX++;                                      
                    }
                    break;
                case Keys.Down:                   
                    ClearArea();
                    if (!CheckBottomBorderMap() && !CheckFigureMapBelowShape())
                    {
                        shapePositionY++;
                    }
                    break;
                case Keys.Space:                  
                    ClearArea();
                    shapePositionY += DistanceBetweenShapeAndFigureGameField();
                    PreparationGameFieldForDrawShape();
                    ClearFullLines();
                    shapePositionX = 4;
                    shapePositionY = 0;
                    numberShape = random.Next(0, 7);                   
                    break;
            }
        }

        private void PreparationGameFieldForDrawShape()
        {         
            for (int i = 0; i < figure.structure.GetLength(0); i++)
            {
                for (int j = 0; j < figure.structure.GetLength(1); j++)
                {
                    if (figure.structure[i, j] == 1)
                        gameField[shapePositionY + i, shapePositionX + j] = 1;
                }
                
            }
        }

        private void ClearArea()
        {
            for (int y = shapePositionY; y < shapePositionY + figure.structure.GetLength(0); y++)
            {
                for (int x = shapePositionX; x < shapePositionX + figure.structure.GetLength(1); x++)
                {                   
                    if (x >= 0 && y >= 0 && x < gameFieldWidthInCells && y < gameFieldHeightInCells)
                    {
                        if(figure.structure[y - shapePositionY, x - shapePositionX] == 1)
                        {
                            gameField[y, x] = 0;
                        }
                    }
                }
            }
        }

        private bool CheckFigureMapBelowShape()
        {
            for (int y = shapePositionY; y < shapePositionY + figure.structure.GetLength(0); y++)
            {
                for (int x = shapePositionX; x < shapePositionX + figure.structure.GetLength(1); x++)
                {
                    if (figure.structure[y - shapePositionY, x - shapePositionX] == 1 && 
                       (figure.structure[y - shapePositionY, x - shapePositionX] == gameField[y + 1, x]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckFigureMapLeftShape()
        {
            for (int y = shapePositionY; y < shapePositionY + figure.structure.GetLength(0); y++)
            {
                for (int x = shapePositionX; x < shapePositionX + figure.structure.GetLength(1); x++)
                {
                    if (figure.structure[y - shapePositionY, x - shapePositionX] == 1 &&
                        figure.structure[y - shapePositionY, x - shapePositionX] == gameField[y, x - 1])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckFigureMapRightShape()
        {
            for (int y = shapePositionY; y < shapePositionY + figure.structure.GetLength(0); y++)
            {
                for (int x = shapePositionX; x < shapePositionX + figure.structure.GetLength(1); x++)
                {
                    if (figure.structure[y - shapePositionY, x - shapePositionX] == 1 &&
                        figure.structure[y - shapePositionY, x - shapePositionX] == gameField[y, x + 1])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckBottomBorderMap()
        {
            for(int y = shapePositionY + figure.structure.GetLength(0) - 1; y >= shapePositionY; y--)
            {
                for(int x = shapePositionX; x < shapePositionX + figure.structure.GetLength(1); x++)
                {
                    if (figure.structure[y - shapePositionY, x - shapePositionX] == 1 && gameFieldHeightInCells == y + 1)                   
                        return true;
                }
            }
            return false;
        }

        private bool CheckLeftBorderMap()
        {
            for (int y = shapePositionY; y < shapePositionY + figure.structure.GetLength(0); y++)
            {
                for (int x = shapePositionX; x < shapePositionX + figure.structure.GetLength(1); x++)
                {
                    if (figure.structure[y - shapePositionY, x - shapePositionX] == 1 && x == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckRightBorderMap()
        {
            for (int y = shapePositionY; y < shapePositionY + figure.structure.GetLength(0); y++)
            {
                for (int x = shapePositionX; x < shapePositionX + figure.structure.GetLength(1); x++)
                {
                    if (figure.structure[y - shapePositionY, x - shapePositionX] == 1 && x == gameFieldWidthInCells - 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }     
        
        private void ClearFullLines()
        {
            int countFullLines = 0;
            for(int y = 0; y < gameFieldHeightInCells; y++)
            {
                for(int x = 0; x < gameFieldWidthInCells; x++)
                {
                    if (gameField[y, x] == 1)
                    {
                        filledCellsInLine.Add(1);
                    }
                    else
                    {
                        filledCellsInLine.Clear();
                        break;
                    }
                    if (filledCellsInLine.Count(t => t == 1) == gameFieldWidthInCells)
                    {
                        for (int z = y; z >= 0; z--)
                        {
                            for (int k = 0; k < gameFieldWidthInCells; k++)
                            {
                                gameField[z, k] = z == 0 ? 0 : gameField[z - 1, k];
                            }
                        }
                        filledCellsInLine.Clear();
                        countFullLines++;                        
                    }
                }
            }
            if (countFullLines > 0)
            {
                CountPoints(countFullLines);          
            }
        }

        private void CountPoints(int count)
        {
            Points += (int)(Math.Pow(2, count) * 100 - 100);
        }

        private int DistanceBetweenShapeAndFigureGameField()
        {
            int maxDistance = 0;
            for (int y = shapePositionY + figure.structure.GetLength(0) - 1; y >= shapePositionY; y--)
            {
                for (int x = shapePositionX; x < shapePositionX + figure.structure.GetLength(1); x++)
                {
                    if (figure.structure[y - shapePositionY, x - shapePositionX] == 1)
                    {
                        int distance = 0;
                        for (int t = y + 1; t < gameFieldHeightInCells; t++)
                        {
                            if (gameField[t, x] == 0)
                            {
                                distance++;
                                if (gameFieldHeightInCells - 1 - y == distance && maxDistance == 0)
                                {
                                    maxDistance = distance;
                                }
                            }
                            else
                            {
                                if (distance == 0)
                                {
                                    return 0;
                                }
                                if (maxDistance > distance || maxDistance == 0)
                                {
                                    maxDistance = distance;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return maxDistance;
        }

        private bool GameOver()
        {
            for (int y = shapePositionY + figure.structure.GetLength(0) - 1; y >= shapePositionY; y--)
            {
                for (int x = shapePositionX; x < shapePositionX + figure.structure.GetLength(1); x++)
                {
                    if (figure.structure[y - shapePositionY, x - shapePositionX] == 1 &&
                        figure.structure[y - shapePositionY, x - shapePositionX] == gameField[y, x])
                        return true;
                }
            }
            return false;
        }
    }
}