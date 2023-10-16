using static SadRogue.Primitives.Color;
using static CaveGame.GameSettings;

namespace CaveGame;

public static class TileManager
{
    public static bool[,] ToBlocking(Tile[,] idChunk)
    {
        var blocking = new bool[idChunk.GetLength(0), idChunk.GetLength(1)];

        for (var y = 0; y < idChunk.GetLength(0); y++)
        {
            for (var x = 0; x < idChunk.GetLength(1); x++)
            {
                blocking[y,x] = idChunk[y,x].Blocking;
            }
        }

        return blocking;
    }
    public class Tile
    {
        public string Id = "";
        public ColoredGlyph Glyph = new ColoredGlyph();
        public string Name = "";
        public string Description = "";
        protected int MaxHealth;
        public int Health;
        public bool Blocking;
        public string State = "";
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
            State = "gas";
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
            State = "solid";
        }
    }
}