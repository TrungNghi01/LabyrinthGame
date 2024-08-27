using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activity6_7_8
{
    // An interface to represent one of many commands in the game. Each new command should
    // implement this interface.
    public interface ICommand
    {
        void Execute(LabyrinthGame game);
    }

    // Represents a movement command, along with a specific direction to move.
    public class MoveCommand : ICommand
    {
        // The direction to move.
        public Direction Direction { get; }


        // Creates a new movement command with a specific direction to move.
        public MoveCommand(Direction direction)
        {
            Direction = direction;
        }

        // Causes the player's position to be updated with a new position, shifted in the intended direction,
        // but only if the destination stays on the map. Otherwise, nothing happens.
        public void Execute(LabyrinthGame game)
        {
            
            Location currentLocation = game.Player.Location;
            Location newLocation = Direction switch
            {
                Direction.North => new Location(currentLocation.Row - 1, currentLocation.Column),
                Direction.South => new Location(currentLocation.Row + 1, currentLocation.Column),
                Direction.West => new Location(currentLocation.Row, currentLocation.Column - 1),
                Direction.East => new Location(currentLocation.Row, currentLocation.Column + 1)
            };  

            if (game.Map.IsOnMap(newLocation))
                game.Player.Location = newLocation;
            else
                ConsoleHelper.WriteLine("There is a wall there.", ConsoleColor.Red);

            //TODO: (A7) add a condition that kills the player when they enter a pit room
            
            if (game.Map.GetRoomTypeAtLocation(game.Player.Location) == RoomType.PitRoom)
            {              
                game.Player.Kill("you went to a PIT room!!");
            }

        }
    }

    // A command that represents a request to pick up the sword.
    public class GetSwordCommand : ICommand
    {
        // Retrieves the sword if the player is in the room with the sword. Otherwise, nothing happens.
        public void Execute(LabyrinthGame game)
        {
            if (game.Map.GetRoomTypeAtLocation(game.Player.Location) == RoomType.Sword)
            {
                game.PlayerHasSword = true;
                game.Map.SetRoomTypeAtLocation(game.Player.Location, RoomType.Normal); // pick the sword up, no longer a sword room
            }
            else
            {
                ConsoleHelper.WriteLine("The sword is not in this room. There was no effect.", ConsoleColor.Red);
            }

        }
    }

    // A command that represents a request to display the map.
    public class ViewMapCommand : ICommand
    {
        // Displays a map of the labryrinth if the player is in possession of the map
        public void Execute(LabyrinthGame game)
        {
            if (game.PlayerHasMap) game.Map.Display(game.Player.Location);
            else ConsoleHelper.WriteLine($"You've lost the map!", ConsoleColor.Red);
        }
    }

    // Displays all the room occurrences - for debugging and testing
    public class DebugMap : ICommand
    {
        public void Execute(LabyrinthGame game)
        {
            game.Map.Display(game.Player.Location, true);
        }
    }

    public class CommandList
    {
        private readonly Dictionary<ICommand, List<string>> _commands = new Dictionary<ICommand, List<string>>();

        public CommandList()
        {
            _commands.Add(new MoveCommand(Direction.North), new List<string>() { "move north", "n", "1" });
            _commands.Add(new MoveCommand(Direction.South), new List<string>() { "move south", "s", "2" });
            _commands.Add(new MoveCommand(Direction.East), new List<string>() { "move east", "e", "3" });
            _commands.Add(new MoveCommand(Direction.West), new List<string>() { "move west", "w", "4" });
            _commands.Add(new ViewMapCommand(), new List<string>() { "view map", "vm", "5" });
            _commands.Add(new GetSwordCommand(), new List<string>() { "take sword", "ts", "6" });

            // Remove this after testing
            _commands.Add(new DebugMap(), new List<string>() { "debug", "d" });
        }

        public ICommand GetCommand(string input)
        {
            input = input.ToLower();
            foreach (var kvp in _commands)
            {
                foreach (string command in kvp.Value)
                {
                    if (input == command.ToLower()) return kvp.Key;
                }
            }
            return null;
        }

        public override string ToString()
        {
            string ret = "";
            int commandNo = 1;

            foreach (var kvp in _commands)
            {
                ret += $"{commandNo++}.";
                foreach (string command in kvp.Value)
                {
                    ret += $" ({command}) ";
                }
                ret += '\n';
            }
            return ret;
        }
    }
}
