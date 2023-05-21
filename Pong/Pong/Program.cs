using Pong;
class Program
{
    public static void Main()
    {
        try
        {
            PongGame p = new PongGame();
            p.Menu();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
