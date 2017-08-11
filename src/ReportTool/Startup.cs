namespace ReportTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

            var window = new MainWindow(viewmodel);
            application.Run(window);
        }
    }
}
