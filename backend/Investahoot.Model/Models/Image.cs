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

        public void SetText(int x, int y, string text)
        {
            if (y < 0 || y >= Height)
                return;

            for (var i = 0; i < text.Length; i++)
            {
                if (x + i >= Width)
                    break;

                _cells[y, x + i] = text[i];
            }
        }

        public void SetCentered(int y, string text)
        {
            if (y < 0 || y >= Height)
                return;

            var x = (Width / 2) - (text.Length / 2);

            for (var i = 0; i < text.Length; i++)
            {
                if (x + i >= Width)
                    break;

                _cells[y, x + i] = text[i];
            }
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

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    sb.Append(_cells[y, x]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
