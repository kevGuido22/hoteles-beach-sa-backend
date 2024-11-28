namespace HotelesBeachSABackend.Models.Custom
{
    public class UsuarioRolDTO
    {
        public int UsuarioRolID { get; set; }

        public int UsuarioId { get; set; }

        public string? NombreCompleto { get; set; }

        public string? Email { get; set; }

        public int RolId { get; set; }

        public string? RolName { get; set; }
    }
}
