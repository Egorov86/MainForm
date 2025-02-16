using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm2
{
    public partial class Form1 : Form
    {
        private NetworkManager _networkManager;
        public Form1()
        {
            InitializeComponent();
            _networkManager = new NetworkManager();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            string ipAddress = txtIPAddress.Text;
            int port = int.Parse(txtPort.Text);
            await _networkManager.ConnectToServer(ipAddress, port);
            await ReceiveMessages();
        }
        private async Task ReceiveMessages()
        {
            while (true)
            {
                string message = await _networkManager.ReadMessage();
                if (message == "Bye")
                {
                    MessageBox.Show("Сервер отключился.");
                    break;
                }
                listBoxMessages.Items.Add($"Сервер: {message}");
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text;
            await _networkManager.SendMessage(message);
            listBoxMessages.Items.Add($"Вы: {message}");
            if (message == "Bye")
            {
                _networkManager.Stop();
                this.Close();
            }
        }
    }
}
