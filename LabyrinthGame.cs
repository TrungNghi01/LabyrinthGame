using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Activity6_7_8
{
    // The minotaur labyrinth game. Tracks the progression of a single round of gameplay.
    public class LabyrinthGame
    {
        // The map being used by the game.
        public Map Map { get; }

        // The player playing the game.
        public Player Player { get; }

        // The list of monsters in the game.
        public Monster[] Monsters { get; }

        // Whether the player has the sword yet or not. (Defaults to `false`.)
        public bool PlayerHasSword
        {
            get => Player.HasSword;
            set => Player.HasSword = value;
        }

        // Whether the player has the map. (Defaults to `true`.)
        public bool PlayerHasMap
        {
            get => Player.HasMap;
            set => Player.HasMap = value;
        }

        // A list of senses that the player can detect. Add to this collection in the constructor.
        private readonly ISense[] _senses;

        // Contains all the commands that a player can access.
        private readonly CommandList _commandList = new CommandList();

        // Initializes a new game round with a specific map and player.
        public LabyrinthGame(Map map, Player player, Monster[] monsters)
        {
            Map = map;
            Player = player;
            Monsters = monsters;

            // Each of these senses will be used during the game. Add new senses here.
            _senses = new ISense[]
            {
                new LightInEntranceSense(),
                new SwordSense(),
				//TODO: (A7) add the pit sense you created in ISense
                new PitSense(),
				//TODO: (A8) add the minotaur sense you created in ISense
                new MinotaurSense(),
            };
        }

        // Runs the game one turn at a time.
        public void Run()
        {
            ConsoleHelper.WriteLine("Choose from the list of possible commands.", ConsoleColor.Green);
            ConsoleHelper.Write($"{_commandList}", ConsoleColor.White);
            ConsoleHelper.WriteLine("Quit", ConsoleColor.Red);
            // This is the "game loop." Each turn runs through this `while` loop once.
            while (!HasWon && Player.IsAlive)
            {
                DisplayStatus();
                ICommand command = GetCommand();
                // Player quits the game
                if (command == null)
                {
                    Player.Kill("You abandoned your quest.");
                }
                // Valid command to execute
                else
                {
                    command.Execute(this);
                    foreach (Monster monster in Monsters)
                    {
                        Console.WriteLine(monster);
                        if (monster.Location == Player.Location && monster.IsAlive) monster.Activate(this);
                    }
                }
            }

            if (HasWon)
            {
                ConsoleHelper.WriteLine("You have claimed the magic sword, and you have escaped with your life!", ConsoleColor.DarkGreen);
                ConsoleHelper.WriteLine("You win!", ConsoleColor.DarkGreen);
            }
            else
            {
                ConsoleHelper.WriteLine(Player.CauseOfDeath, ConsoleColor.Red);
                ConsoleHelper.WriteLine("You lost.", ConsoleColor.Red);
            }
        }

        // Displays the status to the player, including what room they are in and asks each sense to display itself
        // if it is currently relevant.
        private void DisplayStatus()
        {
            ConsoleHelper.WriteLine("--------------------------------------------------------------------------------", ConsoleColor.Gray);
            bool somethingSensed = DisplaySenses();
            if (!somethingSensed)
                ConsoleHelper.WriteLine($"You sense nothing of interest nearby.", ConsoleColor.Gray);
            if (PlayerHasSword)
                ConsoleHelper.WriteLine($"You are currently carrying the sword! Make haste for the exit!", ConsoleColor.DarkYellow);
        }

        // Asks each sense to display itself if relevant. Returns true if something is sensed and false otherwise.
        private bool DisplaySenses()
        {
            bool somethingSensed = false;
            foreach (ISense sense in _senses)
            {
                if (sense.CanSense(this))
                {
                    somethingSensed = true;
                    sense.DisplaySense(this);
                }
            }
            return somethingSensed;
        }

        // Gets an `ICommand` object that represents the player's desires.
        private ICommand GetCommand()
        {
            while (true) // Until we get a legitimate command, keep asking.
            {
                ConsoleHelper.Write("What do you want to do? ", ConsoleColor.White);
                Console.ForegroundColor = ConsoleColor.Cyan;
                string input = Console.ReadLine().ToLower();
                if (input == "quit" || input == "q") return null;
                var command = _commandList.GetCommand(input);
                if (command == null)
                {
                    // If the input is not found in the command list, we have no clue what the command was. Try again.
                    ConsoleHelper.WriteLine($"I did not understand '{input}'.", ConsoleColor.Red);
                }
                else
                {
                    return command;
                }
            }
        }

        // Indicates if the player has won or not.
        public bool HasWon => CurrentRoom == RoomType.Entrance && PlayerHasSword;

        // Looks up what room type the player is currently in.
        public RoomType CurrentRoom => Map.GetRoomTypeAtLocation(Player.Location);
    }

    public static class ConsoleHelper
    {
        // Changes to the specified color and then displays the text on its own line.
        public static void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }

        // Changes to the specified color and then displays the text without moving to the next line.
        public static void Write(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }
    }
}
