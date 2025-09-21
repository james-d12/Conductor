using Conductor.Infrastructure.Score.Models;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Conductor.Infrastructure.Score;

public interface IScoreParser
{
    Task<ScoreFile?> ParseAsync(string file);
}

public sealed class ScoreParser : IScoreParser
{
    private readonly ILogger<ScoreParser> _logger;

    public ScoreParser(ILogger<ScoreParser> logger)
    {
        _logger = logger;
    }

    public async Task<ScoreFile?> ParseAsync(string file)
    {
        if (!File.Exists(file))
        {
            Console.WriteLine("File not exists for score parser");
            return null;
        }

        var fileContents = await File.ReadAllTextAsync(file);

        var scoreFile = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build()
            .Deserialize<ScoreFile>(fileContents);

        _logger.LogInformation("Score File: {Contents}", string.Join(",", scoreFile?.Resources?.Keys.ToList() ?? []));

        return scoreFile;
    }
}