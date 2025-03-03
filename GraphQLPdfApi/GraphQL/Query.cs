using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using GraphQL.AspNet.Interfaces.Controllers;

public class Query : GraphController
{
    [QueryRoot("generatePdfAsFile")]
    public async Task<string> GeneratePdfAsFile()
    {
        // Call the service to generate the PDF
        var pdfService = new PdfService();
        var pdfBytes = await pdfService.GeneratePdfAsync();
        var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedPdf.pdf");
        await File.WriteAllBytesAsync(pdfPath, pdfBytes);
        return pdfPath;
    }

    [QueryRoot("generatePdfAsStream",typeof(PdfStreamResponse))]
    public async Task<IGraphActionResult> GeneratePdfAsStream()
    {
        //Call the service to generate the PDF
        var pdfService = new PdfService();
        var pdf = await pdfService.GeneratePdfAsync();
        
        var pdfFileStream = new PdfStreamResponse()
        {
            FileName = "large_document.pdf",
            FileContent = pdf
        };

        return this.Ok(pdfFileStream);
    }

    public class PdfStreamResponse
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}
