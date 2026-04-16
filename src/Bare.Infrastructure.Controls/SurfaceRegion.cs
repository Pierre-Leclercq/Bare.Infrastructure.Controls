using Bare.Primitive.UI;

namespace Bare.Infrastructure.Controls;

public abstract class SurfaceRegion : ISurfaceRegion
{
    private static readonly char[] EmptyChars = [];

    protected SurfaceRegion(int left, int top, int width, int height)
    {
        SetLeft(left);
        SetTop(top);
        SetWidth(width);
        SetHeight(height);
    }

    public int Left { get; protected set; }
    public int Top { get; protected set; }
    public int Width { get; protected set; }
    public int Height { get; protected set; }

    public virtual void SetLeft(int left)
    {
        Left = left < 0 ? 0 : left;
    }

    public virtual void SetTop(int top)
    {
        Top = top < 0 ? 0 : top;
    }

    public virtual void SetWidth(int width)
    {
        Width = width < 1 ? 1 : width;
    }

    public virtual void SetHeight(int height)
    {
        Height = height < 1 ? 1 : height;
    }

    public abstract void RenderTo(TextSurface surface);

    protected static string FitToWidth(string? text, int width)
    {
        if (width <= 0)
        {
            return string.Empty;
        }

        var safe = text ?? string.Empty;
        return UiText.Clip(safe, width).PadRight(width);
    }

    protected static char[] CreateBlankChars(int length)
    {
        if (length <= 0)
        {
            return EmptyChars;
        }

        var chars = new char[length];
        Array.Fill(chars, ' ');
        return chars;
    }
}
