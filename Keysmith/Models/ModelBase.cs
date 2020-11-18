using Keysmith.Data;
using Keysmith.PropertyChanged;
using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Keysmith.Models
{
    public class ModelBase : PropertyChangedBase
    {
        #region Private Members
        private int _id = 0;
        #endregion
        #region Properties
        [PrimaryKey,AutoIncrement]
        public int ID
        {
            get
            { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Methods
        public virtual bool Equals(ModelBase otherModel)
        {
            if(ID == otherModel.ID)
            { return true; }
            else
            { return false; }
        }
        public virtual bool MatchFilter(string filter)
        {
            if (ID.ToString().Contains(filter))
            { return true; }
            else
            { return false; }

        }
        #endregion
    }
}
