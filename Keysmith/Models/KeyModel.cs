using SQLite;
using System.Collections.Generic;
using System.Text;

namespace Keysmith.Models
{
    public class KeyModel : ModelBase
    {
        #region Private Members
        private bool _isControl = false;
        private string _code = "";
        private int _keywayID = 0;
        private int _lastSerial = 0;
        private string _cuts = "";
        private string _sidebar = "";
        private int _siteID = 0;
        private string _door = "";
        private KeywayModel _keyway = new KeywayModel();
        private IRepository<KeywayModel> _keywayRepo = null;
        private SiteModel _site = new SiteModel();
        private IRepository<SiteModel> _siteRepo = null;
        #endregion
        #region Properties
        public bool IsControl
        {
            get
            { return _isControl; }
            set
            {
                _isControl = value;
                OnPropertyChanged();
            }
        }
        public string Code
        {
            get
            { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged();
            }
        }
        public int KeywayID
        {
            get
            { return _keywayID; }
            set
            { UpdateKeywayAsync(value); }
        }
        public int LastSerial
        {
            get
            { return _lastSerial; }
            set
            {
                _lastSerial = value;
                OnPropertyChanged();
            }
        }
        public string Cuts
        {
            get
            { return _cuts; }
            set
            {
                _cuts = ValidateCuts(value);
                OnPropertyChanged();
            }
        }
        public string Sidebar
        {
            get
            { return _sidebar; }
            set
            {
                _sidebar = value;
                OnPropertyChanged();
            }
        }
        public int SiteID
        {
            get
            { return _siteID; }
            set
            { UpdateSiteAsync(value); }
        }
        public string Door
        {
            get
            { return _door; }
            set
            {
                _door = value;
                OnPropertyChanged();
            }
        }
        [Ignore]
        public List<int> CutsList { get { return GetCutsList(); } }
        [Ignore]
        public KeywayModel Keyway
        {
            get
            { return _keyway; }
            set
            { UpdateKeywayAsync(value.ID); }
        }
        [Ignore]
        public SiteModel Site
        {
            get
            { return _site; }
            set
            { UpdateSiteAsync(value.ID); }
        }
        [Ignore]
        private IRepository<KeywayModel> KeywayRepo
        {
            get
            {
                if(_keywayRepo == null)
                { _keywayRepo = Repository<KeywayModel>.Instance; }

                return _keywayRepo;
            }
            set { _keywayRepo = value; }
        }
        private IRepository<SiteModel> SiteRepo
        {
            get
            {
                if( _siteRepo == null)
                { _siteRepo = Repository<SiteModel>.Instance; }

                return _siteRepo;
            }
            set { _siteRepo = value; }
        }
        #endregion
        #region Methods
        private async void UpdateKeywayAsync(int id)
        {
            if(KeywayRepo.IsValidID(id))
            {
                _keywayID = id;
                _keyway = await KeywayRepo.GetItemAsync(id);
            }
            else
            {
                _keywayID = -1;
                _keyway = new KeywayModel();
            }

            OnPropertyChanged("Keyway");
            OnPropertyChanged("KeywayID");
        }
        private async void UpdateSiteAsync(int id)
        {
            if(SiteRepo.IsValidID(id))
            {
                _siteID = id;
                _site = await SiteRepo.GetItemAsync(id);
            }
            else
            {
                _siteID = -1;
                _site = new SiteModel();
            }

            OnPropertyChanged("Site");
            OnPropertyChanged("SiteID");
        }
        public override bool MatchFilter(string filter)
        {
            string ucfilter = filter.ToUpper();

            if (base.MatchFilter(filter) == true)
            { return true; }
            else if (Code.ToUpper().Contains(ucfilter))
            { return true; }
            else if (Cuts.ToUpper().Contains(ucfilter))
            { return true; }
            else if (Sidebar.ToUpper().Contains(ucfilter))
            { return true; }
            else if (Door.ToUpper().Contains(ucfilter))
            { return true; }
            else
            { return false; }
        }
        public override bool Equals(ModelBase otherModel)
        {
            if (otherModel.GetType() != typeof(KeyModel))
            { return false; }

            KeyModel otherKey = otherModel as KeyModel;

            if (otherKey.ID != this.ID)
            { return false; }
            else if (otherKey.IsControl != this.IsControl)
            { return false; }
            else if (otherKey.Code != this.Code)
            { return false; }
            else if (otherKey.KeywayID != this.KeywayID)
            { return false; }
            else if (otherKey.LastSerial != this.LastSerial)
            { return false; }
            else if (otherKey.Cuts != this.Cuts)
            { return false; }
            else if (otherKey.Sidebar != this.Sidebar)
            { return false; }
            else if (otherKey.SiteID != this.SiteID)
            { return false; }
            else if (otherKey.Door != this.Door)
            { return false; }
            else
            { return true; }

        }
        public override string ToString()
        {
            return $"[{ID}] {Code} ({Keyway}) {Cuts} <{Sidebar}> #{LastSerial} -- [{Site}] {Door} ({IsControl})";
        }
        public static string ValidateCuts(string input)
        {
            char[] inputArray = input.ToCharArray();
            StringBuilder output = new StringBuilder();

            foreach (char current in inputArray)
            {
                if (char.IsDigit(current))
                { output.Append(current); }
            }

            return output.ToString();
        }
        public List<int> GetCutsList()
        {
            List<int> output = new List<int>();
            char[] cutsArray = Cuts.ToCharArray();
            foreach (char currentChar in cutsArray)
            { output.Add(int.Parse(currentChar.ToString())); }
            return output;
        }
        #endregion
    }
}
