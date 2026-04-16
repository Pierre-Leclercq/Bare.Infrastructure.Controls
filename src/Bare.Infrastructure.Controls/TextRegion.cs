namespace Bare.Infrastructure.Controls;

public sealed class TextRegion : SurfaceRegion, ITextRegion
{
    private readonly ITextRegionConsumerCollection _consumers;

    public TextRegion(int left, int top, int width, int height, ITextRegionConsumerCollection consumers)
        : base(left, top, width, height)
    {
        _consumers = consumers ?? throw new ArgumentNullException(nameof(consumers));
        Lines = new string[Height];
        ClearLines();
    }

    public string[] Lines { get; private set; }

    public override void SetHeight(int height)
    {
        base.SetHeight(height);
        Lines = new string[Height];
        ClearLines();
    }

    public void SetLine(int index, string? text)
    {
        if (index < 0 || index >= Lines.Length)
        {
            return;
        }

        Lines[index] = text ?? string.Empty;
    }

    public void ClearLines()
    {
        for (var i = 0; i < Lines.Length; i++)
        {
            Lines[i] = string.Empty;
        }
    }

    public override void RenderTo(TextSurface surface)
    {
        ArgumentNullException.ThrowIfNull(surface);

        _consumers.Render(this);

        for (var row = 0; row < Height; row++)
        {
            var text = row < Lines.Length ? Lines[row] : string.Empty;
            surface.SetText(Left, Top + row, FitToWidth(text, Width));
        }
    }
}
