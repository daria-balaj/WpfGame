using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfGame.ViewModels;
using WpfGame.Services;
using NavigationService = WpfGame.Services.NavigationService;

namespace WpfGame
{
    public partial class MainWindow : Window
    {
        //public GameVM _gameVM;
        public NavigationService NavigationService { get; }
        public MainWindow()
        {
            InitializeComponent();
            //NavigationService ns = new NavigationService(Frame);
            NavigationService = new NavigationService(Frame);
            Frame.Navigate(new MenuPage(this.Frame));
            GameVM _gameVM = new GameVM();
            this.DataContext = _gameVM;
        }
    }
}