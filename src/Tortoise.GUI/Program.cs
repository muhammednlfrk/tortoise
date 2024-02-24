using System.Numerics;
using Tortoise.GUI.UI;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Tortoise.GUI.Resource;

const int SCREEN_WIDTH = 976;
const int SCREEN_HEIGHT = 576;

const int SQUARE_SIZE = 64;
const int PIECE_SIZE = 55;

ResourceManager resourceManager = new("res", SQUARE_SIZE, PIECE_SIZE);

Vector2 boardLocation = new(SCREEN_WIDTH - SQUARE_SIZE * 8f - 32f, 32f);

IUIElementCollection uiElements = new UIElementCollection
{
    new BoardUIElement(
        resource: resourceManager,
        location: boardLocation,
        squareSize: SQUARE_SIZE,
        pieceSize: PIECE_SIZE)
};

// Initialization.
InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Tortoise");
resourceManager.LoadPieceTextures();

// Main game loop.
while (!WindowShouldClose())
{
    BeginDrawing();
    ClearBackground(new Color(20, 20, 20, 255));

    // Draw UI elements.
    foreach (IUIElement element in uiElements)
        element.Draw();

    EndDrawing();
}

// Close window and OpenGL context.
CloseWindow();
