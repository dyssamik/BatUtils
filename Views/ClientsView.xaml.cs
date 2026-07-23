using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BatUtils.Models;
using BatUtils.Services;

namespace BatUtils.Views
{
    public partial class ClientsView : UserControl
    {
        public ObservableCollection<Client> Clients { get; }
            = new ObservableCollection<Client>();

        public ClientsView()
        {
            InitializeComponent();

            ClientsGrid.ItemsSource = Clients;

            ReloadClients();
        }

        private void ReloadClients()
        {
            Clients.Clear();

            foreach (Client client in ClientStorage.Load())
            {
                Clients.Add(client);
            }
        }

        private void SaveClients()
        {
            ClientStorage.Save(Clients);
        }

        private Client SelectedClient
        {
            get
            {
                return ClientsGrid.SelectedItem as Client;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ClientWindow window = new ClientWindow();

            window.Owner = Window.GetWindow(this);

            if (window.ShowDialog() != true)
                return;

            Clients.Add(window.Client);

            SaveClients();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedClient == null)
                return;

            ClientWindow window = new ClientWindow(SelectedClient);
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() != true)
                return;

            SaveClients();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedClient == null)
                return;

            if (MessageBox.Show(
                    string.Format(
                        Properties.Strings.ClientsDeleteQuestion,
                        SelectedClient.Name),
                    Properties.Strings.ClientsDeleteTitle,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            Clients.Remove(SelectedClient);

            SaveClients();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadClients();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedClient == null)
                return;

            // TODO
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedClient == null)
                return;

            // TODO
        }

        private void ExploreButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedClient == null)
                return;

            // TODO
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedClient == null)
                return;

            // TODO
        }
    }
}