using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activity6_7_8
{
    public class Map
    {
        // Stores which room type each room in the world is. The default is `Normal` because that is the first
        // member in the enumeration list.
        private readonly RoomType[,] _rooms;

        // The total number of rows in this specific game world.
        public int Rows { get; }

        // The total number of columns in this specific game world.
        public int Columns { get; }

        // Creates a new map with a specific size.
        public Map(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _rooms = new RoomType[rows, columns];
        }

        // Returns what type a room at a specific location is.
        public RoomType GetRoomTypeAtLocation(Location location) => IsOnMap(location) ? _rooms[location.Row, location.Column] : RoomType.OffTheMap;

        // Determines if a neighboring room is of the given type.
        public bool HasNeighborWithType(Location location, RoomType roomType)
        {
            if (GetRoomTypeAtLocation(new Location(location.Row - 1, location.Column - 1)) == roomType) return true;
            if (GetRoomTypeAtLocation(new Location(location.Row - 1, location.Column)) == roomType) return true;
            if (GetRoomTypeAtLocation(new Location(location.Row - 1, location.Column + 1)) == roomType) return true;
            if (GetRoomTypeAtLocation(new Location(location.Row, location.Column - 1)) == roomType) return true;
            if (GetRoomTypeAtLocation(new Location(location.Row, location.Column)) == roomType) return true;
            if (GetRoomTypeAtLocation(new Location(location.Row, location.Column + 1)) == roomType) return true;
            if (GetRoomTypeAtLocation(new Location(location.Row + 1, location.Column - 1)) == roomType) return true;
            if (GetRoomTypeAtLocation(new Location(location.Row + 1, location.Column)) == roomType) return true;
            if (GetRoomTypeAtLocation(new Location(location.Row + 1, location.Column + 1)) == roomType) return true;
            return false;
        }

        // Indicates whether a specific location is actually on the map or not.
        public bool IsOnMap(Location location) =>
            location.Row >= 0 &&
            location.Row < _rooms.GetLength(0) &&
            location.Column >= 0 &&
            location.Column < _rooms.GetLength(1);

        // Changes the type of room at a specific spot in the world to a new type.
        public void SetRoomTypeAtLocation(Location location, RoomType type) => _rooms[location.Row, location.Column] = type;

        // Finds a random normal room 
        public Location GetRandomNormalLocation()
        {
            Random rng = new Random();

            int randomRow;
            int randomColumn;
            do
            {
                randomRow = rng.Next(0, this.Rows-1);
                randomColumn = rng.Next(0, this.Columns-1);
                if (_rooms[randomRow, randomColumn] == RoomType.Normal)
                {
                    break;
                }
            } while (true);
            
            return new Location(randomRow, randomColumn);

        }

        public void Display(Location? playerLocation, bool debug = false)
        {
            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Columns; ++j)
                {
                    var room = _rooms[i, j];
                    if (debug == false)
                    {
                        if (playerLocation.Row == i && playerLocation.Column == j)
                            ConsoleHelper.Write($"< >", System.ConsoleColor.Cyan);
                        else
                            ConsoleHelper.Write($"[ ]", System.ConsoleColor.Gray);
                    }
                    else
                    {
                        if (playerLocation.Row == i && playerLocation.Column == j)
                            ConsoleHelper.Write($"<{room.ToString()[0]}>", System.ConsoleColor.Cyan);
                        else
                            ConsoleHelper.Write($"[{room.ToString()[0]}]", System.ConsoleColor.Gray);
                    }
                        
                }
                System.Console.WriteLine();
            }

        }
    }

    // Represents a location in the 2D game world, based on its row and column.
    public record Location(int Row, int Column);
    // Represents one of the four directions of movement.
    public enum Direction { North, South, West, East }
    // Represents one of the different types of rooms in the game.

    // TODO: (A7) add a extra RoomType enum option for Pit rooms
    // TODO: (A8) Add a extra RoomType enum option for the Minotaur
    public enum RoomType { Normal, Entrance, Sword, OffTheMap, PitRoom, Minotaur}	
  
}
