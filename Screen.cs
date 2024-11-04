using System.Data.Common;

class Screen
{
    
    public Hive hive;
    public Player player;
    public List<Shot> shots = new();
    public int screenWidth;
    public int screenHeight;
    public string[,] screen;


    public Screen(Hive hive, Player player, int screenWidth, int screenHeight)
    {
        this.hive = hive;
        this.player = player;
        this.screenWidth = screenWidth;
        this.screen = new string[screenHeight, screenWidth];
    }

    private void ClearScreen()
    {
        for (int i = 0; i < screen.GetLength(0); i++)
        {
            for (int j = 0; j < screen.GetLength(1); j++)
            {
                screen[i, j] = " ";
            }
        }
    }

    private int tickSpeed = 2;
    private int tickCount = 0;

    private void Tick(){
        tickCount++;
        shots = shots.Where(shot => shot.position.xPos < 19 && shot.position.xPos > 0).ToList(); //filter shots
        if (tickCount%tickSpeed == 0)
        {
            tickCount = 0;
            return;
        }

        //move shots
        for (int i = 0; i < shots.Count; i++ )
        {   
            if (shots[i].mine){
                shots[i].position = new Position(shots[i].position.xPos + 1, shots[i].position.yPos);
            }else{
                shots[i].position = new Position(shots[i].position.xPos - 1, shots[i].position.yPos);
            }
        }

        //check hits
        foreach(var shot in shots){
            foreach (var enemy in hive.enemyList)
            {
                if(shot.position.xPos == enemy.position.xPos && shot.position.yPos == enemy.position.yPos && shot.mine){
                    enemy.OnHit(shot.damage);
                    shot.OnHit(1);
                    player.score += 5;
                }
            }
        }

        if(hive.defeated){
            return;
        }

        shots = shots.Where(shot => shot.hp > 0).ToList();
        hive.OnUpdate();
        hive.OnMove();

    }

    public void UpdateScreenContent()
    {
        ClearScreen();
        
        Tick();

        screen[player.position.xIndex, player.position.yIndex] = player.shape;
        
        foreach (var enemy in hive.enemyList)
        {
            screen[enemy.position.xIndex, enemy.position.yIndex] = enemy.shape;
        }

        foreach (var shot in shots)
        {
            screen[shot.position.xIndex, shot.position.yIndex] = shot.shape;
        }
    }

    public void Show()
    {
        Console.Clear();
        UpdateScreenContent();
        Console.WriteLine(("SCORE: "  + player.score.ToString()).PadRight(25) + ("HP: " + player.hp.ToString()).PadLeft(25));
        for (int i = 0; i < screen.GetLength(0); i++)
        {
            for (int j = 0; j < screen.GetLength(1); j++)
            {
                Console.Write(screen[i, j]);
            }
            Console.WriteLine(); // Move to the next row after each row of cells
        }
    }

    public void Shoot(bool isMine){
        Shot shot = new Shot(new Position(player.position.xPos+1, player.position.yPos), isMine);
        shots.Add(shot);
    }

}