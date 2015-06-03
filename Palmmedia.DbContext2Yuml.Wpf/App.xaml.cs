using System;
using System.IO;
using System.Windows;
using Palmmedia.DbContext2Yuml.Core;
using Palmmedia.DbContext2Yuml.Wpf.Interaction;
using Palmmedia.DbContext2Yuml.Wpf.ViewModels;

namespace Palmmedia.DbContext2Yuml.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args != null && args.Length > 1)
            {
                this.CreateGraphs(args[1]);
            }
            else
            {
                base.OnStartup(e);

                var mainViewModel = new MainViewModel(
                    new YumlGraphBuilder(),
                    new YumlGraphRenderer(),
                    new FormFileAccess());

                var mainWindow = new MainWindow();
                mainWindow.DataContext = mainViewModel;

                this.MainWindow = mainWindow;
                this.MainWindow.Show();
            }
        }

        private async void CreateGraphs(string pathToDll)
        {
            try
            {
                IYumlGraphBuilder builder = new YumlGraphBuilder();
                string yumlDiagramm = builder.CreateYumlGraph(pathToDll);

                IYumlGraphRenderer renderer = new YumlGraphRenderer();
                byte[] yumlGraphImage = await renderer.RenderYumlGraphAync(yumlDiagramm);
                File.WriteAllBytes("UML.png", yumlGraphImage);

                this.Shutdown();
            }
            catch (Exception)
            {
                this.Shutdown(1);
            }
        }
    }
}
