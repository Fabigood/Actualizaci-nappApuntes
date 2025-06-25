using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ActualizaciónappApuntes.Models;
using ActualizaciónappApuntes.Repositories;

namespace ActualizaciónappApuntes.ViewModels
{
    public class NotesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Note> Notes { get; } = new();

        private string newNoteText;
        public string NewNoteText
        {
            get => newNoteText;
            set
            {
                if (newNoteText != value)
                {
                    newNoteText = value;
                    OnPropertyChanged();
                }
            }
        }

        private Note selectedNote;
        public Note SelectedNote
        {
            get => selectedNote;
            set
            {
                if (selectedNote != value)
                {
                    selectedNote = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddOrUpdateNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }
        public ICommand EditNoteCommand { get; }

        public NotesViewModel()
        {
            AddOrUpdateNoteCommand = new Command<string>(OnAddOrUpdateNote);
            DeleteNoteCommand = new Command<Note>(OnDeleteNote);
            EditNoteCommand = new Command<Note>(async (note) => await OnEditNoteAsync(note));
            LoadNotesAsync();
        }

        async void LoadNotesAsync()
        {
            var notes = await NotesRepository.GetAllNotesAsync();
            foreach (var note in notes)
                Notes.Add(note);
        }

        async void OnAddOrUpdateNote(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            if (SelectedNote != null)
            {
                // Editando una nota
                SelectedNote.Text = text.Trim();
                SelectedNote.Date = DateTime.Now;
                SelectedNote = null;
            }
            else
            {
                // Añadiendo una nueva nota
                var newNote = new Note
                {
                    Text = text.Trim(),
                    Date = DateTime.Now
                };
                Notes.Add(newNote);
            }

            NewNoteText = string.Empty;
            await NotesRepository.SaveAllNotesAsync(Notes.ToList());
        }

        async void OnDeleteNote(Note note)
        {
            if (note == null)
                return;

            Notes.Remove(note);
            await NotesRepository.SaveAllNotesAsync(Notes.ToList());
        }

        async Task OnEditNoteAsync(Note note)
        {
            if (note == null)
                return;

            string newText = await Application.Current.MainPage.DisplayPromptAsync(
                "Editar Nota",
                "Modifica el contenido:",
                initialValue: note.Text);

            if (!string.IsNullOrWhiteSpace(newText))
            {
                note.Text = newText.Trim();
                note.Date = DateTime.Now;
                await NotesRepository.SaveAllNotesAsync(Notes.ToList());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
