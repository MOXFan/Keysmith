namespace Keysmith.Lib.Controls;

public partial class PinningViewControl : Frame
{
    #region Static Values
    public static GridLength PinRowHeight = GridLength.Auto;
    public static GridLength PinColumnWidth = 30;
    public static GridLength HeaderColumnWidth = GridLength.Auto;
    #endregion
    #region Constructors
    public PinningViewControl()
    {
        InitializeComponent();
        Refresh();
    }
    #endregion
    #region Properties
    public IPinningModel Pinning
    {
        get { return (IPinningModel)GetValue(PinningProperty); }
        set { SetValue(PinningProperty, value); }
    }
    #endregion
    #region Bindable Properties
    public static readonly BindableProperty PinningProperty = GeneratePinningBindableProperty();
    #endregion
    #region Methods
    public void Refresh()
    {
        ClearAll();
        GenerateRowDefinitions();
        GenerateColumnDefinitions();
        PopulateHeaders();
        PopulatePins();
    }
    protected void ClearAll()
    {
        PinningGrid.Children.Clear();
        PinningGrid.ColumnDefinitions.Clear();
        PinningGrid.RowDefinitions.Clear();
    }
    private void GenerateRowDefinitions()
    {
        PinningGrid.RowDefinitions.Clear();
        for (int currentCount = 0; currentCount < Pinning.RowHeaders.Count; currentCount++)
        { PinningGrid.RowDefinitions.Add(new RowDefinition { Height = PinRowHeight }); }
    }
    private void GenerateColumnDefinitions()
    {
        PinningGrid.ColumnDefinitions.Clear();

        ColumnDefinition headerColumn = new ColumnDefinition
        { Width = HeaderColumnWidth };
        PinningGrid.ColumnDefinitions.Add(headerColumn);

        for (int currentCount = 1; currentCount <= Pinning.ColumnCount; currentCount++)
        {
            ColumnDefinition currentColumn = new ColumnDefinition
            { Width = PinColumnWidth };
            PinningGrid.ColumnDefinitions.Add(currentColumn);
        }
    }
    private void PopulateHeaders()
    {
        Style headerStyle = Application.Current.Resources["headerColumnLabelStyle"] as Style;

        for (int rowIndex = 0; rowIndex < Pinning.RowHeaders.Count; rowIndex++)
        {
            Label currentLabel = new Label
            {
                Text = Pinning.RowHeaders[rowIndex],
                Style = headerStyle
            };
            PinningGrid.Add(currentLabel, 0, Pinning.RowHeaders.Count - rowIndex - 1);
        }
    }
    private void PopulatePins()
    {
        Style pinFrameStyle = Application.Current.Resources["pinDisplayFrameStyle"] as Style;
        Style pinLabelStyle = Application.Current.Resources["pinLabelStyle"] as Style;

        for (int rowIndex = 0; rowIndex < Pinning.Rows.Count; rowIndex++)
        {
            ObservableCollection<string> currentRow = Pinning.Rows[rowIndex];

            for (int columnIndex = 0; columnIndex < currentRow.Count; columnIndex++)
            {
                Label currentLabel = new Label
                {
                    Text = currentRow[columnIndex],
                    Style = pinLabelStyle
                };
                Frame currentFrame = new Frame
                {
                    Content = currentLabel,
                    Style = pinFrameStyle
                };

                int gridRowIndex = Pinning.Rows.Count - 1 - rowIndex; // We want row 0 at the bottom of the grid.
                int gridColumnIndex = columnIndex + 1; // Leave space for the label.

                PinningGrid.Add(currentFrame, gridColumnIndex, gridRowIndex);
            }
        }
    }
    #region BindableProperty Support
    protected static BindableProperty GeneratePinningBindableProperty()
    {
        return BindableProperty.Create
        (
            nameof(Pinning),
            typeof(IPinningModel),
            typeof(PinningViewControl),
            new StandardPinningModel(),
            propertyChanged: OnPinningChanged
        );
    }
    protected static void OnPinningChanged(BindableObject controlObject, object oldValueObject, object newValueObject)
    {
        PinningViewControl control = controlObject as PinningViewControl;
        IPinningModel oldValue = oldValueObject as IPinningModel;
        IPinningModel newValue = newValueObject as IPinningModel;

        if (newValue != oldValue)
        { control.Refresh(); }
    }
    #endregion
    #endregion
}