class Program
{   
    static int screenWidth = 50;
    static int screenHeigth = 20;
    public static void Main()
    {
        Player player = new();

        Hive hive = new Hive(PopulateEnemyList(screenWidth, screenHeigth, 3));
        Screen screen = new Screen(hive, player, screenWidth, screenHeigth);


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
                        break;

                    case ConsoleKey.RightArrow:
                        player.MoveRight();
                        break;
                    case ConsoleKey.Spacebar:
                        screen.Shoot(true);
                        break;
                    case ConsoleKey.Escape:
                        gameRunning = false;
                        break;
                }
            }

            screen.UpdateScreenContent();
            screen.Show();
            Thread.Sleep(100);

        }
     
    }

    public static List<Enemy> PopulateEnemyList(int screenWidth, int screenHeigth, int hiveHeigth){
        int lineLength = (int)(screenWidth - 3*screenWidth/5);
        lineLength += lineLength%2==0 ? 0 : 1;
        int startingDistance = screenWidth/2 - lineLength/2;
        
        hiveHeigth *= 2;
        int startingHeight = screenHeigth - hiveHeigth - 1;

        List<Enemy> enemyList = new();
        
        bool jmpFirstPos = false;
        for(int i = startingHeight; i < hiveHeigth+startingHeight; i+=2){
            if(jmpFirstPos){
                for(int j = startingDistance; j < lineLength+startingDistance; j+=2){
                    Enemy e = new(new Position(i, j));
                    enemyList.Add(e);
                }
                jmpFirstPos = false;
            }else{
                for(int j = startingDistance-1; j < lineLength+startingDistance; j+=2){
                    Enemy e = new(new Position(i, j));
                    enemyList.Add(e);
                }
                jmpFirstPos = true;
            }
            
        }
        return enemyList;
    }
}
