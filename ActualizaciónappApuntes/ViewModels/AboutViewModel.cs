using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ActualizaciónappApuntes.Models;

namespace ActualizaciónappApuntes.ViewModels
{
    public class AboutViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Member> _teamMembers;

        public ObservableCollection<Member> TeamMembers
        {
            get => _teamMembers;
            set
            {
                if (_teamMembers != value)
                {
                    _teamMembers = value;
                    OnPropertyChanged();
                }
            }
        }

        public AboutViewModel()
        {
            LoadTeamMembers();
        }

        private void LoadTeamMembers()
        {

            TeamMembers = new ObservableCollection<Member>
            {

                new Member
                {
                    Nombre = "Dana Boada",
                    Edad = 22,
                    DeporteFav = "Judo",
                    Photo = "dana.png",
                    Rol = "Diseñadora UX/UI",
                    Descripcion = "Le apasiona crear experiencias visuales únicas y divertidas. También es una competidora dedicada de judo.",
                    SportIcon = "🥋"
                },
                new Member
                {
                    Nombre = "Abigail Espinosa",
                    Edad = 19,
                    DeporteFav = "Cero Deportes",
                    Photo = "abi.png",
                    Rol = "Desarrolladora en FrontEnd, Arquitecta de Software",
                    Descripcion = "Estratega en APIs y soluciones técnicas.No practica ningun deporte.",
                    SportIcon = "🧘‍♀️"
                },
                new Member
                {
                    Nombre = "Nicolas Recalde",
                    Edad = 20,
                    DeporteFav = "Baloncesto",
                    Photo = "nicolas.png",
                    Rol = "Desarrollador Backend",
                    Descripcion = "Apasionado por el diseño responsivo y las animaciones modernas. Fuera del código, vive para el baloncesto.",
                    SportIcon = "🏀"
                },
                new Member
                {
                    Nombre = "Fabio Gonzalez",
                    Edad = 21,
                    DeporteFav = "Fútbol",
                    Photo = "fabio.png",
                    Rol = "Ingeniero de Datos",
                    Descripcion = "Transforma datos en decisiones. Siempre listo para jugar un partido de fútbol o una partida de ajedrez.",
                    SportIcon = "⚽"
                }
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
