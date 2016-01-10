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

        private IBanksListUIManager banksListUIManager = null;
        private readonly Label resultLabel = null;
        private readonly Entry dollarsToExchangeEntry = null;
        private readonly Button calculateButton = null;

        #endregion :: ^  Internal objects ^ ::

        //      ---     ---     ---     ---     ---

        #region :: ~ Constructors ~ ::

        public BanksListPage(IBanksListUIManager banksListUIManager)
        {
            if (banksListUIManager == null)
                throw new ArgumentNullException(nameof(banksListUIManager));

            // подготавливаем список банков к отображению и запускаем запрос данных по курсу валют
            this.banksListUIManager = banksListUIManager;
            this.banksListUIManager.DataInitializedEvent += BanksListDataInitialized;
            this.banksListUIManager.InitializeData();

            // стек mainStack будет располагать в себе все содержимое страницы
            StackLayout mainStack = new StackLayout();

            // помещаем основной StackLayout в ContentView для иммитации Margin
            ContentView mainContentView = new ContentView()
            {
                Padding = new Thickness(5, 7),
                Content = mainStack
            };

            // объявляем разрешение экрана в платформо-независимых единицах,
            //  которое понадобится для определения размеров ряда элементов
            double resolution = Device.OnPlatform(160, 160, 240);

            // организуем "прокрутку" для списка банков
            ScrollView banksListScrollView = new ScrollView
            {
                Content = this.banksListUIManager.GetStackLayout(),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(5, 0),
            };


            // формируем нижнюю часть страницы с которой будет взаимодействовать пользователь
            // controlsStackLayout отвечает за размещения поля ввода и кнопки расчитать в нижней части приложения
            StackLayout controlsStackLayout = new StackLayout() { BackgroundColor = Color.Silver };

            Label questionLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Сколько долларов Вы хотите поменять?",
                TextColor = Color.Black,
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
                WidthRequest = resolution * 50 / 72,
                TextColor = Color.Black,
                IsEnabled = false
            };
            this.calculateButton = new Button()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Text = "Рассчитать",
                IsEnabled = false
            };
            this.calculateButton.Clicked += CalculateButtonOnClicked;
            actionControlsStackLayout.Children.Add(this.dollarsToExchangeEntry);
            actionControlsStackLayout.Children.Add(this.calculateButton);

            // в этот Label будем отображать результат запроса пользователя
            this.resultLabel = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.Black,
                IsVisible = false
            };
            // помещаем resultLabel в ContentView также для иммитации Margin
            ContentView resultContentView = new ContentView()
            {
                Padding = new Thickness(5, 0),
                Content = this.resultLabel
            };


            // формируем панель предназначенную для взаимодействия пользователя с программой,
            //  вначале и в конце панели размещаем разделители
            controlsStackLayout.Children.Add(new BoxView()
            {
                Color = Color.Accent,
                HeightRequest = resolution / 90
            });
            controlsStackLayout.Children.Add(questionLabel);
            controlsStackLayout.Children.Add(actionControlsStackLayout);
            controlsStackLayout.Children.Add(resultContentView);
            controlsStackLayout.Children.Add(new BoxView()
            {
                Color = Color.Accent,
                HeightRequest = resolution / 90
            });


            // последовательно размещаем в основном StackLayout: блок со списком банков,
            //  и блок контролов для взаимодействия пользователя с программой
            mainStack.Children.Add(banksListScrollView);
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
            if (this.banksListUIManager.Count == 0 || !this.banksListUIManager.IsDataInitialized) return;

            resultLabel.IsVisible = true;

            // горизонтальное размещение намеренно задаётся именно в обработчике событий
            //  после "IsVisible = true", чтобы resultLabel гарантированно стал видимым
            resultLabel.HorizontalOptions = LayoutOptions.Center;


            decimal dollarsToExchange = 0;

            if (decimal.TryParse(this.dollarsToExchangeEntry.Text, out dollarsToExchange) && dollarsToExchange >= 0)
            {
                decimal result = this.banksListUIManager[0].USDtoRUB.Ask*dollarsToExchange;
                resultLabel.Text = $"Максимальная сумма {result:F2} рублей";
            }
            else
            {
                this.resultLabel.Text = "";
                resultLabel.Text = "Введите положительное число и попробуйте снова...";
            }
        }


        private void BanksListDataInitialized(object sender, EventArgs eventArgs)
        {
            this.calculateButton.IsEnabled = true;
            this.dollarsToExchangeEntry.IsEnabled = true;
        }

        #endregion :: ^ Event handlers ^ ::
    }
}
