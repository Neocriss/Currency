using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banks;
using Banks.Cash;
using Xamarin.Forms;


namespace Currency
{
    internal partial class BanksListUIManager : IBanksListUIManager
    {
        #region :: ~ Internal objects ~ ::

        private readonly IFinancialInfoProvider financialInfoProvider = new DummyFinancialInfoProvider();
        private List<BankUIFrame> bankUIFrames = new List<BankUIFrame>();
        private bool isInitializingNeeded = true;
        private bool _isDataInitialized = false;

        private event EventHandler DataInitialized;

        // в banksListStack будет выводиться список банков
        private readonly StackLayout banksListStack = new StackLayout
        {
            Padding = new Thickness(5),
            Spacing = 10
        };

        #endregion :: ^ Internal objects ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Constructors ~ ::

        public BanksListUIManager(string[] bankNames)
        {
            if (bankNames == null)
                throw new ArgumentNullException(nameof(bankNames));

            if (bankNames.Length == 0)
                throw new ArgumentException("bankNames should contain at least one element");
            
            foreach (string bankName in bankNames)
            {
                BankUIFrame bankUIFrame = new BankUIFrame(new Bank(bankName, financialInfoProvider));
                this.bankUIFrames.Add(bankUIFrame);
                this.banksListStack.Children.Add(bankUIFrame.Frame);
                bankUIFrame.DataInitialized += BankUIFrame_DataInitialized;
            }
        }

        #endregion :: ^ Constructors ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Properties ~ ::

        public bool IsDataInitialized
        {
            get { return this._isDataInitialized; }
            protected set
            {
                if (this._isDataInitialized != value)
                {
                    this._isDataInitialized = value;

                    // если все данные инициализированны, то сортируем список банков и обновляем StackLayout
                    if (this._isDataInitialized)
                    {
                        this.OrderByDescendingThenUpdate();
                        this.OnDataInitialized();
                    }
                }
            }
        }


        public Bank this[int index] => this.bankUIFrames[index].Bank;


        public int Count => this.bankUIFrames.Count;


        public EventHandler DataInitializedEvent
        {
            get { return this.DataInitialized; }
            set { this.DataInitialized = value; }
        }

        #endregion :: ^ Properties ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Methods ~ ::

        public void InitializeData()
        {
            if (!this.isInitializingNeeded) return;

            foreach (var bankUIFrame in this.bankUIFrames)
            {
                bankUIFrame.Bank.RefreshData();
            }
        }


        public StackLayout GetStackLayout()
        {
            return this.banksListStack;
        }


        public void Add(Bank bank)
        {
            this.IsDataInitialized = false;
            this.isInitializingNeeded = true;

            // todo: создать BankUIFrame, добавить его в список и подписаться на событие DataInitialized

            throw new NotImplementedException();
        }


        public bool Remove(Bank bank)
        {
            // todo: найти соотвествующий BankUIFrame, удалить его из списка и отписаться от события

            throw new NotImplementedException();
        }

        #endregion :: ^ Methods ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Utility methods ~ ::

        private void OnDataInitialized()
        {
            this.DataInitialized?.Invoke(this, new EventArgs());
        }


        private void OrderByDescendingThenUpdate()
        {
            this.bankUIFrames =
                            this.bankUIFrames.OrderByDescending(bankUIFrame => bankUIFrame.Bank.USDtoRUB.Bid).ToList();

            this.banksListStack.Children.Clear();
            foreach (BankUIFrame bankUIFrame in this.bankUIFrames)
            {
                bankUIFrame.DeltaBid = bankUIFrame.Bank.USDtoRUB.Bid - this.bankUIFrames[0].Bank.USDtoRUB.Bid;
                this.banksListStack.Children.Add(bankUIFrame.Frame);
            }
        }

        #endregion :: ^ Utility methods ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Event handlers ~ ::

        private void BankUIFrame_DataInitialized(object sender, EventArgs e)
        {
            int initializedBankUIFrameCount = this.bankUIFrames.Count(bankUIFrame => bankUIFrame.IsDataInitialized);
            this.IsDataInitialized = initializedBankUIFrameCount == this.bankUIFrames.Count;
        }

        #endregion :: ^ Event handlers ^ ::
    }
}
