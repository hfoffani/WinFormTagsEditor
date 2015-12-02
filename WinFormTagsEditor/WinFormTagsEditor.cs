using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;

namespace WinFormTagsEditor
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class WinFormTagsEditor : UserControl
    {

        private string head = @"
<script>
document.attachEvent('onclick', function(event) {
    clickedon = event.srcElement.id;
    if (clickedon) {
        if (clickedon == 'plus') {
            var ntag = inputnewtags();
            if (ntag)
                window.external._addTag(ntag);
        } else {
            var n = parseInt(clickedon);
            if (n != NaN)
                window.external._delTag(n);
        }
    }
});

function fillcontent(content) {
    document.getElementById('thetags').innerHTML = content
}

function inputnewtags() {
    var r = prompt('Enter new tag', '');
    return r;
}
</script>
<style>
body { background-color:gray; }
.tag { background-color:#FFFF00; }
.del { background-color:#FFFFAA; }
p { background-color:#FFFFFF; }
</style>

<div id='thetags'>
</div>
";

        private string templatetag =
            @"<span class='tag'>{0}</span><span class='del' id='{1}'>x</span>";


        private List<string> tagslist = new List<string>();

        public WinFormTagsEditor()
        {
            InitializeComponent();

            this.webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
            this.webBrowser1.ObjectForScripting = this;
            this.webBrowser1.DocumentText = head;
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var cnt = buildcontent();
            this.webBrowser1.Document.InvokeScript("fillcontent", new String[] { cnt });
        }


        private string buildcontent()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<span class='tag' id='plus'>+</span>");
            int i = 0;
            foreach (var t in this.tagslist) {
                sb.AppendLine(string.Format(templatetag, t, ++i));
            }
            return sb.ToString();
        }

        public void _addTag(string newtag)
        {
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
            this.webBrowser1.Document.InvokeScript("fillcontent", new String[] { cnt });
        }

        public event EventHandler<EventArgs> AfterTagsChanged;
    }
}
