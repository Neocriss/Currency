using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banks;
using Xamarin.Forms;


namespace Currency
{
    interface IBanksListUIManager
    {
        void InitializeData();

        StackLayout GetStackLayout();

        bool IsDataInitialized { get; }

        Bank this[int index] { get; }

        int Count { get; }

        EventHandler DataInitializedEvent { get; set; }
    }
}
