
/*
 * Copyright 2015 Hernán M. Foffani
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

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

            // this.winFormTagsEditor1.ReadOnly = true;

            var ex = "hola\uFDEFque\uFDEFtal";
            this.winFormTagsEditor1.SetTags(ex.Split(new char[] { '\uFDEF' }));

            this.winFormTagsEditor1.AfterTagsChanged += winFormTagsEditor1_AfterTagsChanged;
        }

        void winFormTagsEditor1_AfterTagsChanged(object sender, EventArgs e)
        {
            // var ts = string.Join("|", this.winFormTagsEditor1.GetTags());
            // MessageBox.Show(ts);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
    }
}
