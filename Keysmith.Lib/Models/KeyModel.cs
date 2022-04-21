namespace Keysmith.Lib.Models;

public class KeyModel : PropertyChangedBase, IKeyModel
{
    #region Private Members
    private String _cuts = "";
    private String _label = "";
    #endregion
    #region Properties
    public String Cuts
    {
        get { return _cuts; }
        set 
        { 
            _cuts = value;
            OnPropertyChanged();
        }
    }
    public String Label
    {
        get { return _label; }
        set 
        { 
            _label = value;
            OnPropertyChanged();
        }
    }
    public List<int> CutsList { get { return GetCutsList(); } }
    #endregion
    #region Instance Methods
    public List<int> GetCutsList()
    {
        List<int> output = new();
        char[] cutsArray = Cuts.ToCharArray();
        foreach (char currentChar in cutsArray)
        { output.Add(int.Parse(currentChar.ToString())); }
        return output;
    }
    #endregion
}
