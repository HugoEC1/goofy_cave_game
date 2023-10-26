using CaveGame.Managers;
using static CaveGame.Program;
using static CaveGame.GameSettings;
using static CaveGame.Managers.ChunkManager;

namespace CaveGame;

public class Player : Entity
{
    private readonly InputHandler _inputHandler;
    
    public Player(int y, int x, InputHandler inputHandler)
    {
        Id = "player";
        Name = "Player";
        Pronouns = new []{"they", "them", "their"};
        MaxHealth = 100;
        TurnSpeed = TURN_SPEED;
        Health = MaxHealth;
        Speed = 10;
        Position = new []{y, x};
        Layer = 0;
        GlyphEntity = new SadConsole.Entities.Entity(foreground: Color.Red, background: Color.Black, glyph: '@', zIndex: 9000) { Position = new Point(GAMEVIEW_WIDTH / 2, GAMEVIEW_HEIGHT / 2) };
        _inputHandler = inputHandler;
        TurnIndex = 0;
    }

    private TaskCompletionSource<bool>? _turnActionComplete;
    public async Task Turn()
    {
        TurnIndex += Speed;

        while (TurnIndex >= TurnSpeed)
        {
            TurnIndex -= TurnSpeed;
            
            // regen 1 health per turn
            if (Health < 100) {
                Health += 1;
            }

            _turnActionComplete = new TaskCompletionSource<bool>();
            _inputHandler.PlayerInputEnabled = true;
            await _turnActionComplete.Task;
            _inputHandler.PlayerInputEnabled = false;
            System.Console.WriteLine(Position[1] + ", " + Position[0]);
            ViewManager.UpdateView(this);
        }
    }
    public void Wait()
    {
        _turnActionComplete?.TrySetResult(true);
    }

    private int _previousChunkY;
    private int _previousChunkX;
    public void Move(int[] direction)
    {
        int[] wantedPosition = {Position[0] + direction[0], Position[1] + direction[1]};
        var wantedLocalPosition = ToLocalPosition(wantedPosition);
        var chunkPosition = GetChunkPosition(wantedPosition);

        var silly = ToLocalPosition(wantedPosition);
        System.Console.WriteLine("Local Chunk Position: " + silly[1] + ", " + silly[0]);
        
        //if (GetChunk(chunkPosition[0], chunkPosition[1], Layer).Blocking[wantedLocalPosition[0], wantedLocalPosition[1]]) return;
        if (_previousChunkY != chunkPosition[0])
        {
            _previousChunkY = chunkPosition[0];
            LoadSurroundingChunks(chunkPosition[0], chunkPosition[1], Layer);
        }
        else if (_previousChunkX != chunkPosition[1])
        {
            _previousChunkX = chunkPosition[1];
            LoadSurroundingChunks(chunkPosition[0], chunkPosition[1], Layer);
        }
        Position = wantedPosition;
        _turnActionComplete?.TrySetResult(true);
    }
}