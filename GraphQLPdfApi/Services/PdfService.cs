using DinkToPdf;
using DinkToPdf.Contracts;
using SkiaSharp;
using System;
using System.IO;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;

public class PdfService
{
    private readonly IConverter _converter;

    public PdfService()
    {
        _converter = new SynchronizedConverter(new PdfTools());
    }

    public async Task<byte[]> GeneratePdfAsync()
    {
        var htmlContent = GenerateHtmlContent();
        var pdfDocument = new HtmlToPdfDocument
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            },
            Objects = {
                new ObjectSettings {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
        };

        var pdfBytes = _converter.Convert(pdfDocument);

        return pdfBytes;
    }

    private string GenerateHtmlContent()
    {
        // Generate dot chart using SkiaSharp
        var barChartImage = GenerateBarChart();
        var barChartImage2 = GenerateBarChart2();
        var lineChartImage = GenerateLineChart();
        var lineChartImage2 = GenerateLineChart2();
        var chartImage = GenerateDotChart();
        var barChartBase64 = Convert.ToBase64String(barChartImage);
        var barChartBase642 = Convert.ToBase64String(barChartImage2);
        var lineChartBase64 = Convert.ToBase64String(lineChartImage);
        var lineChartBase642 = Convert.ToBase64String(lineChartImage2);
        var chartBase64 = Convert.ToBase64String(chartImage);

        // HTML content with table and chart
        return $@"
            <html>
            <head>
                <style>
                    table {{
                        width: 100%;
                        border-collapse: collapse;
                    }}
                    table, th, td {{
                        border: 1px solid black;
                    }}
                    th, td {{
                        padding: 8px;
                        text-align: left;
                    }}
                </style>
            </head>
            <body>
                <h1>Data Table</h1>
                <table>
                    <tr>
                        <th>Column 1</th>
                        <th>Column 2</th>
                    </tr>
                    <tr>
                        <td>Data 1</td>
                        <td>Data 2</td>
                    </tr>
                    <tr>
                        <td>Data 3</td>
                        <td>Data 4</td>
                    </tr>
                </table>
                <h1>Dot Chart</h1>
                <img src='data:image/png;base64,{chartBase64}' />
                <h1>Line Chart</h1>
                <img src='data:image/png;base64,{lineChartBase64}' />
                <h1>Bar Chart</h1>
                <img src='data:image/png;base64,{barChartBase64}' />
                <h1>Line Chart 2</h1>
                <img src='data:image/png;base64,{lineChartBase642}' />
                <h1>Bar Chart 2</h1>
                <img src='data:image/png;base64,{barChartBase642}' />                    
            </body>
            </html>";
    }

    private byte[] GenerateBarChart()
    {
        using var bitmap = new SKBitmap(400, 300);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        var paint = new SKPaint
        {
            Color = SKColors.Blue,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        // Draw bars
        canvas.DrawRect(new SKRect(50, 200, 100, 50), paint);
        canvas.DrawRect(new SKRect(150, 200, 200, 100), paint);
        canvas.DrawRect(new SKRect(250, 200, 300, 75), paint);

        // Draw axes
        var axisPaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2
        };

        canvas.DrawLine(40, 200, 350, 200, axisPaint); // X-axis
        canvas.DrawLine(40, 200, 40, 40, axisPaint); // Y-axis

        // Draw axis labels
        var textPaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            TextSize = 16
        };

        canvas.DrawText("X-Axis", 180, 220, textPaint);
        canvas.DrawText("Y-Axis", 10, 30, textPaint);

