using System;
using System.Windows;
using System.Windows.Controls;

namespace GraforWpfDll.UserControls
{
    /// <summary>
    /// Interaction logic for UCStripMenu.xaml
    /// </summary>
    public partial class UCStripMenu : UserControl
    {
        public UCStripMenu()
        {
            InitializeComponent();
        }

    /*
        private void Exception_Click(object sender, RoutedEventArgs e)
        {
            //throw new Exception();
            MessageBox.Show("Under construction", "Warning");
        }
        */
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("GRAFOR application", "About");
        }
    }
}
