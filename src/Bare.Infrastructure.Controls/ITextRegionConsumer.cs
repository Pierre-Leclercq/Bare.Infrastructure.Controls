namespace Bare.Infrastructure.Controls;

public interface ITextRegionConsumer
{
    void Render(TextRegion region, int startIndex, out int newStartIndex);
}
