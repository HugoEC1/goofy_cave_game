using static SadRogue.Primitives.Color;

namespace CaveGame.Tiles;

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