// These classes are temporary and are just here for the initial vectorDB setup.
public class AddRequest
{
    public string Id { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

public class QueryRequest
{
    public string Text { get; set; } = string.Empty;
    public int NResults { get; set; } = 5;
}