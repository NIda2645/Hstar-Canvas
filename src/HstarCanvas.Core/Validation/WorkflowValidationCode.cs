namespace HstarCanvas.Core.Validation;

public enum WorkflowValidationCode
{
    NodeNotFound,
    NodeTypeUnknown,
    PortNotFound,
    TypeMismatch,
    RequiredInputMissing,
    CycleDetected
}
