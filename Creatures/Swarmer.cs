namespace CaveGame.Creatures;

public class Swarmer : Entity, INonPlayerEntity
{
    protected override string Id => "swarmer";
    public override int SpawnWeight => 10;

    public Swarmer()
    {
        Name = "Swarmer";
        Pronouns = new []{"it", "it", "itself"};
    }
}