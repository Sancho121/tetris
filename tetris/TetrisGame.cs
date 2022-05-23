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

        private List<int> filledCellsInLine = new List<int>();
        

        private int[,] shape = new int[3, 3]
        {
            {0, 1, 0},
            {0, 1, 0},
            {0, 1, 0}
        };
        public int Points { get; set; }

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
            if (CheckBottomBorderMap() || CheckFigureMapBelowShape())
            {
                DrawShape();
                ClearFullLines();
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
                    if (CheckLeftBorderMap() && CheckFigureMapLeftShape())
                    {
                        startPositionX--;
                    }
                    break;
                case Keys.Right:
                    ClearArea();
                    if (CheckRightBorderMap() && CheckFigureMapRightShape())
                    {
                        startPositionX++;                                      
                    }
                    break;
                case Keys.Down:
                    ClearArea();
                    if (!CheckBottomBorderMap() && !CheckFigureMapBelowShape())
                    {
                        startPositionY++;
                    }
                    break;
                case Keys.Space:
                    ClearArea();
                    CheckPositionYBeforeKeySpace();                   
                    break;
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

        private bool CheckFigureMapBelowShape()
        {
            for (int y = startPositionY; y < startPositionY + 3; y++)
            {
                for (int x = startPositionX; x < startPositionX + 3; x++)
                {
                    if (shape[y - startPositionY, x - startPositionX] == 1 && 
                       (shape[y - startPositionY, x - startPositionX] == map[y + 1, x]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckFigureMapLeftShape()
        {
            for (int y = startPositionY; y < startPositionY + 3; y++)
            {
                for (int x = startPositionX; x < startPositionX + 3; x++)
                {
                    if (shape[y - startPositionY, x - startPositionX] == 1 &&
                        shape[y - startPositionY, x - startPositionX] == map[y, x - 1])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckFigureMapRightShape()
        {
            for (int y = startPositionY; y < startPositionY + 3; y++)
            {
                for (int x = startPositionX; x < startPositionX + 3; x++)
                {
                    if (shape[y - startPositionY, x - startPositionX] == 1 &&
                        shape[y - startPositionY, x - startPositionX] == map[y, x + 1])
                    {
                        return false;
                    }
                }
            }
            return true;
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

        private bool CheckLeftBorderMap()
        {
            for (int y = startPositionY; y < startPositionY + 3; y++)
            {
                for (int x = startPositionX; x < startPositionX + 3; x++)
                {
                    if (shape[y - startPositionY, x - startPositionX] == 1 && x == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckRightBorderMap()
        {
            for (int y = startPositionY; y < startPositionY + 3; y++)
            {
                for (int x = startPositionX; x < startPositionX + 3; x++)
                {
                    if (shape[y - startPositionY, x - startPositionX] == 1 && x == gameFieldWidthInCells - 1)
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
                    if (map[y, x] == 1)
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
                        for (int z = y; z > 0; z--)
                        {
                            for (int k = 0; k < gameFieldWidthInCells; k++)
                            {
                                map[z, k] = map[z - 1, k];
                            }
                        }
                        filledCellsInLine.Clear();
                        countFullLines++;                        
                    }
                }
            }
            CountPoints(countFullLines);          
        }

        private void CountPoints(int count)
        {
            if(count == 1)
            {
                Points += 100;
            }
            if(count == 2)
            {
                Points += 300;
            }
            if(count == 3)
            {
                Points += 700;
            }
            if(count == 4)
            {
                Points += 1500;
            }
        }

        private void CheckPositionYBeforeKeySpace()
        {
            int maxDistance = 0;
            for (int y = startPositionY + 2; y >= startPositionY; y--)
            {
                for (int x = startPositionX; x < startPositionX + 3; x++)
                {
                    if (shape[y - startPositionY, x - startPositionX] == 1)
                    {
                        int distance = 0;
                        for (int t = y + 1; t < gameFieldHeightInCells; t++)
                        {
                            if (map[t, x] == 0)
                            {                                
                                distance++;
                                if (gameFieldHeightInCells - 1 - y == distance && maxDistance == 0)
                                {
                                    maxDistance = distance;
                                }
                            }
                            else
                            {
                                if (maxDistance == 0 || maxDistance >= distance)
                                {
                                    maxDistance = distance;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            startPositionY += maxDistance;
        }
    }
}