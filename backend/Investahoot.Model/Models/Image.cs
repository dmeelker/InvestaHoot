using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investahoot.Model.Models
{
    public class Image
    {
        private const int width = 22;
        private const int height = 6;

        private int[,] _cells;

        public Image()
        {
            _cells = EmptyImage();
        }

        private int[,] EmptyImage()
        {
            var cells = new int[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    cells[x, y] = 0;
                }
            }

            return cells;
        }
    }
}
