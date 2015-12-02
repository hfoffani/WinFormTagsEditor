using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormTagsEditor
{
    public partial class NewTagForm : Form
    {
        public NewTagForm()
        {
            InitializeComponent();
            this.btnok.Width = 0;
            this.btncancel.Width = 0;
            this.textBox1.TextChanged += textBox1_TextChanged;
        }


        void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.Value = this.textBox1.Text;
        }

        public string Value { get; set; }
    }
}
