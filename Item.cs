namespace CaveGame;

public abstract class Item{
    public string Id = "";
    public ColoredGlyph Glyph = new ColoredGlyph();
    public string Name = "";
    public string Description = "";
    public int StackSize;
    public int StackCount;
    public int Weight;

    public Player player;

    public void PickUp(){
        // player.inventory.add(this);
    }

    public void Drop(){
        // print(how many to drop?) or (drop all?) or something
        int amount = 1;
        // amount = input
        if(amount <= StackCount){
            StackCount -= amount;
            // spawn item in world
            // print ("You dropped " + amount + " " + Name + "s.")
        }
        else{
            // print("You don't have that many!")
        }
    }

    public void Consumed(){
        // print("You consumed " + Name + ".")
        StackCount -= 1;
        // print(yummy)
    }
}