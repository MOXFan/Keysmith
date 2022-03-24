﻿namespace Keysmith.Lib.ViewModels;

public class StandardPinningViewModel : PropertyChangedBase
{
    #region Private Members
    private StandardPinningModel _pinning = new();
    private ObservableCollection<IKeyModel> _keys = new();
    #endregion
    #region Constructors
    public StandardPinningViewModel()
    {
        AddKeyCommand = new Command(AddKey);
        ClearKeysCommand = new Command(ClearKeys);
        ClearKeys();
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
    public ObservableCollection<IKeyModel> Keys
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
        Keys.Add(NewKey());
        UpdatePinning();
    }
    protected void UpdatePinning()
    { Pinning = new StandardPinningModel(Keys); }
    public void ClearKeys()
    {
        Keys.Clear();
        Keys.Add(NewKey());
        Keys.Add(NewKey());

        UpdatePinning();
    }    
    protected KeyModel NewKey()
    {
        KeyModel output = new() { Cuts = "000000" };
        output.PropertyChanged += Key_PropertyChanged;
        return output;
    }
    private void Key_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged("Keys");
        UpdatePinning();
    }
    #endregion
}
