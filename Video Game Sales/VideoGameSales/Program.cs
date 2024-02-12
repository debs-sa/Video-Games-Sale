using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Game
{
    public readonly string Name;
    public readonly string Platform;
    public readonly int ReleaseYear;
    public readonly string Genre;
    public readonly string Publisher;
    public readonly double NASales;
    public readonly double EUSales;
    public readonly double JPSales;
    public readonly double OtherSales;
    //public readonly double GlobalSales;
    public readonly double CriticScore;
    public readonly int CriticCount;
    public readonly double UserScore;
    public readonly int UserCount;
    public readonly string Rating;

    private string NextValue(string csv, ref int index)
    {
        string result = "";
        if (index < csv.Length)
        {
            if (csv[index] == ',')
            {
                index++;
            }
            else if (csv[index] == '"')
            {
                int endIndex = csv.IndexOf('"', index + 1);
                result = csv.Substring(index + 1, endIndex - (index + 1));
                index = endIndex + 2;
            }
            else
            {
                int endIndex = csv.IndexOf(',', index);
                if (endIndex == -1)
                    result = csv.Substring(index);
                else
                    result = csv.Substring(index, endIndex - index);
                index = endIndex + 1;
            }
        }
        return result;
    }

    public Game(string csv)
    {
        int index = 0;
        Name = NextValue(csv, ref index);
        Platform = NextValue(csv, ref index);
        int.TryParse(NextValue(csv, ref index), out ReleaseYear);
        Genre = NextValue(csv, ref index);
        Publisher = NextValue(csv, ref index);
        double.TryParse(NextValue(csv, ref index), out NASales);
        double.TryParse(NextValue(csv, ref index), out EUSales);
        double.TryParse(NextValue(csv, ref index), out JPSales);
        double.TryParse(NextValue(csv, ref index), out OtherSales);
        NextValue(csv, ref index); //public readonly double GlobalSales;
        double.TryParse(NextValue(csv, ref index), out CriticScore);
        int.TryParse(NextValue(csv, ref index), out CriticCount);
        double.TryParse(NextValue(csv, ref index), out UserScore);
        int.TryParse(NextValue(csv, ref index), out UserCount);
        Rating = NextValue(csv, ref index);
    }
}

class Program
{
    static List<Game> Games = new List<Game>();

    static void BuildDB()
    {
        string input;
        while ((input = Console.ReadLine()) != "")
        {
            var game = new Game(input);
            Games.Add(game);
        }
    }

    static void BuildDBFromFile()
    {
        using (var reader = File.OpenText("Video Game Sales 2017.csv"))
        {
            string input = reader.ReadLine(); // Skip label row
            while ((input = reader.ReadLine()) != null)
            {
                var game = new Game(input);
                Games.Add(game);
            }
        }
    }

    static void Main(string[] args)
    {
        BuildDBFromFile();
        /*

        // Part 1
        {
            Console.WriteLine("10 best-selling games in Japan");
            var results = Games.OrderByDescending(games => games.JPSales)
                            .Take(10);
            foreach (var game in results)
                Console.WriteLine(game.Name + " " + game.JPSales);
            Console.WriteLine("");
        }
        */

        // Part 2
        {
            Console.WriteLine("List the highest rated games (by critics) for each genre");
            var results = Games.OrderByDescending(games => games.CriticScore)
                            .GroupBy(game => game.Genre)
                            .Select(group => new
                            {
                                Genre = group.Key,
                                TopGame = group.First().Name
                            });

            foreach (var game in results)
                Console.WriteLine(game.Genre + " " + game.TopGame);
            Console.WriteLine("");
        }
       
    }
}
