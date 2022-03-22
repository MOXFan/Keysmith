namespace Keysmith.Lib.Models;

public class KeyModel : IKeyModel
{
    #region Properties
    public String Cuts { get; set; } = "";
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
