using BowlingYuval.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BowlingYuval.Models
{
    public class UserScore
    {
        public string name { get; set; } 
        public int Score { get; set; }



        public  List<V_TopScore> getTopScore()
        {

            using (DBManager db = new DBManager())
            {

               var res = db.getTopScoreDB();
                return res;

            }
        }
    }
}