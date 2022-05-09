namespace Analyzer;

public abstract class RequestAnalyzer
{
    protected static readonly Dictionary<string, int> Requests = new();

    public bool Active { get; protected set; }

    public abstract void AnalyzeRequests();

    protected static string[]? ParseInputCommand()
    {
        var command = Console.ReadLine();
        var parsedCommand = command?.Split(' ');
        return parsedCommand;
    }

    public virtual void Start()
    {
        Active = true;
    }

    public virtual void Stop()
    {
        Active = false;
    }

    protected abstract bool IsCorrectCommand(IReadOnlyList<string>? parsedCommand);
}