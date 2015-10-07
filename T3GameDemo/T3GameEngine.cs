using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T3GameDemo
{

    public class T3GameEngineBase
    {
        public static int[,] Winners = new int[,]
		{
			{0,1,2},
			{3,4,5},
			{6,7,8},
			{0,3,6},
			{1,4,7},
			{2,5,8},
			{0,4,8},
			{2,4,6}
		};
    }

    public class T3GameEngine : T3GameEngineBase
    {
        public bool CheckAndProcessWinner(int[] moves, out String winner)
        {
            for (int i = 0; i < 8; i++)
            {
                int a = Winners[i, 0], b = Winners[i, 1], c = Winners[i, 2];

                int b1 = moves[a], b2 = moves[b], b3 = moves[c];

                if (b1 == 0 || b2 == 0 || b3 == 0)
                    continue;

                if (b1 == b2 && b2 == b3)
                {
                    winner = b1 == 1 ? "You" : "Computer";
                    return true;
                }
            }

            Boolean gameTied = this.CheckGameTied(moves);

            winner = gameTied ? "Both" : String.Empty;

            return gameTied ? true : false;
        }


        private Boolean CheckGameTied(int[] moves)
        {
            Boolean gameTied = true;

            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i] == 0)
                {
                    gameTied = false;
                    break;
                }
            }

            return gameTied;
        }


    }


}