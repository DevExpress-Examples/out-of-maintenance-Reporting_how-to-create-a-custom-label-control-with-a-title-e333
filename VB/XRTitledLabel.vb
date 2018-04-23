Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.ComponentModel
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.Serialization
' ...

Namespace CustomControls_TitledLabel

	Public Class XrTitledLabel
		Inherits XRControl
		Private titleText_Renamed As String
		Private titleFont_Renamed As Font
		Private titleColor_Renamed As Color
		Private displayTitle_Renamed As Boolean
		Private contextTextAlignment_Renamed As ContextTextAlignmentEnum = ContextTextAlignmentEnum.BelowTitleText
		Private titleTextAlignment_Renamed As TitleTextAlignmentEnum = TitleTextAlignmentEnum.TopLeft
		Public originalRectY As Single

		Private components As Container = Nothing

		Public Sub New()
			Me.titleFont_Renamed = New Font(Me.Font, FontStyle.Regular)
			Me.titleFont_Renamed = New Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, (CType(0, Byte)))
			Me.titleColor_Renamed = Color.Red
			Me.titleTextAlignment_Renamed = TitleTextAlignmentEnum.TopLeft
			Me.contextTextAlignment_Renamed = ContextTextAlignmentEnum.BelowTitleText

			displayTitle_Renamed = True

			InitializeComponent()
		End Sub

		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				If components IsNot Nothing Then
					components.Dispose()
				End If
			End If
			MyBase.Dispose(disposing)
		End Sub

		Private Sub InitializeComponent()
			components = New Container()
		End Sub

		Public Enum ContextTextAlignmentEnum
			TopLeft = 1
			BelowTitleText = 2
			NextToTitleText = 3
		End Enum

		Public Enum TitleTextAlignmentEnum
			TopLeft = 1
			CenterLeft = 2
			Center = 3
		End Enum

		Public Property TitleText() As String
			Get
				Return titleText_Renamed
			End Get
			Set(ByVal value As String)
				titleText_Renamed = value
			End Set
		End Property

		Public Property ContextTextAlignment() As ContextTextAlignmentEnum
			Get
				Return contextTextAlignment_Renamed
			End Get
			Set(ByVal value As ContextTextAlignmentEnum)
				contextTextAlignment_Renamed = value
			End Set
		End Property

		Public Property TitleTextAlignment() As TitleTextAlignmentEnum
			Get
				Return titleTextAlignment_Renamed
			End Get
			Set(ByVal value As TitleTextAlignmentEnum)
				titleTextAlignment_Renamed = value
			End Set
		End Property

		Public Property TitleColor() As Color
			Get
				Return titleColor_Renamed
			End Get
			Set(ByVal value As Color)
				titleColor_Renamed = value
			End Set
		End Property

		Public Property TitleFont() As Font
			Get
				Return titleFont_Renamed
			End Get
			Set(ByVal value As Font)
				titleFont_Renamed = value
			End Set
		End Property

		Public Property DisplayTitle() As Boolean
			Get
				Return displayTitle_Renamed
			End Get
			Set(ByVal value As Boolean)
				displayTitle_Renamed = value
			End Set
		End Property

		Protected Overrides Function CreateBrick(ByVal childrenBricks() As VisualBrick) As VisualBrick
			Return New PanelBrick(Me)
		End Function

		Protected Overrides Sub PutStateToBrick(ByVal brick As VisualBrick, ByVal ps As PrintingSystemBase)
			MyBase.PutStateToBrick(brick, ps)
			Dim panelBrick As PanelBrick = CType(brick, PanelBrick)

			Dim gr As Graphics = Graphics.FromHwnd(New IntPtr(0))
			gr.PageUnit = GraphicsUnit.Document
			Dim rect As RectangleF = RectangleF.Empty
			Try
				If displayTitle_Renamed AndAlso (Not String.IsNullOrEmpty(TitleText)) AndAlso titleFont_Renamed IsNot Nothing Then
					rect.Size = gr.MeasureString(TitleText, titleFont_Renamed, CInt(Fix(panelBrick.Rect.Width)))
					Dim textBrick As TextBrick = CreateTextBrick(panelBrick)
					textBrick.Rect = rect
					textBrick.ForeColor = TitleColor
					textBrick.Font = TitleFont
					textBrick.Text = TitleText
				End If
				If (Not String.IsNullOrEmpty(Me.Text)) Then
					Dim textBrick2 As TextBrick = CreateTextBrick(panelBrick)
					textBrick2.ForeColor = Me.ForeColor
					textBrick2.Font = Me.Font
					textBrick2.Text = Me.Text

					If Me.contextTextAlignment_Renamed = ContextTextAlignmentEnum.NextToTitleText Then
						Dim width As Single = Math.Max(0, panelBrick.Rect.Width - rect.Width)
						Dim size As SizeF = gr.MeasureString(Me.Text, Me.Font, CInt(Fix(width)))
						textBrick2.Rect = New RectangleF(rect.Right + 10f, rect.Top, width, size.Height)
					Else
						Dim sf As New StringFormat()
						sf.LineAlignment = GraphicsConvertHelper.ToVertStringAlignment(Me.TextAlignment)
						sf.Alignment = GraphicsConvertHelper.ToHorzStringAlignment(Me.TextAlignment)

						Dim size As SizeF = gr.MeasureString(Me.Text, Me.Font, CInt(Fix(panelBrick.Rect.Width)), sf)
						textBrick2.Rect = New RectangleF(rect.Left, rect.Bottom + 10f, panelBrick.Rect.Width, size.Height)
						textBrick2.StringFormat = New BrickStringFormat(sf)
					End If
				End If
			Catch
			Finally
				gr.Dispose()
			End Try
		End Sub

		Private Function CreateTextBrick(ByVal panelBrick As PanelBrick) As TextBrick
			Dim textBrick As New TextBrick()
			textBrick.Sides = BorderSide.None
			textBrick.BackColor = Color.Transparent
			panelBrick.Bricks.Add(textBrick)
			Return textBrick
		End Function

	End Class
End Namespace
