using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESS
{
    public class ComplexForm : Form
    {
        public MainForm mainForm;
        public ComplexForm lastForm;
        public ComplexForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
            this.FormClosing += ComplexForm_FormClosing;
        }
        public ComplexForm()
        {
            this.FormClosing += ComplexForm_FormClosing;
        }

        private void ComplexForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (lastForm == null || lastForm == this)
                Application.Exit();
            else
            {
                e.Cancel = true;
                ChangeForm(lastForm);
            }
        }

        public void ChangeForm(ComplexForm retFrom)
        {
            // retFrom.Show();
            retFrom.Show();
            retFrom.Location = this.Location;
            retFrom.Size = this.Size;
            retFrom.WindowState = this.WindowState;
            if (retFrom.lastForm == null)
            {
                retFrom.lastForm = this;
            }
            this.Visible = false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ComplexForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "ComplexForm";
            this.ResumeLayout(false);

        }



    }
}
