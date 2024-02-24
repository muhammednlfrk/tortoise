using System.Text;

namespace Tortoise.Core.Helpers;

public static class MailboxExtensions
{
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
    public static int GetSquareIndex(int file, int rank) => rank * 8 + file;

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
}
