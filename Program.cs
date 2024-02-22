using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Yatzeee
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool zeroed = false;
            string filter = "";
            Random rand = new Random();
            List<int> dice = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                dice.Add(rand.Next(1, 7));
            }
            List<Player> players = new List<Player>();

            Console.Write("welcome to yatzy... Enter amount of players: ");
            filter = Console.ReadLine();
            while (!int.TryParse(filter, out int playerAmount))
            {
                Console.WriteLine("That is not a valid play amount!");
                filter = Console.ReadLine();
            }
            int currentPlayer = 0;
            string answer = "";
            for (int i = 0; i < Math.Abs(int.Parse(filter)); i++) 
            {
                Console.WriteLine($"Enter name of player {(i + 1).ToString()}: ");
                players.Add(new Player(Console.ReadLine()));
            } //creates players
            for (int k = 0; k < 14; k++)
            {
                while (currentPlayer < players.Count())
                {
                    for (int i = 0; i < dice.Count(); i++)
                    {
                        dice[i] = rand.Next(1, 7);
                    }
                    ShowRoll(dice, players[currentPlayer]);
                    Console.WriteLine($"Reroll? y/n");
                    if (Console.ReadLine().StartsWith("y"))
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            dice = Reroll(rand, dice, players, currentPlayer);

                            if (j != 1)
                            {
                                Console.WriteLine($"Reroll? y/n");
                                if (Console.ReadLine().StartsWith("n"))
                                {
                                    break;
                                }
                            }
                        }
                    }
                    List<string> choices = CheckChoices(dice, players[currentPlayer])[0];

                    Console.WriteLine("Your available Combinations are: ");
                    for (int i = 0; i < choices.Count(); i++)
                    {
                        Console.Write($"{i + 1}: ");
                        foreach (char c in choices[i])
                        {
                            if (c != '-')
                            {
                                Console.Write(c);
                            }
                            else
                            {
                                break;
                            }
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine($"{choices.Count() + 1}: Zero out");
                    Console.WriteLine("Enter the number of the combination that you would like to use: ");
                    answer = Console.ReadLine();
                    while (!int.TryParse(answer, out int choice) || choice < 1 || choice > choices.Count() + 1)
                    {
                        Console.WriteLine("Incorrect combination number! Try again:");
                        answer = Console.ReadLine();
                    }
                    string pairs = "";
                    List<string> dices = CheckChoices(dice, players[currentPlayer])[1];

                    if (int.Parse(answer) - 1 == choices.Count())
                    {
                        zeroed = true;
                        List<string> unused = CheckUnused(players[currentPlayer]);
                        Console.WriteLine("These are the available combinations you can zero out:");
                        for (int i = 0; i < unused.Count(); i++)
                        {
                            Console.WriteLine($"{i+1}: {unused[i]}");
                        }
                        Console.WriteLine("Which would you like to zero out?");
                        answer = Console.ReadLine();
                        while (!int.TryParse(answer, out int choice) || choice < 1 || choice > unused.Count() + 1)
                        {
                            Console.WriteLine("Incorrect number! Try again:");
                            answer = Console.ReadLine();
                        }
                        for (int i = 0; i < unused.Count(); i++)
                        {
                            if (i == int.Parse(answer) - 1)
                            {
                                players[currentPlayer].Combinations[unused[i]] = 0;
                                Console.WriteLine($"You chose to zero out {unused[i]}");
                                zeroed = true;
                            }
                        }
                    }
                    else if (choices[int.Parse(answer)-1] == "ones")
                    {
                        players[currentPlayer].Combinations["ones"] = int.Parse(dices[0]);
                    }
                    else if (choices[int.Parse(answer)-1] == "twos")
                    {
                        players[currentPlayer].Combinations["twos"] = int.Parse(dices[1])*2;
                    }
                    else if (choices[int.Parse(answer) - 1] == "threes")
                    {
                        players[currentPlayer].Combinations["threes"] = int.Parse(dices[2])*3;
                    }
                    else if (choices[int.Parse(answer) - 1] == "fours")
                    {
                        players[currentPlayer].Combinations["fours"] = int.Parse(dices[3])*4;
                    }
                    else if (choices[int.Parse(answer) - 1] == "fives")
                    {
                        players[currentPlayer].Combinations["fives"] = int.Parse(dices[4])*5;
                    }
                    else if (choices[int.Parse(answer) - 1] == "sixes")
                    {
                        players[currentPlayer].Combinations["sixes"] = int.Parse(dices[5])*6;
                    }
                    else if (choices[int.Parse(answer) - 1] == "onePair")
                    {
                        for (int i = 0; i < dices.Count(); i++)
                        {
                            if (int.Parse(dices[i]) > 1)
                            {
                                pairs += (i+1).ToString();
                            } 
                        }
                        if (pairs.Length > 1)
                        {
                            Console.WriteLine("You have 2 available pairs: ");
                            Console.WriteLine($"number 1 is: {pairs[0]}. Number 2 is: {pairs[1]}");
                            Console.WriteLine("Which of these two do you want to use? ");
                            filter = Console.ReadLine();
                            while (!int.TryParse(filter, out int choice) || choice > 2 || choice < 1)
                            {
                                Console.WriteLine("Invalid choice, you have to pick either 1 or 2!");
                                filter = Console.ReadLine();
                            }
                            players[currentPlayer].Combinations["onePair"] = int.Parse(pairs[int.Parse(filter)-1].ToString()) * 2;
                        }
                        else
                        {
                            players[currentPlayer].Combinations["onePair"] = int.Parse(pairs[0].ToString()) * 2;
                        }
                    }
                    else if (choices[int.Parse(answer) - 1] == "twoPair")
                    {
                        for (int i = 0; i < dices.Count(); i++)
                        {
                            if (int.Parse(dices[i]) > 1)
                            {
                                pairs += (i + 1).ToString();
                            }
                        }
                        players[currentPlayer].Combinations["twoPair"] = (int.Parse(pairs[0].ToString()) * 2) + (int.Parse(pairs[1].ToString())*2);
                    }
                    else if (choices[int.Parse(answer) - 1] == "threeAlike")
                    {
                        for (int i = 0; i < dices.Count(); i++)
                        {
                            if (dices[i] == "3")
                            {
                                players[currentPlayer].Combinations["threeAlike"] = (i+1) * 3;
                                break;
                            }
                        }
                    }
                    else if (choices[int.Parse(answer) - 1] == "fourAlike")
                    {
                        for (int i = 0; i < dices.Count(); i++)
                        {
                            if (dices[i] == "4")
                            {
                                players[currentPlayer].Combinations["fourAlike"] = (i + 1) * 4;
                                break;
                            }
                        }
                    }
                    else if (choices[int.Parse(answer) - 1] == "sStraight")
                    {
                        players[currentPlayer].Combinations["yatzy"] = 30;
                    }
                    else if (choices[int.Parse(answer) - 1] == "bStraight")
                    {
                        players[currentPlayer].Combinations["yatzy"] = 40;
                    }
                    else if (choices[int.Parse(answer) - 1] == "house")
                    {
                        players[currentPlayer].Combinations["house"] = 0;
                        for (int i = 0; i < dices.Count(); i++)
                        {
                            if (dices[i] == "3")
                            {
                                players[currentPlayer].Combinations["house"] += (i + 1) * 3;
                            }
                            else if (dices[i] == "2")
                            {
                                players[currentPlayer].Combinations["house"] += (i + 1) * 2;
                            }
                        }
                    }
                    else if (choices[int.Parse(answer) - 1] == "chance")
                    {
                        players[currentPlayer].Combinations["chance"] = 0;
                        foreach (int n in dice)
                        {
                            players[currentPlayer].Combinations["house"] += n;
                        }
                    }
                    else if (choices[int.Parse(answer) - 1] == "yatzy")
                    {
                        for (int i = 0; i < dices.Count(); i++)
                        {
                            if (dices[i] == "5")
                            {
                                players[currentPlayer].Combinations["yatzy"] = 50;
                                break;
                            }
                        }
                    }
                    if (!zeroed)
                    {
                         Console.WriteLine($"You chose to use {choices[int.Parse(answer) - 1]}");
                    }
                    zeroed = false;
                    Console.WriteLine($"This is the end of your turn, you have {CheckPoints(players[currentPlayer])} points");
                    currentPlayer++;
                }
                currentPlayer = 0;
            }
            Player winner = players[0];
            foreach (Player player in players)
            {
                if (player.Points > winner.Points)
                {
                    winner = player;
                }
            }
            Console.WriteLine($"The winner is {winner.Name} with {winner.Points} points. Congratulations!");
            Console.Read();
        }
        static void ShowRoll(List<int> dice, Player player)
        {
            Console.WriteLine($"{player.Name} rolled:");
            foreach (int n in dice)
            {
                Console.Write($"{n}, ");
            }
        }

        static List<List<string>> CheckChoices(List<int> dice, Player player)
        {
            List<string> list = new List<string>();
            List<int> dices = new List<int>();
            int count = 0;
            int pairs = 0;
            bool threeAlike = false;
            bool fourAlike = false;
            bool yatzy = false;
            int straight = 0;
            for (int i = 0; i < 6; i++)
            {
                dices.Add(0);
            }

            foreach (int n in dice)
            {
                if (n == 1)
                {
                    dices[0]++;
                }
                if (n == 2)
                {
                    dices[1]++;
                }
                if (n == 3)
                {
                    dices[2]++;
                }
                if (n == 4)
                {
                    dices[3]++;
                }
                if (n == 5)
                {
                    dices[4]++;
                }
                if (n == 6)
                {
                    dices[5]++;
                }
            }

            foreach (int n in dices)
            {
                if (n == 2)
                {
                    pairs++;
                }
                else if (n == 3)
                {
                    pairs++;
                    threeAlike = true;
                }
                else if (n == 4)
                {
                    pairs += 2;
                    threeAlike = true;
                    fourAlike = true;
                }
                else if (n == 5)
                {
                    pairs += 2;
                    yatzy = true;
                }
            }

            if (dice.Contains(1) && dice.Contains(2) && dice.Contains(3) && dice.Contains(4))
            {
                straight = 1;
                if (dice.Contains(5))
                {
                    straight++;
                }
            }
            else if (dice.Contains(2) && dice.Contains(3) && dice.Contains(4) && dice.Contains(5))
            {
                straight = 1;
                if (dice.Contains(6))
                {
                    straight++;
                }
            }
            else if (dice.Contains(6) && dice.Contains(3) && dice.Contains(4) && dice.Contains(5))
            {
                straight = 1;
            }


            if (player.Combinations["ones"] == -1 && dice.Contains(1))
            {
                foreach (int n in dice)
                {
                    if (n == 1)
                    {
                        count++;
                    }
                }
                list.Add("ones");
                count = 0;
            }
            if (player.Combinations["twos"] == -1 && dice.Contains(2))
            {
                foreach (int n in dice)
                {
                    if (n == 2)
                    {
                        count++;
                    }
                }
                list.Add("twos");
                count = 0;
            }
            if (player.Combinations["threes"] == -1 && dice.Contains(3))
            {
                foreach (int n in dice)
                {
                    if (n == 3)
                    {
                        count++;
                    }
                }
                list.Add("threes");
                count = 0;
            }
            if (player.Combinations["fours"] == -1 && dice.Contains(4))
            {
                foreach (int n in dice)
                {
                    if (n == 4)
                    {
                        count++;
                    }
                }
                list.Add("fours");
                count = 0;
            }
            if (player.Combinations["fives"] == -1 && dice.Contains(5))
            {
                foreach (int n in dice)
                {
                    if (n == 5)
                    {
                        count++;
                    }
                }
                list.Add("fives");
                count = 0;
            }
            if (player.Combinations["sixes"] == -1 && dice.Contains(6))
            {
                foreach (int n in dice)
                {
                    if (n == 6)
                    {
                        count++;
                    }
                }
                list.Add("sixes");
                count = 0;
            }
            if (player.Combinations["onePair"] == -1 && pairs > 0)
            {
                list.Add("onePair");
            }
            if (player.Combinations["twoPair"] == -1 && pairs > 1 && !fourAlike)
            {
                list.Add("twoPair");
            }
            if (player.Combinations["threeAlike"] == -1 && threeAlike)
            {
                list.Add("threeAlike");
            }
            if (player.Combinations["fourAlike"] == -1 && fourAlike)
            {
                list.Add("fourAlike");
            }
            if (player.Combinations["sStraight"] == -1 && straight > 0)
            {
                list.Add("sStraight");
            }
            if (player.Combinations["bStraight"] == -1 && straight == 2)
            {
                list.Add("bStraight");
            }
            if (player.Combinations["house"] == -1 && pairs == 2 && threeAlike && !fourAlike && !yatzy)
            {
                list.Add("house");
            }
            if (player.Combinations["chance"] == -1)
            {
                list.Add("chance");
            }
            if (player.Combinations["yatzy"] == -1 && yatzy)
            {
                list.Add("yatzy");
            }
            List<List<string>> newList = new List<List<string>>();
            newList.Add(list);
            newList.Add(new List<string>());
            foreach (int n in dices)
            {
                newList[1].Add(n.ToString());
            }
            return newList;
        }

        static List<string> CheckUnused(Player player)
        {
            List<string> list = new List<string>();

            if (player.Combinations["ones"] == -1)
            {
                list.Add("ones");
            }
            if (player.Combinations["twos"] == -1)
            {
                list.Add("twos");
            }
            if (player.Combinations["threes"] == -1)
            {
                list.Add("threes");
            }
            if (player.Combinations["fours"] == -1)
            {
                list.Add("fours");
            }
            if (player.Combinations["fives"] == -1)
            {
                list.Add("fives");
            }
            if (player.Combinations["sixes"] == -1)
            {
                list.Add("sixes");
            }
            if (player.Combinations["onePair"] == -1)
            {
                list.Add("onePair");
            }
            if (player.Combinations["twoPair"] == -1)
            {
                list.Add("twoPair");
            }
            if (player.Combinations["threeAlike"] == -1)
            {
                list.Add("threeAlike");
            }
            if (player.Combinations["fourAlike"] == -1)
            {
                list.Add("fourAlike");
            }
            if (player.Combinations["sStraight"] == -1)
            {
                list.Add("sStraight");
            }
            if (player.Combinations["bStraight"] == -1)
            {
                list.Add("bStraight");
            }
            if (player.Combinations["house"] == -1)
            {
                list.Add("house");
            }
            if (player.Combinations["chance"] == -1)
            {
                list.Add("chance");
            }
            if (player.Combinations["yatzy"] == -1)
            {
                list.Add("yatzy");
            }
            return list;
        }

        static int CheckPoints (Player player)
        {
            int points = 0;
            int bonus = 0;
            foreach (int n in player.Combinations.Values)
            {
                if (n != -1)
                {
                    points += n;
                }
            }
            for (int i = 0; i < player.Combinations.Count() - 10; i++)
            {
                bonus += player.Combinations.ElementAt(i).Value;
            }
            if (bonus >= 63 && player.Combinations["bonus"] == 0)
            {
                points += 50;
                player.Combinations["bonus"] = 50;
                Console.WriteLine("You got a bonus of 50 points!");
            }
            player.Points = points;
            return points;
        }

        static List<int> Reroll(Random rand, List<int> dice, List<Player> players, int currentPlayer)
        {
            Console.WriteLine("Which dice would you like to reroll?? 123 / 134 / 2..:");
            string answer = Console.ReadLine();
            while (!int.TryParse(answer, out int reroll)
                || answer.Length > 5
                || answer.Length == 0
                || answer.Contains("6")
                || answer.Contains("7")
                || answer.Contains("8")
                || answer.Contains("9")
                || answer.Contains("0"))
            {
                Console.WriteLine("Incorrect dice numbers! Try again:");
                answer = Console.ReadLine();
            }
            answer = Math.Abs(int.Parse(answer)).ToString();
            for (int i = 0; i < answer.Length; i++)
            {
                dice[int.Parse(answer[i].ToString()) - 1] = rand.Next(1, 7);
            }
            ShowRoll(dice, players[currentPlayer]);
            return dice;
        }
    }
}
