namespace Bare.Infrastructure.Controls;

public abstract class TextRegionOutputBase : ITextRegionConsumer
{
    public string PreOutputString { get; set; } = string.Empty;
    public string PostOutputString { get; set; } = string.Empty;

    public DateTime GetDateTime(long time)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddMilliseconds(time);
    }

    public void GetTimeOutput(
        long time,
        out string year,
        out string month,
        out string day,
        out string hour,
        out string minute,
        out string second)
    {
        var dateTime = GetDateTime(time);

        year = dateTime.Year.ToString("D4");
        month = dateTime.Month.ToString("D2");
        day = dateTime.Day.ToString("D2");
        hour = dateTime.Hour.ToString("D2");
        minute = dateTime.Minute.ToString("D2");
        second = dateTime.Second.ToString("D2");
    }

    public string GetTimeOutputAsString(long time)
    {
        GetTimeOutput(
            time,
            out var year,
            out var month,
            out var day,
            out var hour,
            out var minute,
            out var second);

        return $"{year}/{month}/{day} {hour}:{minute}:{second}";
    }

    public abstract void Render(TextRegion region, int startIndex, out int newStartIndex);

    protected abstract string ComputeToString();

    public override string ToString()
    {
        return ComputeToString();
    }

    public void ClearArray(string[] lines)
    {
        ArgumentNullException.ThrowIfNull(lines);

        for (var i = 0; i < lines.Length; i++)
        {
            lines[i] = string.Empty;
        }
    }

    public void ClearLineInArray(string[] lines, int index)
    {
        ArgumentNullException.ThrowIfNull(lines);

        if (index < 0 || index >= lines.Length)
        {
            return;
        }

        lines[index] = string.Empty;
    }

    public void ClearArray(string[] lines, int startIndex, int length)
    {
        ArgumentNullException.ThrowIfNull(lines);

        if (length < 0)
        {
            return;
        }

        var linesLength = lines.Length;
        var start = startIndex < 0 ? 0 : startIndex;
        var end = startIndex + length >= linesLength ? linesLength - 1 : startIndex + length;

        for (var i = start; i <= end; i++)
        {
            if (i < 0 || i >= linesLength)
            {
                continue;
            }

            lines[i] = string.Empty;
        }
    }
}
