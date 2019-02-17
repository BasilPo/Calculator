/*v 1.0 09.02.2018
 баги - невідомі
 можливість подальшої оптимізації - так*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //для повернення американського стандарту в дробових числах
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            InitializeComponent();
        }
        //стек для збереження результатів обчислень
        Stack<double> stackResult = new Stack<double>();
        //стек, який зберігатиме оператори
        Stack<string> stackOperator = new Stack<string>();
        //індекатор поведінки клавіш
        //змінна рівна 'true' - означатиме що цифри не додаватимуться до існуючого числа,
        //а замінять його
        bool flag = false;
        //індикатор "помилки" - для іншої поведінки клавіш
        bool error = false;
        //змінна яка зберігає результат обчислень
        double result = 0.0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //'помилка' має значення істинності якщо змінна 'result' після 
            //обчислень матиме значення NaN або Infinity
            if (error)
            {
                //після генераціє помилки наступний 'клік' по клавішам
                //повертає початковий стан 'калькулятора'
                stackOperator.Clear();
                stackResult.Clear();
                tbCurr.Clear();
                tbPrev.Clear();
                tbCurr.Text = "0";
                error = false;
                flag = false;
            }
            switch ((sender as Button).Name)
            {
                //поведінка є однаковою для кожної із груп клавіш
                case "b1":
                case "b2":
                case "b3":
                case "b4":
                case "b5":
                case "b6":
                case "b7":
                case "b8":
                case "b9":
                case "b0":
                    if (tbCurr.Text == "0" || flag) 
                    {
                        tbCurr.Clear();
                        tbCurr.Text = ((sender as Button).Content).ToString();
                    }
                    else
                        tbCurr.Text += ((sender as Button).Content).ToString();
                    flag = false;
                    break;
                case "bDot":
                    //перевірити чи вже не вводилась 'крапка'
                    //якщо крапку вводили - нічого не робити
                    if (!tbCurr.Text.Contains('.'))
                    {
                        if (tbCurr.Text == "0" || flag)
                            tbCurr.Text = "0" + ((sender as Button).Content).ToString();
                        else
                            tbCurr.Text += ((sender as Button).Content).ToString();
                        flag = false;
                    }
                    else return;
                    break;
                case "bCE":
                    tbCurr.Text = "0";
                    break;
                case "bC":
                    stackOperator.Clear();
                    stackResult.Clear();
                    tbCurr.Clear();
                    tbPrev.Clear();
                    tbCurr.Text = "0";
                    error = false;
                    flag = false;
                    break;
                case "bLT":
                    //тут індикатор забороняє стирати останню цифру тоді,
                    //якщо табло містить результат обчислень, а не результат вводу з клавіш
                    if (flag) return;
                    if (tbCurr.Text.Length > 1)
                        tbCurr.Text = tbCurr.Text.Substring(0, tbCurr.Text.Length - 1);
                    else
                        tbCurr.Text = "0";
                    break;
                case "bAdd":
                case "bSub":
                case "bDiv":
                case "bMul":
                case "bRes":
                    {
                        //змінна 'opTor' зберігає значення 'знака оператора', яке вона отримує
                        //зі стеку операторів
                        string opTor = (stackOperator.Count == 0) ? null : stackOperator.Pop();
                        if (opTor == null)
                        {
                            //блок обчислень
                            if (stackResult.Count == 0) result = Convert.ToDouble(tbCurr.Text);
                            else
                            {
                                switch ((sender as Button).Content.ToString())
                                {
                                    case "+": result = stackResult.Pop() + Convert.ToDouble(tbCurr.Text);
                                        break;
                                    case "-": result = stackResult.Pop() - Convert.ToDouble(tbCurr.Text);
                                        break;
                                    case "/": result = stackResult.Pop() / Convert.ToDouble(tbCurr.Text);
                                        break;
                                    case "*": result = stackResult.Pop() * Convert.ToDouble(tbCurr.Text);
                                        break;
                                }
                            }
                            //блок виводу на два табла та дій із стеками
                            //поведінка залежитиме від знаку '='
                            if ((sender as Button).Content.ToString() != "=")
                            {
                                tbPrev.Text += " " + tbCurr.Text + " " + ((sender as Button).Content).ToString();
                                stackOperator.Push(((sender as Button).Content).ToString());
                                stackResult.Push(result);
                            }
                            else
                            {
                                tbPrev.Clear();
                                stackResult.Clear();
                            }
                            tbCurr.Text = result.ToString();
                            flag = true;
                        }
                        else
                        {
                            //блок обчислень
                            switch (opTor)
                            {
                                case "+": result = stackResult.Pop() + Convert.ToDouble(tbCurr.Text);
                                    break;
                                case "-": result = stackResult.Pop() - Convert.ToDouble(tbCurr.Text);
                                    break;
                                case "/": result = stackResult.Pop() / Convert.ToDouble(tbCurr.Text);
                                    break;
                                case "*": result = stackResult.Pop() * Convert.ToDouble(tbCurr.Text);
                                    break;
                            }
                            //блок виводу на два табла та дій із стеками
                            //поведінка залежитиме від знаку '='
                            if ((sender as Button).Content.ToString() != "=")
                            {
                                tbPrev.Text += " " + tbCurr.Text + " " + ((sender as Button).Content).ToString();
                                stackOperator.Push(((sender as Button).Content).ToString());
                                stackResult.Push(result);
                            }
                            else
                            {
                                tbPrev.Clear();
                                stackResult.Clear();
                            }
                            tbCurr.Text = result.ToString();
                            flag = true;
                        }
                        break;
                    }
            }
            //перевірка на помилку обчислень
            if (result == Double.PositiveInfinity || 
                result == Double.NegativeInfinity ||
                tbCurr.Text == "NaN") 
            {
                tbCurr.Text = "Error";
                result = 0.0;
                error = true;
            } 
        }
    }
}