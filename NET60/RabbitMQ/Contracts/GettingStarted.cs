namespace Contracts
{
    // C# 9 Above: new "record" keyword, easy to create types immutable properties by using positional parameters or standard property syntax.
    public record GettingStarted
    {
        public string Value { get; init; }
    }
}