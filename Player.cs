using static CaveGame.Program;
using static CaveGame.GameSettings;
using static CaveGame.Managers.ChunkManager;
using static CaveGame.Managers.TileManager;

namespace CaveGame;

public class Player
{
    private int _turnSpeed = TURN_SPEED;
    private const int MAX_HEALTH = 100;

    public int Health;
    public int Hunger; 
    private int Speed;
    public int[] Position;
    public Chunk Chunk;
    public InputHandler InputHandler;
    private int TurnIndex;
    
    public Player(int y, int x, Chunk chunk, InputHandler inputHandler)
    {
        Health = MAX_HEALTH;
        Speed = 10;
        Hunger = 100;
        Position = new []{y, x};
        Chunk = chunk;
        InputHandler = inputHandler;
        TurnIndex = 0;
    }
    
    public void TakeDamage(int damage, string causeId)
    {
        Health -= damage;
        if(Health <= 0){
            Die(causeId);
        }
    }
    
    private static void Die(string causeId)
    {
        switch (causeId) {
            case "swarmer":
                GetGameScreen().PrintLog("You were torn apart by a swarmer.", Color.Red);
                break;
            case "goldenFreddy":
                GetGameScreen().PrintLog("WAS THAT THE BITE OF 87???", Color.Red);
                break;
        }
        GetGameScreen().PrintLog("--- YOU DIED ---", Color.Red);
    }

    private TaskCompletionSource<bool>? _turnActionComplete;
    public async void Turn()
    {
        TurnIndex += Speed;

        while (TurnIndex >= _turnSpeed)
        {
            TurnIndex -= _turnSpeed;
            
            // regen 1 health per turn
            if (Health < 100) {
                Health += 1;
            }

            _turnActionComplete = new TaskCompletionSource<bool>();
            InputHandler.PlayerInputEnabled = true;
            await _turnActionComplete.Task;
            InputHandler.PlayerInputEnabled = false;
        }
    }
    public void Wait()
    {
        _turnActionComplete?.TrySetResult(true);
    }
    public void Move(int[] direction)
    {
        int[] wantedPosition = {Position[0] + direction[0], Position[1] + direction[1]};
        if (Chunk.ToBlocking()[wantedPosition[0], wantedPosition[1]] == false)
        {
            Position = wantedPosition;
        }
    }
}