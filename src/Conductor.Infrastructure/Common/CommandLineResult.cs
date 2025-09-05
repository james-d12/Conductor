namespace Conductor.Infrastructure.Common;

public sealed record CommandLineResult(string StdOut, string StdErr, int ExitCode);