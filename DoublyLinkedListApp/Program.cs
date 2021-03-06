﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublyLinkedListApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            DoublyLinkedList personsList = new DoublyLinkedList();
            ConsoleKeyInfo cki;
            string dir = "";
            do
            {
                DrawMenu();
                cki = Console.ReadKey(true);
                switch (cki.Key)
                {
                    case ConsoleKey.D1:
                        Display(personsList, true);
                        break;
                    case ConsoleKey.NumPad1:
                        Display(personsList, true);
                        break;
                    case ConsoleKey.D2:
                        Display(personsList, false);
                        break;
                    case ConsoleKey.NumPad2:
                        Display(personsList, false);
                        break;
                    case ConsoleKey.D3:
                        AddPerson(personsList);
                        break;
                    case ConsoleKey.NumPad3:
                        AddPerson(personsList);
                        break;
                    case ConsoleKey.D4:
                        DeleteById(personsList);
                        break;
                    case ConsoleKey.NumPad4:
                        DeleteById(personsList);
                        break;
                    case ConsoleKey.D5:
                        DeleteByLastName(personsList);
                        break;
                    case ConsoleKey.NumPad5:
                        DeleteByLastName(personsList);
                        break;
                    case ConsoleKey.D6:
                        Sort(personsList);
                        break;
                    case ConsoleKey.NumPad6:
                        Sort(personsList);
                        break;
                    case ConsoleKey.D7:
                        dir = Wtf(dir, personsList, true);
                        break;
                    case ConsoleKey.NumPad7:
                        dir = Wtf(dir, personsList, true);
                        break;
                    case ConsoleKey.D8:
                        dir = Wtf(dir, personsList, false);
                        break;
                    case ConsoleKey.NumPad8:
                        dir = Wtf(dir, personsList, false);
                        break;
                }
            } while ((cki.Key != ConsoleKey.Escape) && (cki.Key != ConsoleKey.D9) && (cki.Key != ConsoleKey.NumPad9));
        }
        
        //Нарисовать меню
        private static void DrawMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Вывести на экран все элементы двусвязного списка");
            Console.WriteLine("2. Вывести на экран все элементы двусвязного списка в обратном порядке");
            Console.WriteLine("3. Добавить элемент в список по индексу");
            Console.WriteLine("4. Удалить элемент из списка по индексу");
            Console.WriteLine("5. Удалить все элементы с заданной фамилией");
            Console.WriteLine("6. Сортировка элементов списка по Фамилии");
            Console.WriteLine("7. Сохранение списка в файл");
            Console.WriteLine("8. Загрузка списка из файла");
            Console.WriteLine("9. Выход");
        }
       
        //Вывести на консоль
        private static void Display(DoublyLinkedList pList, bool order)
        {
            Console.Clear();
            if (!pList.IsEmpty)
            {
                if (order)
                {
                    Console.WriteLine("Вывод в прямом порядке:\n");
                    pList.Display();
                }
                else
                {
                    Console.WriteLine("Вывод в обратном порядке:\n");
                    pList.ReverseDisplay();
                }
                Console.WriteLine("\nDone. Press any key");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("Список пуст");
                Console.ReadKey(true);
            }
        }

        //Добавть Person
        private static void AddPerson(DoublyLinkedList personsList)
        {
            Console.Clear();
            Console.WriteLine("Добавление по индексу");
            ConsoleKeyInfo cki;
            do
            {
                string str;
                uint ind;
                do
                {
                    Console.Write("\nИндекс (от 1 до {0}): ", personsList.Count + 1);
                    str = Console.ReadLine();
                    while (!uint.TryParse(str, out ind)) //должно быть число
                    {
                        Console.WriteLine("\nНеверный ввод");
                        Console.Write("Индекс (от 1 до {0}): ", personsList.Count + 1);
                        str = Console.ReadLine();
                    }
                } while ((ind<1)||(ind > personsList.Count +1)); //от 1 до размер списка+1
                
                Console.Write("Фамилия: ");
                string ln = Console.ReadLine();

                Console.Write("Рост: ");
                str = Console.ReadLine();
                uint h;
                while (!uint.TryParse(str, out h)) //должно быть число
                {
                    Console.WriteLine("\nНеверный тип");
                    Console.Write("Рост: ");
                    str = Console.ReadLine();
                }

                Console.Write("Дата рождения: ");
                str = Console.ReadLine();
                bool correct = false;
                DateTime bd = new DateTime(1900, 1, 1);
                while (!correct)
                {
                    try
                    {
                        bd = Convert.ToDateTime(str); //попытка перевода строки в дату
                        correct = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\n" + ex.Message);
                        Console.Write("Дата рождения: ");
                        str = Console.ReadLine();
                    }
                }

                personsList.InsertByIndex(ln, h, bd, ind);

                Console.WriteLine("\nСделано. Ещё одного? (y/n)");
                do
                {
                    cki = Console.ReadKey(true);
                } while ((cki.Key!=ConsoleKey.Y)&&(cki.Key != ConsoleKey.N));
            } while (cki.Key==ConsoleKey.Y);
            
        }
        
        //Удалить по номеру
        private static void DeleteById(DoublyLinkedList pList)
        {
            Console.Clear();
            Console.Write("Удаление по индексу: ");
            ConsoleKeyInfo cki;
            do
            {
                uint ind;
                do
                {
                    Console.Write("\nИндекс (от 1 до {0}): ", pList.Count);
                    string str = Console.ReadLine();
                    while (!uint.TryParse(str, out ind))
                    {
                        Console.WriteLine("\nНеверный ввод");//проверяем, что это число,
                        Console.Write("\nИндекс (от 1 до {0}): ", pList.Count);
                        str = Console.ReadLine();
                    }
                } while ((ind < 1) || (ind > pList.Count));//не выходящее за размер списка.
                pList.DelById(ind);
                Console.WriteLine("\nСделано. Ещё одного? (y/n)");
                do
                {
                    cki = Console.ReadKey();
                } while ((cki.Key != ConsoleKey.Y) && (cki.Key != ConsoleKey.N));
            } while (cki.Key == ConsoleKey.Y);
        }
        
        //Удалить по фамилии
        private static void DeleteByLastName(DoublyLinkedList pList)
        {
            Console.Clear();
            Console.Write("Удаление по фамилии: ");
            string ln = Console.ReadLine();
            uint delco = pList.DelByLastName(ln);
            Console.WriteLine("Done. Удалений : {0}. Press any key", delco);
            Console.ReadKey(true);
        }
        
        //Сортировать
        private static void Sort(DoublyLinkedList pList)
        {
            if (!pList.IsEmpty)
            {
                pList.Sort();
                Console.Clear();
                Console.WriteLine("Done. Press any key");
                Console.ReadKey(true);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Список пуст");
                Console.ReadKey(true);
            }
            
        }
        
        //Записать в файл или считать из файла
        private static string Wtf(string dir, DoublyLinkedList pList, bool write)//true - пишем, false - читаем
        {
            //Не знаю, почему сделал интерфейс в этом методе на английском - захотелось.
            ConsoleKeyInfo cki;
            do
            {
                Console.Clear();
                Console.WriteLine("Working dir is: {0}\n", dir);
                Console.WriteLine("1. Set working directory (existing)"); /*Можно было бы, конечно написать анализатор (parser),
                чтобы создавать папки и подпапки, но задание, вроде, не об этом*/
                Console.WriteLine(" or");
                Console.WriteLine("2. Enter filename");
                Console.WriteLine(" or");
                Console.WriteLine("3. Back");
                cki = Console.ReadKey(true);
                string path;
                string filename;
                switch (cki.Key)
                {
                    case ConsoleKey.D1:
                        Console.Write("\nEnter the path to an existing directory: ");
                        dir = Console.ReadLine()+"\\";
                        break;
                    case ConsoleKey.NumPad1:
                        Console.Write("\nEnter the path to an existing directory: ");
                        dir = Console.ReadLine()+"\\";
                        break;
                    case ConsoleKey.D2:
                        Console.Write("\nEnter filename: ");
                        filename = Console.ReadLine();
                        while (filename=="")
                        {
                            Console.Write("Please, enter filename: ");
                            filename = Console.ReadLine();
                        }
                        if (!filename.EndsWith(".txt"))
                        {
                            filename += ".txt";
                        }
                        path = dir + filename;
                        if (write)
                        {
                            pList.WriteToFile(path);
                        }
                        else
                        {
                            pList.ReadFromFile(path);
                        }
                        Console.WriteLine("\nDone. Press any key");
                        Console.ReadKey(true);
                        return dir;
                    case ConsoleKey.NumPad2:
                        Console.Write("\nEnter filename: ");
                        filename = Console.ReadLine();
                        while (filename == "")
                        {
                            Console.Write("Please, enter filename: ");
                            filename = Console.ReadLine();
                        }
                        if (!filename.EndsWith(".txt"))
                        {
                            filename += ".txt";
                        }
                        path = dir + filename;
                        if (write)
                        {
                            pList.WriteToFile(path);
                        }
                        else
                        {
                            pList.ReadFromFile(path);
                        }
                        Console.WriteLine("\nDone. Press any key");
                        Console.ReadKey(true);
                        return dir;
                }
            } while ((cki.Key != ConsoleKey.Escape) && (cki.Key != ConsoleKey.D3) && (cki.Key != ConsoleKey.NumPad3));
            return dir;
        }
    }
}

