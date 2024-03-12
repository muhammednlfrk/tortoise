
namespace Tortoise.Core;

public struct Bitboard
{
    private ulong _bb;

    public Bitboard() : this(0) { }

    public Bitboard(ulong bb)
    {
        _bb = bb;
    }

    public const ulong Empty = 0x0ul;
    public const ulong Universe = 0xFFFFFFFFFFFFFFFFul;

    #region Operators

    public static implicit operator ulong(Bitboard bitboard) => bitboard._bb;
    public static explicit operator Bitboard(ulong bitboard) => new(bitboard);

    public static Bitboard operator +(Bitboard a, Bitboard b) => new(a._bb + b._bb);
    public static Bitboard operator +(ulong a, Bitboard b) => new(a + b._bb);
    public static Bitboard operator +(Bitboard a, ulong b) => new(a._bb + b);

    public static Bitboard operator -(Bitboard a, Bitboard b) => new(a._bb - b._bb);
    public static Bitboard operator -(ulong a, Bitboard b) => new(a - b._bb);
    public static Bitboard operator -(Bitboard a, ulong b) => new(a._bb - b);

    public static Bitboard operator *(Bitboard a, Bitboard b) => new(a._bb * b._bb);
    public static Bitboard operator *(ulong a, Bitboard b) => new(a * b._bb);
    public static Bitboard operator *(Bitboard a, ulong b) => new(a._bb * b);

    public static Bitboard operator /(Bitboard a, Bitboard b) => new(a._bb / b._bb);
    public static Bitboard operator /(ulong a, Bitboard b) => new(a / b._bb);
    public static Bitboard operator /(Bitboard a, ulong b) => new(a._bb / b);

    public static Bitboard operator %(Bitboard a, Bitboard b) => new(a._bb % b._bb);
    public static Bitboard operator %(ulong a, Bitboard b) => new(a % b._bb);
    public static Bitboard operator %(Bitboard a, ulong b) => new(a._bb % b);

    public static Bitboard operator &(Bitboard a, Bitboard b) => new(a._bb & b._bb);
    public static Bitboard operator &(ulong a, Bitboard b) => new(a & b._bb);
    public static Bitboard operator &(Bitboard a, ulong b) => new(a._bb & b);

    public static Bitboard operator |(Bitboard a, Bitboard b) => new(a._bb | b._bb);
    public static Bitboard operator |(ulong a, Bitboard b) => new(a | b._bb);
    public static Bitboard operator |(Bitboard a, ulong b) => new(a._bb | b);


    public static Bitboard operator ^(Bitboard a, Bitboard b) => new(a._bb ^ b._bb);
    public static Bitboard operator ^(ulong a, Bitboard b) => new(a ^ b._bb);
    public static Bitboard operator ^(Bitboard a, ulong b) => new(a._bb ^ b);

    public static Bitboard operator >>(Bitboard a, int b) => new(a._bb >> b);
    public static Bitboard operator <<(Bitboard a, int b) => new(a._bb << b);

    public static Bitboard operator ~(Bitboard a) => new(~a._bb);
    public static Bitboard operator +(Bitboard a) => new(+a._bb);
    public static Bitboard operator ++(Bitboard a) => new(a._bb + 1ul);
    public static Bitboard operator --(Bitboard a) => new(a._bb - 1ul);

    public static bool operator ==(Bitboard a, Bitboard b) => a._bb == b._bb;
    public static bool operator ==(ulong a, Bitboard b) => a == b._bb;
    public static bool operator ==(Bitboard a, ulong b) => a._bb == b;

    public static bool operator !=(Bitboard a, Bitboard b) => a._bb != b._bb;
    public static bool operator !=(ulong a, Bitboard b) => a != b._bb;
    public static bool operator !=(Bitboard a, ulong b) => a._bb != b;

    public static bool operator <(Bitboard a, Bitboard b) => a._bb < b._bb;
    public static bool operator <(ulong a, Bitboard b) => a < b._bb;
    public static bool operator <(Bitboard a, ulong b) => a._bb < b;

    public static bool operator >(Bitboard a, Bitboard b) => a._bb > b._bb;
    public static bool operator >(ulong a, Bitboard b) => a > b._bb;
    public static bool operator >(Bitboard a, ulong b) => a._bb > b;

    public static bool operator <=(Bitboard a, Bitboard b) => a._bb <= b._bb;
    public static bool operator <=(ulong a, Bitboard b) => a <= b._bb;
    public static bool operator <=(Bitboard a, ulong b) => a._bb <= b;

    public static bool operator >=(Bitboard a, Bitboard b) => a._bb >= b._bb;
    public static bool operator >=(ulong a, Bitboard b) => a >= b._bb;
    public static bool operator >=(Bitboard a, ulong b) => a._bb >= b;

    public override readonly bool Equals(object? obj)
    {
        return obj is Bitboard bitboard &&
               _bb == bitboard._bb;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(_bb);
    }

    #endregion
}
