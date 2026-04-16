namespace Bare.Infrastructure.Controls;

public class RenderCappedQueue<T> : CappedQueue<T>, ITextRegionConsumerCollection
    where T : ITextRegionConsumer
{
    public RenderCappedQueue(int maxCount)
        : base(maxCount)
    {
    }

    public virtual void Render(TextRegion region)
    {
        ArgumentNullException.ThrowIfNull(region);

        var elements = Elements;
        var startIndex = 0;

        for (var i = 0; i < elements.Length; i++)
        {
            elements[i].Render(region, startIndex, out startIndex);
            if (startIndex >= region.Lines.Length)
            {
                break;
            }
        }
    }
}
