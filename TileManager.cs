using static SadRogue.Primitives.Color;

namespace CaveGame;

public class TileManager
{
    static readonly Tile[] TileInit = {new Air(), new Stone()};
    
    public TileManager()
    {
        
    }

    public static ColoredGlyph[,] GetGlyphGrid(string[,] idGrid, Point position, int width, int height = -1)
    {
        if (height == -1) { height = width; }

        var glyphGrid = new ColoredGlyph[height,width];
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                foreach (var tile in TileInit)
                {
                    if (tile.ID == idGrid[y + position.Y, x + position.X]) { glyphGrid[y, x] = tile.TileGlyph; }
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