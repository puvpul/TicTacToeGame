using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T3GameDemo
{

    public enum GameState { Stopped, Runing, Over }

    public class Game
    {
        public GameState CurrentGameState { get; set; }
        public int[] Moves { get; set; }
    }

    public class SystemResponse
    {
        public String CurrentGameStatus { get; set; }
        public int CellIDofComputersMove { get; set; }
        public String Winner { get; set; }
    }

}