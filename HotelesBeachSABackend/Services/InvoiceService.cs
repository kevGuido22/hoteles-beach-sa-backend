using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System.Security.Cryptography;
namespace HotelesBeachSABackend.Services
{
    public class InvoiceService
    {
        public PdfDocument GetInvoice() {
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

            //Estilos personalizados con MigraDoc
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

            // Encabezado del documento
            paragraph.AddFormattedText("Hotel Beach S.A.", TextFormat.Bold);
            paragraph.AddLineBreak();

            // Información del sitio web
            paragraph.AddText("Sitio Web: ");
            paragraph.AddHyperlink("www.hotelbeach.com");
            paragraph.AddLineBreak();

            // Información del correo electrónico
            paragraph.AddText("Correo Electrónico: ");
            paragraph.AddText("hotelbeachnotificaciones@gmail.com");



            //Informacion del usuario

            // Bloque de información del usuario
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

            // Nombre del usuario
            userParagraph.AddText("Cédula: 123456789");
            userParagraph.AddLineBreak();

            // Correo electrónico del usuario
            userParagraph.AddText("Correo Electrónico: juan.perez@example.com");
            userParagraph.AddLineBreak();

        }
    }
}
