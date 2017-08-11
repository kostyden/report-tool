namespace ReportTool
{
    using ReportTool.DataProviders;
    using ReportTool.DataProviders.FileDataProviders;
    using ReportTool.DataProviders.FileDataProviders.DataReaders;
    using ReportTool.Reports.Calculators;
    using ReportTool.UI.ViewModels;
    using System;

    class Startup
    {
        [STAThread]
        static void Main()
        {
            var readers = new IDataReader[]
            {
                new ExcelDataReader(),
                new CsvDataReader(','),
                new JsonDataReader()
            };
            var readerProvider = new DataReaderProvider(readers);
            var dataProvider = new FileDataProvider(readerProvider);
            var calculator = new ScatterReportCalculator();
            var viewmodel = new MainViewModel(dataProvider, calculator);

            var application = new App();
            application.InitializeComponent();

            var window = new UI.MainWindow(viewmodel);
            application.Run(window);
        }
    }
}
