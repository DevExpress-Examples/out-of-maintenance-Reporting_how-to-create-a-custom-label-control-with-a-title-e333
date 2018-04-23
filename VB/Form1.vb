Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Drawing

Namespace CustomControls_TitledLabel
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			' Create a report.
			Dim report As New XtraReport1()

			' Create a label with a title.
			Dim label As New XrTitledLabel()
			label.Height = 100
			label.Width = 150
			label.Borders = DevExpress.XtraPrinting.BorderSide.All
			label.BackColor = Color.Pink
			label.ForeColor = Color.White

			' Set its text properties.
			label.Text = "Some very-very-very-very-very large text"

			' Set its title properties.
			label.DisplayTitle = True
			label.TitleText = "Title"
			label.TitleColor = Color.Blue

			' Add a label to a report.
			report.Detail.Controls.Add(label)

			' Show the Print Preview form.
			report.ShowPreview()
		End Sub
	End Class
End Namespace