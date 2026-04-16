using Bare.Primitive.UI;

namespace Bare.Infrastructure.Controls;

public sealed class TextSurface
{
    private readonly char[,] _buffer;

    public int Width { get; }
    public int Height { get; }

    public TextSurface(int width, int height)
    {
        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }

        Width = width;
        Height = height;
        _buffer = new char[height, width];
        Fill(' ');
    }

    public void Fill(char value)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                _buffer[y, x] = value;
            }
        }
    }

    public void SetText(int left, int top, string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (top < 0 || top >= Height)
        {
            return;
        }

        if (left >= Width)
        {
            return;
        }

        if (left < 0)
        {
            var skipCount = -left;
            if (skipCount >= text.Length)
            {
                return;
            }

            text = text[skipCount..];
            left = 0;
        }

        var clipped = UiText.Clip(text, Width - left);
        for (var i = 0; i < clipped.Length; i++)
        {
            _buffer[top, left + i] = clipped[i];
        }
    }

    public string GetLine(int row)
    {
        if (row < 0 || row >= Height)
        {
            throw new ArgumentOutOfRangeException(nameof(row));
        }

        var chars = new char[Width];
        for (var x = 0; x < Width; x++)
        {
            chars[x] = _buffer[row, x];
        }

        return new string(chars);
    }

    public void RenderTo(IUiOutput output, int left = 0, int top = 0)
    {
        ArgumentNullException.ThrowIfNull(output);

        for (var row = 0; row < Height; row++)
        {
            var targetRow = top + row;
            if (targetRow < 0 || targetRow >= output.Height)
            {
                continue;
            }

            var line = GetLine(row);
            var targetLeft = left;

            if (targetLeft < 0)
            {
                var skipCount = -targetLeft;
                if (skipCount >= line.Length)
                {
                    continue;
                }

                line = line[skipCount..];
                targetLeft = 0;
            }

            var maxWidth = Math.Max(0, output.Width - targetLeft);
            if (maxWidth <= 0)
            {
                continue;
            }

            output.WriteAt(targetLeft, targetRow, UiText.Clip(line, maxWidth));
        }
    }
}
