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

        private int[,] _cells;

        public Image()
        {
            _cells = EmptyImage();
        }

        private int[,] EmptyImage()
        {
            var cells = new int[Width, Height];

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    cells[x, y] = 0;
                }
            }

            return cells;
        }

        public void Set(int x, int y, int value)
        {
            _cells[x, y] = value;
        }
    }
}
