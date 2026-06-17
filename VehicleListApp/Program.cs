using System;

namespace VehicleListApp
{
    class Program
    {
        static BidirectionalList vehicleList = new BidirectionalList();
        static string filePath = "vehicles.json";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            // Базове наповнення для тесту
            vehicleList.AddAt(0, new VehicleRecord(VehicleType.Sedan, 1.5, true));   
            vehicleList.AddAt(1, new VehicleRecord(VehicleType.SUV, 2.5, true));     
            vehicleList.AddAt(2, new VehicleRecord(VehicleType.Hatchback, 1.2, true)); 
            vehicleList.AddAt(3, new VehicleRecord(VehicleType.Truck, 3.0, false));   

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                PrintTable();
                Console.WriteLine("\n=== МЕНЮ КЕРУВАННЯ АТД СПИСКОМ ===");
                Console.WriteLine("1. Додати елемент на n-позицію");
                Console.WriteLine("2. Видалити елемент з початку");
                Console.WriteLine("3. Змінити елемент за індексом");
                Console.WriteLine("4. Продемонструвати ітерацію списку");
                Console.WriteLine("5. Реверс списку");
                Console.WriteLine("6. Пошук (Електро/Гібрид < 2.0L)");
                Console.WriteLine("7. Зберегти у JSON (Серіалізація)");
                Console.WriteLine("8. Завантажити з JSON (Десеріалізація)");
                Console.WriteLine("0. Вихід");
                Console.Write("\nОберіть дію: ");

                string choice = Console.ReadLine();
                try
                {
                    switch (choice)
                    {
                        case "1": AddItemMenu(); break;
                        case "2":
                            var removed = vehicleList.RemoveFromStart();
                            Console.WriteLine($"\n[Успішно видалено]: {removed}");
                            Pause();
                            break;
                        case "3": EditItemMenu(); break;
                        case "4": DemonstrateIteration(); break;
                        case "5":
                            vehicleList.Reverse();
                            Console.WriteLine("\n[Список успішно реверсовано!]");
                            Pause();
                            break;
                        case "6": PerformSearch(); break;
                        case "7":
                            vehicleList.Serialize(filePath);
                            Console.WriteLine($"\n[Дані збережено у {filePath}]");
                            Pause();
                            break;
                        case "8":
                            vehicleList.Deserialize(filePath);
                            Console.WriteLine($"\n[Дані завантажено з {filePath}]");
                            Pause();
                            break;
                        case "0": exit = true; break;
                        default: Console.WriteLine("\nПомилка: Невідомий пункт."); Pause(); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[ПОМИЛКА]: {ex.Message}");
                    Console.ResetColor();
                    Pause();
                }
            }
        }

        static void PrintTable()
        {
            Console.WriteLine($"Поточний стан списку (Всього: {vehicleList.Length}):");
            Console.WriteLine(new string('-', 55));
            Console.WriteLine($"{"Індекс",-6} | {"Тип ТЗ",-12} | {"Об'єм двигуна",-14} | {"Електро/Гібрид",-14}");
            Console.WriteLine(new string('-', 55));

            if (vehicleList.Length == 0)
            {
                Console.WriteLine("                     Список порожній");
            }
            else
            {
                for (int i = 0; i < vehicleList.Length; i++)
                {
                    Console.WriteLine($"{i,-6} | {vehicleList[i]}");
                }
            }
            Console.WriteLine(new string('-', 55));
        }

        static void AddItemMenu()
        {
            Console.Write($"Введіть індекс (0 - {vehicleList.Length}): ");
            int idx = int.Parse(Console.ReadLine());
            VehicleType type = ChooseType();
            Console.Write("Введіть об'єм двигуна: ");
            double cap = double.Parse(Console.ReadLine().Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
            Console.Write("Електро/Гібрид? (y/n): ");
            bool isElec = Console.ReadLine().Trim().ToLower() == "y";

            vehicleList.AddAt(idx, new VehicleRecord(type, cap, isElec));
            Console.WriteLine("\n[Елемент додано!]");
            Pause();
        }

        static void EditItemMenu()
        {
            Console.Write($"Введіть індекс (0 - {vehicleList.Length - 1}): ");
            int idx = int.Parse(Console.ReadLine());
            var current = vehicleList[idx]; 

            VehicleType type = ChooseType();
            Console.Write("Введіть новий об'єм: ");
            double cap = double.Parse(Console.ReadLine().Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
            Console.Write("Електро/Гібрид? (y/n): ");
            bool isElec = Console.ReadLine().Trim().ToLower() == "y";

            vehicleList[idx] = new VehicleRecord(type, cap, isElec);
            Console.WriteLine("\n[Елемент змінено!]");
            Pause();
        }

        static void DemonstrateIteration()
        {
            Console.WriteLine("\n--- Ітерація ---");
            VehicleRecord cur = vehicleList.GetInitialValue();
            if (cur == null) Console.WriteLine("Список порожній.");
            else
            {
                Console.WriteLine($"Старт: {cur}");
                while ((cur = vehicleList.GetNextValue()) != null)
                {
                    Console.WriteLine($"Далі:  {cur}");
                }
            }
            Pause();
        }

        static void PerformSearch()
        {
            Console.WriteLine("\n--- Результати пошуку (Електро/Гібрид та двигун < 2.0L) ---");
            BidirectionalList matched = vehicleList.Search(v => v.IsElectricOrHybrid && v.EngineCapacity < 2.0);

            if (matched.Length == 0)
            {
                Console.WriteLine("Нічого не знайдено.");
            }
            else
            {
                for (int i = 0; i < matched.Length; i++)
                {
                    Console.WriteLine(matched[i]);
                }
            }
            Pause();
        }

        static VehicleType ChooseType()
        {
            var values = Enum.GetValues(typeof(VehicleType));
            for (int i = 0; i < values.Length; i++) Console.WriteLine($"{i}. {values.GetValue(i)}");
            Console.Write("Оберіть номер типу: ");
            int choice = int.Parse(Console.ReadLine());
            return (VehicleType)values.GetValue(choice);
        }

        static void Pause()
        {
            Console.WriteLine("\nНатисніть Enter...");
            Console.ReadLine();
        }
    }
}