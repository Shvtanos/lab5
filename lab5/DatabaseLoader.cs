using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace lab5
{
    internal class DatabaseLoader
    {
        public static List<Client> LoadClients(string filePath)
        {
            var clients = new List<Client>();
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet("Клиенты");
                foreach (var row in worksheet.RowsUsed().Skip(1)) // Пропускаем заголовок
                {
                    clients.Add(new Client
                    {
                        ClientId = int.Parse(row.Cell(1).GetValue<string>()),
                        LastName = row.Cell(2).GetValue<string>(),
                        FirstName = row.Cell(3).GetValue<string>(),
                        MiddleName = row.Cell(4).GetValue<string>(),
                        Address = row.Cell(5).GetValue<string>()
                    });
                }
            }
            return clients;
        }

        public static List<Booking> LoadBookings(string filePath)
        {
            var bookings = new List<Booking>();
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet("Бронирование");
                foreach (var row in worksheet.RowsUsed().Skip(1)) // Пропускаем заголовок
                {
                    bookings.Add(new Booking
                    {
                        BookingId = int.Parse(row.Cell(1).GetValue<string>()),
                        ClientId = int.Parse(row.Cell(2).GetValue<string>()),
                        RoomId = int.Parse(row.Cell(3).GetValue<string>()),
                        BookingDate = DateTime.Parse(row.Cell(4).GetValue<string>()),
                        CheckInDate = DateTime.Parse(row.Cell(5).GetValue<string>()),
                        CheckOutDate = DateTime.Parse(row.Cell(6).GetValue<string>())
                    });
                }
            }
            return bookings;
        }

        public static List<Room> LoadRooms(string filePath)
        {
            var rooms = new List<Room>();
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet("Номера");
                foreach (var row in worksheet.RowsUsed().Skip(1)) // Пропускаем заголовок
                {
                    rooms.Add(new Room
                    {
                        RoomId = int.Parse(row.Cell(1).GetValue<string>()),
                        Floor = int.Parse(row.Cell(2).GetValue<string>()),
                        Capacity = int.Parse(row.Cell(3).GetValue<string>()),
                        PricePerDay = decimal.Parse(row.Cell(4).GetValue<string>()),
                        Category = int.Parse(row.Cell(5).GetValue<string>())
                    });
                }
            }
            return rooms;
        }
    }
}
