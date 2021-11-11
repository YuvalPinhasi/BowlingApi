using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq;

namespace BowlingYuval.Models
{
    public class GameStatus 
    {
        public int GameID { get; set; }
        public int TotalScore { get; set; }
        public bool isLastShot { get; set; } = false;
        public List<TurnStatus> TurnsStatus { get; set; } 
        public GameStatus() 
        {
        }

        // create new game, in user table and game table
        public static GameStatus CreateNewGame(string name)
        {
            int GameId;
            using (DBManager db = new DBManager())
            {
              GameId = db.CreateNewGameDB(name);
            }

            GameStatus NewGameStatus = new GameStatus
            {
                GameID = GameId
            };
            return NewGameStatus;
        }

        // get input shot and game id 
        public static GameStatus calcultateGame(int shot, int gameID)
        {
            GameStatus gameStatus = new GameStatus
            {
                GameID = gameID
            };
            var shotsData = "";
            
            //add shot to data base and return shots rowdata
            using (DBManager db = new DBManager())
            {
                shotsData = db.AddShot(shot, gameID);
            }

            gameStatus = gameStatus.BuildGameStatus(shotsData);
            
            //Check if is last shot
            if (gameStatus.TurnsStatus.Count() == 10)
            {
                bool isLastShot = false;
                // if shot number 2 was strikr or spare
                if (gameStatus.TurnsStatus.Last().DisplayValues[1] == 20 || gameStatus.TurnsStatus.Last().DisplayValues[1] == 30)
                {
                    //The last shot was strike or spare - have another shot
                    if (gameStatus.TurnsStatus.Last().DisplayValues[2]>0)
                    {
                        isLastShot = true;
                    }
                }
                // else - is last shot
                else if(gameStatus.TurnsStatus.Last().DisplayValues[1] > 0)
                {
                    isLastShot = true;
                }

                if(isLastShot)
                {
                    gameStatus.isLastShot = true;
                    gameStatus.TotalScore = gameStatus.TurnsStatus.Last().Score;
                    //Update db total score
                    using (DBManager db = new DBManager())
                    {
                        db.UpdateTotalScore(gameID, gameStatus.TurnsStatus.Last().Score);
                    }
                }
            }
            
            return gameStatus;
        }


        // algoritm handle game
        private GameStatus BuildGameStatus(string shotsRowData)
        {

          GameStatus gameStatus = new GameStatus();
          // get string as row data and 
          string[] shotsMockArr = shotsRowData.Split(',');
          // build array shots 
            int[] shotsArrInt = new int[shotsMockArr.Length];
       
           for (int i = 0; i < shotsArrInt.Length; i++)
           { 
               shotsArrInt[i] = Int32.Parse(shotsMockArr[i]);
           }
          
           int currentTurn = -1;
            
           List <TurnStatus>  newListTurns = new List<TurnStatus>();
            
           for (int shot = 0; shot < shotsArrInt.Length; shot++)
           {
                int currentShot = shotsArrInt[shot];
                int currentScore = 0;
               
                //if it isnt the last shot -> update score
                if (newListTurns.Any())
                {
                     currentScore = newListTurns.Last().Score;
                }

                
                // if is the first shot in turn 
                if (currentTurn == -1 && shot != 20)
                {
                    // create new tern
                    newListTurns.Add(new TurnStatus());
                    
                    // if strike
                    if (currentShot == 10)
                    {
                        //Strike = 30
                        newListTurns.Last().DisplayValues[0] = 30;

                        if (shot + 2 == shotsArrInt.Length)
                        {
                            newListTurns.Last().Score = (currentShot + currentScore + shotsArrInt[shot + 1]);
                        }
                        else if (shot + 1 == shotsArrInt.Length)
                        { 
                         newListTurns.Last().Score = (currentShot + currentScore);
                        }
                         else
                        {
                            newListTurns.Last().Score = (currentShot + currentScore + shotsArrInt[shot + 1]+ shotsArrInt[shot + 2]);
                        }

                        continue;
                    }

                    // update shot and score
                    newListTurns.Last().DisplayValues[0] = currentShot;
                    newListTurns.Last().Score = (currentShot+ currentScore);
                    currentTurn = currentShot;
                    continue;
                }
                
                //if spare (shot number 2 in turn)
                if (currentTurn + currentShot == 10 && shot != 20)
                {
                    //Spare = 20
                    newListTurns.Last().DisplayValues[1] = 20;
                   // If you have the next shot -> update score
                    if (shot+1 == shotsArrInt.Length)
                    {
                        newListTurns.Last().Score = (currentShot + currentScore);
                    }
                    else {
                        newListTurns.Last().Score = (currentShot + currentScore + shotsArrInt[shot + 1]);
                    }
                    currentTurn = -1;
                    continue;
                }
               
                // if it is the last shot with strike or spare
                if (shot == 20)
                {
                    newListTurns.Last().DisplayValues[2] = currentShot;
                    newListTurns.Last().Score = (currentShot + currentScore);
                    gameStatus.isLastShot = true;
                    gameStatus.TotalScore = (currentShot + currentScore);
                    continue;
                }
                else 
                {
                    newListTurns.Last().DisplayValues[1] = currentShot;

                }
                newListTurns.Last().Score = (currentShot + currentScore);
                currentTurn = -1;
                continue;
            }
            gameStatus.TotalScore = newListTurns.Last().Score;
            gameStatus.TurnsStatus = newListTurns;           
            return gameStatus;

        }
    }
}

