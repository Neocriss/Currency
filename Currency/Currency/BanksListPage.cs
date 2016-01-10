using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;


namespace Currency
{
    internal class BanksListPage : ContentPage
    {
        #region :: ~ Internal objects ~ ::

        private Label resultLabel = null;
        private Entry dollarsToExchangeEntry = null;
        private Button calculateButton = null;

        #endregion :: ^  Internal objects ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Constructors ~ ::

        public BanksListPage()
        {
            StackLayout mainStack = new StackLayout();

            // помещаем основной StackLayout в ContentView для иммитации Margin
            ContentView mainContentView = new ContentView()
            {
                Padding = new Thickness(5, 7),
                Content = mainStack
            };

            // объявляем разрешение экрана в платформо-независимых единицах
            double resolution = Device.OnPlatform(160, 160, 240);


            // в banksListStack будет выводиться список банков
            StackLayout banksListStack = new StackLayout
            {
                Padding = new Thickness(5),
                Spacing = 10
            };

            ScrollView banksListScrollView = new ScrollView
            {
                Content = banksListStack,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(5, 0),
            };


            // controlsStackLayout отвечает за размещения поля ввода и кнопки расчитать в нижней части приложения
            StackLayout controlsStackLayout = new StackLayout();

            Label questionLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Сколько долларов вы хотите поменять?",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                FontAttributes = FontAttributes.Bold
            };

            StackLayout actionControlsStackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };
            this.dollarsToExchangeEntry = new Entry()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.End,
                //HeightRequest = resolution * 10 / 72,
                WidthRequest = resolution * 50 / 72
            };
            this.calculateButton = new Button()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Text = "Рассчитать"
            };
            this.calculateButton.Clicked += CalculateButtonOnClicked;
            actionControlsStackLayout.Children.Add(this.dollarsToExchangeEntry);
            actionControlsStackLayout.Children.Add(this.calculateButton);

            this.resultLabel = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                IsVisible = false
            };

            controlsStackLayout.Children.Add(questionLabel);
            controlsStackLayout.Children.Add(actionControlsStackLayout);
            controlsStackLayout.Children.Add(this.resultLabel);


            // последовательно размещаем в основном StackLayout: блок со списком банков,
            //  разделитель и блок с контролами для взаимодействия пользователя с программой
            mainStack.Children.Add(banksListScrollView);
            mainStack.Children.Add(
                new BoxView()
                {
                    Color = Color.Accent,
                    HeightRequest = resolution / 90
                });
            mainStack.Children.Add(controlsStackLayout);


            // добавляем немного отступа сверху для основной страницы на iOS устройствах
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            
            // помещаем весь созданный интерфейс в содержимое страницы
            this.Content = mainContentView;
        }

        #endregion :: ^ Constructors ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Event handlers ~ ::

        private void CalculateButtonOnClicked(object sender, EventArgs eventArgs)
        {
            resultLabel.IsVisible = true;

            // горизонтальное размещение намеренно задаётся именно в обработчике событий
            //  после "IsVisible = true", чтобы resultLabel гарантированно стал видимым
            resultLabel.HorizontalOptions = LayoutOptions.Center;

            resultLabel.Text = "Максимальная сумма NNNNNNN рублей";
        }

        #endregion :: ^ Event handlers ^ ::
    }
}
