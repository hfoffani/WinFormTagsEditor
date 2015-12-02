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
<style>
body { background-color:gray; }
.highlightme { background-color:#FFFF00; }
p { background-color:#FFFFFF; }
</style>
";

        private string templatetag = @"<span class=""highlightme"">{0}</span>";

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
            sb.AppendLine("<div>");
            foreach (var t in this.Tags) {
                sb.AppendLine(string.Format(templatetag, t));
            }
            sb.AppendLine("</div>");
            return sb.ToString();
        }

    }
}
