using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VaR.XSWY3L.Entities;

namespace VaR.XSWY3L
{
    public partial class Form1 : Form
    {
        PortfolioEntities context = new PortfolioEntities();
        List<Tick> Ticks;
        List<PortfolioItem> Portfolio = new List<PortfolioItem>();
        public Form1()
        {
            InitializeComponent();
            Ticks = context.Ticks.ToList();
            dataGridView1.DataSource = Ticks;

            CreatePortfolio();

            List<decimal> Nyereségek = new List<decimal>();
            int intervalum = 30;
            DateTime kezdőDátum = (from x in Ticks select x.TradingDay).Min();
            DateTime záróDátum = new DateTime(2016, 12, 30);
            TimeSpan z = záróDátum - kezdőDátum;
            for (int i = 0; i < z.Days - intervalum; i++)
            {
                decimal ny = GetPortfolioValue(kezdőDátum.AddDays(i + intervalum))
                           - GetPortfolioValue(kezdőDátum.AddDays(i));
                Nyereségek.Add(ny);
                Console.WriteLine(i + " " + ny);
            }

            var nyereségekRendezve = (from x in Nyereségek
                                      orderby x
                                      select x)
                                        .ToList();
            MessageBox.Show(nyereségekRendezve[nyereségekRendezve.Count() / 5].ToString());



            int elemszám = Portfolio.Count();
            decimal részvényekSzáma = (from x in Portfolio select x.Volume).Sum();
            //MessageBox.Show(string.Format("Részvények száma: {0}", részvényekSzáma));
            var otp = from x in Ticks
                      where x.Index.Trim().Equals("OTP")
                      select x;
            Console.WriteLine("OTP darabszám: " + otp.Count().ToString());
            var top = from o in otp
                      where o.Price > 7000
                      select o;
            Console.WriteLine("OTP darabszám nagyobb 7000: " + otp.Count().ToString());
            var topsum = (from t in top
                          select t.Price).Sum();

            DateTime minDátum = (from x in Ticks select x.TradingDay).Min();
            DateTime maxDátum = (from x in Ticks select x.TradingDay).Max();
            int elteltNapokSzáma = (maxDátum - minDátum).Days;
            Console.WriteLine((maxDátum - minDátum).ToString());


        }

        private void CreatePortfolio()
        {
            Portfolio.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });

            dataGridView2.DataSource = Portfolio;

        }


        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;
            foreach (var item in Portfolio)
            {
                var last = (from x in Ticks
                            where item.Index == x.Index.Trim()
                               && date <= x.TradingDay
                            select x)
                            .First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }
    }
}
