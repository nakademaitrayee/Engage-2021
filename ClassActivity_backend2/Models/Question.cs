using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassActivity_backend2.Models
{
    public class Question
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }
        public string WrongAnswer1 { get; set; }
        public string WrongAnswer2 { get; set; }
        public string WrongAnswer3 { get; set; }

        public int c1 { get; set; }
        public int w1 { get; set; }
        public int w2 { get; set; }
        public int w3 { get; set; }

        public string ownerID { get; set; }
        public int questionId { get; set; }

    }
}
