using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PharmaSysAPI.Models
{
    public class Proveedores
    {
        public int IdProveedor {  get; set; }
        public string NombreProveedor { get; set; }
        public string DireccionProveedor { get; set; }
        public string RUC { get; set; }
        public int TelefonoProveedor { get; set;}
        public string Email {  get; set; }

    }
}
