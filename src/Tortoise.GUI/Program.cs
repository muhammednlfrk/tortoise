using static Raylib_cs.Raylib;
using System.Numerics;
using Tortoise.GUI.UI;
using Raylib_cs;

IUIElementCollection uiElements = new UIElementCollection
{
    new BoardUIElement(
        location: new Vector2(432f, 32f),
        squareSize: 64f,
        pieceSize: 60f)
};

InitWindow(976, 576, "Tortoise");

while (!WindowShouldClose())
{
    ClearBackground(new Color(20, 20, 20, 255));
    BeginDrawing();

    foreach (IUIElement element in uiElements)
        element.Draw();

    EndDrawing();
}

CloseWindow();
