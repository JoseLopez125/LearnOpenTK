class Program
{
    static void Main()
    {
        using (Window game = new Window(800, 600, "Draw Rectangle"))
        {
            game.Run();
        }
    }
}
