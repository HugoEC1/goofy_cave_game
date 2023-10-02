namespace CaveGame.Scenes;

class StartScreen : ScreenObject
{
    private ScreenSurface _mainSurface;

    public StartScreen()
    {
        // Create a surface that's the same size as the screen.
        _mainSurface = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);

        int xPos = 0;
        int yPos = 4;
        int tempHeight = 0;

        string title = "sex";
        foreach (var item in title.ToAscii())
        {
            if (_selectedFont.IsCharacterSupported(item))
            {
                var charInfo = _selectedFont.GetCharacter(item);

                if (xPos + charInfo.Width >= Width)
                {
                    yPos += tempHeight + 1;
                    xPos = 0;
                }

                if (yPos >= Height)
                    break;

                var surfaceCharacter = _selectedFont.GetSurface(item);
                surfaceCharacter.Copy(this, xPos, yPos);

                if (surfaceCharacter.Height > tempHeight)
                    tempHeight = surfaceCharacter.Height;

                xPos += charInfo.Width;
            }
            else if (item == ' ')
            {
                // If the space character isn't supported, try to use some others
                if (_selectedFont.IsCharacterSupported('i'))
                    xPos += _selectedFont.GetCharacter('i').Width;
                else if (_selectedFont.IsCharacterSupported('1'))
                    xPos += _selectedFont.GetCharacter('1').Width;
                else if (_selectedFont.IsCharacterSupported('a'))
                    xPos += _selectedFont.GetCharacter('a').Width;
            }

                Children.Add(_mainSurface);
    }
}
