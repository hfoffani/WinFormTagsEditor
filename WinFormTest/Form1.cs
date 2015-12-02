using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var ex = "hola\uFDEFque\uFDEFtal";
            this.winFormTagsEditor1.SetTags(ex.Split(new char[] { '\uFDEF' }));

            this.winFormTagsEditor1.AfterTagsChanged += winFormTagsEditor1_AfterTagsChanged;
        }

        void winFormTagsEditor1_AfterTagsChanged(object sender, EventArgs e)
        {
            var ts = string.Join("|", this.winFormTagsEditor1.GetTags());
            MessageBox.Show(ts);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
    }
}
