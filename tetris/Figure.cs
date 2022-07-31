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
        public int figurePositionX = 4;
        public int figurePositionY = 0;

        private int countRotateFigure = 0;

        public int ReferencePointStructureFigureX { get; private set; }
        public int ReferencePointStructureFigureY { get; private set; }

        public FigureType type;
        public int[,] structureFigure;

        public Figure(FigureType type, int[,] structureFigure, int referencePointStructureFigureY, int referencePointStructureFigureX)
        {
            this.type = type;
            this.structureFigure = structureFigure;
            ReferencePointStructureFigureY = referencePointStructureFigureY;
            ReferencePointStructureFigureX = referencePointStructureFigureX;
        }

        public void DrowFigure(Graphics graphics, int cellSize)
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

        public List<Point> GetStructureFigurePoints()
        {
            List<Point> figureStructurePoints = new List<Point>();

            for (int y = 0; y < structureFigure.GetLength(0); y++)
            {
                for (int x = 0; x < structureFigure.GetLength(1); x++)
                {
                    if (structureFigure[y, x] == 1)
                        figureStructurePoints.Add(new Point(x, y));
                }
            }
            return figureStructurePoints;
        }

        public static Figure CreateNewFigure(FigureType type)
        {
            switch (type)
            {
                case FigureType.O:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 1, 1, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    }, 0, 0);
                case FigureType.J:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {0, 0, 1, 0},
                        {0, 0, 1, 0},
                        {0, 1, 1, 0},
                    }, 2, 2);
                case FigureType.L:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {0, 1, 0, 0},
                        {0, 1, 0, 0},
                        {0, 1, 1, 0},
                    }, 2, 1);
                case FigureType.S:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {0, 1, 1, 0},
                        {1, 1, 0, 0},
                        {0, 0, 0, 0},
                    }, 2, 1);
                case FigureType.Z:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {1, 1, 0, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                    }, 2, 1);
                case FigureType.T:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {1, 1, 1, 0},
                        {0, 1, 0, 0},
                        {0, 0, 0, 0},
                    }, 1, 1);
                case FigureType.I:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {1, 1, 1, 1},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0}
                    }, 1, 1);
                case FigureType.Point:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    }, 0, 0);
                default:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    }, 0, 0);
            }
        }

        public void RotateFigure()
        {
            int referencePointFigureX = ReferencePointStructureFigureX + figurePositionX;
            int referencePointFigureY = ReferencePointStructureFigureY + figurePositionY;

            if (type == FigureType.O || type == FigureType.Point)
                return;

            if (type == FigureType.I || type == FigureType.S || type == FigureType.Z)
            {
                if (countRotateFigure % 2 == 0)
                {
                    countRotateFigure++;
                    RotateFigureClockWise(GetFigurePoints(), referencePointFigureX, referencePointFigureY);
                }
                else
                {
                    countRotateFigure--;
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
