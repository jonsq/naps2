using Eto.Drawing;
using Eto.Forms;

namespace NAPS2.EtoForms.Layout;

public class LayoutRoot : LayoutElement
{
    private readonly LayoutElement[] _children;
    private Size? _spacing;

    public LayoutRoot(LayoutElement[] children)
    {
        _children = children;
    }

    public LayoutRoot DefaultSpacing(Size spacing)
    {
        _spacing = spacing;
        return this;
    }

    public LayoutRoot DefaultSpacing(int xSpacing, int ySpacing)
    {
        _spacing = new Size(xSpacing, ySpacing);
        return this;
    }

    public LayoutRoot DefaultSpacing(int spacing)
    {
        _spacing = new Size(spacing, spacing);
        return this;
    }

    public override void AddTo(DynamicLayout layout)
    {
        if (_spacing != null)
        {
            layout.DefaultSpacing = _spacing;
        }
        foreach (var child in _children)
        {
            child.AddTo(layout);
        }
    }

    public override void DoLayout(LayoutContext context, RectangleF bounds)
    {
        if (_children.Length != 1) throw new InvalidOperationException();
        _children[0].DoLayout(context, bounds);
    }

    public override SizeF GetPreferredSize(LayoutContext context, RectangleF parentBounds)
    {
        if (_children.Length != 1) throw new InvalidOperationException();
        return _children[0].GetPreferredSize(context, parentBounds);
    }
}