using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;

namespace WFTE
{
    /// <summary>
    /// A user control to edit a list of tags.
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class WinFormTagsEditor : UserControl
    {

        #region implementation

        private string templatestyles = @"
<style>
body {{
    cursor: default;
    background-color: {0};
}}
.tag {{
    background-color: {1};
    font: {2};
    color: {3};
}}
.del {{
    background-color: {4};
    display:none;
}}
.plus {{
    background-color: {4};
    opacity:0.30;
    display: inline-block;
    filter:alpha(opacity=30);
}}
.plusin {{
    background-color: {4};
    opacity:1.0;
    display: inline-block;
    filter:alpha(opacity=100);
}}

</style>
";

        private string document = @"
<script>

document.attachEvent('onclick', function(event) {
    clickedon = event.srcElement.id;
    if (clickedon) {
        if (clickedon == 'plus') {
            window.external._addTag();
        } else if (clickedon.match('del$')){
            var n = parseInt(clickedon);
            if (n != NaN)
                window.external._delTag(n);
        }
    }
});

document.attachEvent('onmouseover', function(event) {
    overelem = event.srcElement.id;
    if (overelem) {
        if (overelem == 'plus') {
            event.srcElement.className = 'plusin';
        } else if (overelem.match('tag$') || overelem.match('del$')) {
            var tid = parseInt(overelem);
            document.getElementById(tid+'del').style.display = 'inline-block';
        }
    }
});

document.attachEvent('onmouseout', function(event) {
    overelem = event.srcElement.id;
    if (overelem) {
        if (overelem == 'plus') {
            event.srcElement.className = 'plus';
        } else if (overelem.match('tag$') || overelem.match('del$')) {
            var tid = parseInt(overelem);
            document.getElementById(tid+'del').style.display = 'none';
        }
    }
});

function fillcontent(content) {
    document.getElementById('thetags').innerHTML = content
}

</script>

<div id='thetags'>
</div>

";

        private string templatetag =
            @"<span class='tag' id='{0}tag'>{1}</span>";
        private string templatedel =
            @"<span class='del' id='{0}del'>&#x2717;</span>"; // BALLOT


        private List<string> tagslist = new List<string>();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var doc =
                string.Format(this.templatestyles,
                    colorToCSS(this.BackColor),
                    colorToCSS(this.HighlightColor),
                    fontToCSS(this.Font),
                    colorToCSS(this.ForeColor),
                    colorToCSS(this.HighlightColor)
                ) + document;
            this.webBrowser1.DocumentText = doc;
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var cnt = buildcontent();
            this.webBrowser1.Document.InvokeScript("fillcontent", new String[] { cnt });
        }

        private string colorToCSS(Color color)
        {
            return string.Format("rgb({0},{1},{2})", color.R, color.G, color.B);
        }

        private string fontToCSS(Font font)
        {
            var f = string.Format("{1}pt {2} {3} {0}",
                font.FontFamily.Name,
                Convert.ToInt16(font.SizeInPoints),
                font.Bold ? "bold" : "",
                font.Italic ? "italic" : "");
            return f;
        }

        private string buildcontent()
        {
            var sb = new StringBuilder();
            if (!ReadOnly)
                sb.AppendLine("<span class='plus' id='plus'>&#x2795;</span>"); // HEAVY PLUS SIGN
            int i = 0;
            foreach (var t in this.tagslist) {
                sb.Append(string.Format(templatetag, ++i, t));
                if (!ReadOnly)
                    sb.Append(string.Format(templatedel, i));
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private Form getParentForm()
        {
            var parent = this.Parent;
            while (!(parent is Form)) parent = parent.Parent;
            return parent as Form;
        }

        public void _addTag()
        {
            var newtag = "";
            var parent = getParentForm();
            using (var f = new NewTagForm()) {
                f.StartPosition = FormStartPosition.Manual;
                f.Location = new Point(Cursor.Position.X, Cursor.Position.Y - f.Height);
                var dres = f.ShowDialog(parent);
                if (dres != DialogResult.OK || string.IsNullOrWhiteSpace(f.Value))
                    return;
                newtag = f.Value;
            }

            this.tagslist.AddRange(
                newtag.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => t != "")
            );
            var cnt = buildcontent();
            this.webBrowser1.Document.InvokeScript("fillcontent", new String[] { cnt });
            var handler = this.AfterTagsChanged;
            if (handler != null) {
                var e = new EventArgs();
                handler(this, e);
            }
        }

        public void _delTag(int n)
        {
            this.tagslist.RemoveAt(n-1);
            var cnt = buildcontent();
            this.webBrowser1.Document.InvokeScript("fillcontent", new String[] { cnt });
            var handler = this.AfterTagsChanged;
            if (handler != null) {
                var e = new EventArgs();
                handler(this, e);
            }
        }

        #endregion

        #region public API

        /// <summary>
        /// Constructor.
        /// </summary>
        public WinFormTagsEditor()
        {
            InitializeComponent();

            this.BackColor = Color.White;
            this.HighlightColor = Color.Yellow;
            this.Font = new Font("Courier New", 10, GraphicsUnit.Point);
            this.webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
            this.webBrowser1.ObjectForScripting = this;
        }

        /// <summary>
        /// Get the current collection of tags.
        /// </summary>
        /// <returns>An enumerable of strings.</returns>
        public IEnumerable<string> GetTags()
        {
            return this.tagslist;
        }

        /// <summary>
        /// Loads the tags to the control.
        /// </summary>
        /// <param name="tags">An enumerable of strings.</param>
        public void SetTags(IEnumerable<string> tags)
        {
            this.tagslist = new List<string>();
            this.tagslist.AddRange(tags);
            var cnt = buildcontent();
            if (this.webBrowser1.Document != null)
                this.webBrowser1.Document.InvokeScript("fillcontent", new String[] { cnt });
        }

        /// <summary>
        /// Fired when the collection of tags changes.
        /// </summary>
        public event EventHandler<EventArgs> AfterTagsChanged;

        /// <summary>
        /// "Sets the content as read only.
        /// </summary>
        [Category("Behavior")]
        [Description("Sets the content as read only.")]
        [DefaultValue(false)]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Sets the highlight color.
        /// </summary>
        [Category("Appearance")]
        [Description("Sets the highlight color.")]
        [DefaultValue(typeof(Color), "0xFFFF00")] // Color.Yellow
        public Color HighlightColor { get; set; }

        /// <summary>
        /// Sets the background color.
        /// </summary>
        [Category("Appearance")]
        [Description("Sets the background color.")]
        [DefaultValue(typeof(Color), "0xFFFFFF")] // Color.White
        public override Color BackColor { get; set; }

        /// <summary>
        /// Sets the font for the tags.
        /// </summary>
        [Category("Appearance")]
        [Description("Sets the font for the tags.")]
        [DefaultValue(typeof(Font), "Courier New, 10pt")]
        public override Font Font { get; set; }

        #endregion
    }
}
