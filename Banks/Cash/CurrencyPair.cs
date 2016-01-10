using System;


namespace Banks.Cash
{
    public class CurrencyPair
    {
        #region :: ~ Internal objects ~ ::

        private string _name = null;

        #endregion :: ^ Internal objects ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Constructors ~ ::
        
        public CurrencyPair(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length != 7)
                throw new ArgumentNullException(nameof(name), "CurrencyPair must have a valid name like \"USD/RUB\"");

            this._name = name.ToUpper();
        }


        public CurrencyPair(string name, decimal bid, decimal ask) : this(name)
        {
            if (bid < 0)
                throw new ArgumentOutOfRangeException(nameof(bid), "the value must be positive");
            else if (ask < 0)
                throw new ArgumentOutOfRangeException(nameof(ask), "the value must be positive");

            this.Bid = bid;
            this.Ask = ask;
        }

        #endregion :: ^ Constructors ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Properties ~ ::

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }


        public decimal Bid { get; set; }    // <-- покупка (как правило меньше чем Ask)


        public decimal Ask { get; set; }    // <-- продажа (как правило больше чем Bid) 

        #endregion :: ^ Properties ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Methods ~ ::

        public override string ToString() { return this.Name; }

        #endregion :: ^ Methods ^ ::
    }
}
