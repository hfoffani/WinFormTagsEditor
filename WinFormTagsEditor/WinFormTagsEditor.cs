using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormTagsEditor
{
    public partial class WinFormTagsEditor: UserControl
    {

        private string head = @"
<script>
    document.attachEvent('onclick', function(event) {
    var specifiedElement = document.getElementById('T2');
    var isClickInside = specifiedElement.contains(event.srcElement);
    if (isClickInside) {
        alert('You clicked inside Tag 2.')
    } else {
        alert('You clicked outside Tag 2.')
    }
});
</script>
<style>
body { background-color:gray; }
.highlightme { background-color:#FFFF00; }
p { background-color:#FFFFFF; }
</style>
";

        private string templatetag = @"<span class='highlightme' id='T{1}'>{0}</span>";

        public IList<string> Tags { get; private set; }

        public WinFormTagsEditor()
        {
            InitializeComponent();

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
            sb.AppendLine(@"<div id='A'>");
            int i = 0;
            foreach (var t in this.Tags) {
                sb.AppendLine(string.Format(templatetag, t, ++i));
            }
            sb.AppendLine("</div>");
            return sb.ToString();
        }

    }
}
