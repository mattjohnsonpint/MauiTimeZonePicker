using System.Collections;

namespace MauiTimeZonePicker;


public class TimeZonePicker : CollectionView, IDisposable
{
    private readonly ITimeZoneResourceProvider _resourceProvider = new TimeZoneResourceProvider();
    
    public TimeZonePicker()
    {
        Header = "Pick a time zone...";
        ItemsSource = (IList) _resourceProvider.GetTimeZoneResources();
        VerticalScrollBarVisibility = ScrollBarVisibility.Always;
        MaximumHeightRequest = 200;
        SelectionMode = SelectionMode.Single;

        ((LinearItemsLayout) ItemsLayout).ItemSpacing = 10;

        ItemTemplate = new DataTemplate(() =>
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
    }

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
}