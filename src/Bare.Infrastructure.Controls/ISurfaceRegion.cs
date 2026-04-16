namespace Bare.Infrastructure.Controls;

public interface ISurfaceRegion
{
    int Left { get; }
    int Top { get; }
    int Width { get; }
    int Height { get; }

    void SetLeft(int left);
    void SetTop(int top);
    void SetWidth(int width);
    void SetHeight(int height);
    void RenderTo(TextSurface surface);
}
