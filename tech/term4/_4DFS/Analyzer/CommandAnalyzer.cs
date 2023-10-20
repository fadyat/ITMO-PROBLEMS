using Microsoft.CodeAnalysis;

namespace Analyzer;

public static class CommandAnalyzer
{
    public static IEnumerable<string> ParseInputCommand(string? command)
    {
        return CommandLineParser.SplitCommandLineIntoArguments(command, false);
    }

    public static bool IsCorrectCommand(Dictionary<string, int> requests, IReadOnlyList<string> parsedCommand)
    {
        var correctMainCommand = requests.Any() && requests.ContainsKey(parsedCommand[0]);
        if (!correctMainCommand)
        {
            throw new FormatException("Unknown command!");
        }

        var correctArgs = Equals(requests[parsedCommand[0]], parsedCommand.Count - 1);
        if (!correctArgs)
        {
            throw new FormatException($"Expected {requests[parsedCommand[0]]} args, was {parsedCommand.Count - 1}");
        }

        return correctMainCommand & correctArgs;
    }
}