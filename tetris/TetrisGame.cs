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
        private int[,] gameField;

        private Figure figure;
        private Figure nextFigure;

        private Random random = new Random();

        public int Points { get; set; }

        public TetrisGame(int gameFieldHeightInCells, int gameFieldWidthInCells, int cellSize)
        {
            this.gameFieldHeightInCells = gameFieldHeightInCells;
            this.gameFieldWidthInCells = gameFieldWidthInCells;
            this.cellSize = cellSize;
            gameField = new int[gameFieldHeightInCells, gameFieldWidthInCells];
        }

        public void Draw(Graphics graphics)
        {
            figure.DrowFigure(graphics, cellSize);
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

        public void DrawNextFigure(Graphics graphics)
        {
            graphics.DrawRectangle(new Pen(Color.Black), 0, 0, 80, 80);
            
            for (int y = 0; y < nextFigure.structureFigure.GetLength(0); y++)
            {
                for (int x = 0; x < nextFigure.structureFigure.GetLength(1); x++)
                {
                    if (nextFigure.structureFigure[y, x] == 1)
                    {
                        graphics.FillRectangle(Brushes.BlueViolet, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                }
            }
        }

        public void Restart()
        {
            Points = 0;
            Array.Clear(gameField, 0, gameField.Length);
            figure = Figure.CreateNewFigure((FigureType)random.Next(Enum.GetNames(typeof(FigureType)).Length));
            nextFigure = Figure.CreateNewFigure((FigureType)random.Next(Enum.GetNames(typeof(FigureType)).Length));
        }

        public void Update()
        {
            if (IsPossibleMoveDown())
            {
                figure.figurePositionY++;
            }
            else
            {
                AddFigureIngameField();
                ClearFullLines();
                figure.figurePositionX = 4;
                figure.figurePositionY = 0;
                figure = Figure.CreateNewFigure(nextFigure.type);
                nextFigure = Figure.CreateNewFigure((FigureType)random.Next(Enum.GetNames(typeof(FigureType)).Length));
            }

            if (GameOver())
            {
                Restart();
            }
        }

        public void Move(Keys direction)
        {
            switch (direction)
            {
                case Keys.Left:
                    if (IsPossibleMoveLeft())
                    {
                        figure.figurePositionX--;
                    }
                    break;
                case Keys.Right:
                    if (IsPossibleMoveRight())
                    {
                        figure.figurePositionX++;
                    }
                    break;
                case Keys.Down:
                    if (IsPossibleMoveDown())
                    {
                        figure.figurePositionY++;
                    }
                    break;
                case Keys.Space:
                    while (IsPossibleMoveDown())
                    {
                        figure.figurePositionY++;
                    }
                    Update();
                    break;
            }
        }
        private void AddFigureIngameField()
        {
            foreach (Point point in figure.GetFigurePoints())
            {
                gameField[point.Y, point.X] = 1;
            }
        }

        private bool IsPossibleMoveLeft()
        {
            return figure.GetFigurePoints().All(point => point.X != 0 && gameField[point.Y, point.X - 1] == 0);
        }

        private bool IsPossibleMoveRight()
        {
            return figure.GetFigurePoints().All(point => point.X != gameFieldWidthInCells - 1 && gameField[point.Y, point.X + 1] == 0);
        }

        private bool IsPossibleMoveDown()
        {
            return figure.GetFigurePoints().All(point => point.Y != gameFieldHeightInCells - 1 && gameField[point.Y + 1, point.X] == 0);
        }

        private bool GameOver()
        {
            return figure.GetFigurePoints().Any(point => gameField[point.Y, point.X] == 1);
        }

        private void ClearFullLines()
        {
            int countFullLines = 0;
            for (int y = 0; y < gameFieldHeightInCells; y++)
            {
                int filledCellsInLine = 0;
                for (int x = 0; x < gameFieldWidthInCells; x++)
                {
                    if (gameField[y, x] == 1)
                    {
                        filledCellsInLine++;
                    }
                    else
                    {
                        filledCellsInLine = 0;
                        break;
                    }
                    if (filledCellsInLine == gameFieldWidthInCells)
                    {
                        for (int z = y; z >= 0; z--)
                        {
                            for (int k = 0; k < gameFieldWidthInCells; k++)
                            {
                                gameField[z, k] = z == 0 ? 0 : gameField[z - 1, k];
                            }
                        }
                        filledCellsInLine = 0;
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
    }
}