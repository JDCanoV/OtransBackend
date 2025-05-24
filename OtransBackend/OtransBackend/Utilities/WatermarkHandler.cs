// WatermarkHandler.cs
using iText.Commons.Actions;
using iText.IO.Font.Constants;      // StandardFonts
using iText.Kernel.Font;            // PdfFontFactory
using iText.Kernel.Pdf.Canvas;      // PdfCanvas
using iText.Kernel.Pdf.Event;
using iText.Kernel.Pdf.Extgstate;   // PdfExtGState

namespace OtransBackend.Utilities
{
    /// <summary>
    /// Inserta una marca de agua semitransparente en cada página.
    /// </summary>
    public class WatermarkHandler/* : IEventHandler*/
    {
        readonly string _watermark;
        public WatermarkHandler(string watermark) => _watermark = watermark;

        //public void HandleEvent(/*Event*/ currentEvent)
        //{
        //    var pdfEvent = (PdfDocumentEvent)currentEvent;
        //    var page = pdfEvent.GetPage();
        //    var canvas = new PdfCanvas(
        //        page.NewContentStreamBefore(),
        //        page.GetResources(),
        //        pdfEvent.GetDocument()
        //    );

        //    // Semitransparencia
        //    var gs = new PdfExtGState().SetFillOpacity(0.1f);
        //    canvas.SaveState().SetExtGState(gs);

        //    // Texto centrado
        //    var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        //    canvas.BeginText()
        //          .SetFontAndSize(font, 60)
        //          .MoveText(
        //            (page.GetPageSize().GetWidth() - 200) / 2,
        //            (page.GetPageSize().GetHeight() - 60) / 2
        //          )
        //          .ShowText(_watermark)
        //          .EndText()
        //          .RestoreState();
        //}
    }
}
