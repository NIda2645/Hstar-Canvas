namespace HstarCanvas.Core.Workflows;

public sealed record WorkflowNode(string Id, string Type, string Title, double X, double Y)
{
    public static WorkflowNode Create(string id, string type, string title, double x, double y)
        => new(id, type, title, x, y);
}

