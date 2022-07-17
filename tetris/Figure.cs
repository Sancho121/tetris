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

        public FigureType type;
        public int[,] structureFigure;

        public Figure(FigureType type, int[,] structureFigure)
        {
            this.type = type;
            this.structureFigure = structureFigure;
        }

        public void DrowFigure(Graphics graphics, int cellSize)
        {
            foreach (Point point in GetFigurePoints())
            {
                graphics.FillRectangle(Brushes.BlueViolet, point.X * cellSize, point.Y * cellSize, cellSize, cellSize);
            }
        }

        public Point[] GetFigurePoints()
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
            return figurePoints.ToArray();
        }

        public static Figure CreateNewFigure(FigureType type)
        {
            switch (type)
            {
                case FigureType.O:
                    return new Figure (type, new int[4, 4]
                    {
                        {0, 1, 1, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    });
                case FigureType.J:
                    return new Figure (type, new int[4, 4]
                    {
                        {0, 0, 1, 0},
                        {0, 0, 1, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                    });
                case FigureType.L:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 1, 0, 0},
                        {0, 1, 0, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                    });
                case FigureType.S:
                    return new Figure (type, new int[4, 4]
                    {
                        {0, 0, 1, 1},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    });
                case FigureType.Z:
                    return new Figure (type, new int[4, 4]
                    {
                        {0, 1, 1, 0},
                        {0, 0, 1, 1},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    });
                case FigureType.T:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 1, 1, 1},
                        {0, 0, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    });
                case FigureType.I:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {1, 1, 1, 1},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0}
                    });
                case FigureType.Point:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    });
                default:
                    return new Figure(type, new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    });
            }
        }
    }
}
