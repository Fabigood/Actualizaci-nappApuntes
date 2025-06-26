using System.Text.Json;
using ActualizaciónappApuntes.Models;

namespace ActualizaciónappApuntes.Services
{
    public class RecordatoriosService
    {
        private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "recordatorios.json");

        public async Task<List<Recordatorio>> GetRecordatoriosAsync()
        {
            if (!File.Exists(_filePath))
                return new List<Recordatorio>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Recordatorio>>(json) ?? new List<Recordatorio>();
        }

        public async Task<bool> AddRecordatorioAsync(Recordatorio nuevo)
        {
            var lista = await GetRecordatoriosAsync();
            lista.Add(nuevo);
            return await GuardarAsync(lista);
        }

        public async Task<bool> DeleteRecordatorioAsync(Guid id)
        {
            var lista = await GetRecordatoriosAsync();
            var item = lista.FirstOrDefault(x => x.Id == id);
            if (item == null) return false;

            lista.Remove(item);
            return await GuardarAsync(lista);
        }

        public async Task<bool> UpdateRecordatorioAsync(Recordatorio actualizado)
        {
            var lista = await GetRecordatoriosAsync();
            var index = lista.FindIndex(x => x.Id == actualizado.Id);
            if (index == -1) return false;

            lista[index] = actualizado;
            return await GuardarAsync(lista);
        }

        private async Task<bool> GuardarAsync(List<Recordatorio> lista)
        {
            try
            {
                var json = JsonSerializer.Serialize(lista);
                await File.WriteAllTextAsync(_filePath, json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
