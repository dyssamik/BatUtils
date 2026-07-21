using System.Collections;
using System.Windows.Controls;

namespace BatUtils.Views
{
    public partial class ClientsView : UserControl
    {
        public ClientsView(IEnumerable clients)
        {
            InitializeComponent();

            ClientsGrid.ItemsSource = clients;
        }
    }
}