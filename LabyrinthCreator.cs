using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Activity6_7_8
{
    public static class LabyrinthCreator
    {
        static readonly (int rows, int cols) smallCoords = (4, 4);
        static readonly (int rows, int cols) medCoords = (6, 6);
        static readonly (int rows, int cols) largeCoords = (8, 8);

        // Creates a small 4x4 game.
        public static LabyrinthGame CreateSmallGame()
        {
            (Map map, Location start) = InitializeMap(Size.Small);
            return CreateGame(map, start);
        }

        // Creates a medium 6x6 game.
        public static LabyrinthGame CreateMediumGame()
        {
            (Map map, Location start) = InitializeMap(Size.Medium);
            return CreateGame(map, start);
        }

        // Creates a large 8x8 game.
        public static LabyrinthGame CreateLargeGame()
        {
            (Map map, Location start) = InitializeMap(Size.Large);
            return CreateGame(map, start);
        }

        // Helper function that initializes the map size and all the map locations
        // Returns the initialized map and the entrance location (start) so we can
        // set the player starting location accordingly.
        private static (Map, Location) InitializeMap(Size mapSize)
        {
            Map map = mapSize switch
            {
                Size.Small => new Map(smallCoords.rows, smallCoords.cols),
                Size.Medium => new Map(medCoords.rows, medCoords.cols),
                Size.Large => new Map(largeCoords.rows, largeCoords.cols),
            };
            Location start = RandomizeMap(map);
            return (map, start);
        }

        // Creates a map with randomly placed and non-overlapping features
        // The Entrance will be located in a random position along the edge of the map
        // The Sword will be placed randomly in the map but it will not be adjacent to the Entrance
        // Traps & Monsters will be placed randomly
        private static Location RandomizeMap(Map map)
        {
            //TODO: (A6) create an instance of the LabryinthCreatorRng class to get random locations
            LabryinthCreatorRng randomizeIndex = new LabryinthCreatorRng(map);

            //TODO: (A6) Randomize the start location of the entrance
            // Need to return the start location when placing the Player
            // Location start = new Location(0, 0);    
            Location start = randomizeIndex.RandomEntrance();
            map.SetRoomTypeAtLocation(start, RoomType.Entrance);

            //TODO: (A6) Randomize the location of the sword
            //Location swordStart = new Location(2, 2);
            Location swordStart = randomizeIndex.RandomSword();
            map.SetRoomTypeAtLocation(swordStart, RoomType.Sword);

            //TODO: (A7) Randomize the location of the pit rooms
            //add pit room(s) to the map - consider scaling the number of obstacle rooms to the game size.

            int index = 0; // assign the value for the while loop ( I put the assign value in here to make it easy to track only on activity7)
            do
            {
                Location pitRoom = randomizeIndex.RandomPitRoom(map);
                map.SetRoomTypeAtLocation(pitRoom, RoomType.PitRoom);
                index++;
            }while (index < map.Rows - 2 ); // create the pit room base on the map size
            
            return start;
        }

        private static Player InitializePlayer(Location start)
        {
            return new Player(start);
        }

        private static Monster[] InitializeMonsters(Map map)
        {
            //TODO: (A8) Initalize an array of monsters and fill it with one Minotaur instance (and other monster instances).
            //Consider scaling the number of your monster type to the size of the game. However, there should only ever be one minotaur in the labyrinth.
            //Set every new monster's location to be random (hint: use the LabryinthCreatorRng class)
            
            LabryinthCreatorRng randomizeIndex = new LabryinthCreatorRng(map);

            Monster[] monsters = new Monster[map.Rows]; // create a size of array base on the map size small = 4 / medium = 6 / large = 8 ( I am not create another monsters)
            
            // there is always one minotaur in the labyrinth. 
            Location minatourRoom = randomizeIndex.RandomMinotaurRoom(map);
//            monsters[0] = new Minotaur(minatourRoom);
            monsters = new Monster[] { new Minotaur(minatourRoom) };
            map.SetRoomTypeAtLocation(minatourRoom, RoomType.Minotaur);

            /* // this is for create other monster. check later
            for (int i = 1; i < map.Rows; i++)
            {
                Location monsterRoom = randomizeIndex.RandomMinotaurRoom(map);
                monsters[i] = new Minotaur(monsterRoom);
            }
            */
            // Ensure monster locations do not overlap existing locations on the map

            return monsters;
        }

        private static LabyrinthGame CreateGame(Map map, Location start)
        {
            Player player = InitializePlayer(start);
            Monster[] monsters = InitializeMonsters(map);
            return new LabyrinthGame(map, player, monsters);
        }

        private enum Size { Small, Medium, Large };
    }

    //TODO: (A6) Complete this new class that will be used to get random room locations in the labyrinth that are suited for the entrance and the sword
    public class LabryinthCreatorRng
    {
        //TODO: (A6) data fields
        private int rowSize, colSize;
        private int rowEntrance, columnEntrance;
        private int rowSword, columnSword;
        private int rowPitRoom, columnPitRoom;
        private int rowMinotaurRoom, columnMinotaurRoom;
        //TODO: (A6) constuctor
        public LabryinthCreatorRng(Map map)
        {
            this.rowSize = map.Rows;
            this.colSize = map.Columns;
        }

        //TODO: (A6) method that returns a random location suited to the entrance
        // the entrance should always be located on the edge of the 2D grid.
        /// <summary>
        /// Generates a random entrance location based on the map size. 
        /// The entrance will be at the edge of the map. 
        /// </summary>
        /// <returns>A Location object representing the randomized sword location.</returns>
        public Location RandomEntrance()
        {
            if (rowSize == 4 && colSize == 4) // check the map size
            {
                Random rowRandom = new Random();
                rowEntrance = rowRandom.Next(0, 3); // random number in the range between 0 and 3 to get the row value
                if (rowEntrance == 0 || rowEntrance == 3) 
                {
                    Random colRandom = new Random();
                    columnEntrance = colRandom.Next(0, 3); // random number to get the column in the range between 0 and 3 
                }
                else
                {
                    Random colRandom = new Random();
                    List<int> smallMapnumbers = new List<int> { 0, 3 };
                    columnEntrance = smallMapnumbers[colRandom.Next(smallMapnumbers.Count)]; // random pick 1 number from a list of 2 number 0 or 3 for the column  
                }
            }
            // another else if is working the same logic just the different when user choice medium or large map
            else if (rowSize == 6 && colSize == 6) 
            {
                Random rowRandom = new Random();
                rowEntrance = rowRandom.Next(0, 5); 
                if (rowEntrance == 0 || rowEntrance == 5)
                {
                    Random colRandom = new Random();
                    columnEntrance = colRandom.Next(0, 5); 
                }
                else
                {
                    Random colRandom = new Random();
                    List<int> smallMapnumbers = new List<int> { 0, 5 };
                    columnEntrance = smallMapnumbers[colRandom.Next(smallMapnumbers.Count)];
                }
            }
            else if (rowSize == 8 && colSize == 8) 
            {
                Random rowRandom = new Random();
                rowEntrance = rowRandom.Next(0, 7);
                if (rowEntrance == 0 || rowEntrance == 7)
                {
                    Random colRandom = new Random();
                    columnEntrance = colRandom.Next(0, 7);
                }
                else
                {
                    Random colRandom = new Random();
                    List<int> smallMapnumbers = new List<int> { 0, 7 };
                    columnEntrance = smallMapnumbers[colRandom.Next(smallMapnumbers.Count)];
                }
            }
            else
            {
                Console.WriteLine("something went wrong with the creating process!");
            }

            return new Location(rowEntrance, columnEntrance);
        }

        //TODO: (A6) method that returns a random location suited to the sword
        //sword location should not be adjacent or overlapping the start location

        /// <summary>
        /// remove all of the possible room that is placed at or near the entrance.
        /// </summary>
        /// <returns>A Location object representing the randomized sword location.</returns>
        public Location RandomSword()
        {
            // using for loop to create a list of available rooms based on the size of the map
            List<(int, int)> availableRooms = GetAvailableRooms();

            // remove all of the possibility value that is the entrance and value around it from the list
            // even the value not in the available list we for example (0,-1) we can run the code without problem.
            availableRooms.Remove((rowEntrance, columnEntrance));
            availableRooms.Remove((rowEntrance, columnEntrance + 1 ));
            availableRooms.Remove((rowEntrance, columnEntrance - 1 ));
            availableRooms.Remove((rowEntrance + 1, columnEntrance));
            availableRooms.Remove((rowEntrance - 1, columnEntrance));
            availableRooms.Remove((rowEntrance + 1, columnEntrance + 1));
            availableRooms.Remove((rowEntrance - 1, columnEntrance - 1 ));
            availableRooms.Remove((rowEntrance + 1, columnEntrance - 1));
            availableRooms.Remove((rowEntrance - 1, columnEntrance + 1));
            //random from the the list that we already remove the value
            Random random = new Random();
            (int,int) randomSword = availableRooms[random.Next(availableRooms.Count)];
            // destructure the tuple and assign them to a value for each int.
            (rowSword, columnSword) = randomSword; 

            return new Location(rowSword, columnSword);
        }
        //any additional methods (if needed)
        /// <summary>
        /// Generates a list of available rooms based on the size of the map.
        /// </summary>
        /// <returns>
        /// A list of tuples, where each tuple represents the coordinates (row, column) of an available room.
        /// </returns>
        public List<(int, int)> GetAvailableRooms()
        {
            List<(int, int)> availableRooms = new List<(int, int)>();

            for (int row = 0; row < rowSize; row++)
            {
                for (int column = 0; column < colSize; ++column)
                {
                    availableRooms.Add((row, column));
                }
            }

            return availableRooms;
        }
        //TODO: (A7) Create a method that returns a random location suited to the to a Pit room
        //pit room locations should only be added to locations that are empty (RoomType.Normal)

        /// <summary>
        /// reusing the GetRandomNormalLocation() in the map file to random the normal room.
        /// </summary>
        /// <returns>A Location object representing the randomized Pit room location.</returns>
        
        public Location RandomPitRoom(Map map)
        {
            Location randomPitRoom = map.GetRandomNormalLocation();
            // destructure the tuple then assign them to a value for each int
            (rowPitRoom, columnPitRoom) = randomPitRoom;
            return new Location(rowPitRoom, columnPitRoom);
        }

        //TODO: (A8) Create a method that returns a random location where it is suitable to add the minotaur monster
        //a minotaur should only be added to a location that is empty (RoomType.Normal)
        /// <summary>
        /// reusing the GetRandomNormalLocation() in the map file to random the normal room.
        /// </summary>
        /// <returns>A Location object representing the randomized Minotaur room location.</returns>
        public Location RandomMinotaurRoom(Map map)
        {
            Location randomMinotaurRoom = map.GetRandomNormalLocation();
            // destructure the tuple then assign them to a value for each int
            (rowMinotaurRoom, columnMinotaurRoom) = randomMinotaurRoom;
            return new Location(rowMinotaurRoom, columnMinotaurRoom);
        }

    }

    public class CreatorException : Exception
    {
        public CreatorException(int count) : base($"Not enough rooms remaining!\n" +
            $"Used a total of {count} rooms.")
        { }
    }
}

