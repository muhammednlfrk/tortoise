using Tortoise.GUI.UI;
using Raylib_cs;
using Tortoise.GUI.Resource;
using Tortoise.GUI.UI.BoardUI;

// Game screen properties:
const int SCREEN_WIDTH = 976;
const int SCREEN_HEIGHT = 576;
const int TARGET_FPS = 60;

// Board properties:
const int SQUARE_SIZE = 64;
const int PIECE_SIZE = 64;

// Initialization.
Raylib.InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Tortoise");
Raylib.SetTargetFPS(TARGET_FPS);

// Load resources.
ResourceManager resourceManager = new("res", PIECE_SIZE);
resourceManager.LoadPieceTextures();

// List of uis.
IReadOnlyCollection<UIElement> uiElements = new List<UIElement>()
{
    new BoardUI(new BoardUIProperties
    {
        SquareSize = SQUARE_SIZE,
        PieceSize = PIECE_SIZE,
        Location = new(SCREEN_WIDTH - SQUARE_SIZE * 8f - 32f, 32f),
        PieceTextures = resourceManager.PieceTextures,
        LightSquareColor = Color.RayWhite,
        DarkSquareColor = Color.DarkGray,
    })
};

// Main game loop.
while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(new Color(20, 20, 20, 255));

    // Draw UI elements.
    foreach (UIElement element in uiElements)
    {
        element.Update();
        element.Draw();
    }

    Raylib.EndDrawing();
}

// Clear memory.
foreach (UIElement element in uiElements)
    element.Dispose();
resourceManager.Dispose();

// Close window and OpenGL context.
Raylib.CloseWindow();
