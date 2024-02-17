using Raylib_cs;
using Tortoise.GUI.UI;

BoardUI boardUI = new(432f, 32f);

Raylib.InitWindow(976, 576, "Hello World");

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(new Color(20, 20, 20, 255));
    boardUI.Draw();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
