using Enigma.Gui;
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

namespace SzyfryGUI
{
    /// <summary>
    /// Interaction logic for EnigmaWindow.xaml
    /// </summary>
    public partial class EnigmaWindow : Window
    {
        public FormMain enigmaForm;
        public EnigmaWindow()
        {
            InitializeComponent();
            enigmaForm = new FormMain();
            enigmaForm.TopLevel = false;
            EnigmaHost.Child = enigmaForm;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

    }
}
