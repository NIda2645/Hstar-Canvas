using HstarCanvas.Core.Nodes;
using HstarCanvas.Core.Workflows;

namespace HstarCanvas.Core.Validation;

public sealed class WorkflowValidator
{
    private readonly NodeRegistry _registry;

    private WorkflowValidator(NodeRegistry registry)
    {
        _registry = registry;
    }

    public static WorkflowValidator CreateDefault() => new(NodeRegistry.CreateDefault());

    public IReadOnlyList<WorkflowValidationError> Validate(WorkflowDocument document)
    {
        var errors = new List<WorkflowValidationError>();
        var nodes = document.Nodes.ToDictionary(node => node.Id, StringComparer.Ordinal);
        var connectedInputs = new HashSet<(string NodeId, string PortId)>();

        foreach (var node in document.Nodes)
        {
            if (!_registry.Contains(node.Type))
            {
                errors.Add(new(WorkflowValidationCode.NodeTypeUnknown, "Unknown node type", NodeId: node.Id));
            }
        }

        foreach (var edge in document.Edges)
        {
            if (!nodes.TryGetValue(edge.SourceNodeId, out var source) || !nodes.TryGetValue(edge.TargetNodeId, out var target))
            {
                errors.Add(new(WorkflowValidationCode.NodeNotFound, "Edge references a missing node", EdgeId: edge.Id));
                continue;
            }

            var sourceDefinition = _registry.Get(source.Type);
            var targetDefinition = _registry.Get(target.Type);
            var sourcePort = sourceDefinition.Outputs.FirstOrDefault(port => port.Id == edge.SourcePortId);
            var targetPort = targetDefinition.Inputs.FirstOrDefault(port => port.Id == edge.TargetPortId);

            if (sourcePort is null || targetPort is null)
            {
                errors.Add(new(WorkflowValidationCode.PortNotFound, "Edge references a missing port", EdgeId: edge.Id));
                continue;
            }

            connectedInputs.Add((target.Id, targetPort.Id));

            if (!AreCompatible(sourcePort.DataType, targetPort.DataType))
            {
                errors.Add(new(WorkflowValidationCode.TypeMismatch, "Port types are incompatible", NodeId: target.Id, EdgeId: edge.Id, PortId: targetPort.Id));
            }
        }

        foreach (var node in document.Nodes.Where(node => _registry.Contains(node.Type)))
        {
            foreach (var input in _registry.Get(node.Type).Inputs.Where(port => port.Required))
            {
                if (!connectedInputs.Contains((node.Id, input.Id)))
                {
                    errors.Add(new(WorkflowValidationCode.RequiredInputMissing, "Required input is not connected", NodeId: node.Id, PortId: input.Id));
                }
            }
        }

        if (HasCycle(document))
        {
            errors.Add(new(WorkflowValidationCode.CycleDetected, "Workflow contains a cycle"));
        }

        return errors;
    }

    private static bool AreCompatible(PortDataType source, PortDataType target)
        => source == PortDataType.Any || target == PortDataType.Any || source == target;

    private static bool HasCycle(WorkflowDocument document)
    {
        var outgoing = document.Edges
            .GroupBy(edge => edge.SourceNodeId, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.Select(edge => edge.TargetNodeId).ToArray(), StringComparer.Ordinal);
        var visiting = new HashSet<string>(StringComparer.Ordinal);
        var visited = new HashSet<string>(StringComparer.Ordinal);

        return document.Nodes.Any(node => Visit(node.Id));

        bool Visit(string nodeId)
        {
            if (visited.Contains(nodeId))
            {
                return false;
            }

            if (!visiting.Add(nodeId))
            {
                return true;
            }

            if (outgoing.TryGetValue(nodeId, out var targets) && targets.Any(Visit))
            {
                return true;
            }

            visiting.Remove(nodeId);
            visited.Add(nodeId);
            return false;
        }
    }
}
