struct Position{
    public int xPos;
    public int yPos;
    public int xIndex;
    public int yIndex;

    public Position(int xPos, int yPos){
        this.xPos = xPos;
        this.yPos = yPos;
        xIndex = 20-xPos-1;
        yIndex = yPos;
    } 
}

class Object{
    public double id;
    public int hp;
    public string shape;
    public Position position;

    public Object(double id, int hp, string shape, Position position){
        this.id = id;
        this.hp = hp;
        this.shape = shape;
        this.position = position;
    }

    public void OnHit(int damage){
        hp-=damage;
    }
}

class Player : Object{
    public int score = 0;
    public int damage = 1;
    
    
    public Player() : base(0d, 3, "@", new Position(0, 0)){}

    public void MoveLeft(){
        if(position.yPos>0){
            position = new Position(position.xPos, position.yPos -1);
        }
    }

    public void MoveRight(){
        if(position.yPos<49){
            position = new Position(position.xPos, position.yPos+1);
        }
    }

}

class Enemy : Object {
    public int damage = 1;

    public Enemy(double id, Position position) : base(id, 1, "#", position){
        this.position = position;
    }
}

class Hive{
    public List<Enemy> enemyList;
    public bool defeated = false;
    private bool goingLeft = true;
    public Hive(List<Enemy> enemyList){
        this.enemyList = enemyList;
    }

    public void OnUpdate(){
	    enemyList =  enemyList.Where(enemy => enemy.hp > 0).ToList();
        if(enemyList.Count==0){
            defeated = true;
        }
    }

    public void OnMove(){
        List<Enemy> sorted = enemyList.OrderBy(enemy => enemy.position.yPos).ToList();
        Position firstEnemyPos = sorted[0].position;
        Position LastEnemyPos = sorted[sorted.Count-1].position;

        if(firstEnemyPos.yPos == 0 ){
            goingLeft = false;
            foreach(var enemy in enemyList){
                enemy.position = new Position(enemy.position.xPos - 1, enemy.position.yPos);
            }
        }
        if(LastEnemyPos.yPos == 49){
            goingLeft = true;
        }

        if(goingLeft){
            foreach(var enemy in enemyList){
                enemy.position = new Position(enemy.position.xPos, enemy.position.yPos-1);
            }
        }else{
            foreach(var enemy in enemyList){
                enemy.position = new Position(enemy.position.xPos, enemy.position.yPos+1);
            }
        }

    }

}

class Shot : Object{
    public bool mine;
    public int damage = 1;

    public Shot(double id, Position position, bool mine) : base(id, 1, "|", position){
        this.position = new Position(position.xPos, position.yPos);
        this.mine = mine;
    }
}

