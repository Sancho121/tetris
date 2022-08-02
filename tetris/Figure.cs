using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    class Figure
    {
        public int figurePositionX;
        public int figurePositionY;

        private int stateRotateFigure = 0;
        private static Random random = new Random();

        private int referencePointStructureFigureX;
        private int referencePointStructureFigureY;

        private FigureType type;
        public int[,] structureFigure;

        private Figure(FigureType type, int[,] structureFigure, int referencePointStructureFigureY, int referencePointStructureFigureX, int figurePositionX, int figurePositionY)
        {
            this.type = type;
            this.structureFigure = structureFigure;
            this.referencePointStructureFigureY = referencePointStructureFigureY;
            this.referencePointStructureFigureX = referencePointStructureFigureX;
            this.figurePositionX = figurePositionX;
            this.figurePositionY = figurePositionY;
        }

        public void DrawFigure(Graphics graphics, int cellSize)
        {
            foreach (Point point in GetFigurePoints())
            {
                graphics.FillRectangle(Brushes.BlueViolet, point.X * cellSize, point.Y * cellSize, cellSize, cellSize);
            }
        }

        public List<Point> GetFigurePoints()
        {
            List<Point> figurePoints = new List<Point>();

            for (int y = 0; y < structureFigure.GetLength(0); y++)
            {
                for (int x = 0; x < structureFigure.GetLength(1); x++)
                {
                    if (structureFigure[y, x] == 1)
                        figurePoints.Add(new Point(figurePositionX + x, figurePositionY + y));
                }
            }
            return figurePoints;
        }

        public static Figure CreateRandomFigure(int figurePositionX, int figurePositionY)
        {
            FigureType type = (FigureType)random.Next(Enum.GetNames(typeof(FigureType)).Length);
            switch (type)
            {
                case FigureType.O:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 1, 1, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    }, 0, 0, figurePositionX, figurePositionY);
                case FigureType.J:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {0, 0, 1, 0},
                        {0, 0, 1, 0},
                        {0, 1, 1, 0},
                    }, 2, 2, figurePositionX, figurePositionY);
                case FigureType.L:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {0, 1, 0, 0},
                        {0, 1, 0, 0},
                        {0, 1, 1, 0},
                    }, 2, 1, figurePositionX, figurePositionY);
                case FigureType.S:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {0, 1, 1, 0},
                        {1, 1, 0, 0},
                        {0, 0, 0, 0},
                    }, 2, 1, figurePositionX, figurePositionY);
                case FigureType.Z:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {1, 1, 0, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                    }, 2, 1, figurePositionX, figurePositionY);
                case FigureType.T:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {1, 1, 1, 0},
                        {0, 1, 0, 0},
                        {0, 0, 0, 0},
                    }, 1, 1, figurePositionX, figurePositionY);
                case FigureType.I:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {1, 1, 1, 1},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0}
                    }, 1, 1, figurePositionX, figurePositionY);
                case FigureType.Point:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    }, 0, 0, figurePositionX, figurePositionY);
                default:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    }, 0, 0, figurePositionX, figurePositionY);
            }
        }

        public void RotateFigure()
        {
            int referencePointFigureX = referencePointStructureFigureX + figurePositionX;
            int referencePointFigureY = referencePointStructureFigureY + figurePositionY;

            if (type == FigureType.O || type == FigureType.Point)
                return;

            if (type == FigureType.I || type == FigureType.S || type == FigureType.Z)
            {
                if (stateRotateFigure == 0)
                {
                    stateRotateFigure++;
                    RotateFigureClockWise(GetFigurePoints(), referencePointFigureX, referencePointFigureY);
                }
                else
                {
                    stateRotateFigure--;
                    RotateFigureCounterClockWise(GetFigurePoints(), referencePointFigureX, referencePointFigureY);
                }
            }

            if (type == FigureType.T || type == FigureType.J || type == FigureType.L)
            {
                RotateFigureClockWise(GetFigurePoints(), referencePointFigureX, referencePointFigureY);
            }
        }

        private void RotateFigureClockWise(List<Point> figurePoints, int pX, int pY)
        {
            Array.Clear(structureFigure, 0, structureFigure.Length);
            figurePoints.ConvertAll(point => new Point(pX + pY - point.Y, pY + point.X - pX))
                        .ForEach(point => structureFigure[point.Y - figurePositionY, point.X - figurePositionX] = 1);
        }

        private void RotateFigureCounterClockWise(List<Point> figurePoints, int pX, int pY)
        {
            Array.Clear(structureFigure, 0, structureFigure.Length);
            figurePoints.ConvertAll(point => new Point(point.Y - pY + pX, pX + pY - point.X))
                        .ForEach(point => structureFigure[point.Y - figurePositionY, point.X - figurePositionX] = 1);
        }
    }
}
