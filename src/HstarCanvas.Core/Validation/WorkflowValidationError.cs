namespace HstarCanvas.Core.Validation;

public sealed record WorkflowValidationError(
    WorkflowValidationCode Code,
    string Message,
    string? NodeId = null,
    string? EdgeId = null,
    string? PortId = null);
