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
        private Person First { get; set; } //Первый
        private Person Current { get; set; }//Текущий
        public uint Count { get; private set; } //Размерчик.

        //Конструктор
        public DoublyLinkedList()
        {
            Count = 0;
            First = Current = null;
        }

        //проверка на пустоту
        public bool IsEmpty => Count == 0;

        //вставить по индексу
        public void InsertByIndex(string ln, uint h, DateTime bd, uint index)
        {
            if (index < 1 || index > (Count + 1)) //вброс ошибки, если неправильный индекс
            {
                throw new InvalidOperationException();
            }
            if (index == 1) //если начало
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
                if (Current != null)
                {
                    Current.Prev.Next = newPerson;
                    newPerson.Prev = Current.Prev;
                    Current.Prev = newPerson;
                    newPerson.Next = Current;
                }
                Count++;
            }
        }

        //Вставить в начало
        private void Push_Front(string ln, uint h, DateTime bd)
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
        private void Pop_Front()
        {
            if (First == null)
            {
                throw new InvalidOperationException();
            }
            if (First.Next != null)
            {
                First.Next.Prev = null;
            }
            First = First.Next;
            Count--;
        }

        //Вставить в конец
        private void Push_Back(string ln, uint h, DateTime bd)
        {
            Person newPerson = new Person(ln, h, bd);
            Current = GetLast();//Поиск последнего элемента
            Current.Next = newPerson;
            newPerson.Prev = Current;
            Current = newPerson;
            Count++;
        }

        //Удалить последний элемент
        private void Pop_Back()
        {
            Current = GetLast();//Поиск последнего элемента
            if (Current.Prev != null)
            {
                Current.Prev.Next = null;
            }
            Current = Current.Prev;
            Count--;
        }

        //Поиск последнего элемента
        private Person GetLast()
        {
            Current = First;
            while (Current.Next != null)
            {
                Current = Current.Next;
            }
            return Current;
        }

        //полностью очистить список
        private void ClearList()
        {
            while (!IsEmpty)
            {
                Pop_Front();
            }
        }

        //вывести в прямом порядке
        public void Display()
        {
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
        }

        //вывести в обратном порядке
        public void ReverseDisplay()
        {
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
        }

        //удалить элемент по индексу
        public void DelById(uint index)
        {
            if (index < 1 || index > Count)
            {
                throw new InvalidOperationException();
            }
            if (index == 1)
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
                if (Current != null)
                {
                    Current.Prev.Next = Current.Next;
                    Current.Next.Prev = Current.Prev;
                }
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
            ClearList();
            try
            {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    uint count = 0;
                    string ln = "";
                    uint h = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        count++;
                        uint type = count % 3;
                        switch (type)
                        {
                            case 1:
                                ln = line;
                                break;
                            case 2:
                                h = uint.Parse(line);
                                break;
                            case 0:
                                DateTime bd = Convert.ToDateTime(line);
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
                    using (File.Create(path))
                    {
                    }
                }
                Current = First;
                do
                {
                    string ln = Current.LastName;
                    string h = Current.Height.ToString();
                    string bd = Current.BirthDate.Day + "." + Current.BirthDate.Month + "." + Current.BirthDate.Year;
                    using (StreamWriter sw = new StreamWriter(path, notFirst, Encoding.Default))
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
            bool bNotSorted = true; //не сортирован
            Current = First; //начинаем с начала
            while (bNotSorted) //пока список не сортирован
            {
                bNotSorted = false; //сейчас не известно. Может быть уже сортирован
                Current = First; //начинаем с начала
                while (Current.Next != null) //пока не достигнем последнего элемента
                {
                    bool bSwapped = false; //ещё не меняли
                    if (String.Compare(Current.LastName, Current.Next.LastName) > 0) //проверяем то поле, по которому сортируем (понятно, что culture-specific, и хорошо)
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
                        Person tmp = Current.Next.Next;

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
