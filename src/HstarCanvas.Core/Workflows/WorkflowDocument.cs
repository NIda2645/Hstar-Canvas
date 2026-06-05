namespace HstarCanvas.Core.Workflows;

public sealed record WorkflowDocument(
    string SchemaVersion,
    string Id,
    string Name,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    CanvasViewport Viewport,
    IReadOnlyList<WorkflowNode> Nodes,
    IReadOnlyList<WorkflowEdge> Edges)
{
    public static WorkflowDocument CreateEmpty(string id, string name)
    {
        var now = DateTimeOffset.UtcNow;
        return new WorkflowDocument("1.0", id, name, now, now, CanvasViewport.Default, [], []);
    }

    public WorkflowDocument WithNodes(IReadOnlyList<WorkflowNode> nodes)
        => this with { Nodes = nodes, UpdatedAt = DateTimeOffset.UtcNow };

    public WorkflowDocument WithEdges(IReadOnlyList<WorkflowEdge> edges)
        => this with { Edges = edges, UpdatedAt = DateTimeOffset.UtcNow };
}
