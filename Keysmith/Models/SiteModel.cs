using SQLite;

namespace Keysmith.Models
{
    public class SiteModel : ModelBase
    {
        #region Private Members
        private string _name = "";
        private int _customerID = 0;
        private string _accountNumber = "";
        private string _address = "";
        private string _city = "";
        private string _province = "";
        private string _postalCode = "";
        private CustomerModel _customer = new CustomerModel();
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
        public int CustomerID
        {
            get
            { return _customerID; }
            set
            { UpdateCustomerAsync(value); }
        }
        public string AccountNumber
        {
            get
            { return _accountNumber; }
            set
            {
                _accountNumber = value;
                OnPropertyChanged();
            }
        }
        public string Address
        {
            get
            { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }
        public string City
        {
            get
            { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }
        public string Province
        {
            get
            { return _province; }
            set
            {
                _province = value;
                OnPropertyChanged();
            }
        }
        public string PostalCode
        {
            get
            { return _postalCode; }
            set
            {
                _postalCode = value;
                OnPropertyChanged();
            }
        }
        [Ignore]
        public CustomerModel Customer
        {
            get
            { return _customer; }
            private set
            { UpdateCustomerAsync(value.ID); }
        }
        [Ignore]
        private Repository<CustomerModel> CustomerRepo { get; set; } = Repository<CustomerModel>.Instance;
        #endregion
        #region Methods
        private async void UpdateCustomerAsync(int id)
        {            
            if(CustomerRepo.IsValidID(id))
            {
                _customerID = id;
                _customer = await CustomerRepo.GetItemAsync(CustomerID);
            }
            else
            {
                _customer = new CustomerModel();
                _customerID = -1;
            }

            OnPropertyChanged("CustomerID");
            OnPropertyChanged("Customer");
        }
        public override bool MatchFilter(string filter)
        {
            string ucfilter = filter.ToUpper();
            if (base.MatchFilter(filter) == true)
            { return true; }
            else if (Name.ToUpper().Contains(ucfilter))
            { return true; }
            else if (CustomerID.ToString().Contains(filter))
            { return true; }
            else if (AccountNumber.ToUpper().Contains(ucfilter))
            { return true; }
            else if (Address.ToUpper().Contains(ucfilter))
            { return true; }
            else if (City.ToUpper().Contains(ucfilter))
            { return true; }
            else if (Province.ToUpper().Contains(ucfilter))
            { return true; }
            else if (PostalCode.ToUpper().Contains(ucfilter))
            { return true; }
            else if (Customer.Name.ToUpper().Contains(ucfilter))
            { return true; }
            else
            { return false; }
        }
        public override bool Equals(ModelBase otherModel)
        {
            if (otherModel.GetType() != typeof(SiteModel))
            { return false; }

            SiteModel otherSite = otherModel as SiteModel;

            if (otherSite.ID != this.ID)
            { return false; }
            else if (otherSite.Name != this.Name)
            { return false; }
            else if (otherSite.CustomerID != this.CustomerID)
            { return false; }
            else if (otherSite.AccountNumber != this.AccountNumber)
            { return false; }
            else if (otherSite.Address != this.Address)
            { return false; }
            else if (otherSite.City != this.City)
            { return false; }
            else if (otherSite.Province != this.Province)
            { return false; }
            else if (otherSite.PostalCode != this.PostalCode)
            { return false; }
            else
            { return true; }
        }
        public override string ToString()
        {
            return $"[{ID}] {Name} ({Customer.Name}/{AccountNumber}) {Address}, {City} {Province} {PostalCode}";
        }
        #endregion
    }
}
