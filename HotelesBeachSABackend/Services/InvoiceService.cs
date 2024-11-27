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

        public PdfDocument GetInvoice(int idFactura) {
            //Se crea un nuevo documento de tipo MigraDoc
            var document = new Document();

            BuildDocument(document, idFactura);

            //se crea un renderer para el documento de MigraDoc
            var pdfRenderer = new PdfDocumentRenderer
            {
                Document = document
            };

            //Se renderiza el documento
            pdfRenderer.RenderDocument();
            return pdfRenderer.PdfDocument;
        }

        private void BuildDocument(Document document, int idFactura)
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
            userParagraph.AddText($"Nombre: {idFactura}");
            userParagraph.AddLineBreak();

            // Cédula del usuario
            userParagraph.AddText("Cédula: 123456789");
            userParagraph.AddLineBreak();

            // Correo electrónico del usuario
            userParagraph.AddText("Correo Electrónico: juan.perez@example.com");
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
            reservationParagraph.AddText("Fecha de Inicio: 1/10/2024");
            reservationParagraph.AddLineBreak();
            reservationParagraph.AddText("Fecha de Finalización: 1/10/2024");
            reservationParagraph.AddLineBreak();
            reservationParagraph.AddText("Cantidad de Noches: 2");
            reservationParagraph.AddLineBreak();
            reservationParagraph.AddText("Forma de Pago: Efectivo");
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
            row.Cells[3].AddParagraph("Costo por Persona").Format.Font.Bold = true;

            // Valores de la columna
            row = table.AddRow();
            row.Cells[0].AddParagraph("Todo Incluido");
            row.Cells[1].AddParagraph("12");
            row.Cells[2].AddParagraph("5");
            row.Cells[3].AddParagraph("$25");

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
            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Subtotal (Dólares)").Format.Font.Bold = true;
            row.Cells[1].AddParagraph("$1000").Format.Font.Bold = true;

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("IVA (13%)");
            row.Cells[1].AddParagraph("$130");

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Total con IVA");
            row.Cells[1].AddParagraph("$1130");

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Descuento");
            row.Cells[1].AddParagraph("$25");

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Total (Dólares)");
            row.Cells[1].AddParagraph("$1105");

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Total (Colones)");
            row.Cells[1].AddParagraph("₡530,400");

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Prima (10%)");
            row.Cells[1].AddParagraph("$110.50");

            row = costTable.AddRow();
            row.Cells[0].AddParagraph("Mensualidad");
            row.Cells[1].AddParagraph("$82.13");

            // Fin del documento
            paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Hotel Beach S.A | Todos los derechos reservados");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
        }


    }
}
