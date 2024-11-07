using System.Text.Json.Nodes;
using Newtonsoft.Json;

class Program
{
    static int screenWidth = 50;
    static int screenHeigth = 20;
    static int hiveHeigth = 3;
    public static void Main()
    {
        string? gameSave = null;
        Screen screen;

        if(File.Exists("gameSave.json") && JsonConvert.DeserializeObject<Screen>(File.ReadAllText("gameSave.json"))==null){
            screen = JsonConvert.DeserializeObject<Screen>(File.ReadAllText("gameSave.json"));
        }else{
            Player player = new();
            Hive hive = new Hive(PopulateEnemyList(screenWidth, screenHeigth, hiveHeigth));
            screen = new Screen(hive, player, screenWidth, screenHeigth);
        }

        
        

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
                        screen.player.MoveLeft();
                        break;

                    case ConsoleKey.RightArrow:
                        screen.player.MoveRight();
                        break;
                    case ConsoleKey.Spacebar:
                        screen.Shoot(true);
                        break;
                    case ConsoleKey.Escape:
                        gameRunning = false;
                        gameSave = screen.ToJson();
                        break;
                }
            }

            if(screen.player.hp <= 0 || screen.hive.firstEnemyPos.xPos == 1){
                gameRunning = false;
                File.Delete("gameSave.json");
            }

            if(screen.hive.defeated){
                if(hiveHeigth<8)
                    screen.hive.enemyList = PopulateEnemyList(screenWidth, screenHeigth, hiveHeigth+1);
            }

            screen.UpdateScreenContent();
            screen.Show();
            Thread.Sleep(100);

        }

        File.WriteAllText("gameSave.json", gameSave);

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
