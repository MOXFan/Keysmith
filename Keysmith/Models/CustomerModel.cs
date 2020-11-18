namespace Keysmith.Models
{
    public class CustomerModel : ModelBase
    {
        #region Private Members
        private string _name = "";
        private string _accountNumber = "";
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
        #endregion
        #region Methods
        public override bool MatchFilter(string filter)
        {
            string ucfilter = filter.ToUpper();
            if (base.MatchFilter(filter) == true)
            { return true; }
            else if (Name.ToUpper().Contains(ucfilter))
            { return true; }
            else if (AccountNumber.ToUpper().Contains(ucfilter))
            { return true; }
            else
            { return false; }
        }
        public override bool Equals(ModelBase otherModel)
        {
            if (otherModel.GetType() != typeof(CustomerModel))
            { return false; }

            CustomerModel otherCustomer = otherModel as CustomerModel;

            if (otherCustomer.ID != this.ID)
            { return false; }
            else if (otherCustomer.Name != this.Name)
            { return false; }
            else if (otherCustomer.AccountNumber != this.AccountNumber)
            { return false; }
            else
            { return true; }
        }
        public override string ToString()
        {
            return $"[{ID}] {Name} {AccountNumber}";
        }
        #endregion
    }
}
