using System;
using System.Collections.Generic;

namespace Monopoly
{
    class Program
    {
        static void Main()
        {
            Tile[] tiles;
            Player[] players;
            TurnResult result;
            
            players = new[]
            {
                new Player("Bigbob"),
                new Player("San4ello"),
                new Player("Darksidius"),
                new Player("Saul Goodman")
            };
            
            tiles = new Tile[]
            {
                new Infrastructure("Startovo", 0, 1000),
                new Infrastructure("City 1", 1, 1000),
                new Infrastructure("Bigbobovo", 2, 1000),
                new Infrastructure("Darksidiusovo", 3, 1000),
                new Infrastructure("Saul-Goodmanovo", 4, 1000),
                new Infrastructure("San4ellovo", 5, 1000)
            };
            Random rnd = new Random();
            foreach (Player player in players)
            {
                player.MoveTo(tiles[0]);
            }

            while (true)
            {
                foreach (var player in players)
                {
                    int roll = rnd.Next(1, 7);
                    for (int i = 0; i < roll; i++)
                    {
                        player.MoveTo(tiles[(player.Location.Position + 1) % tiles.Length]);
                    }
                    result = tiles[player.Location.Position].OnStepped(player);
                    Console.WriteLine($"Player {player.Name} Landed on {tiles[player.Location.Position].TileName}");
                    Console.WriteLine($"Action will {result.Message}");
                    if (Console.ReadLine() != "")
                    {
                        return;
                    }
                }
            }
        }
    }
}
