using System;
using System.Collections.Generic;
using Banks.Cash;


namespace Banks
{
    public class Bank
    {
        #region :: ~ Internal objects ~ ::

        private string _name = null;
        private CurrencyPair _usd_to_rub = new CurrencyPair("USD/RUB");
        private CurrencyPair _eur_to_rub = new CurrencyPair("EUR/RUB");
        private IFinancialInfoProvider infoProvider = null;

        public event EventHandler DataRefreshed;

        #endregion :: ^ Internal objects ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Constructors ~ ::

        public Bank(string name, IFinancialInfoProvider infoProvider)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Bank must have a valid name");

            if (infoProvider == null)
                throw new ArgumentNullException(nameof(infoProvider));

            this._name = name;
            this.infoProvider = infoProvider;
        }

        #endregion :: ^ Constructors ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Properties ~ ::

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }


        public bool IsDataProvided { get; protected set; } = false;


        public CurrencyPair USDtoRUB
        {
            get { return this._usd_to_rub; }
            set { this._usd_to_rub = value; }
        }


        public CurrencyPair EURtoRUB
        {
            get { return this._eur_to_rub; }
            set { this._eur_to_rub = value; }
        }

        #endregion :: ^ Properties ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Methods ~ ::

        public async void RefreshData()
        {
            IEnumerable<CurrencyPair> currencyPairs;

            try
            {
                currencyPairs = await infoProvider.GetActualCurrencyPairsAsync(new string[] { this.USDtoRUB.Name, this.EURtoRUB.Name });
            }
            catch (Exception)
            {
                throw new Exception("a fake FinancialInfoProvider has broken");
            }

            foreach (var currencyPair in currencyPairs)
            {
                switch (currencyPair.Name)
                {
                    case "USD/RUB":
                        this.USDtoRUB = currencyPair;
                        break;

                    case "EUR/RUB":
                        this.EURtoRUB = currencyPair;
                        break;
                }
            }

            this.IsDataProvided = true;
            this.OnDataRefreshed();
        }


        public override string ToString()
        {
            return this.Name;
        }

        #endregion :: ^ Methods ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Utility methods ~ ::

        private void OnDataRefreshed()
        {
            if (DataRefreshed != null)
            {
                this.DataRefreshed(this, new EventArgs());
            }
        }

        #endregion :: ^ Utility methods ^ ::
    }
}
