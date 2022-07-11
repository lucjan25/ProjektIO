using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIT38
{
    
    public class Operacja
    {
        Kurs kursy = new Kurs();
        public bool Closed { get; set; }
        public bool Buy { get; set; }
        public bool OnlyCost { get; set; }
        public string Ticker { get; set; }
        public double OpenPrice { get; set; }
        public double ClosePrice { get; set; }
        public int Amount { get; set; }
        public int Leverage { get; set; }
        public DateOnly OpenTime { get; set; }
        public DateOnly CloseTime { get; set; }
        public Waluta Currency { get; set; }
        public double OpenKurs { get; set; }
        public double CloseKurs { get; set; }


        public Operacja(bool closed, bool buy, bool onlycost, string ticker, double openprice, double closeprice,
            int amount, int leverage, DateOnly opentime, DateOnly closetime, Waluta currency)
        {
            Closed = closed;
            Buy = buy;
            OnlyCost = onlycost;
            Ticker = ticker;
            OpenPrice = openprice;
            ClosePrice = closeprice;
            Amount = amount;
            Leverage = leverage;
            OpenTime = opentime;
            CloseTime = closetime;
            Currency = currency;
            switch (Currency)
            { 
                case Waluta.PLN:
                    OpenKurs = 1.0;
                    CloseKurs = 1.0;
                    break;
                case Waluta.USD:
                    OpenKurs = kursy.KursUSD[OpenTime];
                    CloseKurs = kursy.KursUSD[CloseTime];
                    break;
                case Waluta.EUR:
                    OpenKurs = kursy.KursEUR[OpenTime];
                    CloseKurs = kursy.KursEUR[CloseTime];
                    break;
                case Waluta.GBP:
                    OpenKurs = kursy.KursGBP[OpenTime];
                    CloseKurs = kursy.KursGBP[CloseTime];
                    break;
                case Waluta.CAD:
                    OpenKurs = kursy.KursCAD[OpenTime];
                    CloseKurs = kursy.KursCAD[CloseTime];
                    break;
                case Waluta.CHF:
                    OpenKurs = kursy.KursCHF[OpenTime];
                    CloseKurs = kursy.KursCHF[CloseTime];
                    break;
                case Waluta.CNY:
                    OpenKurs = kursy.KursCNY[OpenTime];
                    CloseKurs = kursy.KursCNY[CloseTime];
                    break;
            }
        }
        public Operacja()
            : this(false, false, true, "", 0.0, 0.0, 0, 1, new DateOnly(2021, 01, 01), new DateOnly(2021, 01, 01), Waluta.PLN)
                { }
            public double GetProfitLoss()
        {
            if (OnlyCost)
            {
                return -GetCost();
            }
            else if (Closed)
            {
                return GetRevenue() - GetCost();
            }
            else return 0.0;
        }
        public double GetCost()
        {
            if (OnlyCost)
            {
                return (Amount * ClosePrice * CloseKurs);
            }
            else if (Closed && Buy)
                return (Amount * OpenPrice * OpenKurs);
            else if (Closed && !Buy)
                return (Amount * ClosePrice * CloseKurs);
            else return 0.0;
        }
        public double GetRevenue()
        {
            if (Closed && Buy)
                return (Leverage * Amount * ClosePrice * CloseKurs);
            else if (Closed && !Buy)
                return (Leverage * Amount * OpenPrice * OpenKurs);
            else return 0.0;
        }
        public override string ToString()
        {
            string s = "";
            if (OnlyCost)
            {
                s = String.Format("{0}: -{1}", CloseTime.ToString(), Math.Round(GetCost(), 2));
            }
            else if (Closed)
            {
                string b;
                if (Buy)
                    b = "buy";
                else
                    b = "sell";
                s = String.Format("{0} - {1}-{2}, {3}: {4} szt. Cena otwarcia {5} {6}, Cena zamknięcia {7} {8}", 
                    Ticker, OpenTime, CloseTime, b, Amount, Math.Round(OpenPrice, 2), Currency, Math.Round(ClosePrice, 2), Currency);
            }
            else
            {
                string b;
                if (Buy)
                    b = "buy";
                else
                    b = "sell";
                s = String.Format("{0} - {1}-{2}, {3}: {4} szt. Cena otwarcia {5} {6}, Cena zamknięcia {7} {8}",
                    Ticker, OpenTime, b, Amount, OpenPrice, Currency);
            }
            return s;
        }
    }
    public enum Waluta
    { 
        PLN,
        USD,
        EUR,
        GBP,
        CAD,
        CHF,
        CNY
    }
}
