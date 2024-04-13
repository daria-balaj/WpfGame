using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfGame.Views;

namespace WpfGame.Services
{
    public class NavigationService
    {
        private Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateToPage(string page)
        {
            switch (page)
            {
                case "NewGame":
                    _frame.Content = new GamePage();
                    break;  
                case "Statistics":
                    _frame.Content = new StatsPage();
                    break;
                //case "About":
                //    Utility.About();
                //    break;
                //case "Save":
                //    //Utility.SaveGame();
                //    break;
                //case "Quit":
                //    _frame.Content = new MenuPage(_frame);
                //    break;

            }
        }

    }
}
