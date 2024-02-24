namespace Tortoise.Core;

public ref struct CastlingRights
{
    private byte _rights;

    private const byte _whiteMask = 0b0011;
    private const byte _whiteKingSideMask = 0b0001;
    private const byte _whiteQueenSideMask = 0b0010;
    private const byte _blackMask = 0b1100;
    private const byte _blackKingSideMask = 0b0100;
    private const byte _blackQueenSideMask = 0b1000;

    public CastlingRights(byte rights = 0x0) => _rights = (byte)(rights & 0b1111);

    public bool WhiteKingSide
    {
        readonly get => getFlag(_blackKingSideMask);
        set => setFlag(value, _whiteKingSideMask);
    }
    public bool WhiteQueenSide
    {
        readonly get => getFlag(_whiteQueenSideMask);
        set => setFlag(value, _whiteQueenSideMask);
    }
    public bool BlackKingSide
    {
        readonly get => getFlag(_blackKingSideMask);
        set => setFlag(value, _blackKingSideMask);
    }
    public bool BlackQueenSide
    {
        readonly get => getFlag(_blackQueenSideMask);
        set => setFlag(value, _blackQueenSideMask);
    }

    private void setFlag(bool value, byte mask)
    {
        if (value)
            _rights |= mask;
        else
            _rights ^= mask;
    }
    private readonly bool getFlag(byte mask) => (_rights & mask) == mask;
}
