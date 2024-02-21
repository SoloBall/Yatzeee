using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzeee
{
    public class Player
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public Dictionary<string, int> Combinations = new Dictionary<string, int>();

        public Player(string name) 
        {
            Name = name;
            Points = 0;
            
            Combinations.Add("ones", -1);
            Combinations.Add("twos", -1);
            Combinations.Add("threes", -1);
            Combinations.Add("fours",-1);
            Combinations.Add("fives", -1);
            Combinations.Add("sixes", -1);
            Combinations.Add("onePair", -1);
            Combinations.Add("twoPair", -1);
            Combinations.Add("threeAlike", -1);
            Combinations.Add("fourAlike", -1);
            Combinations.Add("sStraight", -1);
            Combinations.Add("bStraight", -1);
            Combinations.Add("house", -1);
            Combinations.Add("chance", -1);
            Combinations.Add("yatzy", -1);
            Combinations.Add("bonus", 0);
        }
    }
}
