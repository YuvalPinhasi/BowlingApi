using BowlingYuval.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BowlingYuval
{
    public class DBManager : IDisposable
    {

        static readonly string connStr = ConfigurationManager.ConnectionStrings["bowlingDBConnectionString"].ConnectionString;
        private bool disposedValue;
        public int CreateNewGameDB(string userName)
        {
            using (var dc = new BowlingDBDataContext(connStr))
            {
                var newUser = new User
                {
                    UserName = userName
                };
                dc.Users.InsertOnSubmit(newUser);
                dc.SubmitChanges();

                var newGame = new GameData
                {
                    UserID = newUser.UserID
                };
                dc.GameDatas.InsertOnSubmit(newGame);
                dc.SubmitChanges();
                return newGame.GameID;


            }
        }
        public string AddShot(int shot, int id)
        {
            using (var dc = new BowlingDBDataContext(connStr))
            {
                var currentGame = dc.GameDatas.First(g => g.GameID == id);
                if (currentGame.Shots == null)
                {
                    currentGame.Shots += $"{shot}";
                }
                else
                {
                    currentGame.Shots += $",{shot}";
                }
                dc.SubmitChanges();
                return currentGame.Shots;
            }
        }

        public void UpdateTotalScore(int id, int totalScore)
        {
            using (var dc = new BowlingDBDataContext(connStr))
            {
                var currentGame = dc.GameDatas.First(g => g.GameID == id);
                currentGame.TotalScore =totalScore;
                dc.SubmitChanges();
            }
        }

        public List<V_TopScore> getTopScoreDB()
        {
            using (var dc = new BowlingDBDataContext(connStr))
            {
                var res = dc.V_TopScores.OrderBy(V_TopScore=>V_TopScore.TotalScore).Take(5).ToList();
                return res;
            }
            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}