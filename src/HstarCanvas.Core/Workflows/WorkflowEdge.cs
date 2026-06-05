namespace HstarCanvas.Core.Workflows;

public sealed record WorkflowEdge(string Id, string SourceNodeId, string SourcePortId, string TargetNodeId, string TargetPortId);
