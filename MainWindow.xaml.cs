using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            //public int LenghtArray { get; private set; }
            //public void SetLenght(int NewLenght)
            //{
            //    LenghtArray = NewLenght;
            //}
            //public SortingArray(int СountElement)
            //{
            //    LenghtArray = СountElement;
            //    OriginalArray = new int[LenghtArray];
            //    //ArrayForSort = new int[LenghtArray];
            //}


            /// <summary>
            /// Создание нового массива для сортировки
            /// </summary>
            /// <returns></returns>
            int[] CreateNewArray(int LenghtArray)
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

            public int[] GetNewArray(int LenghtArray)
            {
                OriginalArray = CreateNewArray(LenghtArray);
           
                return GetArray();
            }
        }


        public MainWindow()
        {
            InitializeComponent();

            labelBubbleSpeed.Content = "";
            labelInsertionSpeed.Content = "";
            labelQuickSpeed.Content = "";
            labelShellSpeed.Content = "";

            textBoxSource.Text = string.Empty;
            textBoxSorted.Text = string.Empty;

            if (int.TryParse(textBoxCountElement.Text, out int count))
            { 
                Sort = new SortingArray();
                Sort.GetNewArray(count);
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


        static void BubleSort(int[] arr)
        {
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
        }


        static void InsertionSort(int[] arr)
        {
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
        }



        private void buttonSort_Click(object sender, RoutedEventArgs e)
        {

            //для создания массива длинной CountElement
            int CountElement = int.Parse(textBoxCountElement.Text);

            //если перед сортировкой нужно получить новый массив
            if ((checkBox.IsChecked == true) || (Sort.OriginalArray.Length != CountElement))
                Sort.GetNewArray(CountElement);
            
            //если нужно отобразить исходный массив
            if (checkBoxShowSourceArray.IsChecked == true)
            ShowArray(Sort.OriginalArray, textBoxSource);

            //этот массив будет передавать в методы для сортировки
            int[] ArrayForSort;

            //для измерения времени выполнения сортировок
            Stopwatch stopwatch;

            if (checkBoxBubble.IsChecked == true)
            {
                ArrayForSort = Sort.GetArray();

                stopwatch = Stopwatch.StartNew();
                BubleSort(ArrayForSort);
                stopwatch.Stop();

                labelBubbleSpeed.Content = stopwatch.ElapsedMilliseconds.ToString();
                //ShowArray(OriginalArray, textBoxSorted);
                if (!CheckSort(ArrayForSort))
                    labelBubbleSpeed.Content = "Ошибка сортировки!";
            }

            if (checkBoxInsertion.IsChecked == true)
            {
                ArrayForSort = Sort.GetArray();

                stopwatch = Stopwatch.StartNew();
                InsertionSort(ArrayForSort);
                stopwatch.Stop();

                labelInsertionSpeed.Content = stopwatch.ElapsedMilliseconds.ToString();
                // ShowArray(NewArray, textBoxSorted);
                if (!CheckSort(ArrayForSort))
                    labelInsertionSpeed.Content = "Ошибка сортировки!";
            }

            if (checkBoxQuick.IsChecked == true)
            {
                ArrayForSort = Sort.GetArray();

                stopwatch = Stopwatch.StartNew();
                QuickSort(ArrayForSort);
                stopwatch.Stop();

                labelQuickSpeed.Content = stopwatch.ElapsedMilliseconds.ToString();
                //ShowArray(ArrayForSort, textBoxSorted);
                if (!CheckSort(ArrayForSort))
                    labelQuickSpeed.Content = "Ошибка сортировки!";
            }

            if (checkBoxShell.IsChecked == true)
            {
                ArrayForSort = Sort.GetArray();

                stopwatch = Stopwatch.StartNew();
                ShellSort(ArrayForSort);
                stopwatch.Stop();

                labelShellSpeed.Content = stopwatch.ElapsedMilliseconds.ToString();
                
                if (checkBoxShowSortedArray.IsChecked == true )
                ShowArray(ArrayForSort, textBoxSorted);
                
                if (!CheckSort(ArrayForSort))
                    labelShellSpeed.Content = "Ошибка сортировки!";
            }
        }

        private void ShellSort(int[] arr)
        {
            int tmp;
            int d = arr.Length / 2;
            while (d >= 1)
            {
                for (int j = d; j < arr.Length; j++)
                {

                    int i = j;

                    while ((i >= d) && (arr[i - d] > arr[i]))
                    {
                        tmp = arr[j - d];
                        arr[i - d] = arr[i];
                        arr[i] = tmp;
                        i -= d;
                    }                   
                }
                d /= 2;
            }
        }

        static void QuickSort(int[] arr)
        {
            Sort(arr, 0, arr.Length - 1);
            
            void Sort(int[] arrInner, int first, int last)
            {
                //задаем опорный эллемент
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
