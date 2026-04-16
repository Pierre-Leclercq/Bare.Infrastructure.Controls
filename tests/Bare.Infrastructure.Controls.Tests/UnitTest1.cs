namespace Bare.Infrastructure.Controls.Tests;

public class InfrastructureControlsUnitTests
{
    private sealed class StubConsumerCollection : ITextRegionConsumerCollection
    {
        private readonly Action<TextRegion> _render;

        public StubConsumerCollection(Action<TextRegion>? render = null)
        {
            _render = render ?? (_ => { });
        }

        public void Render(TextRegion region)
        {
            _render(region);
        }
    }

    private sealed class StubConsumer(string text) : ITextRegionConsumer
    {
        public void Render(TextRegion region, int startIndex, out int newStartIndex)
        {
            region.SetLine(startIndex, text);
            newStartIndex = startIndex + 1;
        }
    }

    private sealed class ConcreteOutput : TextRegionOutputBase
    {
        public string Value { get; set; } = string.Empty;

        public override void Render(TextRegion region, int startIndex, out int newStartIndex)
        {
            region.SetLine(startIndex, Value);
            newStartIndex = startIndex + 1;
        }

        protected override string ComputeToString() => Value;
    }

    [Fact]
    public void Identity_Name_Is_Stable()
    {
        Assert.Equal("Bare.Infrastructure.Controls", InfrastructureControlsIdentity.Name);
    }

    [Fact]
    public void SetText_Clips_To_Surface_Width()
    {
        var sut = new TextSurface(4, 2);

        sut.SetText(0, 0, "abcdef");

        Assert.Equal("abcd", sut.GetLine(0));
    }

    [Fact]
    public void SetText_With_Negative_Left_Shifts_Text()
    {
        var sut = new TextSurface(5, 1);

        sut.SetText(-2, 0, "ABCDE");

        Assert.Equal("CDE  ", sut.GetLine(0));
    }

    [Fact]
    public void SurfaceRegion_Clamps_Bounds_To_Minimums()
    {
        var sut = new TextRegion(-3, -2, 0, 0, EmptyTextRegionConsumers.Instance);

        Assert.Equal(0, sut.Left);
        Assert.Equal(0, sut.Top);
        Assert.Equal(1, sut.Width);
        Assert.Equal(1, sut.Height);
    }

    [Fact]
    public void TextRegion_Renders_Consumer_Content_With_Padding()
    {
        var consumers = new StubConsumerCollection(r => r.SetLine(0, "AB"));
        var sut = new TextRegion(1, 0, 4, 1, consumers);
        var surface = new TextSurface(8, 2);

        surface.Fill('.');
        sut.RenderTo(surface);

        Assert.Equal(".AB  ...", surface.GetLine(0));
    }

    [Fact]
    public void SurfaceCanvas_Composes_Regions_In_Order()
    {
        var canvas = new SurfaceCanvas(8, 1);
        var first = new TextRegion(0, 0, 8, 1, new StubConsumerCollection(r => r.SetLine(0, "AAAA")));
        var second = new TextRegion(0, 0, 8, 1, new StubConsumerCollection(r => r.SetLine(0, "BBBB")));

        canvas.AddRegion(first);
        canvas.AddRegion(second);

        var surface = canvas.BuildSurface('.');

        Assert.Equal("BBBB    ", surface.GetLine(0));
    }

    [Fact]
    public void BackgroundSurface_Fills_Then_Renders_Children()
    {
        var background = new BackgroundSurface(6, 1)
        {
            FillCharacter = '.'
        };

        var child = new TextRegion(1, 0, 3, 1, new StubConsumerCollection(r => r.SetLine(0, "XY")));
        background.AddRegion(child);

        var surface = new TextSurface(6, 1);
        surface.Fill(' ');

        background.RenderTo(surface);

        Assert.Equal(".XY ..", surface.GetLine(0));
    }

    [Fact]
    public void CappedQueue_Drops_Oldest_When_At_Capacity()
    {
        var queue = new CappedQueue<string>(3);

        queue.AddRange(["1", "2", "3", "4"]);

        Assert.Equal(["2", "3", "4"], queue.Elements);
    }

    [Fact]
    public void CappedQueue_Raises_Events()
    {
        var queue = new CappedQueue<string>(3);
        string? lastSingle = null;
        string[]? lastMany = null;

        queue.NewItem += item => lastSingle = item;
        queue.NewItems += items => lastMany = items;

        queue.AddNewItem("A");
        queue.AddRange(["B", "C"]);

        Assert.Equal("A", lastSingle);
        Assert.NotNull(lastMany);
        Assert.Equal(["B", "C"], lastMany);
    }

    [Fact]
    public void CappedQueue_MaxCount_Setter_Trims_Existing_Buffer()
    {
        var queue = new CappedQueue<int>(5);
        queue.AddRange([1, 2, 3, 4, 5]);

        queue.MaxCount = 3;

        Assert.Equal([3, 4, 5], queue.Elements);
    }

    [Fact]
    public void RenderCappedQueue_Renders_Consumers_Sequentially()
    {
        var queue = new RenderCappedQueue<StubConsumer>(5);
        queue.AddNewItem(new StubConsumer("L1"));
        queue.AddNewItem(new StubConsumer("L2"));

        var region = new TextRegion(0, 0, 6, 3, queue);
        var surface = new TextSurface(6, 3);
        surface.Fill('.');

        region.RenderTo(surface);

        Assert.Equal("L1    ", surface.GetLine(0));
        Assert.Equal("L2    ", surface.GetLine(1));
    }

    [Fact]
    public void TextRegionOutputBase_Formats_Time_And_Clears_Ranges()
    {
        var output = new ConcreteOutput();
        var formatted = output.GetTimeOutputAsString(1609459200000);

        Assert.Equal("2021/01/01 00:00:00", formatted);

        var lines = new[] { "a", "b", "c", "d" };
        output.ClearArray(lines, 1, 2);

        Assert.Equal(new[] { "a", string.Empty, string.Empty, string.Empty }, lines);
    }

    [Fact]
    public void SurfaceCanvas_RenderTo_Writes_To_InMemory_Output()
    {
        var canvas = new SurfaceCanvas(5, 2);
        var region = new TextRegion(0, 0, 5, 2, new StubConsumerCollection(r =>
        {
            r.SetLine(0, "HELLO");
            r.SetLine(1, "X");
        }));

        canvas.AddRegion(region);

        var output = new Bare.Primitive.UI.InMemoryUiOutput(5, 2);
        canvas.RenderTo(output);

        Assert.Equal("HELLO", output.GetLine(0));
        Assert.Equal("X    ", output.GetLine(1));
    }
}





