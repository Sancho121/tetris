using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    enum FigureType
    {
        O,
        J,
        L,
        S,
        Z,
        T,
        I,
        Point
    }

    class Figure
    {
        public FigureType type;
        public int[,] structure;

        public Figure(FigureType type, int[,] structure)
        {
            this.type = type;
            this.structure = structure;
        }

        private List<Figure> shapes1 = new List<Figure>()
        {
            new Figure(FigureType.O, new int[4, 4]
            {
                {0, 1, 1, 0},
                {0, 1, 1, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            }),

            new Figure(FigureType.J, new int[4, 4]
            {
                {0, 0, 1, 0},
                {0, 0, 1, 0},
                {0, 1, 1, 0},
                {0, 0, 0, 0},
            }),

            new Figure(FigureType.L, new int[4, 4]
            {
                {0, 1, 0, 0},
                {0, 1, 0, 0},
                {0, 1, 1, 0},
                {0, 0, 0, 0},
            }),

            new Figure(FigureType.S, new int[4, 4]
            {
                {0, 0, 1, 1},
                {0, 1, 1, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            }),

            new Figure(FigureType.Z, new int[4, 4]
            {
                {0, 1, 1, 0},
                {0, 0, 1, 1},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            }),

            new Figure(FigureType.T, new int[4, 4]
            {
                {0, 1, 1, 1},
                {0, 0, 1, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            }),

            new Figure(FigureType.I, new int[4, 4]
            {
                {0, 0, 0, 0},
                {1, 1, 1, 1},
                {0, 0, 0, 0},
                {0, 0, 0, 0}
            }),

            new Figure(FigureType.Point, new int[4, 4]
            {
                {0, 0, 1, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            })
        };
    }
}
