using Pong;
class Program
{
    public static void Main()
    {
        try
        {
            PongGame p = new PongGame();
            p.Game();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
