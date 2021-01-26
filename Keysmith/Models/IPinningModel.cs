using System;
using System.Collections.ObjectModel;

namespace Keysmith.Models
{
    public interface IPinningModel
    {
        ObservableCollection<KeyModel> Keys { get; }
        ObservableCollection<String> RowHeaders { get; }
        int ColumnCount { get; }
        ObservableCollection<ObservableCollection<String>> Rows { get; }
    }
}
