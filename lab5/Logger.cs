using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace lab5
{
    // Класс для логирования действий
    internal class Logger
    {
        // Путь к файлу логирования
        private static string logFilePath;

        // Метод для инициализации логирования, создание или очистка файла
        public static void Initialize(string filePath)
        {
            Console.WriteLine("Создать новый файл журнала или дописать в существующий?");
            Console.WriteLine("1. Новый файл");
            Console.WriteLine("2. Дописать в существующий");
            int choice = GetValidIntInput("");

            logFilePath = filePath;
            if (choice == 1 && File.Exists(logFilePath))
            {
                File.Delete(logFilePath); // Удаляем существующий файл, если выбран вариант 1
            }

            Log("Логирование инициализировано.");
        }

        // Получение валидного целочисленного ввода
        static int GetValidIntInput(string prompt)
        {
            int result;
            Console.WriteLine(prompt);
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out result))
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Ошибка. Введите целое число.");
                }
            }
        }

        // Логирование сообщения в файл
        public static void Log(string message)
        {
            // Записываем сообщение с текущей датой и временем
            File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}\n");
        }
    }
}
