using Bare.Primitive.UI;

namespace Bare.Infrastructure.Controls;

public sealed class SurfaceCanvas
{
    private readonly List<ISurfaceRegion> _regions = [];

    public SurfaceCanvas(int width, int height)
    {
        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }

        Width = width;
        Height = height;
    }

    public int Width { get; }
    public int Height { get; }

    public IReadOnlyList<ISurfaceRegion> Regions => _regions;

    public void AddRegion(ISurfaceRegion region)
    {
        ArgumentNullException.ThrowIfNull(region);
        _regions.Add(region);
    }

    public bool RemoveRegion(ISurfaceRegion region)
    {
        ArgumentNullException.ThrowIfNull(region);
        return _regions.Remove(region);
    }

    public void ClearRegions()
    {
        _regions.Clear();
    }

    public TextSurface BuildSurface(char fill = ' ')
    {
        var surface = new TextSurface(Width, Height);
        surface.Fill(fill);

        foreach (var region in _regions)
        {
            region.RenderTo(surface);
        }

        return surface;
    }

    public void RenderTo(IUiOutput output, int left = 0, int top = 0, char fill = ' ')
    {
        ArgumentNullException.ThrowIfNull(output);

        if (output.Width <= 0 || output.Height <= 0)
        {
            return;
        }

        var surface = BuildSurface(fill);
        surface.RenderTo(output, left, top);
    }
}
