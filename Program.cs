using System.Text.Json.Nodes;
using Newtonsoft.Json;

class Program
{
    static int screenWidth = 50;
    static int screenHeigth = 20;
    static int hiveHeigth = 3;
    static Screen screen;
    public static void Main()
    {
        string? gameSave = null;


        bool gameRunning = true;
        if (File.Exists("gameSave.json"))
            Menu3(ref gameRunning, gameSave);
        else
            Menu1(ref gameRunning, gameSave);

        if (File.Exists("gameSave.json") && JsonConvert.DeserializeObject<Screen>(File.ReadAllText("gameSave.json")) != null)
        {
            screen = JsonConvert.DeserializeObject<Screen>(File.ReadAllText("gameSave.json"));
        }
        else
        {
            Player player = new();
            Hive hive = new Hive(PopulateEnemyList(screenWidth, screenHeigth, hiveHeigth));
            screen = new Screen(hive, player, screenWidth, screenHeigth);
        }


        ConsoleKeyInfo keyInfo;


        while (gameRunning)
        {
            if (Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        screen.player.MoveLeft();
                        break;

                    case ConsoleKey.RightArrow:
                        screen.player.MoveRight();
                        break;
                    case ConsoleKey.Spacebar:
                        screen.Shoot(true);
                        break;
                    case ConsoleKey.Escape:
                        Menu3(ref gameRunning, gameSave);
                        break;
                }
            }

            if (screen.player.hp <= 0 || screen.hive.firstEnemyPos.xPos == 1)
            {
                gameRunning = false;
                File.Delete("gameSave.json");
            }

            if (screen.hive.defeated)
            {
                if (hiveHeigth < 8)
                    screen.hive.enemyList = PopulateEnemyList(screenWidth, screenHeigth, hiveHeigth + 1);
            }

            screen.UpdateScreenContent();
            screen.Show();
            Thread.Sleep(100);

        }
    }

    public static void Menu1(ref bool gameRunning, string? gameSave)
    {
        Console.Clear();
        Console.WriteLine("RETURN OF THE SPACE INVADERS\n\n");
        Console.WriteLine("(1) - begin new game");
        Console.WriteLine("(2) - help & bindings");
        Console.WriteLine("(3) - continue or exit\n\n\n\n\n");
        switch (Console.ReadLine())
        {
            case "1":
                gameRunning = true;
                break;
            case "2":
                Menu2();
                gameRunning = true;
                break;
            case "3":
                Menu4(ref gameRunning, gameSave);
                break;
            default:
                Menu4(ref gameRunning, gameSave);
                break;
        }
    }

    public static void Menu2()
    {
        Console.Clear();
        Console.WriteLine("Welcome!");
        Console.WriteLine("Your goal is to kill all enemies, before they kill you.");
        Console.WriteLine("Controls: ");
        Console.WriteLine("left & rignt arrow - character movemet");
        Console.WriteLine("spacebar - shooting");
        Console.WriteLine("ESC - main menu\n\n\n\n");
        Console.WriteLine("press return to continue...");
        Console.ReadLine();
    }

    public static void Menu3(ref bool gameRunning, string? gameSave)
    {
        Console.Clear();
        Console.WriteLine("RETURN OF THE SPACE INVADERS\n\n");
        Console.WriteLine("(1) - begin new game");
        Console.WriteLine("(2) - load save");
        Console.WriteLine("(3) - help & bindings");
        Console.WriteLine("(4) - continue or exit\n\n\n\n\n");
        switch (Console.ReadLine())
        {
            case "1":
                File.Delete("gameSave.json");
                screen = new Screen(new Hive(PopulateEnemyList(screenWidth, screenHeigth, hiveHeigth)), new Player(), screenWidth, screenHeigth);
                gameRunning = true;
                break;
            case "2":
                if (File.Exists("gameSave.json") && JsonConvert.DeserializeObject<Screen>(File.ReadAllText("gameSave.json")) != null)
                {
                    screen = JsonConvert.DeserializeObject<Screen>(File.ReadAllText("gameSave.json"));
                }
                else
                {
                    Console.WriteLine("No savefile found");
                }

                break;
            case "3":
                Menu2();
                gameRunning = true;
                break;
            case "4":
                Menu4(ref gameRunning, gameSave);
                break;
            default:
                Menu4(ref gameRunning, gameSave);
                break;
        }
    }

    public static void Menu4(ref bool gameRunning, string? gameSave)
    {
        Console.Clear();
        Console.WriteLine("(1) - save and exit");
        Console.WriteLine("(2) - exit without saving");
        Console.WriteLine("(3) - return to game");

        switch (Console.ReadLine())
        {
            case "1":
                gameRunning = false;
                gameSave = screen.ToJson();
                File.WriteAllText("gameSave.json", gameSave);
                break;
            case "2":
                gameRunning = false;
                break;
            case "3":
                gameRunning = true;
                break;
            default:
                gameRunning = true;
                break;
        }


    }
    public static List<Enemy> PopulateEnemyList(int screenWidth, int screenHeigth, int hiveHeigth)
    {
        int lineLength = (int)(screenWidth - 3 * screenWidth / 5);
        lineLength += lineLength % 2 == 0 ? 0 : 1;
        int startingDistance = screenWidth / 2 - lineLength / 2;

        hiveHeigth *= 2;
        int startingHeight = screenHeigth - hiveHeigth - 1;

        List<Enemy> enemyList = new();

        bool jmpFirstPos = false;
        for (int i = startingHeight; i < hiveHeigth + startingHeight; i += 2)
        {
            if (jmpFirstPos)
            {
                for (int j = startingDistance; j < lineLength + startingDistance; j += 2)
                {
                    Enemy e = new(new Position(i, j));
                    enemyList.Add(e);
                }
                jmpFirstPos = false;
            }
            else
            {
                for (int j = startingDistance - 1; j < lineLength + startingDistance; j += 2)
                {
                    Enemy e = new(new Position(i, j));
                    enemyList.Add(e);
                }
                jmpFirstPos = true;
            }

        }
        return enemyList;
    }
}
