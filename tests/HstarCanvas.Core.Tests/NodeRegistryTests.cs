using HstarCanvas.Core.Nodes;

namespace HstarCanvas.Core.Tests;

public sealed class NodeRegistryTests
{
    [Fact]
    public void CreateDefault_RegistersImageMvpNodes()
    {
        var registry = NodeRegistry.CreateDefault();

        Assert.True(registry.Contains("prompt.text"));
        Assert.True(registry.Contains("image.asset"));
        Assert.True(registry.Contains("image.generate"));
        Assert.True(registry.Contains("image.output"));
    }

    [Fact]
    public void ImageGenerateNode_RequiresPromptAndOutputsImage()
    {
        var registry = NodeRegistry.CreateDefault();
        var definition = registry.Get("image.generate");

        Assert.Contains(definition.Inputs, port => port.Id == "prompt" && port.DataType == PortDataType.Text && port.Required);
        Assert.Contains(definition.Inputs, port => port.Id == "reference" && port.DataType == PortDataType.Image && !port.Required);
        Assert.Contains(definition.Outputs, port => port.Id == "image" && port.DataType == PortDataType.Image);
        Assert.Contains(definition.Outputs, port => port.Id == "metadata" && port.DataType == PortDataType.Any);
    }
}
