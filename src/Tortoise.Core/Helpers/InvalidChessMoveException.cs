namespace Tortoise.Core.Helpers;

public class InvalidChessMoveException : Exception
{
    public InvalidChessMoveException() : base("Invalid chess move!") { }

    public InvalidChessMoveException(string message) : base(message) { }
}
