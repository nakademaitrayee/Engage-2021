using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClassActivity_backend2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        readonly QuizContext context;

        public QuizController(QuizContext context)
        {
            this.context = context;
        }

        //Get Request to get the quizzes of particular user
        [HttpGet]
        public IEnumerable<Models.Question> Get()
        {
            var userID = HttpContext.User.Claims.First().Value;
            return context.Quizzes.Where(w => w.ownerID == userID);
        }

        //Get Request for getting the particular quiz with provided ID
        [HttpGet("{questionId}")]
        public IEnumerable<Models.Question> Get([FromRoute] int questionId)
        {
            var question = context.Quizzes.Where(q => q.ID == questionId);
            return question;
        }

        //Get Request for getting all the quiz, of all the users, that were created
        [HttpGet("all")]
        public ActionResult<IEnumerable<Models.Question>> GetAllQuizzes()
        {
            return context.Quizzes;
        }

        //Post request for adding question to the DB
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.Question ques)
        {
            var userID = HttpContext.User.Claims.First().Value;

            ques.ownerID = userID;

            context.Quizzes.Add(ques);
            await context.SaveChangesAsync();

            return Ok(ques);
        }

        //Put Request for updating the DB for storing the answers of quizzes
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,[FromBody] Models.Question question)
        {

            //var question = await context.Quizzes.FirstOrDefaultAsync(q => q.ID == id);

            //question.Poll[option] = question.Poll[option] + 1;
            if (id != question.ID)
                return BadRequest();

            var local = context.Quizzes.Local.FirstOrDefault(entry => entry.ID == (id));

            // check if local is not null 
            if (local != null)
            {
                // detach
                context.Entry(local).State = EntityState.Detached;
            }

            context.Entry(question).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return Ok(question);
            
        }
    }
}
