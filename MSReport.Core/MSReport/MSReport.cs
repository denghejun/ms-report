using Microsoft.Reporting.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;

namespace MSReport.Core
{
    public abstract class MSReport : MSReportBase
    {
        protected virtual List<MSReportDataSource> SetDataSources()
        {
            return null;
        }

        protected virtual List<MSReportParameter> SetParameters()
        {
            return null;
        }

        protected virtual object SetParameter()
        {
            return null;
        }

        protected virtual List<MSReport> SetSubReports()
        {
            return null;
        }

        protected sealed override List<MSReportDataSource> GetSubReportDataSources(MSReportBase subReport)
        {
            return (subReport as MSReport)?.SetDataSources();
        }

        protected sealed override void BuildReport()
        {
            base.LoadMainReport();

            this._subReports = this.SetSubReports().Cast<MSReportBase>()?.ToList();
            base.LoadSubReports(this._subReports);

            var dataSources = this.SetDataSources();
            base.SetDataSources(dataSources);

            var parameters = this.SetParameters();
            base.SetParameters(parameters);

            var parametersFromObj = this.SetParameter();
            base.SetParameter(parametersFromObj);
        }

        public void PrintToDocument(PageMargin? margin = null, short copies = 1)
        {
            base.InnerPrintToDocument(margin, copies);
        }

        public byte[] PrintToPDF(PageMargin? margin = null, short copies = 1)
        {
            return base.InnerPrintToPDF(margin, copies);
        }

        public byte[] PrintToImage(ImageFormat format, PageMargin? margin = null, short copies = 1)
        {
            return base.InnerPrintToImage(format, margin, copies);
        }
    }

    public abstract class MSReport<TReportData> : MSReportBase where TReportData : class
    {
        private TReportData _reportData;

        protected virtual List<MSReportDataSource> SetDataSources(TReportData reportData)
        {
            return null;
        }

        protected virtual List<MSReportParameter> SetParameters(TReportData reportData)
        {
            return null;
        }

        protected virtual object SetParameter(TReportData reportData)
        {
            return null;
        }

        protected virtual List<MSReport<TReportData>> SetSubReports(TReportData reportData)
        {
            return null;
        }

        protected sealed override List<MSReportDataSource> GetSubReportDataSources(MSReportBase subReport)
        {
            return (subReport as MSReport<TReportData>)?.SetDataSources(this._reportData);
        }

        protected sealed override void BuildReport()
        {
            base.LoadMainReport();

            this._subReports = this.SetSubReports(this._reportData)?.Cast<MSReportBase>().ToList();
            base.LoadSubReports(this._subReports);

            var dataSources = this.SetDataSources(this._reportData);
            base.SetDataSources(dataSources);

            var parameters = this.SetParameters(this._reportData);
            base.SetParameters(parameters);

            var parametersFromObj = this.SetParameter(this._reportData);
            base.SetParameter(parametersFromObj);
        }

        public void PrintToDocument(TReportData reportData, PageMargin? margin = null, short copies = 1)
        {
            this._reportData = reportData;
            base.InnerPrintToDocument(margin, copies);
        }

        public byte[] PrintToPDF(TReportData reportData, PageMargin? margin = null, short copies = 1)
        {
            this._reportData = reportData;
            return base.InnerPrintToPDF(margin, copies);
        }

        public byte[] PrintToImage(TReportData reportData, ImageFormat format, PageMargin? margin = null, short copies = 1)
        {
            this._reportData = reportData;
            return base.InnerPrintToImage(format, margin, copies);
        }
    }
}
