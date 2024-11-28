using HotelesBeachSABackend.Controllers;
using HotelesBeachSABackend.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System.Security.Cryptography;
namespace HotelesBeachSABackend.Services
{
    public class InvoiceService
    {
        private string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "LogoHotelBeach.png");

        public PdfDocument GetInvoice(Factura factura, Usuario usuario, Reservacion reservacion, Paquete paquete, string formaPago, DetallePago tempDetallePago) {
            //Se crea un nuevo documento de tipo MigraDoc
            var document = new Document();

            BuildDocument(document, factura, usuario, reservacion, paquete, formaPago, tempDetallePago);

            //se crea un renderer para el documento de MigraDoc
            var pdfRenderer = new PdfDocumentRenderer
            {
                Document = document
            };

            //Se renderiza el documento
            pdfRenderer.RenderDocument();
            return pdfRenderer.PdfDocument;
        }

        private void BuildDocument(Document document, Factura tempFactura, Usuario tempUsuario, Reservacion tempReservacion, Paquete tempPaquete, string tempFormaPago, DetallePago tempDetallePago)
        {
            Section section = document.AddSection();

            // Estilos personalizados
            var style = document.Styles["Heading1"];
            if (style == null)
            {
                style = document.Styles.AddStyle("Heading1", "Normal");
                style.Font.Size = 14;
                style.Font.Bold = true;
                style.ParagraphFormat.SpaceAfter = "8pt";
            }
            //Estilos para el logotipo
            Image image = section.Headers.Primary.AddImage(logoPath);
            image.Height = "4cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Margin;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Right;
            image.WrapFormat.Style = WrapStyle.Through;

            // Estilo de parrafo
            var paragraph = section.AddParagraph();
            paragraph.Format.Font.Name = "Arial";
            paragraph.Format.Font.Size = 12;
            paragraph.Format.SpaceAfter = "20pt";

            // Estilo de encabezado
            var paragraphHeader = section.AddParagraph();
            paragraphHeader.Format.Font.Name = "Arial";
            paragraphHeader.Format.Font.Size = 12;
            paragraphHeader.Format.SpaceAfter = "4pt";
            paragraphHeader.Format.SpaceBefore = "20pt";

            // Encabezado del documento
            paragraphHeader.AddFormattedText("Hotel Beach S.A.", TextFormat.Bold);
            paragraphHeader.AddLineBreak();

            // Información del sitio web
            paragraphHeader.AddText("Sitio Web: ");
            paragraphHeader.AddHyperlink("www.hotelbeach.com");
            paragraphHeader.AddLineBreak();

            // Información del correo electrónico
            paragraphHeader.AddText("Correo Electrónico: ");
            paragraphHeader.AddText("hotelbeachnotificaciones@gmail.com");
            paragraphHeader.AddLineBreak();

            // Información del usuario
            var userParagraph = section.AddParagraph();
            userParagraph.Format.Font.Name = "Arial";
            userParagraph.Format.Font.Size = 12;
            userParagraph.Format.SpaceAfter = "6pt";

            // Encabezado del bloque de usuario
            userParagraph.AddFormattedText("Información del Usuario", TextFormat.Bold);
            userParagraph.AddLineBreak();

            // Nombre del usuario
            userParagraph.AddText($"Nombre: {tempUsuario.Nombre_Completo}");
            userParagraph.AddLineBreak();

            // Cédula del usuario
            userParagraph.AddText($"Cédula: {tempUsuario.Cedula}");
            userParagraph.AddLineBreak();

            // Correo electrónico del usuario
            userParagraph.AddText($"Correo Electrónico: {tempUsuario.Email}");
            userParagraph.AddLineBreak();

            // Información de la reservación
            var reservationParagraph = section.AddParagraph();
            reservationParagraph.Format.Font.Name = "Arial";
            reservationParagraph.Format.Font.Size = 12;
            reservationParagraph.Format.SpaceAfter = "6pt";

            // Encabezado de la reservación
            reservationParagraph.AddFormattedText("Detalles de la Reservación", TextFormat.Bold);
            reservationParagraph.AddLineBreak();

            // Fecha de inicio y fin
            reservationParagraph.AddText($"No Factura: {tempFactura.Id}");
            reservationParagraph.AddLineBreak();
            reservationParagraph.AddText($"Fecha de Inicio: {tempReservacion.FechaInicio}");
            reservationParagraph.AddLineBreak();
            reservationParagraph.AddText($"Fecha de Finalización: {tempReservacion.FechaFin}");
            reservationParagraph.AddLineBreak();
            reservationParagraph.AddText($"Forma de Pago: {tempFormaPago}");
            reservationParagraph.AddLineBreak();

            if(tempFormaPago != "Efectivo")
            {
                if (tempFormaPago == "Cheque")
                {
                    reservationParagraph.AddText($"Número de Cheque: {tempDetallePago.NumeroCheque}");
                    reservationParagraph.AddLineBreak();
                }
                if(tempFormaPago == "Tarjeta")
                {
                    reservationParagraph.AddText($"Número de Cheque: {tempDetallePago.NumeroTarjeta}");
                    reservationParagraph.AddLineBreak();
                }
                reservationParagraph.AddText($"Banco: {tempDetallePago.Banco}");
                reservationParagraph.AddLineBreak();
            }


            reservationParagraph.AddText($"Cantidad de Noches: {tempFactura.CantidadNoches}");
            reservationParagraph.AddLineBreak();


            // Creacion de Tabla para 
            var table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Format.SpaceAfter = "12pt";  

            // Definir las columnas de la tabla
            table.AddColumn(Unit.FromCentimeter(5)); // Columna 1: Paquete
            table.AddColumn(Unit.FromCentimeter(3)); // Columna 2: Mensualidades
            table.AddColumn(Unit.FromCentimeter(4)); // Columna 3: Cantidad de Personas
            table.AddColumn(Unit.FromCentimeter(4)); // Columna 4: Costo por Persona

            // Nombres de las columnas
            var row = table.AddRow();
            row.Cells[0].AddParagraph("Paquete").Format.Font.Bold = true;
            row.Cells[1].AddParagraph("Mensualidades").Format.Font.Bold = true;
            row.Cells[2].AddParagraph("Cantidad de Personas").Format.Font.Bold = true;
            row.Cells[3].AddParagraph("Persona/Noche").Format.Font.Bold = true;

            // Valores de la columna
            row = table.AddRow();
            row.Cells[0].AddParagraph($"{tempPaquete.Nombre}");
            row.Cells[1].AddParagraph($"{tempPaquete.Mensualidades}");
            row.Cells[2].AddParagraph($"{tempReservacion.CantidadPersonas}");
            row.Cells[3].AddParagraph($"${tempPaquete.CostoPersona}");

            section.AddParagraph().Format.SpaceAfter = "12pt"; 

            // Costos
            reservationParagraph.AddLineBreak();
            reservationParagraph.AddFormattedText("Resumen de Costos", TextFormat.Bold);
            reservationParagraph.AddLineBreak();

            // Tabla Costos
            var costTable = section.AddTable();
            costTable.Borders.Width = 0.75;
            costTable.Format.SpaceAfter = "12pt";  
            costTable.Format.Alignment = ParagraphAlignment.Right;
            costTable.Rows.LeftIndent = Unit.FromCentimeter(7.90);
            costTable.AddColumn(Unit.FromCentimeter(4)); // Columna 1: Descripción
            costTable.AddColumn(Unit.FromCentimeter(4)); // Columna 2: Monto

            // Valores
            //row = costTable.AddRow();
            //row.Cells[0].AddParagraph("Subtotal (Dólares)").Format.Font.Bold = true;
            //row.Cells[1].AddParagraph($"{tempFactura.TotalDolares}").Format.Font.Bold = true;

            //row = costTable.AddRow();
            //row.Cells[0].AddParagraph("Total con IVA");
            //row.Cells[1].AddParagraph("$1130");
            // Valores

            decimal Subtotal = tempFactura.TotalDolares - (tempFactura.TotalDolares * (tempFactura.ValorDescuento / 100m));

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Subtotal (Dólares)").Format.Font.Bold = true;
            row.Cells[1].AddParagraph($"${Subtotal:F2}").Format.Font.Bold = true;

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Descuento");
            row.Cells[1].AddParagraph($"{tempFactura.ValorDescuento}%"); //listo

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Total (Dólares)");
            row.Cells[1].AddParagraph($"${tempFactura.TotalDolares}"); //listo

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Total (Colones)");
            row.Cells[1].AddParagraph($"₡{tempFactura.TotalColones}");  //listo

            decimal prima = tempFactura.TotalDolares * (tempPaquete.PrimaReserva / 100m);

            row = costTable.AddRow();
            row.Cells[0].AddParagraph($"Prima ({tempPaquete.PrimaReserva}%)");
            row.Cells[1].AddParagraph($"${prima:F2}"); //listo

            decimal mensualidad = (tempFactura.TotalDolares - prima) / tempPaquete.Mensualidades;

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Mensualidad(Dolares)");
            row.Cells[1].AddParagraph($"${mensualidad:F2}");

            // Fin del documento
            paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Hotel Beach S.A | Todos los derechos reservados");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
        }


    }
}
