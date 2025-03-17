using System;
using System.Collections.Generic;

namespace Monopoly
{
    class Program
    {
        static void Main()
        {
            // Random rnd= new Random();
            // Board board = new Board();
            // TurnResult temp;
            // board.SetupTestBoard();
            // Console.WriteLine(board.GetBoardScheme());
            // Console.WriteLine(board.CurrentPlayer());
            // temp = board.MovePlayer(2);
            // Console.WriteLine($"{temp.Message}; {temp.MandatoryAction}");
            // Console.WriteLine(board.GetBoardScheme());
            // Console.WriteLine(board.CurrentPlayer());
            // Console.WriteLine(board.NextPlayer());
            //
            
            new GameUI().StartGame();
        }
    }
    
    public class GameUI
    {
        private readonly Board _board = new Board();
        private readonly Random _rng = new Random();

        public void StartGame()
        {
            _board.SetupTestBoard();
            
            while(true)
            {
                Console.Clear();
                PrintBoardState();
                PrintPlayerState();
                HandleCurrentTurn();
            }
        }

        private void PrintBoardState()
        {
            Console.WriteLine("=== GAME BOARD ===");
            foreach(var tile in _board._landmarks)
            {
                var players = string.Join(", ", tile.Players.Select(p => p.Name));
                Console.WriteLine($"[{tile.Position,2}] {tile.TileName,-20} {players}");
            }
        }

        private void PrintPlayerState()
        {
            var player = _board.GetPlayer();
            Console.WriteLine($"\n{player.Name}'s Turn | Money: ${player.Money}");
            Console.WriteLine($"Location: {player.Location.TileName}");
            
            if(player.PropertiesList().Count > 0)
            {
                Console.WriteLine("Properties:");
                foreach(var prop in player.PropertiesList().OfType<IBuyable>())
                    if (prop is Tile t)
                        Console.WriteLine($"- {t.TileName}");
            }
        }

        private void HandleCurrentTurn()
        {
            if(!_board.MovementDone)
            {
                Console.WriteLine("\n1. Roll dice\n2. Manage properties\n3. Quit");
                switch(GetInput(1, 3))
                {
                    case 1: HandleDiceRoll(); break;
                    case 2: ShowPropertyManagement(); break;
                    case 3: Environment.Exit(0); break;
                }
            }
            else if(!_board.ActionDone && _board.Mandatory)
            {
                Console.WriteLine($"\n1. Do Action ({_board.TurnRes.Message})\n2. Trade\n3. Manage properties");
                switch(GetInput(1, 3))
                {
                    case 1: _board.DoAction(); break;
                    case 2: StartTrade(); break;
                    case 3: ShowPropertyManagement(); break;
                }
            }
            else if (!_board.ActionDone && !_board.Mandatory)
            {
                Console.WriteLine($"\n1. Do Action ({_board.TurnRes.Message})\n2. Trade\n3. Manage properties\n4. End Turn");
                switch(GetInput(1, 3))
                {
                    case 1: _board.DoAction(); break;
                    case 2: StartTrade(); break;
                    case 3: ShowPropertyManagement(); break;
                    case 4: _board.NextPlayer(); break;
                }
            }
            else
            {
                Console.WriteLine("\n1. End turn\n2. Trade\n3. Manage properties");
                switch(GetInput(1, 3))
                {
                    case 1: _board.NextPlayer(); break;
                    case 2: StartTrade(); break;
                    case 3: ShowPropertyManagement(); break;
                }
            }
        }

        private void HandleDiceRoll()
        {
            int roll = _rng.Next(1, 7);
            Console.WriteLine($"You rolled: {roll}");
            var result = _board.MovePlayer(roll);
            
            Console.WriteLine(result.Message);
            if(result.MoneyNeeded > 0)
                Console.WriteLine($"Amount needed: ${result.MoneyNeeded}");
            
            Console.ReadKey();
        }

        private void ShowPropertyManagement()
        {
            var player = _board.GetPlayer();
            var currentTile = player.Location;

            if (currentTile is City city && city.Owner == player)
            {
                Console.WriteLine($"\n=== DEVELOP {city.TileName} ===");
                Console.WriteLine($"Houses: {city.Houses} | Hotel: {(city.HasHotel ? "YES" : "NO")}");
                Console.WriteLine($"1. Build {(city.Houses < 4 ? $"House (${city.Group.HouseCost})" : "Hotel")}");
                Console.WriteLine($"2. Sell Improvement");
                Console.WriteLine("3. Back");
        
                switch(GetInput(1, 3))
                {
                    case 1:
                        if (_board.DoAction())
                            Console.WriteLine("Development successful!");
                        else
                            Console.WriteLine("Can't develop right now!");
                        break;
                    case 2:
                        // Implement sell logic if needed
                        break;
                }
            }
            else
            {
                Console.WriteLine("\nYou can only manage properties you're standing on!");
                Console.ReadKey();
            }
        }

        private void HandleTileAction()
        {
            var player = _board.GetPlayer();
            var tile = player.Location;

            Console.WriteLine($"\nCurrent tile: {tile.TileName}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            _board.DoAction();
        }

        private int GetInput(int min, int max)
        {
            int input;
            do Console.Write("> ");
            while(!int.TryParse(Console.ReadLine(), out input) || input < min || input > max);
            return input;
        }

        // Simplified trade starter (would need expansion)
        private void StartTrade()
        {
            Console.WriteLine("Feature not implemented yet!");
            Console.ReadKey();
        }
    }
}


