using System;
using System.Drawing;
using System.ComponentModel;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Serialization;
// ...

namespace CustomControls_TitledLabel {

    public class XrTitledLabel : XRControl {
        private string titleText;
        private Font titleFont;
        private Color titleColor;
        private bool displayTitle;
        private ContextTextAlignmentEnum contextTextAlignment = ContextTextAlignmentEnum.BelowTitleText;
        private TitleTextAlignmentEnum titleTextAlignment = TitleTextAlignmentEnum.TopLeft;
        public float originalRectY;

        private Container components = null;

        public XrTitledLabel() {
            this.titleFont = new Font(this.Font, FontStyle.Regular);
            this.titleFont = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, ((Byte)(0)));
            this.titleColor = Color.Red;
            this.titleTextAlignment = TitleTextAlignmentEnum.TopLeft;
            this.contextTextAlignment = ContextTextAlignmentEnum.BelowTitleText;

            displayTitle = true;

            InitializeComponent();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            components = new Container();
        }

        public enum ContextTextAlignmentEnum {
            TopLeft = 1,
            BelowTitleText = 2,
            NextToTitleText = 3
        }

        public enum TitleTextAlignmentEnum {
            TopLeft = 1,
            CenterLeft = 2,
            Center = 3
        }

        public string TitleText {
            get {
                return titleText;
            }
            set {
                titleText = value;
            }
        }

        public ContextTextAlignmentEnum ContextTextAlignment {
            get {
                return contextTextAlignment;
            }
            set {
                contextTextAlignment = value;
            }
        }

        public TitleTextAlignmentEnum TitleTextAlignment {
            get {
                return titleTextAlignment;
            }
            set {
                titleTextAlignment = value;
            }
        }

        public Color TitleColor {
            get {
                return titleColor;
            }
            set {
                titleColor = value;
            }
        }

        public Font TitleFont {
            get {
                return titleFont;
            }
            set {
                titleFont = value;
            }
        }

        public bool DisplayTitle {
            get { return displayTitle; }
            set { displayTitle = value; }
        }

        protected override VisualBrick CreateBrick(VisualBrick[] childrenBricks) {
            return new PanelBrick(this);
        }

        protected override void PutStateToBrick(VisualBrick brick, PrintingSystemBase ps) {
            base.PutStateToBrick(brick, ps);
            PanelBrick panelBrick = (PanelBrick)brick;

            Graphics gr = Graphics.FromHwnd(new IntPtr(0));
            gr.PageUnit = GraphicsUnit.Document;
            RectangleF rect = RectangleF.Empty;
            try {
                if (displayTitle && !string.IsNullOrEmpty(TitleText) && titleFont != null) {
                    rect.Size = gr.MeasureString(TitleText, titleFont, (int)panelBrick.Rect.Width);
                    TextBrick textBrick = CreateTextBrick(panelBrick);
                    textBrick.Rect = rect;
                    textBrick.ForeColor = TitleColor;
                    textBrick.Font = TitleFont;
                    textBrick.Text = TitleText;
                }
                if (!string.IsNullOrEmpty(this.Text)) {
                    TextBrick textBrick2 = CreateTextBrick(panelBrick);
                    textBrick2.ForeColor = this.ForeColor;
                    textBrick2.Font = this.Font;
                    textBrick2.Text = this.Text;

                    if (this.contextTextAlignment == ContextTextAlignmentEnum.NextToTitleText) {
                        float width = Math.Max(0, panelBrick.Rect.Width - rect.Width);
                        SizeF size = gr.MeasureString(this.Text, this.Font, (int)width);
                        textBrick2.Rect = new RectangleF(rect.Right + 10f, rect.Top, width, size.Height);
                    }
                    else {
                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = GraphicsConvertHelper.ToVertStringAlignment(this.TextAlignment);
                        sf.Alignment = GraphicsConvertHelper.ToHorzStringAlignment(this.TextAlignment);

                        SizeF size = gr.MeasureString(this.Text, this.Font, (int)panelBrick.Rect.Width, sf);
                        textBrick2.Rect = new RectangleF(rect.Left, rect.Bottom + 10f, panelBrick.Rect.Width, size.Height);
                        textBrick2.StringFormat = new BrickStringFormat(sf);
                    }
                }
            }
            catch {
            }
            finally {
                gr.Dispose();
            }
        }

        TextBrick CreateTextBrick(PanelBrick panelBrick) {
            TextBrick textBrick = new TextBrick();
            textBrick.Sides = BorderSide.None;
            textBrick.BackColor = Color.Transparent;
            panelBrick.Bricks.Add(textBrick);
            return textBrick;
        }

    }
}
