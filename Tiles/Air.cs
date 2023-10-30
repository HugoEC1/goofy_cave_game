using static SadRogue.Primitives.Color;

namespace CaveGame.Tiles;

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