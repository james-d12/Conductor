namespace Conductor.Core.ResourceTemplate.Domain;

public sealed record ResourceTemplateVersionSource
{
    public required Uri BaseUrl { get; init; }
    public required string FolderPath { get; init; }
    public required string Tag { get; init; }

    public static ResourceTemplateVersionSource CreateFromUrl(Uri url)
    {
        /*
        if (url.Host == "github.com")
        {
            // We know we have been provided the base url in essence.
            if (url.ToString().EndsWith(".git"))
            {S
            }

            // We have been provided a folder path
        }*/

        return new ResourceTemplateVersionSource
        {
            BaseUrl = url,
            FolderPath = string.Empty,
            Tag = string.Empty
        };
    }
}