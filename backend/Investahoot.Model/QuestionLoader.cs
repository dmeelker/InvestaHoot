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

            for (var x = 0; x < Image.Width; x++)
            {
                for (var y = 0; y < Image.Height; y++)
                {
                    image.Set(x, y, question[y][x]);
                }
            }

            return image;
        }

        private class QuestionModel
        {
            public List<List<char>> Question { get; set; } = new();
            public List<string> Answers { get; set; } = new();
            public int CorrectAnswer { get; set; }
        }
    }
}