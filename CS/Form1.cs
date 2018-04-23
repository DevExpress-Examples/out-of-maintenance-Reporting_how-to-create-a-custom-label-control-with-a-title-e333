using System;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.XtraReports.UI;

namespace CustomControls_TitledLabel {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            // Create a report.
            XtraReport1 report = new XtraReport1();

            // Create a label with a title.
            XrTitledLabel label = new XrTitledLabel();
            label.Height = 100;
            label.Width = 150;
            label.Borders = DevExpress.XtraPrinting.BorderSide.All;
            label.BackColor = Color.Pink;
            label.ForeColor = Color.White;
            
            // Set its text properties.
            label.Text = "Some very-very-very-very-very large text";

            // Set its title properties.
            label.DisplayTitle = true;
            label.TitleText = "Title";
            label.TitleColor = Color.Blue;

            // Add a label to a report.
            report.Detail.Controls.Add(label);

            // Show the Print Preview form.
            ReportPrintTool printTool = new ReportPrintTool(report);

            printTool.ShowPreview();
        }
    }
}