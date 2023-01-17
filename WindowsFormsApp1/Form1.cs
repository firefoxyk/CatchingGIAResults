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

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //---Объявление переменных--//
        //ГИА-11
        public String connString11 = "Data Source=***;Initial Catalog=erbd_***;Persist Security Info=True;User ID=***;Password=***"; //объявляем переменную для запроса подключения к БД
        public String sqlQuery11 = "SELECT COUNT (h.HumanTestID) from res_HumanTests h"; // объявляем переменную для запроса в БД
        public int old_rez11 = 3; //старое кол-во результатов
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer(); // таймер
        public int counter11 = 1;
        public int new_rez11 = 0; //новое кол-во результатов
        public String counter211 = "";

        static bool exitFlag11 = false;

        //ГИА-9
        public String connString9 = "Data Source=***;Initial Catalog=erbd_***;Persist Security Info=True;User ID=***;Password=***"; //объявляем переменную для запроса подключения к БД
        public String sqlQuery9 = "SELECT COUNT (h.HumanTestID) from res_HumanTests h"; // объявляем переменную для запроса в БД
        public int old_rez9 = 3; //старое кол-во результатов
        static System.Windows.Forms.Timer myTimer2 = new System.Windows.Forms.Timer(); // таймер
        public int counter9 = 1;
        public int new_rez9 = 0; //новое кол-во результатов
        public String counter29 = "";

        static bool exitFlag9 = false;


        public Form1()
        {
            InitializeComponent();// инициализация формы
        }

        //////////////ГИА-11
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connForExamDates = new SqlConnection(connString11);// создаем подключение к базе данных connString - параметры для подклчеия
            SqlCommand command = new SqlCommand("select distinct ExamDate from dbo.dat_Exams", connForExamDates);
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.sysobjects", connForExamDates); // создаем SQL запрос для полчения списка таблиц базы данных// строка соединения
            DataTable dt = new DataTable(); // создаем таблицу данных
            da.Fill(dt); // заполняем список DataTable
            textBox3.Text = "БД подключена";//как только бд подключена - выступает эта надпись

            ////--запрос в базу на определение кол-ва строк которые были--//
            using (SqlConnection SQLconn1 = new SqlConnection(connString11)) //инициализировали новое соединение
            {
                SQLconn1.Open(); //открыли соединение
                SqlCommand command_long = new SqlCommand(sqlQuery11, SQLconn1); //инициализировали команду sql
                old_rez11 = Convert.ToInt32(command_long.ExecuteScalar()); //в переменную загрузили значние выолнения команды, переведенное в число
                textBox1.Text = Convert.ToString(old_rez11);//кол-во строк было
            }

            ////--сколько строк стало--//
            using (SqlConnection SQLconn1 = new SqlConnection(connString11)) //инициализировали новое соединение
            {
                SQLconn1.Open(); //открыли соединение
                SqlCommand command_long = new SqlCommand(sqlQuery11, SQLconn1); //инициализировали команду sql
                new_rez11 = Convert.ToInt32(command_long.ExecuteScalar()); //в переменную загрузили значние выолнения команды, переведенное в число
                textBox2.Text = Convert.ToString(new_rez11);//кол-во строк стало
            }

            textBox4.Text = Convert.ToString(counter11); // отображаем что пошла первая итерация

            //вывод текущего времени на форму
            label9.Text = DateTime.Now.ToString();
            Timer timer3 = new Timer();
            timer3.Interval = 9000;
            timer3.Tick += new EventHandler(tmr_Tick);
            timer3.Start();

            


            ////--работа с таймером--//
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);//Подключить обработчик событий тика таймера.

            timer1.Interval = 600000; //интервал в милисекундах
            timer1.Start();

            while (exitFlag11 == false)
            {
                Application.DoEvents();
            }
        }

        ////--Обработка события таймера--//
        private async void timer1_Tick(object sender, System.EventArgs e) //лобработчик события таймер
        {

            if (new_rez11 == old_rez11)
            {
                // Что делаем пока таймер идет
                counter11++;
                textBox4.Text = Convert.ToString(counter11);
                textBox4.Refresh();

                //--запрос в БД на кол-во строк которые стало2--//
                using (SqlConnection SQLconn1 = new SqlConnection(connString11)) //инициализировали новое соединение
                {
                    SQLconn1.Open(); //открыли соединение
                    SqlCommand command_long = new SqlCommand(sqlQuery11, SQLconn1); //инициализировали команду sql
                    new_rez11 = Convert.ToInt32(command_long.ExecuteScalar()); //в переменную загрузили значние выолнения команды, переведенное в число
                    textBox2.Text = Convert.ToString(new_rez11);//кол-во строк стало
                }
            }
            else
            {
                exitFlag11 = true;
                var telegrambot = new TelegramBotClient("5315666946:AAFBaZKJchHKMffRx4ZXExXWHpB7-****");
                await telegrambot.SendTextMessageAsync(chatId: "id чата", text: "Пришли результаты ГИА-11", parseMode: ParseMode.Html);
                MessageBox.Show("Результаты пришли ГИА-11 + Напомни Насте про ФИПИ", "ВНИМАНИЕ", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                timer1.Stop();//*
            }

        }

        //////////ГИА-9
        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection connForExamDates = new SqlConnection(connString9);// создаем подключение к базе данных connString - параметры для подклчеия
            SqlCommand command = new SqlCommand("select distinct ExamDate from dbo.dat_Exams", connForExamDates);
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.sysobjects", connForExamDates); // создаем SQL запрос для полчения списка таблиц базы данных// строка соединения
            DataTable dt = new DataTable(); // создаем таблицу данных
            da.Fill(dt); // заполняем список DataTable
            textBox6.Text = "БД подключена";//как только бд подключена - выступает эта надпись

            ////--запрос в базу на определение кол-ва строк которые были--//
            using (SqlConnection SQLconn1 = new SqlConnection(connString9)) //инициализировали новое соединение
            {
                SQLconn1.Open(); //открыли соединение
                SqlCommand command_long = new SqlCommand(sqlQuery9, SQLconn1); //инициализировали команду sql
                old_rez9 = Convert.ToInt32(command_long.ExecuteScalar()); //в переменную загрузили значние выолнения команды, переведенное в число
                textBox7.Text = Convert.ToString(old_rez9);//кол-во строк было
            }

            ////--сколько строк стало--//
            using (SqlConnection SQLconn2 = new SqlConnection(connString9)) //инициализировали новое соединение
            {
                SQLconn2.Open(); //открыли соединение
                SqlCommand command_long = new SqlCommand(sqlQuery9, SQLconn2); //инициализировали команду sql
                new_rez9 = Convert.ToInt32(command_long.ExecuteScalar()); //в переменную загрузили значние выолнения команды, переведенное в число
                textBox8.Text = Convert.ToString(new_rez9);//кол-во строк стало
            }

            textBox5.Text = Convert.ToString(counter9); // отображаем что пошла первая итерация
            //вывод текущего времени на форму
            label9.Text = DateTime.Now.ToString();
            Timer timer3 = new Timer();
            timer3.Interval = 1000;
            timer3.Tick += new EventHandler(tmr_Tick);
            timer3.Start();

            ////--работа с таймером--//
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);//Подключить обработчик событий тика таймера.

            timer2.Interval = 600000; //интервал в милисекундах
            timer2.Start();


            while (exitFlag9 == false)
            {
                Application.DoEvents();
            }
        }
        ////--Обработка события таймера--//
        private async void timer2_Tick(object sender, System.EventArgs e) //лобработчик события таймер
        {
            myTimer.Stop();//*
            if (new_rez9 == old_rez9)
            {
                // Что делаем пока таймер идет
                counter9++;
                textBox5.Text = Convert.ToString(counter11);
                textBox5.Refresh();

                //--запрос в БД на кол-во строк которые стало2--//
                using (SqlConnection SQLconn2 = new SqlConnection(connString9)) //инициализировали новое соединение
                {
                    SQLconn2.Open(); //открыли соединение
                    SqlCommand command_long = new SqlCommand(sqlQuery9, SQLconn2); //инициализировали команду sql
                    new_rez9 = Convert.ToInt32(command_long.ExecuteScalar()); //в переменную загрузили значние выолнения команды, переведенное в число
                    textBox8.Text = Convert.ToString(new_rez9);//кол-во строк стало
                }
            }
            else
            {
                exitFlag9 = true;
                var telegrambot = new TelegramBotClient("5315666946:AAFBaZKJchHKMffRx4ZXExXWHpB7-****");
                await telegrambot.SendTextMessageAsync(chatId: "id чата", text: "Пришли результаты ГИА-9", parseMode: ParseMode.Html);
                MessageBox.Show("Результаты пришли ГИА-9 + Напомни Насте про ФИПИ", "ВНИМАНИЕ", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }

        }
        void tmr_Tick(object sender, EventArgs e)
        {
            label9.Text = DateTime.Now.ToString();
        }

    }
}
