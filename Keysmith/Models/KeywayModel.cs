using SQLite;

namespace Keysmith.Models
{
    public class KeywayModel : ModelBase
    {
        #region Private Members
        private int _manufacturerID = 0;
        private string _name = "";
        private ManufacturerModel _manufacturer = new ManufacturerModel();
        #endregion
        #region Properties
        public int ManufacturerID
        {
            get
            { return _manufacturerID; }
            set
            { UpdateManufacturerAsync(value); }
        }
        public string Name
        {
            get
            { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged("CompoundName");
            }
        }
        [Ignore]
        public ManufacturerModel Manufacturer
        {
            get
            { return _manufacturer; }
            set
            { UpdateManufacturerAsync(value.ID); }
        }
        [Ignore]
        public string CompoundName
        {
            get
            {
                if (string.IsNullOrEmpty(Manufacturer.Name))
                { return Name; }
                else
                { return $"{Manufacturer.Name} {Name}"; }
            }
        }
        [Ignore]
        private Repository<ManufacturerModel> ManufacturerRepo { get; set; } = Repository<ManufacturerModel>.Instance;
        #endregion
        #region Methods
        private async void UpdateManufacturerAsync(int id)
        {
            if (ManufacturerRepo.IsValidID(id))
            {
                _manufacturerID = id;
                _manufacturer = await ManufacturerRepo.GetItemAsync(id);
            }
            else
            {
                _manufacturerID = -1;
                _manufacturer = new ManufacturerModel();
            }
            OnPropertyChanged("Manufacturer");
            OnPropertyChanged("ManufacturerID");
            OnPropertyChanged("CompoundName");
        }
        public override bool MatchFilter(string filter)
        {
            string ucfilter = filter.ToUpper();
            string ucname = CompoundName.ToUpper();
            if (base.MatchFilter(filter) == true)
            { return true; }
            else if (ucname.Contains(ucfilter))
            { return true; }
            else
            { return false; }
        }
        public override bool Equals(ModelBase otherModel)
        {
            if (otherModel.GetType() != typeof(KeywayModel))
            { return false; }

            KeywayModel otherKeyway = otherModel as KeywayModel;

            if (otherKeyway.ID != this.ID)
            { return false; }
            else if (otherKeyway.ManufacturerID != this.ManufacturerID)
            { return false; }
            else if (otherKeyway.Name != this.Name)
            { return false; }
            else
            { return true; }
        }
        public override string ToString()
        {
            return $"[{ID}] {Manufacturer.Name} {Name}";
        }
    }
    #endregion
}
