using HstarCanvas.Core.Workflows;

namespace HstarCanvas.Core.Tests;

public sealed class WorkflowDocumentTests
{
    [Fact]
    public void CreateEmpty_InitializesStableDocumentDefaults()
    {
        var document = WorkflowDocument.CreateEmpty("wf-1", "新工作流");

        Assert.Equal("1.0", document.SchemaVersion);
        Assert.Equal("wf-1", document.Id);
        Assert.Equal("新工作流", document.Name);
        Assert.Empty(document.Nodes);
        Assert.Empty(document.Edges);
        Assert.Equal(1, document.Viewport.Zoom);
        Assert.True(document.CreatedAt <= document.UpdatedAt);
    }

    [Fact]
    public void WithNodesAndEdges_PreservesImageGenerationChain()
    {
        var prompt = WorkflowNode.Create("prompt-1", "prompt.text", "提示词", 80, 120);
        var generate = WorkflowNode.Create("generate-1", "image.generate", "图像生成", 360, 120);
        var output = WorkflowNode.Create("output-1", "image.output", "输出画廊", 680, 120);
        var firstEdge = new WorkflowEdge("edge-1", prompt.Id, "text", generate.Id, "prompt");
        var secondEdge = new WorkflowEdge("edge-2", generate.Id, "image", output.Id, "image");

        var document = WorkflowDocument.CreateEmpty("wf-2", "图片工作流")
            .WithNodes([prompt, generate, output])
            .WithEdges([firstEdge, secondEdge]);

        Assert.Equal(3, document.Nodes.Count);
        Assert.Equal(2, document.Edges.Count);
        Assert.Equal("generate-1", document.Edges[0].TargetNodeId);
        Assert.Equal("image", document.Edges[1].SourcePortId);
    }
}
