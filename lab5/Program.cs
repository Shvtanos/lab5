using System;
using System.Collections.Generic;
using System.Linq;

namespace lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            // Путь к файлу Excel
            string filePath = "LR5-var9.xlsx";

            // Проверяем наличие файла
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"Файл {filePath} не найден. Убедитесь, что файл находится в нужной директории.");
                return;
            }

            // Загружаем данные из Excel
            var clients = DatabaseLoader.LoadClients(filePath);
            var bookings = DatabaseLoader.LoadBookings(filePath);
            var rooms = DatabaseLoader.LoadRooms(filePath);

            Console.WriteLine("Данные успешно загружены из файла.");
            Logger.Initialize("log.txt");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Просмотреть данные");
                Console.WriteLine("2. Удалить элемент");
                Console.WriteLine("3. Добавить элемент");
                Console.WriteLine("4. Корректировать данные");
                Console.WriteLine("5. Запросы");
                Console.WriteLine("6. Выход");
                int choice = GetValidIntInput("Введите номер действия:");

                switch (choice)
                {
                    case 1:
                        ViewData(clients, bookings, rooms);
                        break;

                    case 2:
                        DeleteElement(clients, bookings, rooms);
                        break;

                    case 3:
                        AddElement(clients, bookings, rooms);
                        break;

                    case 4:
                        EditElement(clients, bookings, rooms);
                        break;

                    case 5:
                        RunQueries(clients, bookings, rooms);
                        break;

                    case 6:
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }

            Logger.Log("Сеанс завершён.");
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

        // Получение валидного десятичного числа
        static decimal GetValidDecimalInput(string prompt)
        {
            decimal result;
            Console.WriteLine(prompt);
            while (true)
            {
                if (decimal.TryParse(Console.ReadLine(), out result))
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Ошибка. Введите число.");
                }
            }
        }

        // Получение валидной даты
        static DateTime GetValidDateInput(string prompt)
        {
            DateTime result;
            Console.WriteLine(prompt);
            while (true)
            {
                if (DateTime.TryParse(Console.ReadLine(), out result))
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Ошибка. Введите дату в формате ГГГГ-ММ-ДД.");
                }
            }
        }

        // Просмотр данных
        static void ViewData(List<Client> clients, List<Booking> bookings, List<Room> rooms)
        {
            Console.WriteLine("Выберите таблицу для просмотра:");
            Console.WriteLine("1. Клиенты");
            Console.WriteLine("2. Бронирования");
            Console.WriteLine("3. Номера");
            int viewChoice = GetValidIntInput("Введите номер таблицы:");

            switch (viewChoice)
            {
                case 1:
                    foreach (var client in clients) Console.WriteLine(client);
                    break;
                case 2:
                    foreach (var booking in bookings) Console.WriteLine(booking);
                    break;
                case 3:
                    foreach (var room in rooms) Console.WriteLine(room);
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }

        // Удаление данных
        static void DeleteElement(List<Client> clients, List<Booking> bookings, List<Room> rooms)
        {
            Console.WriteLine("Выберите таблицу для удаления:");
            Console.WriteLine("1. Клиенты");
            Console.WriteLine("2. Бронирования");
            Console.WriteLine("3. Номера");
            int deleteChoice = GetValidIntInput("Введите номер таблицы:");

            switch (deleteChoice)
            {
                case 1:
                    Console.WriteLine("Введите ID клиента для удаления:");
                    int clientIdToDelete = int.Parse(Console.ReadLine());
                    var clientToRemove = clients.FirstOrDefault(c => c.ClientId == clientIdToDelete);
                    if (clientToRemove != null)
                    {
                        clients.Remove(clientToRemove);
                        Console.WriteLine("Клиент удалён.");
                        Logger.Log($"Удалён клиент с ID: {clientIdToDelete}");
                    }
                    else
                    {
                        Console.WriteLine("Клиент с таким ID не найден.");
                    }
                    break;

                case 2:
                    Console.WriteLine("Введите ID бронирования для удаления:");
                    int bookingIdToDelete = int.Parse(Console.ReadLine());
                    var bookingToRemove = bookings.FirstOrDefault(b => b.BookingId == bookingIdToDelete);
                    if (bookingToRemove != null)
                    {
                        bookings.Remove(bookingToRemove);
                        Console.WriteLine("Бронирование удалено.");
                        Logger.Log($"Удалено бронирование с ID: {bookingIdToDelete}");
                    }
                    else
                    {
                        Console.WriteLine("Бронирование с таким ID не найдено.");
                    }
                    break;

                case 3:
                    Console.WriteLine("Введите ID номера для удаления:");
                    int roomIdToDelete = int.Parse(Console.ReadLine());
                    var roomToRemove = rooms.FirstOrDefault(r => r.RoomId == roomIdToDelete);
                    if (roomToRemove != null)
                    {
                        rooms.Remove(roomToRemove);
                        Console.WriteLine("Номер удалён.");
                        Logger.Log($"Удалён номер с ID: {roomIdToDelete}");
                    }
                    else
                    {
                        Console.WriteLine("Номер с таким ID не найден.");
                    }
                    break;

                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }

        // Добавление данных
        static void AddElement(List<Client> clients, List<Booking> bookings, List<Room> rooms)
        {
            Console.WriteLine("Выберите таблицу для добавления:");
            Console.WriteLine("1. Клиенты");
            Console.WriteLine("2. Бронирования");
            Console.WriteLine("3. Номера");
            int addChoice = GetValidIntInput("Введите номер таблицы:");

            switch (addChoice)
            {
                case 1:
                    int lastClientId = clients.Any() ? clients.Max(c => c.ClientId) : 0;
                    Console.WriteLine($"Последний использованный ID клиента: {lastClientId}");
                    int newClientId = GetValidIntInput("Введите новый ID клиента:");
                    Console.WriteLine("Введите фамилию клиента:");
                    string lastName = Console.ReadLine();
                    Console.WriteLine("Введите имя клиента:");
                    string firstName = Console.ReadLine();
                    Console.WriteLine("Введите отчество клиента:");
                    string middleName = Console.ReadLine();
                    Console.WriteLine("Введите адрес клиента:");
                    string address = Console.ReadLine();

                    clients.Add(new Client(newClientId, lastName, firstName, middleName, address));
                    Console.WriteLine("Клиент добавлен.");
                    Logger.Log($"Добавлен клиент с ID: {newClientId}");
                    break;

                case 2:
                    int lastBookingId = bookings.Any() ? bookings.Max(b => b.BookingId) : 0;
                    Console.WriteLine($"Последний использованный ID бронирования: {lastBookingId}");
                    int newBookingId = GetValidIntInput("Введите новый ID бронирования:");
                    int clientId = GetValidIntInput("Введите ID клиента:");
                    int roomId = GetValidIntInput("Введите ID номера:");
                    DateTime bookingDate = GetValidDateInput("Введите дату бронирования (формат: ГГГГ-ММ-ДД):");
                    DateTime checkInDate = GetValidDateInput("Введите дату заезда:");
                    DateTime checkOutDate = GetValidDateInput("Введите дату выезда:");

                    bookings.Add(new Booking(newBookingId, clientId, roomId, bookingDate, checkInDate, checkOutDate));
                    Console.WriteLine("Бронирование добавлено.");
                    Logger.Log($"Добавлено бронирование с ID: {newBookingId}");
                    break;

                case 3:
                    int lastRoomId = rooms.Any() ? rooms.Max(r => r.RoomId) : 0;
                    Console.WriteLine($"Последний использованный ID номера: {lastRoomId}");
                    int newRoomId = GetValidIntInput("Введите новый ID номера:");
                    int floor = GetValidIntInput("Введите этаж:");
                    int capacity = GetValidIntInput("Введите количество мест в номере:");
                    decimal pricePerDay = GetValidDecimalInput("Введите стоимость проживания в сутки:");
                    int category = GetValidIntInput("Введите категорию номера:");

                    rooms.Add(new Room(newRoomId, floor, capacity, pricePerDay, category));
                    Console.WriteLine("Номер добавлен.");
                    Logger.Log($"Добавлен номер с ID: {newRoomId}");
                    break;

                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }

        // Корректировка данных
        static void EditElement(List<Client> clients, List<Booking> bookings, List<Room> rooms)
        {
            Console.WriteLine("Выберите таблицу для корректировки:");
            Console.WriteLine("1. Клиенты");
            Console.WriteLine("2. Бронирования");
            Console.WriteLine("3. Номера");
            int editChoice = GetValidIntInput("Введите номер таблицы:");

            switch (editChoice)
            {
                case 1:
                    Console.WriteLine("Введите ID клиента для редактирования:");
                    int clientIdToEdit = int.Parse(Console.ReadLine());
                    var clientToEdit = clients.FirstOrDefault(c => c.ClientId == clientIdToEdit);
                    if (clientToEdit != null)
                    {
                        Console.WriteLine("Введите новые данные клиента:");
                        Console.WriteLine("Фамилия:");
                        clientToEdit.LastName = Console.ReadLine();
                        Console.WriteLine("Имя:");
                        clientToEdit.FirstName = Console.ReadLine();
                        Console.WriteLine("Отчество:");
                        clientToEdit.MiddleName = Console.ReadLine();
                        Console.WriteLine("Адрес:");
                        clientToEdit.Address = Console.ReadLine();

                        Console.WriteLine("Клиент обновлён.");
                        Logger.Log($"Обновлён клиент с ID: {clientIdToEdit}");
                    }
                    else
                    {
                        Console.WriteLine("Клиент с таким ID не найден.");
                    }
                    break;

                case 2:
                    Console.WriteLine("Введите ID бронирования для редактирования:");
                    int bookingIdToEdit = int.Parse(Console.ReadLine());
                    var bookingToEdit = bookings.FirstOrDefault(b => b.BookingId == bookingIdToEdit);
                    if (bookingToEdit != null)
                    {
                        Console.WriteLine("Введите новые данные бронирования:");

                        bookingToEdit.ClientId = int.Parse(Console.ReadLine());
                        bookingToEdit.RoomId = int.Parse(Console.ReadLine());
                        bookingToEdit.BookingDate = DateTime.Parse(Console.ReadLine());
                        bookingToEdit.CheckInDate = DateTime.Parse(Console.ReadLine());
                        bookingToEdit.CheckOutDate = DateTime.Parse(Console.ReadLine());

                        Console.WriteLine("Бронирование обновлено.");
                        Logger.Log($"Обновлено бронирование с ID: {bookingIdToEdit}");
                    }
                    else
                    {
                        Console.WriteLine("Бронирование с таким ID не найдено.");
                    }
                    break;

                case 3:
                    Console.WriteLine("Введите ID номера для редактирования:");
                    int roomIdToEdit = int.Parse(Console.ReadLine());
                    var roomToEdit = rooms.FirstOrDefault(r => r.RoomId == roomIdToEdit);
                    if (roomToEdit != null)
                    {
                        Console.WriteLine("Введите новые данные номера:");

                        roomToEdit.Floor = int.Parse(Console.ReadLine());
                        roomToEdit.Capacity = int.Parse(Console.ReadLine());
                        roomToEdit.PricePerDay = decimal.Parse(Console.ReadLine());
                        roomToEdit.Category = int.Parse(Console.ReadLine());

                        Console.WriteLine("Номер обновлён.");
                        Logger.Log($"Обновлён номер с ID: {roomIdToEdit}");
                    }
                    else
                    {
                        Console.WriteLine("Номер с таким ID не найден.");
                    }
                    break;

                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }

        // Выполнение запросов
        static void RunQueries(List<Client> clients, List<Booking> bookings, List<Room> rooms)
        {
            Console.WriteLine("Выберите запрос:");
            Console.WriteLine("1. Поиск клиентов по адресу (одна таблица)");
            Console.WriteLine("2. Список бронирований с общей стоимостью (две таблицы)");
            Console.WriteLine("3. Общий доход от клиента (три таблицы)");
            Console.WriteLine("4. Общее количество бронирований по категории номера (три таблицы)");
            int queryChoice = GetValidIntInput("Введите номер запроса:");

            switch (queryChoice)
            {
                case 1:
                    Console.WriteLine("Введите адрес для поиска:");
                    string addressQuery = Console.ReadLine();
                    var foundClients = Queries.GetClientsByAddress(clients, addressQuery);
                    foreach (var client in foundClients) Console.WriteLine(client);
                    Logger.Log($"Выполнен запрос на поиск клиентов по адресу: {addressQuery}");
                    break;

                case 2:
                    Console.WriteLine("Список бронирований с общей стоимостью:");

                    // Выполним новый запрос, который рассчитывает общую стоимость
                    var bookingsWithTotalPrice = Queries.GetBookingsWithTotalPrice(bookings, rooms);

                    foreach (var (booking, totalPrice) in bookingsWithTotalPrice)
                    {
                        Console.WriteLine($"ID бронирования: {booking.BookingId}, " +
                                          $"ID клиента: {booking.ClientId}, " +
                                          $"Дата заезда: {booking.CheckInDate.ToShortDateString()}, " +
                                          $"Дата выезда: {booking.CheckOutDate.ToShortDateString()}, " +
                                          $"Общая стоимость: {totalPrice} руб.");
                    }

                    Logger.Log("Выполнен запрос на список бронирований с подсчитанной общей стоимостью.");
                    break;

                case 3:
                    Console.WriteLine("Введите ID клиента для расчёта дохода:");
                    int clientIdForEarnings = int.Parse(Console.ReadLine());
                    var totalEarnings = Queries.GetTotalEarningsForClient(bookings, rooms, clientIdForEarnings);
                    Console.WriteLine($"Общий доход от клиента с ID {clientIdForEarnings}: {totalEarnings} руб.");
                    Logger.Log($"Выполнен запрос на доход от клиента с ID: {clientIdForEarnings}");
                    break;

                case 4:
                    Console.WriteLine("Введите категорию номера:");
                    int roomCategory = int.Parse(Console.ReadLine());
                    var totalBookings = Queries.GetTotalBookingsForCategory(bookings, rooms, roomCategory);
                    Console.WriteLine($"Общее количество бронирований для категории {roomCategory}: {totalBookings}");
                    Logger.Log($"Выполнен запрос на количество бронирований по категории: {roomCategory}");
                    break;

                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }
}
