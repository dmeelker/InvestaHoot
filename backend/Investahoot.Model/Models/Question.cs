using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investahoot.Model.Models
{
    public class Question
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Image Image { get; set; } = new();
        public List<string> Answers { get; set; } = new();
        public int CorrectAnswerIndex { get; set; }
    }
}
