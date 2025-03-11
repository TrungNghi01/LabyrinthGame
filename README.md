# LabyrinthGame

A text-based Labyrinth game developed using Object-Oriented Programming (OOP) principles in C#. The game involves navigating a labyrinth, completing quests, and managing various commands like moving, picking up items, and viewing the map.

## Table of Contents
- [Description](#description)
- [Features](#features)
- [Classes and Structure](#classes-and-structure)
- [Installation](#installation)
- [Usage](#usage)
- [License](#license)

## Description
LabyrinthGame is a console-based adventure game where players control a character navigating a maze filled with different rooms. The game includes different commands for movement, interaction with items, and viewing the maze's layout. The goal is to avoid dangers, such as pit rooms, and retrieve important items like a sword to aid in progress.

## Features
- **Movement Commands**: Move North, South, East, or West within the labyrinth.
- **Item Interaction**: Pick up the sword if in the room with it.
- **Map Display**: View the maze layout if the player has the map.
- **Pit Rooms**: Pit rooms are deadly. Entering one kills the player.
- **Debug Mode**: View all room occurrences for debugging purposes.
- **Multiple Commands**: Execute commands using short keys or full command names.

## Classes and Structure
The game utilizes Object-Oriented Programming principles like interfaces, inheritance, and encapsulation. The key elements are:

### ICommand Interface
The `ICommand` interface defines the contract for all game commands. Each specific command implements this interface and defines the `Execute` method.

### Command Implementations
- **MoveCommand**: Handles movement in different directions (North, South, East, West).
- **GetSwordCommand**: Picks up the sword if the player is in the sword room.
- **ViewMapCommand**: Displays the map if the player has it.
- **DebugMap**: Displays all room occurrences for debugging.

### Example of Command Usage

```csharp
// Create a new command to move North
ICommand moveNorth = new MoveCommand(Direction.North);
moveNorth.Execute(game);

// Command to take the sword
ICommand takeSword = new GetSwordCommand();
takeSword.Execute(game);
```

## Usage
Once the game is running, you can issue various commands using either full names or short aliases. Example commands include:

Move North: move north or n or 1
Move South: move south or s or 2
Take Sword: take sword or ts or 6
View Map: view map or vm or 5
Debug Map: debug or d
The game will prompt you based on your current location and actions. If you enter a pit room, the game will end, and you'll be notified.

## Installation
- Clone the repository:
```bash
git clone https://github.com/yourusername/LabyrinthGame.git
```
- Open the project in Visual Studio or your preferred C# IDE.
- Build the solution to restore dependencies and compile the code.
