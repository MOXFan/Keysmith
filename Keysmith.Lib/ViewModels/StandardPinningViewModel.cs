namespace Keysmith.Lib.ViewModels;

public class StandardPinningViewModel : PropertyChangedBase
{
    #region Private Members
    private StandardPinningModel _pinning = new();
    private ObservableCollection<KeyModel> _keys = new()
    {
        new(){ Cuts="134567" },
        new(){ Cuts="235764" },
        new(){ Cuts="567423" }
    };
    #endregion
    #region Constructors
    public StandardPinningViewModel()
    {
        AddKeyCommand = new Command(AddKey);
        ClearKeysCommand = new Command(ClearKeys);
        UpdatePinning();
    }
    #endregion
    #region Properties
    public StandardPinningModel Pinning
    {
        get { return _pinning; }
        set
        {
            _pinning = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<KeyModel> Keys
    {
        get { return _keys; }
        set
        {
            _keys = value;
            OnPropertyChanged();
        }
    }
    public ICommand AddKeyCommand { get; private set; }
    public ICommand ClearKeysCommand { get; private set; }
    #endregion
    #region Methods
    protected void AddKey()
    {
        throw new NotImplementedException();
    }
    protected void UpdatePinning()
    { Pinning = new StandardPinningModel(Keys); }
    public void ClearKeys()
    {
        Keys = new ObservableCollection<KeyModel>();
        UpdatePinning();
    }
    #endregion
}
