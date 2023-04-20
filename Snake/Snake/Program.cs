
namespace Snake;

internal class Program
{
    public static void Main()
    {
        try
        {
            Game game = new Game();
            game.StartTheGame();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}