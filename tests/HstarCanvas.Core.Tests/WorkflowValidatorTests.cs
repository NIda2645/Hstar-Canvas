using HstarCanvas.Core.Validation;
using HstarCanvas.Core.Workflows;

namespace HstarCanvas.Core.Tests;

public sealed class WorkflowValidatorTests
{
    [Fact]
    public void Validate_ReturnsNoErrorsForPromptToImageGenerateToOutput()
    {
        var document = WorkflowDocument.CreateEmpty("wf-valid", "Valid")
            .WithNodes([
                WorkflowNode.Create("prompt", "prompt.text", "Prompt", 0, 0),
                WorkflowNode.Create("generate", "image.generate", "Generate", 240, 0),
                WorkflowNode.Create("output", "image.output", "Output", 520, 0)
            ])
            .WithEdges([
                new WorkflowEdge("edge-1", "prompt", "text", "generate", "prompt"),
                new WorkflowEdge("edge-2", "generate", "image", "output", "image")
            ]);

        var errors = WorkflowValidator.CreateDefault().Validate(document);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_ReturnsTypeMismatchForIncompatiblePorts()
    {
        var document = WorkflowDocument.CreateEmpty("wf-mismatch", "Mismatch")
            .WithNodes([
                WorkflowNode.Create("prompt", "prompt.text", "Prompt", 0, 0),
                WorkflowNode.Create("output", "image.output", "Output", 260, 0)
            ])
            .WithEdges([new WorkflowEdge("edge-1", "prompt", "text", "output", "image")]);

        var errors = WorkflowValidator.CreateDefault().Validate(document);

        Assert.Contains(errors, error => error.Code == WorkflowValidationCode.TypeMismatch && error.EdgeId == "edge-1");
    }

    [Fact]
    public void Validate_ReturnsMissingRequiredInputForUnconnectedPrompt()
    {
        var document = WorkflowDocument.CreateEmpty("wf-missing", "Missing")
            .WithNodes([WorkflowNode.Create("generate", "image.generate", "Generate", 0, 0)]);

        var errors = WorkflowValidator.CreateDefault().Validate(document);

        Assert.Contains(errors, error => error.Code == WorkflowValidationCode.RequiredInputMissing && error.NodeId == "generate" && error.PortId == "prompt");
    }

    [Fact]
    public void Validate_ReturnsNodeNotFoundForMissingEdgeEndpoint()
    {
        var document = WorkflowDocument.CreateEmpty("wf-missing-node", "Missing Node")
            .WithNodes([WorkflowNode.Create("prompt", "prompt.text", "Prompt", 0, 0)])
            .WithEdges([new WorkflowEdge("edge-1", "prompt", "text", "missing", "prompt")]);

        var errors = WorkflowValidator.CreateDefault().Validate(document);

        Assert.Contains(errors, error => error.Code == WorkflowValidationCode.NodeNotFound && error.EdgeId == "edge-1");
    }

    [Fact]
    public void Validate_ReturnsCycleDetectedForCyclicGraph()
    {
        var document = WorkflowDocument.CreateEmpty("wf-cycle", "Cycle")
            .WithNodes([
                WorkflowNode.Create("a", "prompt.text", "A", 0, 0),
                WorkflowNode.Create("b", "prompt.text", "B", 200, 0)
            ])
            .WithEdges([
                new WorkflowEdge("edge-1", "a", "text", "b", "text"),
                new WorkflowEdge("edge-2", "b", "text", "a", "text")
            ]);

        var errors = WorkflowValidator.CreateDefault().Validate(document);

        Assert.Contains(errors, error => error.Code == WorkflowValidationCode.CycleDetected);
    }
}
