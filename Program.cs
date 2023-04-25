using System;
using System.IO;

namespace TRPO_LR1._1
{
    //Singleton Log class
    // Класс Одиночка предоставляет метод `GetInstance`, который ведёт себя как
    // альтернативный конструктор и позволяет клиентам получать один и тот же
    // экземпляр класса при каждом вызове.
    public class Log
    {
        // Конструктор Одиночки всегда должен быть скрытым, чтобы предотвратить
        // создание объекта через оператор new.
        private Log() { }

        // Объект одиночки храниться в статичном поле класса. 
        private static Log _instance;

        // Это статический метод, управляющий доступом к экземпляру одиночки.
        // При первом запуске, он создаёт экземпляр одиночки и помещает его в
        // статическое поле. При последующих запусках, он возвращает клиенту
        // объект, хранящийся в статическом поле.
        public static Log GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Log();
            }
            return _instance;
        }

        // Логика логирования
        public void WriteToLog(string msg, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("  :");
            w.WriteLine($"  :{msg}");
            w.WriteLine("-------------------------------");
        }
    }

    // Родительский класс сотрудников
    public class Employee
    {
        public string Name { get; set; }
        public Employee(string name)
        {
            Name = name;
        }

        public virtual void MakeDrink(string a) { }
        public virtual void CleanTables() { }

    }

    // Класс бариста является подсистемой класса фасад
    // Подсистема может принимать запросы либо от фасада, либо от клиента
    // напрямую.
    public class Barista : Employee
    {
        public Log log = Log.GetInstance();
        //конструктор бариста
        public Barista(string name) : base(name) { }
        

        public override void MakeDrink(string a)
        {
            string str = $"Бариста {Name} берется за приготовление напитка";
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }

            switch (a)
            {
                case "1":
                    ClientTM.ClientCode(new ConcreteLatte());
                    break;
                case "2":
                    ClientTM.ClientCode(new ConcreteCappuccino());
                    break;
                case "3":
                    ClientTM.ClientCode(new ConcreteFlat());
                    break;
            }
        }

        public override void CleanTables()
        {
            string str = $"Бариста {Name} протирает столы";
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }
        }

    }

    // Facade class
    public class Facade
    {
        protected Barista _subsystem1;

        public Facade(Barista subsystem1)
        {
            this._subsystem1 = subsystem1;
        }

        // Методы Фасада удобны для быстрого доступа к сложной функциональности
        // подсистем. Однако клиенты получают только часть возможностей
        // подсистемы.
        public void CompleteService(string a)
        {
            _subsystem1.MakeDrink(a);
            _subsystem1.CleanTables();
        }

    }

    //Facade client class
    class ClientF
    {
        // Клиентский код работает со сложными подсистемами через простой
        // интерфейс, предоставляемый Фасадом.
        public static void ClientCode(Facade facade, string a)
        {
            facade.CompleteService(a);
        }
    }

    //TM class
    abstract public class AbstractTM
    {
        public Log log = Log.GetInstance();
        // Шаблонный метод определяет скелет алгоритма.
        public void TemplateMethod()
        {
            string str = "Начинаем готовить напиток!";
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }

            this.TakeCup();
            this.MakeEspresso();
            this.PrepareMilk();
            this.PourMilk();
        }

        // Эти операции уже имеют реализации.
        protected void PourMilk()
        {
            string str = "Вливаем молоко, готово!";
            Console.WriteLine(str);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }
        }

        // А эти операции должны быть реализованы в подклассах.
        protected abstract void TakeCup();

        protected abstract void MakeEspresso();
        protected abstract void PrepareMilk();

    }

    // Конкретные классы должны реализовать все абстрактные операции базового
    // класса. Они также могут переопределить некоторые операции с реализацией
    // по умолчанию.
    class ConcreteLatte : AbstractTM
    {
        protected override void TakeCup()
        {
            string str = "Берем чашку 300 мл";
            Console.WriteLine(str);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }
        }

        protected override void MakeEspresso()
        {
            string str = "Делаем одинарный эспрессо";
            Console.WriteLine(str);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }
        }

        protected override void PrepareMilk()
        {
            string str = "Подготавливаем 250 мл молока";
            Console.WriteLine(str);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }
        }
    }

    // Обычно конкретные классы переопределяют только часть операций базового
    // класса.
    class ConcreteCappuccino : AbstractTM
    {
        protected override void TakeCup()
        {
            string str = "Берем чашку 200 мл";
            Console.WriteLine(str);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }
        }

        protected override void MakeEspresso()
        {
            string str = "Делаем одинарный эспрессо";
            Console.WriteLine(str);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }
        }

        protected override void PrepareMilk()
        {
            string str = "Подготавливаем 125 мл молока";
            Console.WriteLine(str);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }
        }
    }

    class ConcreteFlat : AbstractTM
    {
        protected override void TakeCup()
        {
            string str = "Берем чашку 200 мл";
            Console.WriteLine(str);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }
        }

        protected override void MakeEspresso()
        {
            string str = "Делаем двойной эспрессо";
            Console.WriteLine(str);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }
        }

        protected override void PrepareMilk()
        {
            string str = "Подготавливаем 90 мл молока";
            Console.WriteLine(str);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                log.WriteToLog(str, w);
            }
        }
    }

    class ClientTM
    {
        // Клиентский код вызывает шаблонный метод для выполнения алгоритма.
        // Клиентский код не должен знать конкретный класс объекта, с которым
        // работает, при условии, что он работает с объектами через интерфейс их
        // базового класса.
        public static void ClientCode(AbstractTM abstractClass)
        {
            abstractClass.TemplateMethod();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Barista ivan = new Barista("Иван");
            Facade facade = new Facade(ivan);

            Console.WriteLine("Добрый день! Что будете сегодня?");
            Console.WriteLine("1.Латте\n2.Капучино\n3.Флэт Уайт");
            string a = Console.ReadLine();

            ClientF.ClientCode(facade,a);
        }
    }
}
