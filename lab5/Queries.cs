using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5
{
    // Класс для выполнения запросов
    internal class Queries
    {
        // Запрос на поиск клиентов по адресу
        public static IEnumerable<Client> GetClientsByAddress(List<Client> clients, string address)
        {
            return clients.Where(c => c.Address.Contains(address)); // Возвращает клиентов, чей адрес содержит указанный
        }

        // Запрос на получение списка бронирований с общей стоимостью
        public static IEnumerable<(Booking, decimal)> GetBookingsWithTotalPrice(List<Booking> bookings, List<Room> rooms)
        {
            // Создаём новый список для возврата данных
            var result = new List<(Booking, decimal)>();

            // Перебираем все бронирования
            foreach (var booking in bookings)
            {
                // Находим номер по ID из бронирования
                var room = rooms.FirstOrDefault(r => r.RoomId == booking.RoomId);
                if (room != null)
                {
                    // Рассчитываем общую стоимость проживания
                    int numberOfNights = (booking.CheckOutDate - booking.CheckInDate).Days;
                    decimal totalPrice = room.PricePerDay * numberOfNights;

                    // Добавляем в результат
                    result.Add((booking, totalPrice));
                }
            }

            return result; // Возвращаем список с бронированиями и общей стоимостью
        }

        // Запрос на получение общего дохода от клиента
        public static decimal GetTotalEarningsForClient(List<Booking> bookings, List<Room> rooms, int clientId)
        {
            var clientBookings = bookings.Where(b => b.ClientId == clientId); // Находим все бронирования клиента
            return clientBookings.Join(rooms,
                b => b.RoomId,
                r => r.RoomId,
                (b, r) => r.PricePerDay * (decimal)(b.CheckOutDate - b.CheckInDate).TotalDays) // Считаем общую сумму
                .Sum(); // Суммируем стоимость всех бронирований клиента
        }

        // Запрос на подсчёт общего количества бронирований по категории номера
        public static int GetTotalBookingsForCategory(List<Booking> bookings, List<Room> rooms, int category)
        {
            return bookings.Join(rooms,
                b => b.RoomId,
                r => r.RoomId,
                (b, r) => r) // Соединяем таблицы бронирований и номеров
                .Count(r => r.Category == category); // Считаем количество бронирований для указанной категории номера
        }
    }
}
