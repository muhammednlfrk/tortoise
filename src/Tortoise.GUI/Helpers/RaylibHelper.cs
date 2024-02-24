using Raylib_cs;

namespace Tortoise.GUI.Helpers;

internal static class RaylibHelper
{
    public static Texture2D LoadImageAsTexture(string fileName, int width, int height)
    {
        Image image = Raylib.LoadImage(fileName);
        Raylib.ImageResize(ref image, width, height);
        Texture2D texture = Raylib.LoadTextureFromImage(image);
        return texture;
    }
}