        // Draw axis markings
        for (int i = 0; i <= 5; i++)
        {
            canvas.DrawLine(35, 200 - i * 30, 45, 200 - i * 30, axisPaint);
            canvas.DrawText((i * 20).ToString(), 10, 205 - i * 30, textPaint);
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    private byte[] GenerateLineChart()
    {
        using var bitmap = new SKBitmap(400, 300);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        var paint = new SKPaint
        {
            Color = SKColors.Red,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2
        };

        // Draw lines
        var path = new SKPath();
        path.MoveTo(50, 200);
        path.LineTo(100, 150);
        path.LineTo(150, 175);
        path.LineTo(200, 125);
        path.LineTo(250, 150);
        path.LineTo(300, 100);
        canvas.DrawPath(path, paint);

        // Draw axes
        var axisPaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2
        };

        canvas.DrawLine(40, 200, 350, 200, axisPaint); // X-axis
        canvas.DrawLine(40, 200, 40, 40, axisPaint); // Y-axis

        // Draw axis labels
        var textPaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            TextSize = 16
        };

        canvas.DrawText("X-Axis", 180, 220, textPaint);
        canvas.DrawText("Y-Axis", 10, 30, textPaint);

        // Draw axis markings
        for (int i = 0; i <= 5; i++)
        {
            canvas.DrawLine(35, 200 - i * 30, 45, 200 - i * 30, axisPaint);
            canvas.DrawText((i * 20).ToString(), 10, 205 - i * 30, textPaint);
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    private byte[] GenerateDotChart()
    {
        using var bitmap = new SKBitmap(400, 300);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        var paint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        // Draw dots
        canvas.DrawCircle(100, 100, 2, paint);
        canvas.DrawCircle(200, 100, 2, paint);
        canvas.DrawCircle(300, 100, 2, paint);

        // Draw axes
        var axisPaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2
        };

        canvas.DrawLine(40, 200, 350, 200, axisPaint); // X-axis
        canvas.DrawLine(40, 200, 40, 40, axisPaint); // Y-axis

        // Draw axis labels
        var textPaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            TextSize = 16
        };

        canvas.DrawText("X-Axis", 180, 220, textPaint);
        canvas.DrawText("Y-Axis", 10, 30, textPaint);

        // Draw axis markings
        for (int i = 0; i <= 5; i++)
        {
            canvas.DrawLine(35, 200 - i * 30, 45, 200 - i * 30, axisPaint);
            canvas.DrawText((i * 20).ToString(), 10, 205 - i * 30, textPaint);
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    private byte[] GenerateBarChart2()
    {
        var model = new PlotModel { Title = "Bar Chart" };
        var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom };
        categoryAxis.Labels.Add("Category 1");
        categoryAxis.Labels.Add("Category 2");
        categoryAxis.Labels.Add("Category 3");
        model.Axes.Add(categoryAxis);

        var valueAxis = new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100 };
        model.Axes.Add(valueAxis);

        var series = new BarSeries();
        series.Items.Add(new BarItem(50));
        series.Items.Add(new BarItem(70));
        series.Items.Add(new BarItem(30));
        model.Series.Add(series);

        return RenderPlotModelToPng(model);
    }

    private byte[] GenerateLineChart2()
    {
        var model = new PlotModel { Title = "Line Chart" };
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-Axis" });
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-Axis" });

        var series = new LineSeries();
        series.Points.Add(new DataPoint(0, 0));
        series.Points.Add(new DataPoint(1, 10));
        series.Points.Add(new DataPoint(2, 20));
        series.Points.Add(new DataPoint(3, 15));
        series.Points.Add(new DataPoint(4, 25));
        model.Series.Add(series);
        
        var series2 = new LineSeries();
        series2.Points.Add(new DataPoint(0, 10));
        series2.Points.Add(new DataPoint(1, 1));
        series2.Points.Add(new DataPoint(22, 20));
        series2.Points.Add(new DataPoint(33, 13));
        series2.Points.Add(new DataPoint(41, 25));
        model.Series.Add(series2);

        return RenderPlotModelToPng(model);
    }

    private byte[] RenderPlotModelToPng(PlotModel model)
    {
        using var stream = new MemoryStream();
        var exporter = new PngExporter { Width = 600, Height = 400};
        exporter.Export(model, stream);
        return stream.ToArray();
    }
}
