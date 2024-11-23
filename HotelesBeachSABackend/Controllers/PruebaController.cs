//using Microsoft.AspNetCore.Mvc;
//using HotelesBeachSABackend.Services; 


//namespace HotelesBeachSABackend.Pages
//{
//    public IActionResult : PageModel
//    {
//        public void OnGet() { 
        
//            var invoiceService = new InvoiceService();
//            var document = invoiceService.GetInvoice(); 
//            //document.Save("Invoice.pdf")]

//            //Send PDF to browser
//            MemoryStream stream = new MemoryStream();
//            document.Save(stream);
//            Response.ContentType = "application/pdf"; 
//            Response.Headers.Add("content-length", stream.Length.ToString());
//            byte[] bytes = stream.ToArray();
//            stream.Close();

//            return File(bytes, "application/pdf", "Invoice,pdf"); 
//        } 
//    }
//}
