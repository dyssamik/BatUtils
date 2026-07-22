using System;
using System.Windows;
using BatUtils.Models;

namespace BatUtils.Views
{
    public partial class ClientWindow : Window
    {
        public Client Client { get; private set; }

        public ClientWindow()
        {
            InitializeComponent();

            Title = "Add Client";
        }

        public ClientWindow(Client client)
            : this()
        {
            Title = "Edit Client";

            CodeTextBox.Text = client.Code;
            NameTextBox.Text = client.Name;

            RKAddressTextBox.Text = client.RKServerAddress;
            RKPortTextBox.Text = client.RKServerPort.ToString();

            SHAddressTextBox.Text = client.SHServerAddress;
            SHPortTextBox.Text = client.SHServerPort.ToString();

            Client = client;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ushort rkPort;
            ushort shPort;

            if (!ushort.TryParse(RKPortTextBox.Text, out rkPort))
            {
                MessageBox.Show("Invalid RK7 port.");
                return;
            }

            if (!ushort.TryParse(SHPortTextBox.Text, out shPort))
            {
                MessageBox.Show("Invalid SH5 port.");
                return;
            }

            if (Client == null)
                Client = new Client();

            Client.Enabled = true;
            Client.Code = CodeTextBox.Text.Trim();
            Client.Name = NameTextBox.Text.Trim();

            Client.RKServerAddress = RKAddressTextBox.Text.Trim();
            Client.RKServerPort = rkPort;

            Client.SHServerAddress = SHAddressTextBox.Text.Trim();
            Client.SHServerPort = shPort;

            DialogResult = true;
        }
    }
}