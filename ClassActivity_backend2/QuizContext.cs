using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassActivity_backend2
{
    public class QuizContext : DbContext
    {
        public QuizContext()
        {
        }

        public QuizContext(DbContextOptions<QuizContext> options) : base(options) {}

        public DbSet<Models.Question> Quizzes { get; set; }


    }
}
