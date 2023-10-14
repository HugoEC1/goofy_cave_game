using static SadRogue.Primitives.Color;
using static CaveGame.GameSettings;

namespace CaveGame;

public class TileManager
{
    static readonly Tile[] TileInit = {new Air(), new Stone()};
    
    public TileManager()
    {
        
    }

    public static ColoredGlyph[,] GetGlyphChunk(string[,] idChunk)
    {
        var glyphGrid = new ColoredGlyph[CHUNK_HEIGHT,CHUNK_WIDTH];
        
        for (var y = 0; y < CHUNK_HEIGHT; y++)
        {
            for (var x = 0; x < CHUNK_WIDTH; x++)
            {
                foreach (var tile in TileInit)
                {
                    if (tile.ID == idChunk[y,x]) { glyphGrid[y, x] = tile.TileGlyph; }
                }
            }
        }

        return glyphGrid;
    }
    
    abstract class Tile
    {
        public string ID = "";
        public ColoredGlyph TileGlyph = new ColoredGlyph();
        public string Name = "";
        public string Description = "";
        protected int MaxHealth;
        public int Health;
    }

    private class Air : Tile
    {
        public Air()
        {
            ID = "air";
            TileGlyph = new ColoredGlyph(White, Transparent, '·');
            Name = "Air";
            Description = "";
            MaxHealth = -1;
            Health = MaxHealth;
        }
    }
    
    private class Stone : Tile
    {
        public Stone()
        {
            ID = "stone";
            TileGlyph = new ColoredGlyph(White, Transparent, '#');
            Name = "Stone";
            Description = "Rock and stone!";
            MaxHealth = 1000;
            Health = MaxHealth;
        }
    }
}