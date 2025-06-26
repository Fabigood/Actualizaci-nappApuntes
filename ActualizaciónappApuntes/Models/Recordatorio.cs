namespace ActualizaciónappApuntes.Models
{
    public class Recordatorio
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Texto { get; set; }
        public DateTime FechaHora { get; set; }
        public bool Activo { get; set; }
    }
}
