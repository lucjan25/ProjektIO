using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace PIT38
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Operacja> operacje { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            operacje = new List<Operacja>()
            {
                new Operacja(true, true, false, "TSL", 20.20, 22.15, 2, 1, new DateOnly(2021,02,05), new DateOnly(2021,03,10), Waluta.USD)
            };
            lbData.DataContext = operacje;
            lbData.Items.Clear();
            foreach (object ob in operacje)
                lbData.Items.Add(ob);
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "data.xml";
                saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == true)
                {
                    FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Operacja>));
                    serializer.Serialize(fs, operacje);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Operacja> ls;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.FileName = "data.xml";
                openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == true)
                {
                    FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Operacja>));
                    ls = (List<Operacja>)serializer.Deserialize(fs);
                    fs.Close();
                    operacje.Clear();
                    foreach (Operacja operacja in ls)
                    {
                        operacje.Add(operacja);
                    }
                    lbData.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddWindow();
            if (dialog.ShowDialog() == true)
            {
                operacje.Add(dialog.operacja);
                lbData.Items.Refresh();
                lbData.Items.Clear();
                foreach (object ob in operacje)
                    lbData.Items.Add(ob);
            }
        }

        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lbData.SelectedIndex != -1)
            {
                var dialog = new AddWindow((Operacja)lbData.SelectedItem);
                if (dialog.ShowDialog() == true)
                {
                    operacje.Remove((Operacja)lbData.SelectedItem);
                    operacje.Add(dialog.operacja);
                    lbData.Items.Refresh();
                    lbData.Items.Clear();
                    foreach (object ob in operacje)
                        lbData.Items.Add(ob);
                }
            }
        }

        private void btRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lbData.SelectedItem is Operacja)
                operacje.Remove((Operacja)lbData.SelectedItem);
            lbData.Items.Refresh();
            lbData.Items.Clear();
            foreach (object ob in operacje)
                lbData.Items.Add(ob);
        }


        private void btResult_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ResultWindow(operacje); 
            if (dialog.ShowDialog() == true)
            {
                lbData.Items.Refresh();
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()) == false)
            {
                lbData.Items.Clear();
                foreach (object ob in operacje)
                {
                    if (ob.ToString().Contains(txtSearch.Text.Trim()))
                    {
                        lbData.Items.Add(ob);
                    }
                }
            }

            else if (txtSearch.Text.Trim() == "")
            {
                lbData.Items.Clear();

                foreach (object ob in operacje)
                {
                    lbData.Items.Add(ob);
                }
            }

        }
    }
}
