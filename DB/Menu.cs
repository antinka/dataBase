using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DB
{
    class Menu
    {
        private string type;
        List<Employee> employees;
        private ISerializable serializable;
        public void Start()
        {
            try
            {
                using (FileStream fs = File.OpenRead("option.ini"))
                {
                    byte[] array = new byte[fs.Length];
                    fs.Read(array, 0, array.Length);
                    type = System.Text.Encoding.Default.GetString(array).ToUpper();
                    if (type == "XML")
                    {
                        serializable = new XmlSerialization();
                    }
                    else
                    {
                        serializable = new BinarySerialization();
                    }
                    LoadDatabase(serializable);
                }
            }
           catch (FileNotFoundException ex)
            {
                type = "BIN";
                using (FileStream fs = new FileStream("option.ini", FileMode.Create))
                {
                    byte[] array = System.Text.Encoding.Default.GetBytes(type);
                    fs.Write(array, 0, array.Length);
                }
                serializable = new BinarySerialization();
                LoadDatabase(serializable);
            }
           /* catch (FileNotFoundException ex)
            {
                type = "XML";
                using (FileStream fs = new FileStream("option.ini", FileMode.Create))
                {
                    byte[] array = System.Text.Encoding.Default.GetBytes(type);
                    fs.Write(array, 0, array.Length);
                }
                serializable = new XmlSerialization();
                LoadDatabase(serializable);
            }*/
        }
        private void LoadDatabase(ISerializable ser)
        {
            employees = ser.Read();
            ShowMenu();
        }
        private void ShowMenu()
        {
            while (true)
            {
                string str;
                if (employees.Count == 0)
                {
                    Console.WriteLine("База данных пуста\n");
                    Console.WriteLine("Доступные действие:");
                    Console.WriteLine("\t1 - добавить сотрудника;");
                    Console.WriteLine("\t2 - выход");

                    str = Console.ReadLine();
                    switch (str)
                    {
                        case "1":
                            AddEmployee();
                            break;
                        case "2":
                            Exit(serializable);
                            return;
                        default:
                            continue;
                    }
                }
                else
                {
                    Console.WriteLine("База данных была загружена.\n");
                    Console.WriteLine("Доступные действия:");
                    Console.WriteLine("\t1 - Добавить сотрудника;");
                    Console.WriteLine("\t2 - Удалить сотрудника;");
                    Console.WriteLine("\t3 - Информация о сотруднике по ID;");
                    Console.WriteLine("\t4 - Показать список сотрудников;");
                    Console.WriteLine("\t5 - выход");
                }
                str = Console.ReadLine();
                switch (str)
                {
                    case "1":
                        AddEmployee();
                        break;
                    case "2":
                        RemoveEmployee();
                        break;
                    case "3":
                        GetEmployeeById();
                        break;
                    case "4":
                        GetEmployees();
                        break;
                    case "5":
                        Exit(serializable);
                        return;
                    default:
                        Console.WriteLine("Введите одну из команд!");
                        Console.WriteLine();
                        continue;
                }
            }
        }
        private void AddEmployee()
        {
            Employee emp;
            Console.WriteLine("Введите имя:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Введите фамилию:");
            string lastName = Console.ReadLine();

            if (employees.Count == 0)
            {
                emp = new Employee(firstName, lastName,1);
            }
            else
            {
                emp = new Employee(firstName, lastName, employees.Last().ID + 1);
            }
            employees.Add(emp);
            Console.WriteLine("Сотруник добавлен.\n ");
        }
        private void RemoveEmployee()
        {
            int delId = 0;
            Console.WriteLine("Введите ID сотрудника:");
            string str = Console.ReadLine();
            try
            {
                delId = Int32.Parse(str);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Неправильный формат записи,введите число\n ");
                return;
            }
            foreach (Employee e in employees)
            {
                if (e.ID == delId)
                {
                    employees.Remove(e);
                    Console.WriteLine("Сотрудник {0} {1} был удален\n", e.FirstName,e.LastName);
                    return;
                }
            }
            Console.WriteLine("Cотрудника c таким ID {0} не найдено\n", delId);
        }
        private void GetEmployeeById()
        {
            int empId = 0;
            Console.WriteLine("Введите ID сотрудника:");
            string str = Console.ReadLine();
            try
            {
                empId = Int32.Parse(str);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Неправильный формат записи,введите число\n ");
                return;
            }
            foreach (Employee e in employees)
            {
                if (e.ID == empId)
                {
                    Console.WriteLine("ID:" + e.ID + " \tИмя:\t" + e.FirstName + "\tФамилия:\t" + e.LastName);
                     Console.WriteLine("\n");
                    return;
                }
            }
            Console.WriteLine("Сотрудудника с c ID = {0} не найдено.\n", empId);
        }

        private void GetEmployees()
        {
            foreach (Employee e in employees)
            {
                Console.WriteLine("ID:"+e.ID+" \tИмя:\t"+e.FirstName+"\tФамилия:\t"+e.LastName);
            }
            Console.WriteLine("\n");
        }
        private void Exit(ISerializable ser)
        {
            ser.Write(employees);
        }
    }
}
