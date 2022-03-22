using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keysmith.Lib.Models
{
    public class BasePinningModel : IPinningModel
    {
        public ObservableCollection<IKeyModel> Keys => throw new NotImplementedException();

        public ObservableCollection<string> RowHeaders => throw new NotImplementedException();

        public int ColumnCount => throw new NotImplementedException();

        public ObservableCollection<ObservableCollection<string>> Rows => throw new NotImplementedException();
    }
}
