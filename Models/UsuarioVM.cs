namespace Login.Models
{
    public class UsuarioVM:Usuario
    {
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
    }
}
