using ActualizaciónappApuntes.Services;
using ActualizaciónappApuntes.ViewModels;
using ActualizaciónappApuntes.Views;
using ActualizaciónappApuntes.Converters;

namespace ActualizaciónappApuntes;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Registrar servicios
        builder.Services.AddSingleton<RecordatorioService>();

        // Registrar ViewModels
        builder.Services.AddTransient<RecordatoriosViewModel>();

        // Registrar Views
        builder.Services.AddTransient<RecordatoriosPage>();

        return builder.Build();
    }
}