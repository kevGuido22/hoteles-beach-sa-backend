using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System.Security.Cryptography;
namespace HotelesBeachSABackend.Services
{
    public class InvoiceService
    {
        private string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "LogoHotelBeach.png");

        public PdfDocument GetInvoice()
        {
            //create a new migraDoc document
            var document = new Document();

            BuildDocument(document);

            //create a renderer for the MigraDoc Document
            var pdfRenderer = new PdfDocumentRenderer
            {
                Document = document
            };

            //layout and render document to pdf
            pdfRenderer.RenderDocument();
            return pdfRenderer.PdfDocument;
        }

        private void BuildDocument(Document document)
        {
            Section section = document.AddSection();

            // Estilos personalizados con MigraDoc
            var style = document.Styles["Heading1"];
            if (style == null)
            {
                style = document.Styles.AddStyle("Heading1", "Normal");
                style.Font.Size = 14;
                style.Font.Bold = true;
                style.ParagraphFormat.SpaceAfter = "8pt";
            }

            // Agrega un párrafo con formato
            var paragraph = section.AddParagraph();
            paragraph.Format.Font.Name = "Arial";
            paragraph.Format.Font.Size = 12;
            paragraph.Format.SpaceAfter = "6pt";


            // Agrega una tabla con dos columnas
            Table tableHeader = section.AddTable();
            tableHeader.Borders.Visible = false;

            // Define las columnas
            tableHeader.AddColumn(Unit.FromCentimeter(12)); // Ajusta el ancho según sea necesario
            tableHeader.AddColumn(Unit.FromCentimeter(3));  // Columna para la imagen

            // Agrega una fila a la tabla
            Row rowHeader = tableHeader.AddRow();

            // Celda para el texto
            Cell textCell = rowHeader.Cells[0];
            Paragraph textParagraph = textCell.AddParagraph();
            textParagraph.Format.Font.Name = "Arial";
            textParagraph.Format.Font.Size = 12;
            textParagraph.Format.SpaceAfter = "6pt";

            // Agrega el texto al párrafo
            textParagraph.AddFormattedText("Hotel Beach S.A.", TextFormat.Bold);
            textParagraph.AddLineBreak();
            textParagraph.AddText("Sitio Web: ");
            textParagraph.AddHyperlink("www.hotelbeach.com");
            textParagraph.AddLineBreak();
            textParagraph.AddText("Correo Electrónico: ");
            textParagraph.AddText("hotelbeachnotificaciones@gmail.com");
            textParagraph.AddLineBreak();

            // Celda para la imagen
            Cell imageCell = rowHeader.Cells[1];
            Paragraph imageParagraph = imageCell.AddParagraph();
            imageParagraph.Format.Alignment = ParagraphAlignment.Center; // Centrar horizontalmente

            // Agregar la imagen
            Image image = imageParagraph.AddImage(logoPath);

            // Ajusta las propiedades de la imagen
            image.Width = Unit.FromCentimeter(3); // Tamaño de la imagen
            image.LockAspectRatio = true;



            //logo

            // Información del usuario
            var userParagraph = section.AddParagraph();
            userParagraph.Format.Font.Name = "Arial";
            userParagraph.Format.Font.Size = 12;
            userParagraph.Format.SpaceAfter = "6pt";

            // Encabezado del bloque de usuario
            userParagraph.AddFormattedText("Información del Usuario", TextFormat.Bold);
            userParagraph.AddLineBreak();

            // Nombre del usuario
            userParagraph.AddText("Nombre: Juan Pérez");
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

            // Agregar la tabla con los detalles del paquete
            var table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Format.SpaceAfter = "12pt";  // Controla el espacio después de la tabla

            // Definir las columnas de la tabla
            table.AddColumn(Unit.FromCentimeter(5)); // Columna 1: Paquete
            table.AddColumn(Unit.FromCentimeter(3)); // Columna 2: Mensualidades
            table.AddColumn(Unit.FromCentimeter(4)); // Columna 3: Cantidad de Personas
            table.AddColumn(Unit.FromCentimeter(4)); // Columna 4: Costo por Persona

            // Establecer encabezados con formato en negrita
            var row = table.AddRow();
            row.Cells[0].AddParagraph("Paquete").Format.Font.Bold = true;
            row.Cells[1].AddParagraph("Mensualidades").Format.Font.Bold = true;
            row.Cells[2].AddParagraph("Cantidad de Personas").Format.Font.Bold = true;
            row.Cells[3].AddParagraph("Costo por Persona").Format.Font.Bold = true;

            // Agregar la fila con los datos
            row = table.AddRow();
            row.Cells[0].AddParagraph("Todo Incluido");
            row.Cells[1].AddParagraph("12");
            row.Cells[2].AddParagraph("5");
            row.Cells[3].AddParagraph("$25");

            // Agregar un salto de línea para evitar que las tablas se peguen
            section.AddParagraph().Format.SpaceAfter = "12pt";  // Esto ayuda a agregar un espacio entre las tablas

            // Agregar otra sección para los costos
            reservationParagraph.AddLineBreak();
            reservationParagraph.AddFormattedText("Resumen de Costos", TextFormat.Bold);
            reservationParagraph.AddLineBreak();

            // Agregar otra tabla con los costos
            var costTable = section.AddTable();
            costTable.Borders.Width = 0.75;
            costTable.Format.SpaceAfter = "12pt";  // Controla el espacio después de la tabla

            // Definir las columnas de la tabla de costos
            costTable.AddColumn(Unit.FromCentimeter(8)); // Columna 1: Descripción
            costTable.AddColumn(Unit.FromCentimeter(4)); // Columna 2: Monto

            // Agregar la primera fila con los encabezados
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
