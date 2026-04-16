namespace Bare.Infrastructure.Controls;

public sealed class EmptyTextRegionConsumers : ITextRegionConsumerCollection
{
    public static EmptyTextRegionConsumers Instance { get; } = new();

    private EmptyTextRegionConsumers()
    {
    }

    public void Render(TextRegion region)
    {
        ArgumentNullException.ThrowIfNull(region);
    }
}
