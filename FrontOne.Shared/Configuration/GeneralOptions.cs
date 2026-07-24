namespace FrontOne.Shared.Configuration;

public class GeneralOptions
{
    public const string SectionName = "General";

    public string ApplicationName { get; set; } = "FrontOne";
    public string Environment { get; set; } = "Development";
}
