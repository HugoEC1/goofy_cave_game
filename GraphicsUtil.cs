namespace CaveGame;

public static class GraphicsUtil
{
    public static int HorCentered(ScreenSurface surface, int objectWidth)
    {
        int centerAlign = (surface.Width / 2) - (objectWidth / 2);
        return centerAlign;
    }
}