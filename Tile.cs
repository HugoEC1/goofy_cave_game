namespace CaveGame;

public class Tile
{
    public enum States
    {
        Solid,
        Liquid,
        Gas
    }
    
    public string Id = "";
    public ColoredGlyph Glyph = new ColoredGlyph();
    public string Name = "";
    public string Description = "";
    protected int MaxHealth;
    public int Health;
    public bool Blocking;
    public States State;
}