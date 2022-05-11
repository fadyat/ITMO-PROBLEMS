namespace Analyzer;

public abstract class RequestAnalyzer
{
    protected static readonly Dictionary<string, int> Requests = new();

    public bool Active { get; protected set; }

    public abstract void AnalyzeRequests();

    protected static string[]? ParseInputCommand(string? command)
    {
        var parsedCommand = command?.Split(' ');
        return parsedCommand;
    }

    protected abstract void CommandSelector(string[]? parsedCommand);

    public virtual void Start()
    {
        Active = true;
    }

    protected virtual void Stop()
    {
        Active = false;
    }

    protected abstract bool IsCorrectCommand(IReadOnlyList<string>? parsedCommand);
}