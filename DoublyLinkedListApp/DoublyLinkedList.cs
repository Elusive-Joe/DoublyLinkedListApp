using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublyLinkedListApp
{
    class DoublyLinkedList
    {
        public Person First { get; set; } //Первый
        public Person Current { get; set; }//Текущий
        public uint Count { get; set; } //Размерчик.

        //Конструктор
        public DoublyLinkedList()
        {
            Count = 0;
            First = Current = null;
        }

        //проверка на пустоту
        public bool IsEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        //вставить по индексу
        public void InsertByIndex(string ln, uint h, DateTime bd, uint index)
        {
            if (index < 1 || index > (Count + 1)) //вброс ошибки, если неправильный индекс
            {
                throw new InvalidOperationException();
            }
            else if (index == 1) //если начало
            {
                Push_Front(ln, h, bd);
            }
            else if (index == (Count + 1)) //если конец
            {
                Push_Back(ln, h, bd);
            }
            else //иначе ищем элемент с таким индексом
            {
                uint count = 1;
                Current = First;
                while (Current != null && count != index)
                {
                    Current = Current.Next;
                    count++;
                }
                Person newPerson = new Person(ln, h, bd); //создаем объект
                Current.Prev.Next = newPerson;
                newPerson.Prev = Current.Prev;
                Current.Prev = newPerson;
                newPerson.Next = Current;
                Count++;
            }
        }

        //Вставить в начало
        public void Push_Front(string ln, uint h, DateTime bd)
        {
            Person newPerson = new Person(ln, h, bd);

            if (First == null)
            {
                First = newPerson;
            }
            else
            {
                newPerson.Next = First;
                First = newPerson; //First и newPerson указывают на один и тот же объект
                newPerson.Next.Prev = First;
            }
            Count++;
        }

        //Удалить из начала
        public Person Pop_Front()
        {
            if (First == null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                Person temp = First;
                if (First.Next != null)
                {
                    First.Next.Prev = null;
                }
                First = First.Next;
                Count--;
                return temp;
            }
        }

        //Вставить в конец
        public void Push_Back(string ln, uint h, DateTime bd)
        {
            Person newPerson = new Person(ln, h, bd);
            Current = GetLast();//Поиск последнего элемента
            Current.Next = newPerson;
            newPerson.Prev = Current;
            Current = newPerson;
            Count++;
        }

        //Удалить последний элемент
        public Person Pop_Back()
        {
            Current = GetLast();//Поиск последнего элемента
            Person temp = Current;
            if (Current.Prev != null)
            {
                Current.Prev.Next = null;
            }
            Current = Current.Prev;
            Count--;
            return temp;

        }

        //Поиск последнего элемента
        public Person GetLast()
        {
            Current = First;
            while (Current.Next != null)
            {
                Current = Current.Next;
            }
            return Current;
        }

        //полностью очистить список
        public void ClearList()
        {
            while (!IsEmpty)
            {
                Pop_Front();
            }
        }

        //вывести в прямом порядке
        public void Display()
        {
            //Console.Clear();
            if (First == null)
            {
                Console.WriteLine("Doubly Linked List is empty");
                Console.ReadKey();
                return;
            }
            Current = First;
            uint count = 1;
            while (Current != null)
            {
                Console.Write("Person " + count + " : ");
                Current.Output();
                count++;
                Current = Current.Next;
            }
            //Console.ReadKey();
        }

        //вывести в обратном порядке
        public void ReverseDisplay()
        {
            //Console.Clear();
            if (First == null)
            {
                Console.WriteLine("Doubly Linked List is empty");
                Console.ReadKey();
                return;
            }

            Current = GetLast();
            uint count = 1;
            while (Current != null)
            {
                Console.Write("Element " + count + " : ");
                Current.Output();
                count++;
                Current = Current.Prev;
            }
            //Console.ReadKey();
        }

        //удалить элемент по индексу
        public void DelById(uint index)
        { 
            if (index < 1 || index > Count)
            {
                throw new InvalidOperationException();
            }
            else if (index == 1)
            {
                Pop_Front();
            }
            else if (index == Count)
            {
                Pop_Back();
            }
            else
            {
                uint count = 1;
                Current = First;
                while (Current != null && count != index)
                {
                    Current = Current.Next;
                    count++;
                }
                Current.Prev.Next = Current.Next;
                Current.Next.Prev = Current.Prev;
                Count--;
            }
        }

        //Удалить по фамилии
        public uint DelByLastName(string ln)
        {
            uint count = 0;
            Current = First;
            while ((Current != null))
            {
                if (Current.LastName == ln)
                {
                    if (Current == First)
                    {
                        Pop_Front();
                    }
                    else if (Current.Next == null)
                    {
                        Pop_Back();
                    }
                    else
                    {
                        Current.Prev.Next = Current.Next;
                        Current.Next.Prev = Current.Prev;
                        Count--;
                    }
                    count++;
                }
                Current = Current.Next;
            }
            return count;
        }

        //Считать из файла
        public void ReadFromFile(string path)
        {
            //string path = "Resources\\theList.txt";
            ClearList();
            try
            {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    uint count = 0;
                    uint type;
                    string ln = "";
                    uint h = 0;
                    DateTime bd;
                    while ((line = sr.ReadLine()) != null)
                    {
                        count++;
                        type = count % 3;
                        switch (type)
                        {
                            case 1:
                                ln = line;
                                break;
                            case 2:
                                h = uint.Parse(line);
                                break;
                            case 0:
                                bd = Convert.ToDateTime(line);
                                InsertByIndex(ln, h, bd, count / 3);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //Записать в файл
        public void WriteToFile(string path)
        {
            bool notFirst = false;
            try
            {
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path))
                    {
                        /*Byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);*/
                    }
                }
                Current = First;
                do
                {
                    string ln = Current.LastName;
                    string h = Current.Height.ToString();
                    string bd = Current.BirthDate.Day + "." + Current.BirthDate.Month + "." + Current.BirthDate.Year;
                    using (StreamWriter sw = new StreamWriter(path, notFirst, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(ln);
                        sw.WriteLine(h);
                        sw.WriteLine(bd);
                    }
                    Current = Current.Next;
                    if (!notFirst) notFirst = true;
                } while (Current != null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //Сортировка пузырьком. (По-моему, довольно трудно понять сортировку, если нет индексов)
        public void Sort()
        {
            Person tmp;
            bool bNotSorted = true; //не сортирован
            bool bSwapped; //была смена
            Current = First; //начинаем с начала
            while (bNotSorted) //пока список не сортирован
            {
                bNotSorted = false; //сейчас не известно. Может быть уже сортирован
                Current = First; //начинаем с начала
                while (Current.Next != null) //пока не достигнем последнего элемента
                {
                    bSwapped = false; //ещё не меняли
                    if (String.Compare(Current.LastName, Current.Next.LastName) > 0) //проверяем то поле, по которому сортируем
                    {
                        bNotSorted = true; //оказывается, список не сортирован
                        bSwapped = true; //смена произошла, вернее, произойдёт ниже.
                        if (Current != First) //Если текущий элемент - не первый, то
                        {
                            Current.Prev.Next = Current.Next; //элементом, следующим за предыдущим, становится элемент следующий за текущим,
                        }
                        else
                        {
                            First = Current.Next; //а если первый, то первым становится следующий за текущим.
                        }
                        Current.Next.Prev = Current.Prev; //Предыдущим перед следующим становится предыдущий перед текущим.
                        tmp = Current.Next.Next; //Временный элемент - следующий за следующим.

                        Current.Next.Next = Current; //Следующий за следующим становится текущим.

                        Current.Prev = Current.Next; //Предыдущий перед текущим становится следующим за текущим

                        Current.Next = tmp; //Следующему за текущим присваивается значение из временного.
                        if (Current.Next != null) //Если следующий - не пустой, то
                        {
                            Current.Next.Prev = Current; //предыдущий перед следующим - текущий.
                        }

                    }
                    if (!bSwapped) //Если смена была, то
                    {
                        Current = Current.Next;//переходим к следующему элементу.
                    }
                }
            }

        }

    }
}
