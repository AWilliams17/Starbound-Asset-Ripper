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
using System.Windows.Shapes;

namespace Starbound_Asset_Ripper.Windows
{
    /// <summary>
    /// Interaction logic for UnpackWindow.xaml
    /// </summary>
    public partial class UnpackWindow : Window
    {
        private string _steamPath;
        private string _outputPath;
        private Dictionary<string, string[]> _targetPaksDict;

        public UnpackWindow(string SteamPath, string OutputPath, Dictionary<string, string[]>PaksToUnpack)
        {
            InitializeComponent();
            _steamPath = SteamPath;
            _outputPath = OutputPath;
            _targetPaksDict = PaksToUnpack;
        }

        private void Unpack()
        {
            foreach (KeyValuePair<string, string[]>kvp in _targetPaksDict)
            {

            }
        }
    }
}
