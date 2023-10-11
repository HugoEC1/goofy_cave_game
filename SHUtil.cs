namespace CaveGame;

public static class SHUtil
{
    public static int Random(int min, int max){
        return new Random().Next(min, max);
    }
}