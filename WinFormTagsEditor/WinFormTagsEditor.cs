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
            window.external.AddTag('new tags');
        } else {
            var n = parseInt(clickedon);
            if (n != NaN)
                window.external.DelTag(n);
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

        public IList<string> Tags { get; private set; }

        public WinFormTagsEditor()
        {
            InitializeComponent();
            this.webBrowser1.ObjectForScripting = this;

            Tags = new List<string>();
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
            foreach (var t in this.Tags) {
                sb.AppendLine(string.Format(templatetag, t, ++i));
            }
            sb.AppendLine("</div>");
            return sb.ToString();
        }

        public void AddTag(string newtag)
        {
            MessageBox.Show(" ADD " + newtag);
        }

        public void DelTag(int n)
        {
            MessageBox.Show(" DEL " + n.ToString());
        }
    }
}
