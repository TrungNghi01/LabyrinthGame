using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activity6_7_8
{
    // Represents the player in the game.
    public class Player
    {
        // Creates a new player that starts at the given location.
        public Player(Location start) => Location = start;

        // The player's current location.
        public Location Location { get; set; }

        // Indicates whether the player is alive or not.
        public bool IsAlive { get; private set; } = true;

        // Indicates whether the player currently has the catacomb map.
        public bool HasMap { get; set; } = true;

        // Indicates whether the player currently has the sword.
        public bool HasSword { get; set; } = false;

        // Explains why a player died.
        public string CauseOfDeath { get; private set; }

        public void Kill(string cause)
        {
            IsAlive = false;
            CauseOfDeath = cause;
        }
    }
}
