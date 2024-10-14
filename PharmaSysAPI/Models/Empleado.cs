namespace PharmaSysAPI.Models
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string CédulaEmpleado { get; set; }
        public int TeléfonoEmpleado { get; set; }
        public string Email { get; set; }
        public string DirecciónEmpleado { get; set; }
        public string Genero { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public int IdCargo { get; set; }
    }
}
