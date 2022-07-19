using System.Collections;
using Microsoft.Maui.Controls.Shapes;

namespace MauiTimeZonePicker;

public class TimeZonePicker : VerticalStackLayout, IDisposable
{
    private readonly ITimeZoneResourceProvider _resourceProvider = new TimeZoneResourceProvider();

    public TimeZonePicker()
    {
        CollectionView = GetCollectionView(_resourceProvider.GetTimeZoneResources());
        AddContent();
    }

    public CollectionView CollectionView { get; }

    public TimeZoneResource? SelectedItem => CollectionView.SelectedItem as TimeZoneResource;

    public event EventHandler<SelectedItemChangedEventArgs>? SelectedItemChanged;
    
    private void AddContent()
    {
        Add(new Label
        {
            Text = "Pick a time zone...",
            FontSize = 14
        });

        Add(new Border
        {
            StrokeShape = new Rectangle(),
            Padding = 2,
            Content = CollectionView
        });
    }

    private CollectionView GetCollectionView(IEnumerable itemsSource)
    {
        var view = new CollectionView
        {
            ItemsSource = itemsSource,
            ItemTemplate = GetItemTemplate(),
            MaximumHeightRequest = 200,
            SelectionMode = SelectionMode.Single
        };

        ((LinearItemsLayout) view.ItemsLayout).ItemSpacing = 10;

        view.SelectionChanged += (_, args) =>
        {
            var previousSelection = args.PreviousSelection.FirstOrDefault() as TimeZoneResource;
            var currentSelection = args.CurrentSelection.FirstOrDefault() as TimeZoneResource;
            var eventArgs = new SelectedItemChangedEventArgs(previousSelection, currentSelection);
            SelectedItemChanged?.Invoke(this, eventArgs);
        };

        return view;
    }

    private static DataTemplate GetItemTemplate() =>
        new(() =>
        {
            var layout = new VerticalStackLayout();

            var name = new Label
            {
                FontSize = 16,
                FontAttributes = FontAttributes.Bold
            };
            name.SetBinding(Label.TextProperty, nameof(TimeZoneResource.Name));
            layout.Add(name);

            var detail = new HorizontalStackLayout
            {
                Spacing = 4
            };

            var offset = new Label
            {
                FontSize = 12,
                FontAttributes = FontAttributes.Bold
            };
            offset.SetBinding(Label.TextProperty,
                new Binding(nameof(TimeZoneResource.CurrentOffset), stringFormat: "({0})"));
            detail.Add(offset);

            var location = new Label
            {
                FontSize = 12,
                FontAttributes = FontAttributes.Italic
            };
            location.SetBinding(Label.TextProperty, nameof(TimeZoneResource.Location));
            detail.Add(location);

            layout.Add(detail);

            return layout;
        });

    private void DisposeResources()
    {
        (_resourceProvider as IDisposable)?.Dispose();
    }

    public void Dispose()
    {
        DisposeResources();
        GC.SuppressFinalize(this);
    }

    ~TimeZonePicker()
    {
        DisposeResources();
    }
    
    public class SelectedItemChangedEventArgs : EventArgs
    {
        public SelectedItemChangedEventArgs(TimeZoneResource? previousSelection, TimeZoneResource? currentSelection)
        {
            PreviousSelection = previousSelection;
            CurrentSelection = currentSelection;
        }

        public TimeZoneResource? PreviousSelection { get; }
        public TimeZoneResource? CurrentSelection { get; }
    }
}