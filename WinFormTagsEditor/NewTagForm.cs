using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace WFTE
{
    internal partial class NewTagForm : Form
    {
        string waterMarkText = "<Enter> to add. <Esc> to close.";
        Font prototype;

        public NewTagForm()
        {
            InitializeComponent();
            prototype = this.textBox1.Font;
            this.textBox1.Font = new Font(prototype.FontFamily, prototype.Size-2, prototype.Style);
            this.btnok.Width = 0;
            this.btncancel.Width = 0;
            this.textBox1.TextChanged += textBox1_TextChanged;

            SendMessage(this.textBox1.Handle, 0x1501, 1, waterMarkText);
        }


        void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.Value = this.textBox1.Text;

            if (this.textBox1.Text != "") {
                this.textBox1.Font = prototype;
            } else {
                this.textBox1.Font = new Font(prototype.FontFamily, prototype.Size - 2, prototype.Style);
            }
        }

        public string Value { get; set; }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

    }
}
