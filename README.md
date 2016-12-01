#### 更易用的 `MS Report`
* `MSReport.Core.dll`:报表基础框架，封装了更规范易用的 Microsoft Report对象以及多种报表打印对象；



#### 如何使用`MSReport.Core.dll`定义自己的报表
`MSReport.Core`中只有2种类型的抽象报表`MSReport`、`MSReport<TReportData>`；分别表示不依赖外部数据、依赖外部数据的报表。通过继承它们两者任意之一以实现自己的报表。
```
public class MyNoDataReport : MSReport
    {
        protected override string ReportEmbeddedResource
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override List<MSReportDataSource> SetDataSources()
        {
            throw new NotImplementedException();
        }

        protected override object SetParameter()
        {
            throw new NotImplementedException();
        }

        protected override List<MSReport> SetSubReports()
        {
            throw new NotImplementedException();
        }
    }
```

```
 public class MyHasDataReport : MSReport<ReportData>
    {
        protected override string ReportEmbeddedResource
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override List<MSReportDataSource> SetDataSources(ReportData reportData)
        {
            throw new NotImplementedException();
        }

        protected override object SetParameter(ReportData reportData)
        {
            throw new NotImplementedException();
        }

        protected override List<MSReport<ReportData>> SetSubReports(ReportData reportData)
        {
            throw new NotImplementedException();
        }
    }

    public class ReportData
    {
        public int ID { get; set; }
    }
```
* `ReportEmbeddedResource`: 只有它是被强制要求实现的；表示报表文件资源名，可以缩写为`文件夹名.报表名`（如"PackingListReport.Report.rdlc"）；
* `SetDataSources`:按`报表界面上`预先设置的DataSet Name提供对应的数据源，`Name`即是DataSet Name；
* `SetParameter`:设置`报表界面上`预先设置的`Parameters`；可以是一个Object或匿名对象，将会按照实例的所有`Public`属性（string或value type）直接映射报表中预先设置的`Parameters`；需要特别注意名字和类型需要匹配。
* `SetSubReports`:当主报表包含有子报表时，需要设置子报表，子报表的类型是和主报表匹配的。

#### 如何使用定义好的报表对象进行打印
有3种内置的打印方式：
* `void PrintToDocument`: 以普通文档形式打印到打印机
* `byte[] PrintToImage`:以指定的图片格式（`png`,`bmp`...），返回报表图片文件的byte数组
* `byte[] PrintToPDF`:以`pdf`格式打印，返回报表`pdf`文件的byte数组

对于返回文件byte数组的打印方式，可采用以下方式将字节流写入磁盘文件：
```
 var bytes = MSReportProxy<PackingListReport>.Report.PrintToPDF(reportData, copies: (short)copies);
 File.WriteAllBytes("C://PackListReport.pdf", bytes);
```