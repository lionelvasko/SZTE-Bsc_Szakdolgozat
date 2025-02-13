namespace Szakdoga
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) 
            {
                MinimumWidth = 500,
                MinimumHeight = 400,
                Title = "Szakdolgozat" 
            };
        }
    }
}
