using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Activity6_7_8
{
    /// <summary>
    /// Represents one of the several monster types in the game.
    /// </summary>
    public abstract class Monster
    {
        // The monster's current location.
        public Location Location { get; set; }

        // Whether the monster is alive or not.
        public bool IsAlive { get; set; } = true;

        // Creates a monster at the given location.
        public Monster(Location start) => Location = start;

        // Called when the monster and the player are both in the same room. Gives
        // the monster a chance to do its thing.
        public abstract void Activate(LabyrinthGame game);
    }

    //TODO: (A8) Add a Minotaur class that inherits from the Monster class. Make sure to implement any abstract methods.
    //When activated, the minotaur charges and knocks the player to a random room and the minotaur moves to a random room. 
    //Ensure both player and minotaur stay within the boundaries of the map. 
    //If the player has found the sword, the minotaur will hide it in a random room.
    class Minotaur : Monster
    {
        public Minotaur(Location minatourRoom) : base(minatourRoom) { }
        /// <summary>
        /// Activates the Minotaur when player go to this room.
        /// If the player has the sword, the Minotaur takes it and hides it in a random room.
        /// The Minotaur then moves to a new location row +1 and column -2.
        /// The player is also moved to a new location row -1 column +2.
        /// both the Minotaur and the player stay within the map boundaries and that 
        /// the Minotaur does not enter a non-normal room type (e.g., entrance, sword, or pit room).
        /// </summary>
        /// <param name="game">The current instance of the LabyrinthGame.</param>
        public override void Activate(LabyrinthGame game) 
        {
            
            if (game.PlayerHasSword)
            {
                game.PlayerHasSword = false;
                game.Map.SetRoomTypeAtLocation(game.Map.GetRandomNormalLocation(), RoomType.Sword);
            }
            // move the minatour
            Location minotaurNewLocation = new Location(Location.Row + 1, Location.Column - 2);
            // Ensure the Minotaur stays within the map boundaries and this monster just be hit back to a normal room
            if (game.Map.IsOnMap(minotaurNewLocation) &&
                game.Map.GetRoomTypeAtLocation(minotaurNewLocation) == RoomType.Normal)
            {               
                game.Map.SetRoomTypeAtLocation(base.Location, RoomType.Normal);
                base.Location = minotaurNewLocation;
                game.Map.SetRoomTypeAtLocation(base.Location, RoomType.Minotaur);
            }
            else
            {
                int minotaurNewRow = base.Location.Row + ((game.Map.Rows - 1) - base.Location.Row);
                int minataurNewColumn = 0;
                game.Map.SetRoomTypeAtLocation(base.Location, RoomType.Normal);
                base.Location = new Location(minotaurNewRow, minataurNewColumn);
                game.Map.SetRoomTypeAtLocation(base.Location, RoomType.Minotaur);
            }

            Location newPlayerLocation = new Location(game.Player.Location.Row - 1, game.Player.Location.Column + 2);
            // Ensure the player stays within the map boundaries and the layer can be hit into a pit room
            if (game.Map.IsOnMap(newPlayerLocation))
            {
                game.Player.Location = newPlayerLocation;
            }          
            else
            {
                int newPlayerRow = 0;
                int newPlayerCol = game.Player.Location.Column + ((game.Map.Columns-1) - game.Player.Location.Column);
                game.Player.Location = new Location(newPlayerRow, newPlayerCol);
            }     
        }

        public override string ToString()
        {
            return "M = Minotaur";
        }

    }

}
