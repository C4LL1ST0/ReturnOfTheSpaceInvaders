class Program
{
    public static void Main()
    {
        Player player = new();

        Enemy e1 = new(1d, new Position(10, 3));
        Enemy e2 = new(2d, new Position(10, 42));
        Enemy e3 = new(3d, new Position(10, 15));
        List<Enemy> enemyList = new List<Enemy> { e1, e2, e3 };

        Hive hive = new Hive(enemyList);
        Screen screen = new Screen(hive, player);


        ConsoleKeyInfo keyInfo;
        bool gameRunning = true;

        while (gameRunning)
        {
            if (Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey(true); // true prevents the key from displaying in the console

                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        player.MoveLeft();
                        screen.UpdateScreenContent();
                        break;

                    case ConsoleKey.RightArrow:
                        player.MoveRight();
                        screen.UpdateScreenContent();
                        break;
                    case ConsoleKey.Spacebar:
                        screen.Shoot(true);
                        screen.UpdateScreenContent();
                        break;
                    case ConsoleKey.Escape:
                        gameRunning = false;
                        break;
                }
            }

            screen.UpdateScreenContent();
            screen.Show();
            Thread.Sleep(80);

        }
    }
}
