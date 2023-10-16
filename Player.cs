using static CaveGame.GameSettings;

namespace CaveGame;

public class Player
{
    protected int TurnSpeed = TURN_SPEED;
    private readonly int _maxHealth = 100;

    public int Health;
    public int Hunger; 
    protected int Speed;
    protected int TurnIndex;
    public int[] Position;
    
    public Player (int y, int x)
    {
        Health = _maxHealth;
        Speed = 10;
        Hunger = 100;
        Position = new []{y, x};
        TurnIndex = 0;
    }
    
    public void TakeDamage(int damage, string causeId)
    {
        Health -= damage;
        if(Health <= 0){
            Die(causeId);
        }
    }
    
    private void Die(string causeId)
    {
        switch (causeId) {
            case "swarmer":
                System.out.println(colour.RED + "You were torn apart by a swarmer." + colour.RESET);
                break;
            case "goldenFreddy":
                System.out.println(colour.RED + "WAS THAT THE BITE OF 87???" + colour.RESET);
                break;
        }

        System.out.println("\n--- YOU DIED ---");
    }
}