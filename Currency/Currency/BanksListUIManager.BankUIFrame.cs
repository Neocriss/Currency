using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banks;
using Xamarin.Forms;


namespace Currency
{
    internal partial class BanksListUIManager
    {
        private class BankUIFrame
        {
            #region :: ~ Internal objects ~ ::

            private readonly Label usdAskLabel = null;
            private readonly Label askDeltaLabel = null;
            private decimal _askDelta = 0m;
            public event EventHandler DataInitialized;

            #endregion :: ^ Internal objects ^ ::

            //      ---     ---     ---     ---     ---

            #region :: ~ Constructors ~ ::

            public BankUIFrame(Bank bank)
            {
                if (bank == null)
                    throw new ArgumentNullException(nameof(bank));

                this.Bank = bank;
                this.Bank.DataRefreshed += Bank_DataRefreshed;

                // определяем отображение курса доллара банка
                this.usdAskLabel = new Label
                {
                    Text = "... ..",
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                // определяем отображение разницы в курсах банка
                this.askDeltaLabel = new Label
                {
                    Text = "...",
                    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                // собираем все визуальные составляющие в один блок
                this.Frame = new Frame
                {
                    OutlineColor = Color.Accent,
                    Padding = new Thickness(5),
                    Content = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 15,
                        Children =
                        {
                            new Label
                            {
                                Text = bank.Name,
                                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof (Label)),
                                //FontAttributes = FontAttributes.Bold,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.StartAndExpand
                            },
                            new StackLayout
                            {
                                Children =
                                {
                                    this.usdAskLabel,
                                    this.askDeltaLabel
                                },
                                HorizontalOptions = LayoutOptions.End
                            }
                        }
                    }
                };
            }

            #endregion :: ^ Constructors ^ ::

            //      ---     ---     ---     ---     ---

            #region :: ~ Properties ~ ::

            public bool IsDataInitialized { get; protected set; } = false;


            public decimal AskDelta
            {
                get { return this._askDelta; }
                set
                {
                    if (this._askDelta != value)
                    {
                        this._askDelta = value;
                        this.askDeltaLabel.Text = (value == 0m ? "..." : $"({value:F2})");
                    }
                }
            }


            public Bank Bank { get; private set; }


            public Frame Frame { get; private set; }

            #endregion :: ^ Properties ^ ::

            //      ---     ---     ---     ---     ---

            #region :: ~ Methods ~ ::



            #endregion :: ^ Methods ^ ::

            //      ---     ---     ---     ---     ---

            #region :: ~ Utility methods ~ ::

            private void OnDataInitialized()
            {
                this.DataInitialized?.Invoke(this, new EventArgs());
            }

            #endregion :: ^ Utility methods ^ ::

            //      ---     ---     ---     ---     ---

            #region :: ~ Event handlers ~ ::

            private void Bank_DataRefreshed(object sender, EventArgs e)
            {
                this.usdAskLabel.Text = $"{this.Bank.USDtoRUB.Ask:F2} руб./$";

                if (this.IsDataInitialized == false)
                {
                    this.IsDataInitialized = true;
                    this.OnDataInitialized();
                }
            }

            #endregion :: ^ Event handlers ^ ::
        }
    }
}
