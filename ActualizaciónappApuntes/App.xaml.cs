namespace ActualizaciónappApuntes
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try
            {
                MainPage = new AppShell(); //
            }
            catch (Exception ex)
            {
                MainPage = new ContentPage
                {
                    Content = new Label
                    {
                        Text = $"Error al cargar: {ex.Message}",
                        TextColor = Colors.Red
                    }
                };
            }

        }

    }
}