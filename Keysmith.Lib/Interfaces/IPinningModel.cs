namespace Keysmith.Lib.Interfaces;

public interface IPinningModel
{
    ObservableCollection<IKeyModel> Keys { get; }
    ObservableCollection<String> RowHeaders { get; }
    int ColumnCount { get; }
    ObservableCollection<ObservableCollection<String>> Rows { get; }
}
