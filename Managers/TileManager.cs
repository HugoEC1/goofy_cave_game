using static SadRogue.Primitives.Color;
using static CaveGame.GameSettings;
using static CaveGame.Managers.ChunkManager;

namespace CaveGame.Managers;

public static class TileManager
{
    public enum States
    {
        Solid,
        Liquid,
        Gas
    }
    public abstract class Tile
    {
        public string Id = "";
        public ColoredGlyph Glyph = new ColoredGlyph();
        public string Name = "";
        public string Description = "";
        protected int MaxHealth;
        public int Health;
        public bool Blocking;
        public States State;
    }
    public class Air : Tile
    {
        public Air()
        {
            Id = "air";
            Glyph = new ColoredGlyph(White, Transparent, 250);
            Name = "Air";
            Description = "";
            MaxHealth = -1;
            Health = MaxHealth;
            Blocking = false;
            State = States.Gas;
        }
    }
    public class Stone : Tile
    {
        public Stone()
        {
            Id = "stone";
            Glyph = new ColoredGlyph(White, Transparent, '#');
            Name = "Stone";
            Description = "Rock and stone!";
            MaxHealth = 1000;
            Health = MaxHealth;
            Blocking = true;
            State = States.Solid;
        }
    }
}