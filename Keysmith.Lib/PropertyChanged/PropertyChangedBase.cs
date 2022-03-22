namespace Keysmith.Lib.PropertyChanged;

public class PropertyChangedBase : INotifyPropertyChanged
{
    #region INotifyPropertyChanged Implementation
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string property = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
    #endregion
}
