using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WpfGame.Commands;
using NavigationService = WpfGame.Services.NavigationService;

namespace WpfGame.ViewModels
{
    internal class MenuPageVM
    {
        public NavigationService _navService { get; }

        public MenuPageVM(NavigationService ns)
        {
            _navService = ns;
        }

        public ICommand NavigateToPageCommand => new RelayCommand<string>(NavigateToPage);

        private void NavigateToPage(string page)
        {
            _navService.NavigateToPage(page);
        }
    }
}
