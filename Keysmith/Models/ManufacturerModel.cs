namespace Keysmith.Models
{
    public class ManufacturerModel : ModelBase
    {
        #region Private Members
        private string _name = "";
        #endregion
        #region Properties
        public string Name
        {
            get
            { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Methods
        public override bool MatchFilter(string filter)
        {
            string ucfilter = filter.ToUpper();
            if (base.MatchFilter(filter) == true)
            { return true; }
            else if (Name.ToUpper().Contains(ucfilter))
            { return true; }
            else
            { return false; }
        }
        public override bool Equals(ModelBase otherModel)
        {
            if (otherModel.GetType() != typeof(ManufacturerModel))
            { return false; }

            ManufacturerModel otherManufacturer = otherModel as ManufacturerModel;

            if (otherManufacturer.ID != this.ID)
            { return false; }
            else if (otherManufacturer.Name != this.Name)
            { return false; }
            else
            { return true; }
        }
        public override string ToString()
        {
            return $"[{ID}] {Name}";
        }
        #endregion
    }
}
