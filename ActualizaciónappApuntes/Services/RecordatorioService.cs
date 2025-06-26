using System.Text.Json;
using ActualizaciónappApuntes.Models;

namespace ActualizaciónappApuntes.Services
{
    public class RecordatorioService
    {
        private readonly string _fileName = "recordatorios.json";
        private readonly string _filePath;

        public RecordatorioService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, _fileName);
        }

        public async Task<List<Recordatorio>> GetAllAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<Recordatorio>();

                var json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<Recordatorio>>(json) ?? new List<Recordatorio>();
            }
            catch
            {
                return new List<Recordatorio>();
            }
        }

        public async Task SaveAllAsync(List<Recordatorio> recordatorios)
        {
            try
            {
                var json = JsonSerializer.Serialize(recordatorios, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar: {ex.Message}");
            }
        }

        public async Task AddAsync(Recordatorio recordatorio)
        {
            var recordatorios = await GetAllAsync();
            recordatorios.Add(recordatorio);
            await SaveAllAsync(recordatorios);
        }

        public async Task UpdateAsync(Recordatorio recordatorio)
        {
            var recordatorios = await GetAllAsync();
            var index = recordatorios.FindIndex(r => r.Id == recordatorio.Id);
            if (index >= 0)
            {
                recordatorios[index] = recordatorio;
                await SaveAllAsync(recordatorios);
            }
        }

        public async Task DeleteAsync(string id)
        {
            var recordatorios = await GetAllAsync();
            recordatorios.RemoveAll(r => r.Id == id);
            await SaveAllAsync(recordatorios);
        }
    }
}