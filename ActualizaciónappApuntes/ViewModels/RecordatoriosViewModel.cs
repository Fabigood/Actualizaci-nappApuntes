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
        private readonly RecordatorioService _recordatorioService;
        private string _texto = string.Empty;
        private DateTime _fechaHora = DateTime.Now;
        private bool _activo = true;

        public RecordatoriosViewModel()
        {
            _recordatorioService = new RecordatorioService();
            Recordatorios = new ObservableCollection<Recordatorio>();

            AddCommand = new Command(async () => await AddRecordatorio());
            DeleteCommand = new Command<Recordatorio>(async (r) => await DeleteRecordatorio(r));
            LoadCommand = new Command(async () => await LoadRecordatorios());

            _ = LoadRecordatorios();
        }

        public ObservableCollection<Recordatorio> Recordatorios { get; }

        public string Texto
        {
            get => _texto;
            set
            {
                _texto = value;
                OnPropertyChanged();
            }
        }

        public DateTime FechaHora
        {
            get => _fechaHora;
            set
            {
                _fechaHora = value;
                OnPropertyChanged();
            }
        }

        public bool Activo
        {
            get => _activo;
            set
            {
                _activo = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand LoadCommand { get; }

        private async Task AddRecordatorio()
        {
            if (string.IsNullOrWhiteSpace(Texto))
                return;

            var recordatorio = new Recordatorio
            {
                Texto = Texto,
                FechaHora = FechaHora,
                Activo = Activo
            };

            await _recordatorioService.AddAsync(recordatorio);
            Recordatorios.Add(recordatorio);

            // Limpiar campos
            Texto = string.Empty;
            FechaHora = DateTime.Now;
            Activo = true;
        }

        private async Task DeleteRecordatorio(Recordatorio recordatorio)
        {
            if (recordatorio == null) return;

            await _recordatorioService.DeleteAsync(recordatorio.Id);
            Recordatorios.Remove(recordatorio);
        }

        private async Task LoadRecordatorios()
        {
            var recordatorios = await _recordatorioService.GetAllAsync();
            Recordatorios.Clear();
            foreach (var recordatorio in recordatorios.OrderByDescending(r => r.FechaHora))
            {
                Recordatorios.Add(recordatorio);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}