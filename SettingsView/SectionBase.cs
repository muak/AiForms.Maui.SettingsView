using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace AiForms.Settings;

public class SectionBase: Element, IList<CellBase>, IVisualTreeElement, INotifyCollectionChanged
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TableSectionBase), null);
    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TableSectionBase), Colors.Black);


    readonly ObservableCollection<CellBase> _children = new ObservableCollection<CellBase>();

    /// <summary>
    ///     Constructs a Section without an empty header.
    /// </summary>
    protected SectionBase()
    {
        _children.CollectionChanged += OnChildrenChanged;
    }

    /// <summary>
    ///     Constructs a Section with the specified header.
    /// </summary>
    protected SectionBase(string title)
    {
        if (title == null)
            throw new ArgumentNullException("title");

        Title = title;
        _children.CollectionChanged += OnChildrenChanged;
    }

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    public void Add(CellBase item)
    {
        _children.Add(item);
        if(item is IVisualTreeElement element)
        {
            VisualDiagnostics.OnChildAdded(this, element);
        }
    }

    public void Clear()
    {
        foreach (var item in _children)
        {
            if (item is IVisualTreeElement element)
            {
                VisualDiagnostics.OnChildRemoved(this, element, _children.IndexOf(item));
            }
        }
        _children.Clear();
    }

    public bool Contains(CellBase item)
    {
        return _children.Contains(item);
    }

    public void CopyTo(CellBase[] array, int arrayIndex)
    {
        _children.CopyTo(array, arrayIndex);
    }

    public int Count
    {
        get { return _children.Count; }
    }

    bool ICollection<CellBase>.IsReadOnly
    {
        get { return false; }
    }

    public bool Remove(CellBase item)
    {
        if (item is IVisualTreeElement element)
        {
            VisualDiagnostics.OnChildRemoved(this, element, _children.IndexOf(item));
        }
        return _children.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<CellBase> GetEnumerator()
    {
        return _children.GetEnumerator();
    }

    public int IndexOf(CellBase item)
    {
        return _children.IndexOf(item);
    }

    public void Insert(int index, CellBase item)
    {
        if (item is IVisualTreeElement element)
        {
            VisualDiagnostics.OnChildAdded(this, element, index);
        }
        _children.Insert(index, item);
    }

    public CellBase this[int index]
    {
        get { return _children[index]; }
        set { _children[index] = value; }
    }

    public void RemoveAt(int index)
    {
        var item = _children[index];
        if (item is IVisualTreeElement element)
        {
            VisualDiagnostics.OnChildRemoved(this, element, index);
        }

        _children.RemoveAt(index);
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged
    {
        add { _children.CollectionChanged += value; }
        remove { _children.CollectionChanged -= value; }
    }

    public void Add(IEnumerable<CellBase> items)
    {
        foreach(var item in items)
        {
            _children.Add(item);
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        object bc = BindingContext;
        foreach (CellBase child in _children)
        {
            SetInheritedBindingContext(child, bc);
        }
    }

    void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
        // We need to hook up the binding context for new items.
        if (notifyCollectionChangedEventArgs.NewItems == null)
        {
            return;
        }
        object bc = BindingContext;
        foreach (BindableObject item in notifyCollectionChangedEventArgs.NewItems)
        {
            SetInheritedBindingContext(item, bc);
        }
    }

    IReadOnlyList<IVisualTreeElement> IVisualTreeElement.GetVisualChildren() => this._children.Cast<IVisualTreeElement>().ToList().AsReadOnly();
    IVisualTreeElement IVisualTreeElement.GetVisualParent() => null;
}

