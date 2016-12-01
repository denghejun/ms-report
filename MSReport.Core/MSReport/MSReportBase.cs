using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSReport.Core
{
    public abstract class MSReportBase
    {
        protected List<MSReportBase> _subReports;

        protected readonly LocalReport _innerReport = new LocalReport();

        public MSReportBase()
        {
            this._innerReport.SubreportProcessing += InnerReport_SubreportProcessing;
        }

        private void InnerReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (this._subReports?.Count > 0)
            {
                var foundSubReport = this._subReports.FirstOrDefault(o => e.ReportPath.Equals(o.ReportEmbeddedResource?.Trim()));
                if (foundSubReport == null)
                {
                    foundSubReport = this._subReports.FirstOrDefault(o => e.ReportPath.EndsWith("." + o.ReportEmbeddedResource?.Trim()));
                }

                if (foundSubReport != null)
                {
                    var dataSources = this.GetSubReportDataSources(foundSubReport);
                    if (dataSources?.Count > 0)
                    {
                        foreach (var dataSource in dataSources)
                        {
                            e.DataSources.Add(new ReportDataSource() { Name = dataSource.Name, Value = dataSource.Value });
                        }
                    }
                }
            }
        }

        protected abstract string ReportEmbeddedResource { get; }

        protected abstract List<MSReportDataSource> GetSubReportDataSources(MSReportBase subReport);

        protected abstract void BuildReport();

        protected void LoadMainReport()
        {
            this._innerReport.LoadReportDefinition(Utility.ToStream(this.ReportEmbeddedResource, this.GetType()));
        }

        protected void LoadSubReports(List<MSReportBase> subReports)
        {
            if (subReports?.Count > 0)
            {
                foreach (var report in subReports)
                {
                    var fullEmbeddedResourceName = string.Empty;
                    var stream = Utility.ToStream(report.ReportEmbeddedResource, this.GetType(), out fullEmbeddedResourceName);
                    this._innerReport.LoadSubreportDefinition(fullEmbeddedResourceName, stream);
                }
            }
        }

        protected void SetDataSources(List<MSReportDataSource> dataSources)
        {
            if (dataSources?.Count > 0)
            {
                foreach (var dataSource in dataSources)
                {
                    this._innerReport.DataSources.Add(new ReportDataSource() { Name = dataSource.Name, Value = dataSource.Value });
                }
            }
        }

        protected void SetParameters(List<MSReportParameter> parameters)
        {
            if (parameters?.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    this._innerReport.SetParameters(new ReportParameter(parameter.Name, parameter.Value));
                }
            }
        }

        protected void SetParameter(object obj)
        {
            if (obj != null)
            {
                foreach (var p in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (p.PropertyType == typeof(string) || p.PropertyType.IsValueType)
                    {
                        this._innerReport.SetParameters(new ReportParameter(p.Name, p.GetValue(obj)?.ToString()));
                    }
                }
            }
        }

        protected void InnerPrintToDocument(PageMargin? margin = null, short copies = 1)
        {
            this.BuildReport();
            using (var printer = new DocReportPrinter(this._innerReport))
            {
                printer.Print(margin, copies);
            }
        }

        protected byte[] InnerPrintToPDF(PageMargin? margin = null, short copies = 1)
        {
            this.BuildReport();
            using (var printer = new PDFReportPrinter(this._innerReport))
            {
                return printer.Print(margin, copies);
            }
        }

        protected byte[] InnerPrintToImage(ImageFormat format, PageMargin? margin = null, short copies = 1)
        {
            this.BuildReport();
            using (var printer = new ImageReportPrinter(format, this._innerReport))
            {
                return printer.Print(margin, copies);
            }
        }
    }
}
