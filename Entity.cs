using static CaveGame.GameSettings;

namespace CaveGame;

public abstract class Entity
{ 
    protected int TurnSpeed = TURN_SPEED;
    protected int MaxHealth;

    public int Health;
    public int Hunger; 
    protected int Speed;
    public int[] Position = new int[2];
    public int Layer;
    public SadConsole.Entities.Entity GlyphEntity;
    protected int TurnIndex;
}