namespace Activity6_7_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.Write("Do you want to play a small, medium, or large game? ", ConsoleColor.White);

            Console.ForegroundColor = ConsoleColor.Cyan;

            // Default game setting in the event user does not input a proper size.
            LabyrinthGame game = Console.ReadLine() switch
            {
                "small" => LabyrinthCreator.CreateSmallGame(),
                "large" => LabyrinthCreator.CreateLargeGame(),
                _ => LabyrinthCreator.CreateMediumGame() // Make a medium game if input is "medium" or anything else
            };
            game.Run();
        }
    }
}
