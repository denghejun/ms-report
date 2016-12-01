using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;

namespace MSReport.Core
{
    internal class DocReportPrinter : MSReportPrinter
    {
        private readonly PrintDocument m_PrintDocument = new PrintDocument();

        private readonly List<Stream> m_Streams = new List<Stream>();

        private int m_CurrentPrintPageIndex = 1;

        public DocReportPrinter(LocalReport report) : base(report)
        {
            this.m_PrintDocument.PrintPage += PrintDocumentOnPrintPage;
        }

        protected override PrintSetting PrintSetting
        {
            get
            {
                return new PrintSetting()
                {
                    Format = "Image",
                    OutputType = "EMF"
                };
            }
        }

        public override byte[] Print(PageMargin? margin = default(PageMargin?), short copies = 1)
        {
            if (!this.m_PrintDocument.PrinterSettings.IsValid)
            {
                throw new ApplicationException("Can not find the default printer");
            }

            this.m_Streams.Clear();
            this.m_CurrentPrintPageIndex = 1;
            this.m_PrintDocument.PrinterSettings.Copies = copies > 0 ? copies : (short)1;

            this._msReport.Refresh();
            Warning[] warnings;
            this._msReport.Render(this.PrintSetting.Format, this.PrintSetting.DeviceInfo, (name, extension, encoding, mimeType, willSeek) =>
             {
                 Stream stream = new MemoryStream();
                 this.m_Streams.Add(stream);
                 return stream;
             }, out warnings);

            this.m_Streams.ForEach(stream => stream.Position = 0);
            this.m_PrintDocument.Print();
            return null;
        }

        private void PrintDocumentOnPrintPage(object sender, PrintPageEventArgs printPageEventArgs)
        {
            Metafile page = new Metafile(this.m_Streams[this.m_CurrentPrintPageIndex - 1]);
            Rectangle rectangle = new Rectangle
            (
                printPageEventArgs.PageBounds.Left - (int)printPageEventArgs.PageSettings.HardMarginX,
                printPageEventArgs.PageBounds.Top - (int)printPageEventArgs.PageSettings.HardMarginY,
                printPageEventArgs.PageBounds.Width,
                printPageEventArgs.PageBounds.Height
            );

            printPageEventArgs.Graphics.FillRectangle(Brushes.White, rectangle);
            printPageEventArgs.Graphics.DrawImage(page, rectangle);
            printPageEventArgs.HasMorePages = this.m_CurrentPrintPageIndex < this.m_Streams.Count;
            this.m_CurrentPrintPageIndex++;
        }

        protected override void DisposeManagedSource()
        {
            base.DisposeManagedSource();
            this.m_PrintDocument.Dispose();
            if (this.m_Streams != null)
            {
                this.m_Streams.ForEach(stream => stream.Dispose());
            }
        }
    }
}
