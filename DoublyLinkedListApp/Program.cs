using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublyLinkedListApp
{
    class Program
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
                        personsList.Display();
                        break;
                    case ConsoleKey.NumPad1:
                        personsList.Display();
                        break;
                    case ConsoleKey.D2:
                        personsList.ReverseDisplay();
                        break;
                    case ConsoleKey.NumPad2:
                        personsList.ReverseDisplay();
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
                        personsList.Sort();
                        break;
                    case ConsoleKey.NumPad6:
                        personsList.Sort();
                        break;
                    case ConsoleKey.D7:
                        dir = Wtf(dir, personsList);
                        break;
                    case ConsoleKey.NumPad7:
                        dir = Wtf(dir, personsList);
                        break;
                    case ConsoleKey.D8:
                        personsList.ReadFromFile();
                        break;
                    case ConsoleKey.NumPad8:
                        personsList.ReadFromFile();
                        break;
                }
            } while ((cki.Key != ConsoleKey.Escape) && (cki.Key != ConsoleKey.D9) && (cki.Key != ConsoleKey.NumPad9));
        }
        //Нарисовать меню
        static public void DrawMenu()
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
        
        //Добавть Person
        static public void AddPerson(DoublyLinkedList personsList)
        {
            Console.Clear();
            Console.WriteLine("Добавление по индексу");
            Console.Write("Индекс: ");
            string str = Console.ReadLine();
            uint ind;
            while (!uint.TryParse(str, out ind))
            {
                Console.WriteLine("\nНеверный тип");
                Console.Write("Индекс: ");
                str = Console.ReadLine();
            }
            Console.Write("Фамилия: ");
            string ln = Console.ReadLine();
            Console.Write("Рост: ");
            str = Console.ReadLine();
            uint h;
            while (!uint.TryParse(str, out h))
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
                    bd = Convert.ToDateTime(str);
                    correct = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n"+ex.Message);
                    Console.Write("Дата рождения: ");
                    str = Console.ReadLine();
                }
            }
            
            personsList.InsertByIndex(ln, h, bd, ind);
        }
        
        //Удалить по номеру
        static public void DeleteById(DoublyLinkedList pList)
        {
            Console.Clear();
            Console.Write("Удаление по индексу: ");
            uint ind = UInt32.Parse(Console.ReadLine());
            pList.DelById(ind);
            Console.WriteLine("Done. Press any key");
            Console.ReadKey();
        }
        
        //Удалить по фамилии
        static public void DeleteByLastName(DoublyLinkedList pList)
        {
            Console.Clear();
            Console.Write("Удаление по фамилии: ");
            string ln = Console.ReadLine();
            uint delco = pList.DelByLastName(ln);
            Console.WriteLine("Done. Удалений : {0}. Press any key", delco);
            Console.ReadKey();
        }
        
        //Записать в файл
        static public string Wtf(string dir, DoublyLinkedList pList)
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
                switch (cki.Key)
                {
                    case ConsoleKey.D1:
                        Console.Write("\nEnter the path to an existing directory: ");
                        dir = Console.ReadLine();
                        break;
                    case ConsoleKey.NumPad1:
                        Console.Write("\nEnter the path to an existing directory: ");
                        dir = Console.ReadLine();
                        break;
                    case ConsoleKey.D2:
                        Console.Write("\nEnter filename: ");
                        path = dir + Console.ReadLine();
                        pList.WriteToFile(path);
                        Console.WriteLine("\nDone. Press any key");
                        Console.ReadKey();
                        return dir;
                        break;
                    case ConsoleKey.NumPad2:
                        Console.Write("\nEnter filename: ");
                        path = dir + Console.ReadLine();
                        pList.WriteToFile(path);
                        Console.WriteLine("\nDone. Press any key");
                        Console.ReadKey();
                        return dir;
                        break;
                }
            } while ((cki.Key != ConsoleKey.Escape) && (cki.Key != ConsoleKey.D3) && (cki.Key != ConsoleKey.NumPad3));
            return dir;
        }
    }
}

