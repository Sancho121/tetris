using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    class Figure
    {
        public static int[,] CreateNewFigure(FigureType type)
        {
            switch (type)
            {
                case FigureType.O:
                    return new int[4, 4]
                    {
                        {0, 1, 1, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    };
                case FigureType.J:
                    return new int[4, 4]
                    {
                        {0, 0, 1, 0},
                        {0, 0, 1, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                    };
                case FigureType.L:
                    return new int[4, 4]
                    {
                        {0, 1, 0, 0},
                        {0, 1, 0, 0},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                    };
                case FigureType.S:
                    return new int[4, 4]
                    {
                        {0, 0, 1, 1},
                        {0, 1, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    };
                case FigureType.Z:
                    return new int[4, 4]
                    {
                        {0, 1, 1, 0},
                        {0, 0, 1, 1},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    };
                case FigureType.T:
                    return new int[4, 4]
                    {
                        {0, 1, 1, 1},
                        {0, 0, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    };
                case FigureType.I:
                    return new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {1, 1, 1, 1},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0}
                    };
                case FigureType.Point:
                    return new int[4, 4]
                    {
                        {0, 0, 1, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    };
                default:
                    return new int[4, 4]
                    {
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                        {0, 0, 0, 0},
                    };
            }
        }
    }
}
