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
            var viewmodel = InitializeViewModelWithDependencies();

            var application = new App();
            application.InitializeComponent();

            var window = new UI.MainWindow(viewmodel);
            application.Run(window);
        }

        private static MainViewModel InitializeViewModelWithDependencies()
        {
            const char CSV_COLUMN_DELIMETER = ',';
            var readers = new IDataReader[]
            {
                new ExcelDataReader(),
                new CsvDataReader(CSV_COLUMN_DELIMETER),
                new JsonDataReader()
            };
            var readerProvider = new DataReaderProvider(readers);
            var dataProvider = new FileDataProvider(readerProvider);
            var calculator = new ScatterReportCalculator();
            return new MainViewModel(dataProvider, calculator);

        }
    }
}
