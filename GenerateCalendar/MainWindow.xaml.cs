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

namespace GenerateCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SetupViewModels();
            SetupControls();
        }

        private void SetupViewModels()
        {
            vms.vmHeader = viewHeader.DataContext as ViewModels.vmHeader;
            vms.vmBody = viewBody.DataContext as ViewModels.vmBody;
            vms.vmFooter = viewFooter.DataContext as ViewModels.vmFooter;
        }

        private void SetupControls()
        {
            new Controls.Header();
            new Controls.Body();
            new Controls.Footer();
        }
    }
}
