namespace Conductor.Infrastructure.Common.CommandLine;

public sealed record CommandLineResult(string StdOut, string StdErr, int ExitCode);