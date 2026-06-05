namespace HstarCanvas.Core.Workflows;

public sealed record CanvasViewport(double X, double Y, double Zoom)
{
    public static CanvasViewport Default { get; } = new(0, 0, 1);
}

