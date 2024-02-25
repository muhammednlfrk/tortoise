using System.Text;

namespace Tortoise.Core;

public readonly struct Mailbox
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
        set => _squares[index] = value & 0b1111;
    }

    #region Extensions

    private static readonly IReadOnlyDictionary<int, char> _rankNames = new Dictionary<int, char>(8)
    {
        { 0, 'a' },
        { 1, 'b' },
        { 2, 'c' },
        { 3, 'd' },
        { 4, 'e' },
        { 5, 'f' },
        { 6, 'g' },
        { 7, 'h' }
    };

    private static readonly IReadOnlyDictionary<int, char> _fileNames = new Dictionary<int, char>(8)
    {
        { 0, '1' },
        { 1, '2' },
        { 2, '3' },
        { 3, '4' },
        { 4, '5' },
        { 5, '6' },
        { 6, '7' },
        { 7, '8' }
    };

    public static int GetRankIndex(int squareIndex) => (squareIndex & 0b111000) >> 3;
    public static int GetFileIndex(int squareIndex) => squareIndex & 0b000111;
    public static int GetSquareIndex(int file, int rank)
    {
        if (file < 0 || file > 7 || rank < 0 || rank > 7)
            return -1;

        return (rank * 8) + file;
    }

    public static string GetSquareName(int squareIndex)
    {
        if (squareIndex < 0 || squareIndex > 63)
            throw new ArgumentException("Invalid square index.", nameof(squareIndex));

        StringBuilder squareNameBuilder = new();

        int rankIndex = GetRankIndex(squareIndex);
        squareNameBuilder.Append(_rankNames[rankIndex]);

        int fileIndex = GetFileIndex(squareIndex);
        squareNameBuilder.Append(_fileNames[fileIndex]);

        return squareNameBuilder.ToString();
    }

    #endregion
}
