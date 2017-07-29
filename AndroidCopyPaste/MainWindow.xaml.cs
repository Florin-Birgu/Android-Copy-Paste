using Managed.Adb;
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

namespace AndroidCopyPaste
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Device> _devices;
        private Device _selectedDevice;
        public MainWindow()
        {
            InitializeComponent();

            //DeviceBox.SetCurrentValue(string, "All");
            _devices = GetDevices();
            RefreshDeviceBox();
        }

        public void RefreshDeviceBox()
        {
            //DeviceBox.

            DeviceBox.Items.Add("All");
            DeviceBox.SelectedValue = "All";

            foreach (var device in _devices)
            {
                DeviceBox.Items.Add(device.SerialNumber);
            }

        }
        public List<Device> GetDevices()
        {
            List<Device> devices = AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress);
            return devices;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBox box = (ComboBox) sender;
            
            if (box.SelectedIndex == 0)
                _selectedDevice = null;
            else if (box.Items.Count > 1)
            {
                _selectedDevice = _devices[box.SelectedIndex - 1];
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedDevice == null)
                foreach (var device in _devices)
                {
                    device.ExecuteShellCommand("input text \""+PasteInput.Text+"\"", NullOutputReceiver.Instance);
                }
            else
                _selectedDevice.ExecuteShellCommand("input text \"" + PasteInput.Text + "\"", NullOutputReceiver.Instance);

        }
    }
}
