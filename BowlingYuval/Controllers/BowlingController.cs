using BowlingYuval.DB;
using BowlingYuval.Models;
using System.Collections.Generic;
using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace BowlingYuval.Controllers
{
    public class BowlingController : ApiController
    {
        //TODO - CLEAN, COMMENT , AND LAST SHOT SPARE OR STRIKE

        [HttpPost]
        [Route("Create")]
        public GameStatus BowlingStartGame(string userName)
        {
            return GameStatus.CreateNewGame(userName);
        }

        [HttpPost]
        [Route("Game")]
        public GameStatus CalculationQueue(int shotInput , int gameID)
        {
            return GameStatus.calcultateGame(shotInput, gameID);

        }

        [HttpGet]
        [Route("TopScore")]
        public List<V_TopScore> GetTopScore()
        {
            UserScore newUserScore = new UserScore();
            return newUserScore.getTopScore();

        }


        
    }
}
