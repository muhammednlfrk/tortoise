namespace Tortoise.Core;

public readonly struct Move
{
    // 16 bit word for storing move data
    // 0-5 bits are "from" square index
    // 6-11 bits are "to" square index
    // 12-15 bits are flags
    private readonly ushort _move;
    private readonly ushort _from;
    private readonly ushort _to;
    private readonly ushort _flags;

    private const ushort _fromMask = (ushort)0x003f;  // 0b0000000000111111
    private const ushort _toMask = (ushort)0x0fc0;    // 0b0000111111000000
    private const ushort _flagsMask = (ushort)0xf000; // 0b1111000000000000

    public Move(ushort move)
    {
        _move = move;
        _from = (ushort)(_move & _fromMask);
        _to = (ushort)((_move & _toMask) >> 6);
        _flags = (ushort)((_move & _flagsMask) >> 12);
    }

    public Move(int from, int to, ushort flags)
        : this((ushort)((from & _fromMask) | ((to << 6) & _toMask) | ((flags << 12) & _flagsMask)))
    { }

    public readonly int From => _from;
    public readonly int To => _to;
    public readonly ushort Flags => _flags;

    public static implicit operator ushort(Move m) => m._move;
    public static explicit operator Move(ushort m) => new(m);

    // Flags:
    public const ushort QuietMoveFlag = 0b0000;              // 0
    public const ushort DoublePawnPushFlag = 0b0001;         // 1
    public const ushort KingCastleFlag = 0b0010;             // 2
    public const ushort QueenCastleFlag = 0b0011;            // 3
    public const ushort CapturesFlag = 0b0100;               // 4
    public const ushort EpCaptureFlag = 0b0101;              // 5
    public const ushort KnightPromotionFlag = 0b1000;        // 8
    public const ushort BishopPromotionFlag = 0b1001;        // 9
    public const ushort RookPromotionFlag = 0b1010;          // 10
    public const ushort QueenPromotionFlag = 0b1011;         // 11
    public const ushort KnightPromotionCaptureFlag = 0b1100; // 12
    public const ushort BishopPromotionCaptureFlag = 0b1101; // 13
    public const ushort RookPromotionCaptureFlag = 0b1110;   // 14
    public const ushort QueenPromotionCaptureFlag = 0b1111;  // 15
}
