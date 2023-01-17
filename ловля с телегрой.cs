using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;//библиотека для работы с запросами
using System.IO;
using Telegram.Bot; //библиотека для работы с ботом телеги


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //---Объявление переменных--//
        public String connString = "Data Source=10.89.68.47;Initial Catalog=erbd_ege_reg_22_77_test;Persist Security Info=True;User ID=sa;Password=pszXq2unF"; //объявляем переменную для запроса подключения к БД
        public String sqlQuery = "SELECT COUNT (h.HumanTestID) from res_HumanTests h"; // объявляем переменную для запроса в БД
        public int old_rez = 3; //старое кол-во результатов
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer(); // таймер
        public int counter = 1;
        public int new_rez = 0; //новое кол-во результатов
        public String counter2 = "";
        

        static bool exitFlag = false;

        public Form1()
        {
            InitializeComponent();// инициализация формы
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connForExamDates = new SqlConnection(connString);// создаем подключение к базе данных connString - параметры для подклчеия
            SqlCommand command = new SqlCommand("select distinct ExamDate from dbo.dat_Exams", connForExamDates);
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.sysobjects", connForExamDates); // создаем SQL запрос для полчения списка таблиц базы данных// строка соединения
            DataTable dt = new DataTable(); // создаем таблицу данных
            da.Fill(dt); // заполняем список DataTable
            textBox3.Text = "БД подключена";//как только бд подключена - выступает эта надпись

            ////--запрос в базу на определение кол-ва строк которые были--//
            using (SqlConnection SQLconn1 = new SqlConnection(connString)) //инициализировали новое соединение
            {
                SQLconn1.Open(); //открыли соединение
                SqlCommand command_long = new SqlCommand(sqlQuery, SQLconn1); //инициализировали команду sql
                old_rez = Convert.ToInt32(command_long.ExecuteScalar()); //в переменную загрузили значние выолнения команды, переведенное в число
                textBox1.Text = Convert.ToString(old_rez);//кол-во строк было
            }

            ////--сколько строк стало--//
            using (SqlConnection SQLconn1 = new SqlConnection(connString)) //инициализировали новое соединение
            {
                SQLconn1.Open(); //открыли соединение
                SqlCommand command_long = new SqlCommand(sqlQuery, SQLconn1); //инициализировали команду sql
                new_rez = Convert.ToInt32(command_long.ExecuteScalar()); //в переменную загрузили значние выолнения команды, переведенное в число
                textBox2.Text = Convert.ToString(new_rez);//кол-во строк стало
            }

            textBox4.Text = Convert.ToString(counter); // отображаем что пошла первая итерация

            ////--работа с таймером--//
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);//Подключить обработчик событий тика таймера.

            timer1.Interval = 5000; //интервал в милисекундах
            timer1.Start();

            while (exitFlag == false)
            {
                Application.DoEvents();
            }

            //var botClient = new TelegramBotClient("5315666946:AAFBaZKJchHKMffRx4ZXExXWHpB7-9Gb7BM");



        }

        ////--Обработка события таймера--//
        private void timer1_Tick(object sender, System.EventArgs e) //лобработчик события таймер
        {
            myTimer.Stop();//*
            if (new_rez == old_rez)
            {
                // Что делаем пока таймер идет
                counter++;
                textBox4.Text = Convert.ToString(counter);
                textBox4.Refresh();

                //--запрос в БД на кол-во строк которые стало2--//
                using (SqlConnection SQLconn1 = new SqlConnection(connString)) //инициализировали новое соединение
                {
                    SQLconn1.Open(); //открыли соединение
                    SqlCommand command_long = new SqlCommand(sqlQuery, SQLconn1); //инициализировали команду sql
                    new_rez = Convert.ToInt32(command_long.ExecuteScalar()); //в переменную загрузили значние выолнения команды, переведенное в число
                    textBox2.Text = Convert.ToString(new_rez);//кол-во строк стало
                }
            }
            else
            {
                exitFlag = true;
                MessageBox.Show("Результаты пришли", "ВНИМАНИЕ", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }

        }

       


    }
}
