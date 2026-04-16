namespace Bare.Infrastructure.Controls;

public interface ITextRegion : ISurfaceRegion
{
    string[] Lines { get; }
}
