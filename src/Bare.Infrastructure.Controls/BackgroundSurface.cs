namespace Bare.Infrastructure.Controls;

public sealed class BackgroundSurface : SurfaceRegion
{
    private readonly List<ISurfaceRegion> _regions = [];

    public BackgroundSurface(int width, int height)
        : base(0, 0, width, height)
    {
    }

    public char FillCharacter { get; set; } = ' ';

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

    public override void RenderTo(TextSurface surface)
    {
        ArgumentNullException.ThrowIfNull(surface);

        var line = new string(FillCharacter, Width);
        for (var row = 0; row < Height; row++)
        {
            surface.SetText(Left, Top + row, line);
        }

        foreach (var region in _regions)
        {
            region.RenderTo(surface);
        }
    }
}
