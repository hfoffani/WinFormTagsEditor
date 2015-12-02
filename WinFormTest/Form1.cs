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
            foreach (var t in ex.Split(new char[] { '\uFDEF' })) {
                this.winFormTagsEditor1.Tags.Add(t);
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
    }
}
