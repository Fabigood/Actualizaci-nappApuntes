using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ActualizaciónappApuntes.Models
{
    public class Recordatorio : INotifyPropertyChanged
    {
        private string _texto = string.Empty;
        private DateTime _fechaHora = DateTime.Now;
        private bool _activo = true;

        public string Id { get; set; } = Guid.NewGuid().ToString();

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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}