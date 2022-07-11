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

namespace PIT38
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(List<Operacja> operacje = null)
        {            
            InitializeComponent();
            double Revenue = 0, Cost = 0, ProfitLoss = 0, Tax = 0;
            if (operacje != null)
            {
                foreach (Operacja op in operacje)
                {
                    Revenue += Math.Round(op.GetRevenue(),2);
                    Cost += Math.Round(op.GetCost(),2);
                    ProfitLoss += Math.Round(op.GetProfitLoss(),2);
                    Tax = Math.Ceiling(ProfitLoss * 0.19);

                    tbPrzychod.Text = Math.Round(Revenue, 2).ToString() + "PLN";
                    tbKoszt.Text = Math.Round(Cost, 2).ToString() + "PLN";
                    tbPL.Text = Math.Round(ProfitLoss, 2).ToString() + "PLN";
                    tbTax.Text = Math.Ceiling(Tax).ToString() + "PLN";
                }

            }

        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
