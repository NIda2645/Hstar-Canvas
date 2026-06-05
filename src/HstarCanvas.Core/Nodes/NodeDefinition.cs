namespace HstarCanvas.Core.Nodes;

public sealed record NodeDefinition(string Type, string Title, IReadOnlyList<NodePort> Inputs, IReadOnlyList<NodePort> Outputs);
