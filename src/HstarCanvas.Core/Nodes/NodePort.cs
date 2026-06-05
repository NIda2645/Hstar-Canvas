namespace HstarCanvas.Core.Nodes;

public sealed record NodePort(string Id, string Name, PortDataType DataType, bool Required, bool Multiple)
{
    public static NodePort Input(string id, string name, PortDataType dataType, bool required = true, bool multiple = false)
        => new(id, name, dataType, required, multiple);

    public static NodePort Output(string id, string name, PortDataType dataType, bool multiple = true)
        => new(id, name, dataType, false, multiple);
}

