using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ExcelExport
{
    public partial class Form1 : Form
    {
        RealEstateEntities context = new RealEstateEntities();
        List<Flat> lakasok;

        public Form1()
        {
            InitializeComponent();
            LoadData();

            
        }

        public void LoadData()
        {
            lakasok = context.Flats.ToList();


        }
    }
}
