using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Myrmanryb
{
    class DBConnections
    {
        static string connectionString = @"Database = prac9; Data Source = localhost;
 UserID = root; Password = 7896"; //строка подключения
        static MySqlConnection msConnect; //объект для установки соединения с БД
        static MySqlCommand msCommand; //объект для выполнения запросов
        static public MySqlDataAdapter msDataAdapter;
        static public bool Connect()
        {
            try
            {
                //создание объекта соединения с заданной строкой подключения
                msConnect = new MySqlConnection(connectionString);
                msConnect.Open(); //открытие подключение
                                  //создание объекта-запрос
                msCommand = new MySqlCommand();
                msCommand.Connection = msConnect;
                msDataAdapter = new MySqlDataAdapter(msCommand);
                return true; //результат «истина»
            }
            catch (Exception ex) //при возникновении ошибки
            {
                //вывод сообщения
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Ошибка!");
                return false; //результат «ложь»
            }
        }
        static public void Close()
        {
            msConnect.Close();
        }

        static public string User; //логин авторизованного пользователя
        static public string Role;
        static public void Authorization(string login, string password)
        {
            try
            {
                //формируем запрос: выбрать поле из таблицы значения,
                //где логин и пароль равны введенным пользователем значениям
                string sql = "SELECT Role FROM Users WHERE Login = '" + login
                 + "' AND Password = '" + password + "' ;";
                //создаем объект-запрос
                msCommand = new MySqlCommand(sql, msConnect);
                //фиксируем результат запроса
                Object result = msCommand.ExecuteScalar();
                //если в результате выполнения запроса получено непустое значение
                if (result != null)
                {
                    //заполняем информацию об авторизованном пользователе
                    Role = result.ToString();
                    User = login;
                }
                else
                {
                    //иначе тип пользователя - неавторизованный
                    Role = null;
                }
            }
            catch (Exception ex) //при возникновении ошибки
            {
                Role = User = null; //обнуляем значения полей
                MessageBox.Show(ex.ToString(), "Ошибка!");
            }
        }
        static public DataTable dtUsers = new DataTable();
        static public void GetUserList(string selectedRole = null)
        {
            //если роль не выбрана
            if (selectedRole == null)
            {
                //формируем запрос на выборку всех записей
                msCommand.CommandText = "SELECT * FROM Users";
            }
            else
            {
                //иначе, формируем запрос с фильтрацией
                msCommand.CommandText = "SELECT * FROM Users WHERE Users.role='" +
                selectedRole + "'";
            }
                dtUsers.Clear(); //очистка набора данных
                msDataAdapter.Fill(dtUsers); //заполнение набора данных
            }

        static public bool AddUser(string login, string password, string role)
        {
            //формирование запроса
            msCommand.CommandText = "INSERT INTO users VALUES('" + login +
             "','" + password + "','" + role + "');";
            //выполение запроса
            if (msCommand.ExecuteNonQuery() > 0)
                return true;
            else
                return false;
        }
        //добавление нового заказчика
        static public void AddCustomer(string user, string name, string telephone, string
        adress, string email = null)
        {
            //формирование запроса
            msCommand.CommandText = "INSERT INTO customers VALUES('" + user + "','" + name
             + "','" + telephone + "','" + email + "','" + adress + "');";
            //выполение запроса
            msCommand.ExecuteNonQuery();
        }
        static public void EditCustomer(string user, string name, string telephone, string
adress, string email)
        {
            msCommand.CommandText = "UPDATE customers SET name = '" + name +
"', telephone = '" + telephone + "', adress='" + adress +
//выполение запроса
msCommand.ExecuteNonQuery();
        }

        static public void AddToStore(string Product, string Count, string Date)
        {
            msCommand.CommandText = @"INSERT INTO Store (Product, Count, Date)
 VALUES('" + Product + "', '" + Count + "','" + Date + "');";
            msCommand.ExecuteNonQuery();
        }

        static public int WriteOff()
        {
            //формирование запроса на выборку просроченных товаров
            msCommand.CommandText = @"SELECT Store.PositionId, Store.Product, Store.Count,
 Store.Date, Assortiment.ShelfLife
 FROM Store
 INNER JOIN Assortiment USING(Product)
 WHERE To_Days(CURDATE())-TO_Days(Store.Date) >=
 Assortiment.ShelfLife;";
            DataTable table = new DataTable();
            table.Clear();
            msDataAdapter.Fill(table); //наполнение набора данных
                                            //обход таблицы с просроченными товарами
            foreach (DataRow row in table.Rows)
            {
                //преобразование столбца с датой к используемому формату
                DateTime date = new DateTime();
                date = (DateTime)row[3];
                //формирвание запроса на вставку записи в таблицу с просрочкой
                msCommand.CommandText = @"INSERT INTO WriteOff VALUES('" + row[0] + "','"
                 + row[1] + "','" + row[2] + "','" +
                 date.ToString(("yyyy-MM-dd")) + "','" +
                 DateTime.Today.ToString("yyyy-MM-dd") + "')";
                msCommand.ExecuteNonQuery();
                //формирование запроса на удаление позиции со склада
                msCommand.CommandText = @"DELETE FROM Store
 WHERE PositionId='" + row[0] + "';";
                msCommand.ExecuteNonQuery();
            }
            return table.Rows.Count; //возвращаем количество просроченных товаров
        }


    }
}
