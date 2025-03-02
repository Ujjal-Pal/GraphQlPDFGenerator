using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using GraphQL.AspNet.Interfaces.Controllers;
using System.Threading.Tasks;

public class Query : GraphController
{
    [QueryRoot("generatePdf")]
    public async Task<string> GeneratePdf()
    {
        // Call the service to generate the PDF
        var pdfService = new PdfService();
        var pdfPath = await pdfService.GeneratePdfAsync();
        return pdfPath;
    }
}
