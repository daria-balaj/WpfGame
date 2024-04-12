using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using NavigationService = WpfGame.Services.NavigationService;

namespace WpfGame
{
    public partial class MenuPage : Page
    {
        public NavigationService ns { get; }
        public MenuPage(Frame frame)
        {
            InitializeComponent();
            ns = new NavigationService(((MainWindow)Application.Current.MainWindow).Frame);
            MenuPageVM viewModel = new MenuPageVM(ns);
            DataContext = viewModel;
        }
    }
}
