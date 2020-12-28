using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Myrmanryb
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Form a = new Auth();
            a.Show();
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Form a = new UserList();
            a.Show();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Form a = new Customers();
            a.Show();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Form a = new Storage();
            a.Show();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Form a = new Orders();
            a.Show();
        }
    }
}
