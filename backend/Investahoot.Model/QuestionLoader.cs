using Investahoot.Model.Models;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
using System.Linq;

namespace Investahoot.Model
{
    public class QuestionLoader
    {
        public List<Question> LoadQuestions(string fileName)
        {
            var content = File.ReadAllText(fileName);
            var models = JsonSerializer.Deserialize<List<QuestionModel>>(content);

            return models!.Select(q => ConvertQuestion(q)).ToList();
        }

        private Question ConvertQuestion(QuestionModel model)
        {
            return new Question()
            {
                Image = ConvertImage(model.Question),
                Answers = model.Answers,
                CorrectAnswerIndex = model.CorrectAnswer,
            };
        }

        private Image ConvertImage(List<List<char>> question)
        {
            var image = new Image();

            for (var x = 0; x < Image.Height; x++)
            {
                for (var y = 0; y < Image.Width; y++)
                {
                    image.Set(x, y, ConvertToCode(question[x][y]));
                }
            }

            return image;
        }

        private int ConvertToCode(char bit)
        {
            var mapping = _bitMapping.Where(entry => entry.Value == bit);

            if (mapping.Any())
            {
                return mapping.Single().Key;
            }
            else
            {
                return 69;
            }
        }

        private Dictionary<int, char> _bitMapping = new()
        {
            { 0, ' ' },
            { 1, 'A' },
            { 2, 'B' },
            { 3, 'C' },
            { 4, 'D' },
            { 5, 'E' },
            { 6, 'F' },
            { 7, 'G' },
            { 8, 'H' },
            { 9, 'I' },
            { 10, 'J' },
            { 11, 'K' },
            { 12, 'L' },
            { 13, 'M' },
            { 14, 'N' },
            { 15, 'O' },
            { 16, 'P' },
            { 17, 'Q' },
            { 18, 'R' },
            { 19, 'S' },
            { 20, 'T' },
            { 21, 'U' },
            { 22, 'V' },
            { 23, 'W' },
            { 24, 'X' },
            { 25, 'Y' },
            { 26, 'Z' },
            { 27, '1' },
            { 28, '2' },
            { 29, '3' },
            { 30, '4' },
            { 31, '5' },
            { 32, '6' },
            { 33, '7' },
            { 34, '8' },
            { 35, '9' },
            { 36, '9' },
            { 37, '!' },
            { 38, '@' },
            { 39, '#' },
            { 40, '$' },
            { 41, '(' },
            { 42, ')' },
            { 44, '-' },
            { 46, '+' },
            { 47, '&' },
            { 48, '=' },
            { 49, ';' },
            { 50, ':' },
            { 52, '\'' },
            { 53, '\"' },
            { 54, '%' },
            { 55, ',' },
            { 56, '.' },
            { 59, '/' },
            { 60, '?' },
            { 62, '°' },
            { 63, 'p' },
            { 64, 'o' },
            { 65, 'y' },
            { 66, 'g' },
            { 67, 'b' },
            { 68, 'v' },
            { 69, 'w' },
        };
        private class QuestionModel
        {
            public List<List<char>> Question { get; set; } = new();
            public List<string> Answers { get; set; } = new();
            public int CorrectAnswer { get; set; }
        }
    }
}