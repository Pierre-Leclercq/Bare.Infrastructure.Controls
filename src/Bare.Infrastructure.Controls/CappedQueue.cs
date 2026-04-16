namespace Bare.Infrastructure.Controls;

public class CappedQueue<T>
{
    private readonly object _sync = new();
    private readonly List<T> _elements = [];
    private int _maxCount = 1;

    public CappedQueue(int maxCount)
    {
        MaxCount = maxCount;
    }

    public event Action<T>? NewItem;
    public event Action<T[]>? NewItems;

    public int MaxCount
    {
        get => _maxCount;
        set
        {
            lock (_sync)
            {
                _maxCount = value < 1 ? 1 : value;

                while (_elements.Count > _maxCount)
                {
                    _elements.RemoveAt(0);
                }
            }
        }
    }

    public bool Touched { get; private set; }

    public T[] Elements
    {
        get
        {
            lock (_sync)
            {
                return _elements.ToArray();
            }
        }
    }

    public void Clear()
    {
        lock (_sync)
        {
            _elements.Clear();
            Touched = true;
        }
    }

    public void AddNewItem(T newElement)
    {
        lock (_sync)
        {
            if (_elements.Count >= MaxCount)
            {
                _elements.RemoveAt(0);
            }

            _elements.Add(newElement);
            Touched = true;
        }

        NewItem?.Invoke(newElement);
    }

    public void AddRange(T[] newElements)
    {
        ArgumentNullException.ThrowIfNull(newElements);

        lock (_sync)
        {
            foreach (var element in newElements)
            {
                if (_elements.Count >= MaxCount)
                {
                    _elements.RemoveAt(0);
                }

                _elements.Add(element);
            }

            Touched = true;
        }

        NewItems?.Invoke(newElements);
    }
}
