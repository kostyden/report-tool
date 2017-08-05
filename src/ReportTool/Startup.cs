namespace ReportTool
{
    using System;

    class Startup
    {
        [STAThread]
        static void Main()
        {
            var window = new MainWindow();

            var application = new App();
            application.Run(window);
        }
    }
}
