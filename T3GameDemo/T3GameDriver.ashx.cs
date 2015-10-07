using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace T3GameDemo
{

    public class T3GameDriver : IHttpHandler, IRequiresSessionState
    {

        private T3GameEngine t3GameEngine;


        public T3GameDriver()
        {
            this.t3GameEngine = new T3GameEngine();
        }

        public void ProcessRequest(HttpContext context)
        {
            String requestCode = context.Request["ReqCode"];

            if (requestCode.Equals("UserMove"))
            {

                String winner = String.Empty;

                Boolean gameOver = false;

                int cellIDofComputersMove = 0;

                Game currentGame = context.Session["Game"] as Game;

                int cellID = int.Parse(context.Request["CellID"]);

                //Set the users move;
                currentGame.Moves[cellID] = 1;

                gameOver = this.t3GameEngine.CheckAndProcessWinner(currentGame.Moves, out winner);

                if (!gameOver)
                {
                    //Set a random computer's move
                    cellIDofComputersMove = MakeComputersMove(currentGame.Moves);
                    if (cellIDofComputersMove >= 0)
                    {
                        currentGame.Moves[cellIDofComputersMove] = 2;
                        gameOver = this.t3GameEngine.CheckAndProcessWinner(currentGame.Moves, out winner);
                    }
                }

                SystemResponse response = new SystemResponse();

                if (gameOver)
                {
                    currentGame.CurrentGameState = GameState.Over;
                    response.CellIDofComputersMove = cellIDofComputersMove;
                    response.CurrentGameStatus = currentGame.CurrentGameState.ToString();
                    response.Winner = winner;
                }
                else
                {
                    response.CellIDofComputersMove = cellIDofComputersMove;
                    response.CurrentGameStatus = currentGame.CurrentGameState.ToString();
                }

                SendResponse(response);
            }
            else if (requestCode.Equals("StateChanged"))
            {
                Game currentGame = context.Session["Game"] as Game;

                String stateCode = context.Request["StateCode"];

                if (stateCode.Equals("start"))
                {
                    currentGame.CurrentGameState = GameState.Runing;
                    currentGame.Moves = new int[9];
                }
                else if (stateCode.Equals("stop"))
                {
                    currentGame.CurrentGameState = GameState.Stopped;
                }

                SendResponse(new SystemResponse() { CurrentGameStatus = currentGame.CurrentGameState.ToString() });
            }
        }

        private void SendResponse(SystemResponse response)
        {
            String formattedJSON = ServiceStack.Text.JsonSerializer.SerializeToString(response);

            string callBackID = HttpContext.Current.Request.QueryString["callback"];

            HttpContext.Current.Response.Charset = "utf-8";

            HttpContext.Current.Response.ContentType = "application/json";

            HttpContext.Current.Response.Write(String.Format("{0}( {1} )", callBackID, formattedJSON));
        }



        private int MakeComputersMove(int[] moves)
        {
            List<int> freeMoves = new List<int>();

            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i].Equals(0))
                {
                    freeMoves.Add(i);
                }
            }

            return freeMoves.Count == 0 ? -1 : freeMoves[new Random().Next(freeMoves.Count)];
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}