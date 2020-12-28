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
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {


            switch (DBConnections.Role)
            {
                //если роль не распознана, пользователь не авторизован
                case null:
                    MessageBox.Show("Неверные данные!");
                    break;
                //если авторизован заказчик
                case "customer":
                    this.Hide(); //скрываем текущую форму
                    Customers CustomerMenuFrm = new Customers(); //создаем и показываем
                    CustomerMenuFrm.Show(); //меню заказчика
                    break;
                //если авторизован администратор
                case "admin":
                    this.Hide(); //скрываем текущую форму
                    Admin AdminFrm = new Admin();//создаем и показываем
                    AdminFrm.Show(); //меню администратора
                    break;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Auth_Load(object sender, EventArgs e)
        {
            if (!DBConnections.Connect())
            {
                this.Close(); //выход из программы
            }
        }
    }
}
