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

        private int figurePositionX = 4;
        private int figurePositionY = 0;

        private int[,] figure;

        private Random random = new Random();

        private List<int> filledCellsInLine = new List<int>();
        
        public int Points { get; set; }
    
        public TetrisGame(int gameFieldHeightInCells, int gameFieldWidthInCells, int cellSize)
        {
            this.gameFieldHeightInCells = gameFieldHeightInCells;
            this.gameFieldWidthInCells = gameFieldWidthInCells;
            this.cellSize = cellSize;
        }

        public void Draw(Graphics graphics)
        {
            for (int y = figurePositionY; y < figurePositionY + figure.GetLength(0); y++)
            {
                for (int x = figurePositionX; x < figurePositionX + figure.GetLength(1); x++)
                {
                    if (figure[y - figurePositionY, x - figurePositionX] == 1)
                    {
                        graphics.FillRectangle(Brushes.BlueViolet, x * cellSize, y * cellSize, cellSize, cellSize);                       
                    }
                }
            }
            for (int y = 0; y < gameFieldHeightInCells; y++)
            {
                for (int x = 0; x < gameFieldWidthInCells; x++)
                {
                    if (gameField[y, x] == 1)
                    {
                        graphics.FillRectangle(Brushes.BlueViolet, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                }
            }

            for (int y = 0; y <= gameFieldHeightInCells; y++)
            {
                graphics.DrawLine(Pens.Black, 0, cellSize * y, cellSize * gameFieldWidthInCells, cellSize * y);
            }
            for (int x = 0; x <= gameFieldWidthInCells; x++)
            {
                graphics.DrawLine(Pens.Black, cellSize * x, 0, cellSize * x, cellSize * gameFieldHeightInCells);
            }
        }

        private void AddFigureIngameField()
        {
            for (int y = 0; y < figure.GetLength(0); y++)
            {
                for (int x = 0; x < figure.GetLength(1); x++)
                {
                    if (figure[y, x] == 1)
                        gameField[figurePositionY + y, figurePositionX + x] = 1;
                }
            }
        }

        public void Restart()
        {   
            Points = 0;
            Array.Clear(gameField, 0, gameField.Length);
            figure = Figure.CreateNewFigure((FigureType)random.Next(0, 7));
        }

        public void Update()
        {
            if (CheckBottomBorderMap() || CheckFigureMapBelowShape())
            {
                AddFigureIngameField();
                ClearFullLines();
                figurePositionX = 4;
                figurePositionY = 0;
                figure = Figure.CreateNewFigure((FigureType)random.Next(0, 7));
            }
            else
            {
                figurePositionY++;
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
                    if (CheckLeftBorderMap() && CheckFigureMapLeftShape())
                    {
                        figurePositionX--;
                    }
                    break;
                case Keys.Right:
                    if (CheckRightBorderMap() && CheckFigureMapRightShape())
                    {
                        figurePositionX++;                                      
                    }
                    break;
                case Keys.Down:                   
                    if (!CheckBottomBorderMap() && !CheckFigureMapBelowShape())
                    {
                        figurePositionY++;
                    }
                    break;
                case Keys.Space:
                    figurePositionY += DistanceBetweenShapeAndFigureGameField();
                    AddFigureIngameField();
                    ClearFullLines();
                    figurePositionX = 4;
                    figurePositionY = 0;
                    figure = Figure.CreateNewFigure((FigureType)random.Next(0, 7));
                    break;
            }
        }     

        private bool CheckFigureMapBelowShape()
        {
            for (int y = figurePositionY; y < figurePositionY + figure.GetLength(0); y++)
            {
                for (int x = figurePositionX; x < figurePositionX + figure.GetLength(1); x++)
                {
                    if (figure[y - figurePositionY, x - figurePositionX] == 1 && 
                       (figure[y - figurePositionY, x - figurePositionX] == gameField[y + 1, x]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckFigureMapLeftShape()
        {
            for (int y = figurePositionY; y < figurePositionY + figure.GetLength(0); y++)
            {
                for (int x = figurePositionX; x < figurePositionX + figure.GetLength(1); x++)
                {
                    if (figure[y - figurePositionY, x - figurePositionX] == 1 &&
                        figure[y - figurePositionY, x - figurePositionX] == gameField[y, x - 1])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckFigureMapRightShape()
        {
            for (int y = figurePositionY; y < figurePositionY + figure.GetLength(0); y++)
            {
                for (int x = figurePositionX; x < figurePositionX + figure.GetLength(1); x++)
                {
                    if (figure[y - figurePositionY, x - figurePositionX] == 1 &&
                        figure[y - figurePositionY, x - figurePositionX] == gameField[y, x + 1])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckBottomBorderMap()
        {
            for(int y = figurePositionY + figure.GetLength(0) - 1; y >= figurePositionY; y--)
            {
                for(int x = figurePositionX; x < figurePositionX + figure.GetLength(1); x++)
                {
                    if (figure[y - figurePositionY, x - figurePositionX] == 1 && gameFieldHeightInCells == y + 1)                   
                        return true;
                }
            }
            return false;
        }

        private bool CheckLeftBorderMap()
        {
            for (int y = figurePositionY; y < figurePositionY + figure.GetLength(0); y++)
            {
                for (int x = figurePositionX; x < figurePositionX + figure.GetLength(1); x++)
                {
                    if (figure[y - figurePositionY, x - figurePositionX] == 1 && x == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckRightBorderMap()
        {
            for (int y = figurePositionY; y < figurePositionY + figure.GetLength(0); y++)
            {
                for (int x = figurePositionX; x < figurePositionX + figure.GetLength(1); x++)
                {
                    if (figure[y - figurePositionY, x - figurePositionX] == 1 && x == gameFieldWidthInCells - 1)
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
            for (int y = figurePositionY + figure.GetLength(0) - 1; y >= figurePositionY; y--)
            {
                for (int x = figurePositionX; x < figurePositionX + figure.GetLength(1); x++)
                {
                    if (figure[y - figurePositionY, x - figurePositionX] == 1)
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
            for (int y = figurePositionY + figure.GetLength(0) - 1; y >= figurePositionY; y--)
            {
                for (int x = figurePositionX; x < figurePositionX + figure.GetLength(1); x++)
                {
                    if (figure[y - figurePositionY, x - figurePositionX] == 1 &&
                        figure[y - figurePositionY, x - figurePositionX] == gameField[y, x])
                        return true;
                }
            }
            return false;
        }
    }
}