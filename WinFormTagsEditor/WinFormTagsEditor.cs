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
            // alert('add tag.');
            window.external._addTag('other, tag');
        } else {
            var n = parseInt(clickedon);
            if (n != NaN)
                window.external._delTag(n);
        }
    }
});
</script>
<style>
body { background-color:gray; }
.tag { background-color:#FFFF00; }
.del { background-color:#FFFFAA; }
p { background-color:#FFFFFF; }
</style>
";

        private string templatetag =
            @"<span class='tag'>{0}</span><span class='del' id='{1}'>x</span>";

        private string plus =
            @"<span class='tag' id='plus'>+</span>";

        private List<string> tagslist = new List<string>();

        public WinFormTagsEditor()
        {
            InitializeComponent();
            this.webBrowser1.ObjectForScripting = this;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.webBrowser1.DocumentText = getcontent();
        }

        private string getcontent()
        {
            var sb = new StringBuilder();
            sb.AppendLine(head);
            sb.AppendLine("<div>");
            sb.AppendLine(plus);
            int i = 0;
            foreach (var t in this.tagslist) {
                sb.AppendLine(string.Format(templatetag, t, ++i));
            }
            sb.AppendLine("</div>");
            return sb.ToString();
        }

        public void _addTag(string newtag)
        {
            this.tagslist.AddRange(
                newtag.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => t != "")
            );
            this.webBrowser1.DocumentText = getcontent();
        }

        public void _delTag(int n)
        {
            this.tagslist.RemoveAt(n);
            this.webBrowser1.DocumentText = getcontent();
        }

        public IEnumerable<string> GetTags()
        {
            return this.tagslist;
        }

        public void SetTags(IEnumerable<string> tags)
        {
            this.tagslist = new List<string>();
            this.tagslist.AddRange(tags);
        }
    }
}
