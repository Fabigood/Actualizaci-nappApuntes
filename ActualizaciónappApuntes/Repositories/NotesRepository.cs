using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ActualizaciónappApuntes.Models;

namespace ActualizaciónappApuntes.Repositories
{
    public static class NotesRepository
    {
        const string FileName = "notes.txt";

        static string FilePath =>
            Path.Combine(FileSystem.AppDataDirectory, FileName);

        public static async Task<List<Note>> GetAllNotesAsync()
        {
            if (!File.Exists(FilePath))
                return new List<Note>();

            var lines = await File.ReadAllLinesAsync(FilePath);
            return lines
                .Select(line =>
                {
                    var parts = line.Split('\t');
                    return new Note
                    {
                        Text = parts[0],
                        Date = DateTime.Parse(parts[1])
                    };
                })
                .ToList();
        }

        public static async Task SaveAllNotesAsync(List<Note> notes)
        {
            var lines = notes.Select(n => $"{n.Text}\t{n.Date}");
            await File.WriteAllLinesAsync(FilePath, lines);
        }
    }
}
