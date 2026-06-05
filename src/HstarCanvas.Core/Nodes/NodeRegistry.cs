namespace HstarCanvas.Core.Nodes;

public sealed class NodeRegistry
{
    private readonly IReadOnlyDictionary<string, NodeDefinition> _items;

    private NodeRegistry(IEnumerable<NodeDefinition> definitions)
    {
        _items = definitions.ToDictionary(x => x.Type, StringComparer.Ordinal);
    }

    public static NodeRegistry CreateDefault() => new([
        new("prompt.text", "Prompt", [], [NodePort.Output("text", "Text", PortDataType.Text)]),
        new("image.asset", "Asset Image", [], [NodePort.Output("image", "Image", PortDataType.Image)]),
        new("image.generate", "Image Generate", [NodePort.Input("prompt", "Prompt", PortDataType.Text), NodePort.Input("reference", "Reference", PortDataType.Image, required: false)], [NodePort.Output("image", "Image", PortDataType.Image), NodePort.Output("metadata", "Metadata", PortDataType.Any)]),
        new("image.output", "Output Gallery", [NodePort.Input("image", "Image", PortDataType.Image)], [])
    ]);

    public bool Contains(string type) => _items.ContainsKey(type);

    public NodeDefinition Get(string type) => _items[type];
}
