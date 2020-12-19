using Keysmith.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Keysmith.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StandardPinningView : Frame
    {
        #region Private Members
        private PinningModel _pinning = new PinningModel();
        #endregion
        #region Constructors
        public StandardPinningView()
        {
            InitializeComponent();
            Refresh();
        }
        #endregion
        #region Properties
        public PinningModel Pinning
        {
            get { return (PinningModel)GetValue(PinningProperty); }
            set { SetValue(PinningProperty, value); }
        }
        public int PinColumnWidth
        { get { return 30; } }
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
            for (int currentCount = 1; currentCount <= Pinning.RowCount; currentCount++)
            { PinningGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); }
        }
        private void GenerateColumnDefinitions()
        {
            PinningGrid.ColumnDefinitions.Clear();

            ColumnDefinition headerColumn = new ColumnDefinition
            { Width = GridLength.Auto };
            PinningGrid.ColumnDefinitions.Add(headerColumn);

            for (int currentCount = 1; currentCount <= Pinning.ColumnCount; currentCount++)
            {
                ColumnDefinition currentColumn = new ColumnDefinition
                { Width = new GridLength(PinColumnWidth) };
                PinningGrid.ColumnDefinitions.Add(currentColumn);
            }
        }
        private void PopulateHeaders()
        {
            Style headerStyle = Application.Current.Resources["headerColumnLabelStyle"] as Style;

            for (int rowIndex = 0; rowIndex < Pinning.RowCount; rowIndex++)
            {
                Label currentLabel = new Label
                {
                    Text = Pinning.MasterPinHeader,
                    Style = headerStyle
                };

                if (rowIndex == Pinning.RowCount - 1)
                { currentLabel.Text = Pinning.BottomPinHeader; }

                PinningGrid.Children.Add(currentLabel, 0, rowIndex);
            }
        }
        private void PopulatePins()
        {
            Style pinFrameStyle = Application.Current.Resources["pinDisplayFrameStyle"] as Style;
            Style pinLabelStyle = Application.Current.Resources["pinLabelStyle"] as Style;

            for (int columnIndex = 0; columnIndex < Pinning.ColumnCount; columnIndex++)
            {
                ObservableCollection<string> currentColumn = Pinning.Columns[columnIndex];
                for (int rowIndex = 0; rowIndex < currentColumn.Count; rowIndex++)
                {
                    Label currentLabel = new Label
                    {
                        Text = currentColumn[rowIndex],
                        Style = pinLabelStyle
                    };
                    Frame currentFrame = new Frame
                    {
                        Content = currentLabel,
                        Style = pinFrameStyle
                    };

                    int gridRowIndex = Pinning.RowCount - 1 - rowIndex; // We want row 0 at the bottom of the grid.
                    int gridColumnIndex = columnIndex + 1; // Leave space for the label.

                    PinningGrid.Children.Add(currentFrame, gridColumnIndex, gridRowIndex);
                }
            }
        }
        #region BindableProperty Support
        protected static BindableProperty GeneratePinningBindableProperty()
        {
            return BindableProperty.Create
            (
                nameof(Pinning),
                typeof(PinningModel),
                typeof(StandardPinningView),
                new PinningModel(),
                propertyChanged: OnPinningChanged
            );
        }
        protected static void OnPinningChanged(BindableObject controlObject, object oldValueObject, object newValueObject)
        {
            StandardPinningView control = controlObject as StandardPinningView;
            PinningModel oldValue = oldValueObject as PinningModel;
            PinningModel newValue = newValueObject as PinningModel;

            if (newValue != oldValue)
            { control.Refresh(); }
        }
        #endregion
        #endregion
    }
}