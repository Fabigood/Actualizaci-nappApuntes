using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ActualizaciónappApuntes.Models;
using ActualizaciónappApuntes.Services;

namespace ActualizaciónappApuntes.ViewModels
{
    public class RecordatoriosViewModel : INotifyPropertyChanged
    {
        private readonly RecordatoriosService _recordatoriosService;
        private ObservableCollection<Recordatorio> _recordatorios;
        private bool _isLoading;
        private string _nuevoTexto = "";
        private DateTime _nuevaFecha = DateTime.Now.AddHours(1);
        private TimeSpan _nuevaHora = DateTime.Now.AddHours(1).TimeOfDay;

        public ObservableCollection<Recordatorio> Recordatorios
        {
            get => _recordatorios;
            set { _recordatorios = value; OnPropertyChanged(); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public string NuevoTexto
        {
            get => _nuevoTexto;
            set
            {
                _nuevoTexto = value;
                OnPropertyChanged();
                ((Command)AgregarRecordatorioCommand).ChangeCanExecute();
            }
        }

        public DateTime NuevaFecha
        {
            get => _nuevaFecha;
            set { _nuevaFecha = value; OnPropertyChanged(); }
        }

        public TimeSpan NuevaHora
        {
            get => _nuevaHora;
            set { _nuevaHora = value; OnPropertyChanged(); }
        }

        public ICommand AgregarRecordatorioCommand { get; }
        public ICommand EliminarRecordatorioCommand { get; }
        public ICommand ToggleActivoCommand { get; }
        public ICommand RefreshCommand { get; }

        public RecordatoriosViewModel()
        {
            _recordatoriosService = new RecordatoriosService();
            _recordatorios = new ObservableCollection<Recordatorio>();

            AgregarRecordatorioCommand = new Command(async () => await AgregarRecordatorio(), () => !string.IsNullOrWhiteSpace(NuevoTexto));
            EliminarRecordatorioCommand = new Command<Recordatorio>(async (r) => await EliminarRecordatorio(r));
            ToggleActivoCommand = new Command<Recordatorio>(async (r) => await ToggleActivo(r));
            RefreshCommand = new Command(async () => await CargarRecordatorios());

            _ = CargarRecordatorios();
        }

        private async Task CargarRecordatorios()
        {
            try
            {
                IsLoading = true;
                var data = await _recordatoriosService.GetRecordatoriosAsync();

                Recordatorios.Clear();
                foreach (var r in data.OrderBy(r => r.FechaHora))
                    Recordatorios.Add(r);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AgregarRecordatorio()
        {
            try
            {
                var nuevo = new Recordatorio
                {
                    Texto = NuevoTexto.Trim(),
                    FechaHora = NuevaFecha.Date + NuevaHora,
                    Activo = true
                };

                var exito = await _recordatoriosService.AddRecordatorioAsync(nuevo);
                if (exito)
                {
                    Recordatorios.Add(nuevo);
                    NuevoTexto = "";
                    NuevaFecha = DateTime.Now.AddHours(1);
                    NuevaHora = DateTime.Now.AddHours(1).TimeOfDay;

                    // Ordenar
                    var ordenados = Recordatorios.OrderBy(r => r.FechaHora).ToList();
                    Recordatorios.Clear();
                    foreach (var r in ordenados) Recordatorios.Add(r);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo guardar", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task EliminarRecordatorio(Recordatorio r)
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Eliminar", $"¿Eliminar '{r.Texto}'?", "Sí", "No");
            if (!confirm) return;

            var exito = await _recordatoriosService.DeleteRecordatorioAsync(r.Id);
            if (exito)
                Recordatorios.Remove(r);
        }

        private async Task ToggleActivo(Recordatorio r)
        {
            r.Activo = !r.Activo;
            await _recordatoriosService.UpdateRecordatorioAsync(r);
            OnPropertyChanged(nameof(Recordatorios));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
