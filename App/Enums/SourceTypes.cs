using System.Runtime.Serialization;

public enum SourceTypes
{
    [EnumMember(Value = "cambridge_bay_papers")]
    CambridgeBayPapers,
    [EnumMember(Value = "cambridge_bay_web_articles")]
    CambridgeBayWebArticles,
    [EnumMember(Value = "confluence_json")]
    ConfluenceJson
}