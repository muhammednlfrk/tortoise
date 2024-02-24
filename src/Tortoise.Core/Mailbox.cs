namespace Tortoise.Core;

public struct Mailbox
{
    /**
     * Represents the squares at the board. Each square stores piece info.
     * This board representation type is custom mailbox 8x8. For more
     * information about the representation check this:
     * https://www.chessprogramming.org/8x8_Board
     */
    private readonly uint[] _squares;

    public Mailbox() => _squares = new uint[64];

    public readonly uint this[int index]
    {
        get => _squares[index];
        set => _squares[index] = value & (uint)0b1111;
    }
}
