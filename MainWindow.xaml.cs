﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sorts
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static SortingArray Sort;
        public class SortingArray //для работы с массивами для сортировки
        {

            public int[] OriginalArray { get; private set; }

            public int LenghtArray { get; private set; }
            public void SetLenght(int NewLenght)
            {
                LenghtArray = NewLenght;
            }
            public SortingArray(int СountElement)
            {
                LenghtArray = СountElement;
                OriginalArray = new int[LenghtArray];
                //ArrayForSort = new int[LenghtArray];
            }


            /// <summary>
            /// Задание нового массива для сортировки
            /// </summary>
            /// <returns></returns>
            int[] SetNewArray()
            {
                int[] tmpArray = new int[LenghtArray];

                Random rnd = new Random();

                for (int i = 0; i<tmpArray.Length; i++)
                    tmpArray[i] = rnd.Next(0, tmpArray.Length);

                return tmpArray;
            }

            int[] GetArray(int[] SourceArray)
            {
                //int[] arr = new int[SourceArray.Length];
                //for (int i = 0; i < arr.Length; i++)
                //{
                //    arr[i] = SourceArray[i];
                //}

                //return arr;

                return (int[])SourceArray.Clone();
            }

            public int[] GetArray()
            {
                return GetArray(OriginalArray);
            }

            public int[] GetNewArray()
            {
                OriginalArray = SetNewArray();
           
                return GetArray();
            }
        }


        public MainWindow()
        {
            InitializeComponent();

            //int[] a1 = { 1, 2, 3 };
            //int[] a2 = new int[3];
            //a2 = (int[]) a1.Clone();
            //a2[0] = 22;
            
           // MessageBox.Show(a1[0].ToString());

            if (int.TryParse(textBoxCountElement.Text, out int count))
            { 
                Sort = new SortingArray(count);
                Sort.GetNewArray();
            }
               
            else
            {
                MessageBox.Show("Количество элементов массива должно быть числом");
                //buttonSort.IsEnabled = false;
            }

           // OriginalArray = GetNewArray(LenghtArray);
        }

        static void ShowArray(int[] arr, TextBox textBox)
        {
            textBox.Text = string.Empty;
            foreach (var i in arr)
            {
                textBox.Text += i + " ";
            }

        }


        static TimeSpan BubleSort(int[] arr)
        {
            DateTime TimeStart = DateTime.Now;
            for (int j = 0; j < arr.Length; j++)
            {
                bool flag = false;
                for (int i=0; i < arr.Length-j-1; i++)
                {
                if (arr[i] > arr[i+1])
                    {
                        flag = true;
                        int tmp = arr[i + 1];
                        arr[i + 1] = arr[i];
                        arr[i] = tmp;
                    }
                }
                //если перестановок больше не было, значит массив отсортирован
                if (!flag)  break;
            }
            return DateTime.Now - TimeStart;
        }


        static TimeSpan InsertionSort(int[] arr)
        {
            DateTime TimeStart = DateTime.Now;
            int tmp;
            for (int j = 0; j < arr.Length-1; j++)
            {
                if (arr[j+1] < arr[j])
                    {
                        tmp = arr[j + 1];
                    
                        int i = j;

                        while ((i >= 0) && (arr[i] > tmp ))
                        {
                            arr[i + 1] = arr[i];
                            i--;
                        }
                    
                        arr[++i] = tmp;     
                    }
            }
            return DateTime.Now - TimeStart;
        }



        private void buttonSort_Click(object sender, RoutedEventArgs e)
        {
            //SortingArray Sort = new SortingArray(int.Parse(textBoxCountElement.Text));

            TimeSpan speed;

           //если перед сортировкой нужно получить новый массив
            if ((checkBox.IsChecked == true) || (Sort.OriginalArray.Length != int.Parse(textBoxCountElement.Text)))
                Sort.GetNewArray();

            ShowArray(Sort.OriginalArray, textBox);

            int[] ArrayForSort = new int[Sort.OriginalArray.Length];

            if (checkBoxBubble.IsChecked == true)
            {
                ArrayForSort = Sort.GetArray();
                speed = BubleSort(ArrayForSort);

                labelBubbleSpeed.Content = speed.Milliseconds.ToString();
                //ShowArray(OriginalArray, textBox2);
                if (!CheckSort(ArrayForSort))
                    labelBubbleSpeed.Content = "Ошибка сортировки!";
            }

            if (checkBoxInsertion.IsChecked == true)
            {
                ArrayForSort = Sort.GetArray();
                speed = InsertionSort(ArrayForSort);

                labelInsertionSpeed.Content = speed.Milliseconds.ToString();
                // ShowArray(NewArray, textBox2);
                if (!CheckSort(ArrayForSort))
                    labelInsertionSpeed.Content = "Ошибка сортировки!";
            }

            if (checkBoxQuick.IsChecked == true)
            {
                ArrayForSort = Sort.GetArray();
                speed = QuickSort(ArrayForSort);

                labelQuickSpeed.Content = speed.Milliseconds.ToString();
                //ShowArray(ArrayForSort, textBox2);
                if (!CheckSort(ArrayForSort))
                    labelQuickSpeed.Content = "Ошибка сортировки!";
            }

            if (checkBoxShell.IsChecked == true)
            {
                ArrayForSort = Sort.GetArray();
                speed = ShellSort(ArrayForSort);

                labelShellSpeed.Content = speed.Milliseconds.ToString();
                ShowArray(ArrayForSort, textBox2);
                if (!CheckSort(ArrayForSort))
                    labelShellSpeed.Content = "Ошибка сортировки!";
            }
        }

        private TimeSpan ShellSort(int[] arr)
        {
            //throw new NotImplementedException();
            DateTime TimeStart = DateTime.Now;

            int tmp;
            int d = arr.Length / 2;
            for (int j = 0; j < arr.Length - 1; j++)
            {
                if (arr[j + d] < arr[j])
                {
                    tmp = arr[j + d];

                    int i = j;

                    while ((i >= 0) && (arr[i] > tmp))
                    {
                        arr[i + 1] = arr[i];
                        i--;
                    }

                    arr[++i] = tmp;

                }
                d /= 2;
            }

            return DateTime.Now - TimeStart;
        }

        static TimeSpan QuickSort(int[] arr)
        {
            DateTime TimeStart = DateTime.Now;

            Sort(arr, 0, arr.Length - 1);
            
            void Sort(int[] arrInner, int first, int last)
            {
                //kk++;
                int prop = arrInner[(last - first) / 2 + first];
                int i = first;
                int j = last;
                while ((i <= last) && (arrInner[i] < prop))  i++;
                while ((j >= first) && (arrInner[j] > prop)) j--;
              
                if (i<=j)
                {
                    int tmp = arrInner[j];
                    arrInner[j] = arrInner[i];
                    arrInner[i] = tmp;
                    ++i; --j;
                }

                if (j > first) Sort(arrInner, first, j);
                if (i < last) Sort(arrInner, i, last);
            }
                return DateTime.Now - TimeStart;
        }

        static bool CheckSort(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] < arr[i - 1])
                    return false;
            }
            return true;
        }
    }
}
