namespace Tortoise.Core.Helpers;

public static class MoveExtensions
{
    public static bool IsQuietMove(this Move move) => move.Flags == Move.QuietMoveFlag;
    public static bool IsDoublePawnPush(this Move move) => move.Flags == Move.DoublePawnPushFlag;
    public static bool IsCapture(this Move move) => move.Flags == Move.CapturesFlag;
    public static bool IsEnPassant(this Move move) => move.Flags == Move.EpCaptureFlag;
    public static bool IsCastling(this Move move) => move.Flags is Move.KingCastleFlag or Move.QueenCastleFlag;
    public static bool IsPromotion(this Move move) => move.Flags >= 8;
    public static bool IsPromotionWithCapture(this Move move) => move.Flags >= 12;

    public static bool IsCaptureAny(this Move move) => move.IsCapture() || move.IsPromotionWithCapture();
}
