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
                new ExcelDataReader()
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

    class TestDataProvider : IDataProvider
    {
        public DataResult GetFrom(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                var data = new[]
                {
                    new Dictionary<string, double> {{ "Age", 61 }, { "Height", 1.40714182621643 }, { "Weight", 46.4006415878362 }},
                    new Dictionary<string, double> {{ "Age", 75 }, { "Height", 1.67869994337439 }, { "Weight", 57.573779998469 }},
                    new Dictionary<string, double> {{ "Age", 26 }, { "Height", 1.86521975662936 }, { "Weight", 66.3872632069399 }},
                    new Dictionary<string, double> {{ "Age", 65 }, { "Height", 1.96665785281196 }, { "Weight", 71.5699081336925 }},
                    new Dictionary<string, double> {{ "Age", 28 }, { "Height", 1.48216295762807 }, { "Weight", 49.2907604395303 }},
                    new Dictionary<string, double> {{ "Age", 39 }, { "Height", 1.99740364334163 }, { "Weight", 73.1949508591255 }},
                    new Dictionary<string, double> {{ "Age", 75 }, { "Height", 1.70623376395074 }, { "Weight", 58.8164487632733 }},
                    new Dictionary<string, double> {{ "Age", 73 }, { "Height", 2.03175075698296 }, { "Weight", 75.0401485133442 }},
                    new Dictionary<string, double> {{ "Age", 72 }, { "Height", 1.80436778307552 }, { "Weight", 63.4099079546782 }},
                    new Dictionary<string, double> {{ "Age", 38 }, { "Height", 1.9755330930387 }, { "Weight", 72.0364133558806 }},
                    new Dictionary<string, double> {{ "Age", 73 }, { "Height", 1.96045240597283 }, { "Weight", 71.2449818144619 }},
                    new Dictionary<string, double> {{ "Age", 61 }, { "Height", 1.96944177975045 }, { "Weight", 71.716012317688 }},
                    new Dictionary<string, double> {{ "Age", 42 }, { "Height", 1.81474293372797 }, { "Weight", 63.9105588735412 }},
                    new Dictionary<string, double> {{ "Age", 26 }, { "Height", 1.54751458303522 }, { "Weight", 51.9306851294225 }},
                    new Dictionary<string, double> {{ "Age", 21 }, { "Height", 1.7123632317668 }, { "Weight", 59.0958378334246 }},
                    new Dictionary<string, double> {{ "Age", 48 }, { "Height", 2.00726684912083 }, { "Weight", 73.7216027143931 }},
                    new Dictionary<string, double> {{ "Age", 47 }, { "Height", 1.97260444582274 }, { "Weight", 71.8822439957284 }},
                    new Dictionary<string, double> {{ "Age", 55 }, { "Height", 1.51138467766058 }, { "Weight", 50.4571152515624 }},
                    new Dictionary<string, double> {{ "Age", 36 }, { "Height", 1.47350087468717 }, { "Weight", 48.9493977027179 }},
                    new Dictionary<string, double> {{ "Age", 37 }, { "Height", 2.01740905141609 }, { "Weight", 74.2658570764742 }}
                };

                return DataResult.CreateSuccessful(data);
            }

            return DataResult.CreateFailed(filePath);
        }
    }
}
