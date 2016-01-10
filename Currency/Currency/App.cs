using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;


namespace Currency
{
    public class App : Application
    {
        public App()
        {
            string[] bankNames =
            {
                "Сбербанк России",
                "Альфа-Банк",
                "Россельхозбанк",
                "Газпромбанк",
                "ВТБ 24",
                "Банк Москвы",
                "Промсвязьбанк",
                "Райффайзенбанк",
                "Совкомбанк",
                "ФК Открытие"
            };
            
            // назначаем главную страницу приложению
            MainPage = new BanksListPage(new BanksListUIManager(bankNames));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
