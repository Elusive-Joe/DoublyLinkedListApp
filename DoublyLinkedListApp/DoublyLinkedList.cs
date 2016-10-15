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
        public Person First { get; set; }
        public Person Current { get; set; }
        public uint Count { get; set; } //Размерчик.

        public DoublyLinkedList()
        {
            Count = 0;
            First = Current = /*Last =*/ null;
        }

        public bool IsEmpty //проверка на пустоту
        {
            get
            {
                return Count == 0;
            }
        }

        public void InsertByIndex(string ln, string h, string bd, uint index) //вставить по индекусу
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

        public void Push_Front(string ln, string h, string bd)
        {
            Person newPerson = new Person(ln, h, bd);

            if (First == null)
            {
                First = /*Last =*/ newPerson;
            }
            else
            {
                newPerson.Next = First;
                First = newPerson; //First и newNode указывают на один и тот же объект
                newPerson.Next.Prev = First;
            }
            Count++;
        }

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

        public void Push_Back(string ln, string h, string bd)
        {
            Person newPerson = new Person(ln, h, bd);

            /*if (First == null)
            {
                First = Last = newNode;
            }*/
            Current = First;
            while (Current.Next != null)
            {
                Current = Current.Next;
            }
            Current.Next = newPerson;
            newPerson.Prev = Current;
            Current = newPerson;
            Count++;
        }

        public Person Pop_Back() //удалить последний
        {
            /*if (Last == null)
            {
                throw new InvalidOperationException();
            }*/
            Current = First;
            while (Current.Next != null)
            {
                Current = Current.Next;
            }

            Person temp = Current;
            if (Current.Prev != null)
            {
                Current.Prev.Next = null;
            }
            Current = Current.Prev;
            Count--;
            return temp;

        }

        public void ClearList() //полностью очистить список
        {
            while (!IsEmpty)
            {
                Pop_Front();
            }
        }

        public void Display() //вывести в прямом порядке
        {
            Console.Clear();
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
            Console.ReadKey();
        }

        public void ReverseDisplay() //вывести в обратном порядке
        {
            Console.Clear();
            if (First == null)
            {
                Console.WriteLine("Doubly Linked List is empty");
                Console.ReadKey();
                return;
            }

            Current = First;
            while (Current.Next != null)
            {
                Current = Current.Next;
            }

            uint count = 1;
            while (Current != null)
            {
                Console.Write("Element " + count + " : ");
                Current.Output();
                count++;
                Current = Current.Prev;
            }
            Console.ReadKey();
        }

        public void DelById(uint index)
        { //удалить элемент по индексу
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

        public uint GetIndex(object Data) //достать индекс по значению элемента
        {
            Current = First;
            uint index = 1;
            while (Current != null)
            {
                Current = Current.Next;
                index++;
            }
            return index;

        }

        public void ReadFromFile()
        {
            string path = "Resources\\theList.txt";
            ClearList();
            try
            {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    uint count = 0;
                    uint type;
                    string ln = "";
                    string h = "";
                    string bd = "";
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
                                h = line;
                                break;
                            case 0:
                                bd = line;
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
        public void Sort()
        {
            Person tmp;
            bool bNotSorted = true;
            bool bSwapped;
            Current = First;
            while (bNotSorted)
            {
                bNotSorted = false;
                Current = First;
                while (Current.Next != null)
                {
                    bSwapped = false;
                    if (String.Compare(Current.LastName, Current.Next.LastName) > 0)
                    {
                        bNotSorted = true;
                        bSwapped = true;
                        if (Current != First)
                        {
                            Current.Prev.Next = Current.Next;
                        }
                        else
                        {
                            First = Current.Next;
                        }
                        Current.Next.Prev = Current.Prev; //2
                        tmp = Current.Next.Next;

                        Current.Next.Next = Current; //3

                        Current.Prev = Current.Next; //4

                        Current.Next = tmp; //5
                        if (Current.Next != null)
                        {
                            Current.Next.Prev = Current; //6
                        }

                    }
                    if (!bSwapped)
                    {
                        Current = Current.Next;
                    }
                }
            }

        }

    }
}
