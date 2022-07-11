using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public Operacja operacja;
        public AddWindow(Operacja operacja = null)
        {
            InitializeComponent();
            cbxCurrency.ItemsSource = Enum.GetValues(typeof(Waluta));
            if (operacja != null)
            {
                if (operacja.OnlyCost)
                {
                    rbCost.IsChecked = true;
                    
                    tbTicker.Text = operacja.Ticker;
                    tbOpen.Text = operacja.OpenPrice.ToString();
                    tbClose.Text = operacja.ClosePrice.ToString();
                    tbAmount.Text = operacja.Amount.ToString();

                    dpClose.DisplayDate = operacja.CloseTime.ToDateTime(new TimeOnly(0, 0));
                    cbxCurrency.SelectedIndex = (int)operacja.Currency;

                    tbLeverage.Text = operacja.Leverage.ToString();
                }
                else if (operacja.Closed)
                {
                    rbClosed.IsChecked = true;
                    if (operacja.Buy)
                        rbBuy.IsChecked = true;
                    else
                        rbSell.IsChecked = true;
                    tbTicker.Text = operacja.Ticker;
                    tbOpen.Text = operacja.OpenPrice.ToString();
                    tbClose.Text = operacja.ClosePrice.ToString();
                    tbAmount.Text = operacja.Amount.ToString();
                    
                    dpOpen.DisplayDate = operacja.OpenTime.ToDateTime(new TimeOnly(0, 0));
                    dpClose.DisplayDate = operacja.CloseTime.ToDateTime(new TimeOnly(0, 0));
                    cbxCurrency.SelectedIndex = (int)operacja.Currency;

                    tbLeverage.Text = operacja.Leverage.ToString();
                }
                else
                {
                    rbOpen.IsChecked = true;
                    if (operacja.Buy)
                        rbBuy.IsChecked = true;
                    else
                        rbSell.IsChecked = true;
                    tbTicker.Text = operacja.Ticker;
                    tbOpen.Text = operacja.OpenPrice.ToString();
                    tbClose.Text = operacja.ClosePrice.ToString();
                    tbAmount.Text = operacja.Amount.ToString();

                    dpOpen.DisplayDate = operacja.OpenTime.ToDateTime(new TimeOnly(0, 0));
                    dpClose.DisplayDate = operacja.CloseTime.ToDateTime(new TimeOnly(0, 0));
                    cbxCurrency.SelectedIndex = (int)operacja.Currency;

                    tbLeverage.Text = operacja.Leverage.ToString();
                }                
            }
            this.operacja = operacja ?? new Operacja();
        }

        private void btZapisz_Click(object sender, RoutedEventArgs e)
        {
            Kurs kursy = new Kurs();
            if (rbCost.IsChecked == true)
            {
                if (!Regex.IsMatch(tbClose.Text, @"^[0-9]*(\,[0-9]{1,2})?$") ||
                !Regex.IsMatch(tbAmount.Text, @"^[0-9]{1,6}$"))
                {
                    MessageBox.Show("Podano błędne dane.");
                    return;
                }

                operacja.ClosePrice = double.Parse(tbClose.Text);
                operacja.CloseTime = DateOnly.FromDateTime(dpClose.DisplayDate);
                operacja.OpenTime = operacja.CloseTime;
                operacja.Amount = int.Parse(tbAmount.Text);

                operacja.Currency = (Waluta)cbxCurrency.SelectedIndex;
                switch (operacja.Currency)
                {
                    case Waluta.PLN:
                        operacja.OpenKurs = 1.0;
                        operacja.CloseKurs = 1.0;
                        break;
                    case Waluta.USD:
                        operacja.OpenKurs = kursy.KursUSD[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursUSD[operacja.CloseTime];
                        break;
                    case Waluta.EUR:
                        operacja.OpenKurs = kursy.KursEUR[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursEUR[operacja.CloseTime];
                        break;
                    case Waluta.GBP:
                        operacja.OpenKurs = kursy.KursGBP[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursGBP[operacja.CloseTime];
                        break;
                    case Waluta.CAD:
                        operacja.OpenKurs = kursy.KursCAD[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursCAD[operacja.CloseTime];
                        break;
                    case Waluta.CHF:
                        operacja.OpenKurs = kursy.KursCHF[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursCHF[operacja.CloseTime];
                        break;
                    case Waluta.CNY:
                        operacja.OpenKurs = kursy.KursCNY[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursCNY[operacja.CloseTime];
                        break;
                }

                if (cbLeverage.IsChecked == true && Regex.IsMatch(tbAmount.Text, @"^[0-9]{1,6}$"))
                {
                    operacja.Leverage = int.Parse(tbLeverage.Text);
                }
                
                operacja.Closed = false;
                operacja.Buy = false;
                operacja.OnlyCost = true;
            }
            else if (rbClosed.IsChecked == true)
            {
                if (!Regex.IsMatch(tbClose.Text, @"^[0-9]*(\,[0-9]{1,2})?$") ||
                !Regex.IsMatch(tbAmount.Text, @"^[0-9]{1,6}$") ||
                !Regex.IsMatch(tbOpen.Text, @"^[0-9]*(\,[0-9]{1,2})?$") ||
                !Regex.IsMatch(tbTicker.Text, @"^\p{Lu}{1,12}$"))
                {
                    MessageBox.Show("Podano błędne dane.");
                    return;
                }
                
                if (rbBuy.IsChecked == true)
                    operacja.Buy = true;
                else
                    operacja.Buy = false;
                
                operacja.Ticker = tbTicker.Text;
                operacja.ClosePrice = double.Parse(tbClose.Text);
                operacja.OpenPrice = double.Parse(tbOpen.Text);
                operacja.CloseTime = DateOnly.FromDateTime(dpClose.DisplayDate);
                operacja.OpenTime = DateOnly.FromDateTime(dpOpen.DisplayDate);
                operacja.Amount = int.Parse(tbAmount.Text);
                operacja.Currency = (Waluta)cbxCurrency.SelectedIndex;
                switch (operacja.Currency)
                {
                    case Waluta.PLN:
                        operacja.OpenKurs = 1.0;
                        operacja.CloseKurs = 1.0;
                        break;
                    case Waluta.USD:
                        operacja.OpenKurs = kursy.KursUSD[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursUSD[operacja.CloseTime];
                        break;
                    case Waluta.EUR:
                        operacja.OpenKurs = kursy.KursEUR[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursEUR[operacja.CloseTime];
                        break;
                    case Waluta.GBP:
                        operacja.OpenKurs = kursy.KursGBP[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursGBP[operacja.CloseTime];
                        break;
                    case Waluta.CAD:
                        operacja.OpenKurs = kursy.KursCAD[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursCAD[operacja.CloseTime];
                        break;
                    case Waluta.CHF:
                        operacja.OpenKurs = kursy.KursCHF[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursCHF[operacja.CloseTime];
                        break;
                    case Waluta.CNY:
                        operacja.OpenKurs = kursy.KursCNY[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursCNY[operacja.CloseTime];
                        break;
                }

                if (cbLeverage.IsChecked == true && Regex.IsMatch(tbAmount.Text, @"^[0-9]{1,6}$"))
                {
                    operacja.Leverage = int.Parse(tbLeverage.Text);
                }
                operacja.Closed = true;
                operacja.OnlyCost = false;
            }
            else if (rbOpen.IsChecked == true)
            {
                if (!Regex.IsMatch(tbAmount.Text, @"^[0-9]{1,6}$") ||
                !Regex.IsMatch(tbOpen.Text, @"^[0-9]*(\,[0-9]{1,2})?$") ||
                !Regex.IsMatch(tbTicker.Text, @"^\p{Lu}{1,12}$"))
                {
                    MessageBox.Show("Podano błędne dane.");
                    return;
                }

                if (rbBuy.IsChecked == true)
                    operacja.Buy = true;
                else
                    operacja.Buy = false;

                operacja.Ticker = tbTicker.Text;
                operacja.OpenPrice = double.Parse(tbOpen.Text);
                operacja.OpenTime = DateOnly.FromDateTime(dpOpen.DisplayDate);
                operacja.Amount = int.Parse(tbAmount.Text);
                operacja.Currency = (Waluta)cbxCurrency.SelectedIndex;
                switch (operacja.Currency)
                {
                    case Waluta.PLN:
                        operacja.OpenKurs = 1.0;
                        operacja.CloseKurs = 1.0;
                        break;
                    case Waluta.USD:
                        operacja.OpenKurs = kursy.KursUSD[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursUSD[operacja.CloseTime];
                        break;
                    case Waluta.EUR:
                        operacja.OpenKurs = kursy.KursEUR[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursEUR[operacja.CloseTime];
                        break;
                    case Waluta.GBP:
                        operacja.OpenKurs = kursy.KursGBP[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursGBP[operacja.CloseTime];
                        break;
                    case Waluta.CAD:
                        operacja.OpenKurs = kursy.KursCAD[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursCAD[operacja.CloseTime];
                        break;
                    case Waluta.CHF:
                        operacja.OpenKurs = kursy.KursCHF[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursCHF[operacja.CloseTime];
                        break;
                    case Waluta.CNY:
                        operacja.OpenKurs = kursy.KursCNY[operacja.OpenTime];
                        operacja.CloseKurs = kursy.KursCNY[operacja.CloseTime];
                        break;
                }

                if (cbLeverage.IsChecked == true && Regex.IsMatch(tbAmount.Text, @"^[0-9]{1,6}$"))
                {
                    operacja.Leverage = int.Parse(tbLeverage.Text);
                }
                operacja.Closed = false;
                operacja.OnlyCost = false;
            }
            else
            {
                MessageBox.Show("Podano błędne dane.");
                return;
            }

            this.DialogResult = true;
        }

        private void btCofnij_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cbLeverage_Checked(object sender, RoutedEventArgs e)
        {
            tbLeverage.IsEnabled = !tbLeverage.IsEnabled;
        }
        private void cbLeverage_Unchecked(object sender, RoutedEventArgs e)
        {
            tbLeverage.IsEnabled = !tbLeverage.IsEnabled;
        }

        private void rbOpen_Checked(object sender, RoutedEventArgs e)
        {
            dpClose.IsEnabled = false;
            tbClose.IsEnabled = false;
            dpOpen.IsEnabled = true;
            tbOpen.IsEnabled = true;
            tbTicker.IsEnabled = true;
            rbBuy.IsEnabled = true;
            rbSell.IsEnabled = true;
            tbAmount.IsEnabled = true;
        }

        private void rbClosed_Checked(object sender, RoutedEventArgs e)
        {
            dpClose.IsEnabled = true;
            tbClose.IsEnabled = true;
            dpOpen.IsEnabled = true;
            tbOpen.IsEnabled = true;
            tbTicker.IsEnabled = true;
            rbBuy.IsEnabled = true;
            rbSell.IsEnabled = true;
            tbAmount.IsEnabled = true;
        }

        private void rbCost_Checked(object sender, RoutedEventArgs e)
        {
            dpOpen.IsEnabled = false;
            tbOpen.IsEnabled = false;
            tbTicker.IsEnabled = false;
            rbBuy.IsEnabled = false;
            rbSell.IsEnabled = false;
            tbAmount.IsEnabled = true;
        }
    }
}
