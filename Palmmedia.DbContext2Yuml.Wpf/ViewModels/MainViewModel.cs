using System.Threading.Tasks;
using System.Windows.Input;
using Palmmedia.DbContext2Yuml.Core;
using Palmmedia.DbContext2Yuml.Wpf.Common;
using Palmmedia.DbContext2Yuml.Wpf.Interaction;

namespace Palmmedia.DbContext2Yuml.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IYumlGraphBuilder yumlGraphBuilder;

        private readonly IYumlGraphRenderer yumlGraphRenderer;

        private readonly IFileAccess fileAccess;

        private string file, error;

        private bool showInheritance = true;

        private GraphViewModel graph;

        public MainViewModel(
            IYumlGraphBuilder yumlGraphBuilder,
            IYumlGraphRenderer yumlGraphRenderer,
            IFileAccess fileAccess)
        {
            this.yumlGraphBuilder = yumlGraphBuilder;
            this.yumlGraphRenderer = yumlGraphRenderer;
            this.fileAccess = fileAccess;

            this.SelectFileCommand = new RelayCommand(o =>
            {
                this.File = this.fileAccess.SelectFile("*.dll");
                this.UpdateGraph();
            });
        }

        public string File
        {
            get { return this.file; }
            set { this.SetProperty(ref this.file, value); }
        }

        public bool ShowInheritance
        {
            get
            {
                return this.showInheritance;
            }

            set
            {
                this.SetProperty(ref this.showInheritance, value);
                this.UpdateGraph();
            }
        }

        public string Error
        {
            get { return this.error; }
            set { this.SetProperty(ref this.error, value); }
        }

        public GraphViewModel Graph
        {
            get { return this.graph; }
            set { this.SetProperty(ref this.graph, value); }
        }

        public ICommand SelectFileCommand { get; private set; }

        private void UpdateGraph()
        {
            this.Graph = null;
            this.Error = null;

            if (this.File == null
                || !System.IO.File.Exists(this.File))
            {
                return;
            }

            Task<string>.Factory.StartNew(() =>
            {
                return this.yumlGraphBuilder.CreateYumlGraph(this.File, this.ShowInheritance);
            })
            .ContinueWith(
                async t =>
                {
                    GraphViewModel graph = new GraphViewModel("Full Graph", this.fileAccess);
                    graph.YumlGraph = t.Result;
                    try
                    {
                        graph.YumlImage = await this.yumlGraphRenderer.RenderYumlGraphAync(t.Result);
                    }
                    catch (System.Exception ex)
                    {
                        this.Error = "Failed to create graph: " + ex.Message;
                    }

                    this.Graph = graph;
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.NotOnFaulted,
                TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
