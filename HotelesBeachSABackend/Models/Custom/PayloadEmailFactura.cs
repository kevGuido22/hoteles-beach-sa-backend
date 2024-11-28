namespace HotelesBeachSABackend.Models.Custom
{
    public class PayloadEmailFactura
    {
        public string Email { get; set; }

        public int FacturaId { get; set; }

        public int FormaPagoId { get; set; }
    }
}
