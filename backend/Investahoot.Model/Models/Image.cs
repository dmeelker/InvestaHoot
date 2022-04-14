using Investahoot.Model.Vestaboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investahoot.Model.Models
{
    public class Image
    {
        public const int Width = 22;
        public const int Height = 6;

        private char[,] _cells;
        public char[,] Characters => _cells;

        public Image()
        {
            _cells = EmptyImage();
        }

        private char[,] EmptyImage()
        {
            var cells = new char[Height, Width];

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    cells[y, x] = ' ';
                }
            }

            return cells;
        }

        public void Set(int x, int y, char value)
        {
            _cells[y, x] = value;
        }

        public List<List<int>> ToVestaboardImage()
        {
            var result = new List<List<int>>();

            for (var y = 0; y < Height; y++)
            {
                var line = new List<int>();

                for (var x = 0; x < Width; x++)
                {
                    line.Add(BitMapping.ConvertToCode(_cells[y, x]));
                }
                result.Add(line);
            }

            return result;
        }
    }
}
