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
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class WinFormTagsEditor : UserControl
    {

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
    opacity:0.30;
    display: inline-block;
    filter:alpha(opacity=30);
}}
.delin {{
    background-color: {4};
    opacity:1.0;
    display: inline-block;
    filter:alpha(opacity=100);
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
        } else {
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
        }
    }
});

document.attachEvent('onmouseout', function(event) {
    overelem = event.srcElement.id;
    if (overelem) {
        if (overelem == 'plus') {
            event.srcElement.className = 'plus';
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
            @"<span class='tag'>{0}</span>";
        private string templatedel =
            @"<span class='del' id='{0}'>&#x2717;</span>"; // BALLOT


        private List<string> tagslist = new List<string>();

        public WinFormTagsEditor()
        {
            InitializeComponent();

            this.webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
            this.webBrowser1.ObjectForScripting = this;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var doc =
                string.Format(this.templatestyles,
                    "gray",                 // background color
                    "white",                // highlight background color
                    "Courier New 14px, monospace",  // font
                    "blue",                 // font-color
                    "yellow"                // button background color.
                ) + document;
            this.webBrowser1.DocumentText = doc;
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var cnt = buildcontent();
            this.webBrowser1.Document.InvokeScript("fillcontent", new String[] { cnt });
        }


        private string buildcontent()
        {
            var sb = new StringBuilder();
            if (!ReadOnly)
                sb.AppendLine("<span class='plus' id='plus'>&#x2795;</span>"); // HEAVY PLUS SIGN
            int i = 0;
            foreach (var t in this.tagslist) {
                sb.Append(string.Format(templatetag, t));
                if (!ReadOnly)
                    sb.Append(string.Format(templatedel, ++i));
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
                newtag.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
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

        public IEnumerable<string> GetTags()
        {
            return this.tagslist;
        }

        public void SetTags(IEnumerable<string> tags)
        {
            this.tagslist = new List<string>();
            this.tagslist.AddRange(tags);
            var cnt = buildcontent();
            if (this.webBrowser1.Document != null)
                this.webBrowser1.Document.InvokeScript("fillcontent", new String[] { cnt });
        }

        public event EventHandler<EventArgs> AfterTagsChanged;

        public bool ReadOnly { get; set; }
    }
}
