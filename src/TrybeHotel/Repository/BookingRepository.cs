using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var formatBooking = new Booking{
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                RoomId = booking.RoomId,
                User = _context.Users.First(u => u.Email == email),
            };
            var newBooking = _context.Bookings.Add(formatBooking).Entity;
            _context.SaveChanges();
            var room = _context.Rooms.First(r => r.RoomId == newBooking.RoomId);
            var hotel = _context.Hotels.First(h => h.HotelId == room.HotelId);
            var city = _context.Cities.First(c => c.CityId == hotel.CityId);
            return new BookingResponse{
                BookingId = newBooking.BookingId,
                CheckIn = newBooking.CheckIn,
                CheckOut = newBooking.CheckOut,
                GuestQuant = newBooking.GuestQuant,
                Room = new RoomDto{
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto{
                        HotelId = hotel!.HotelId,
                        Name = hotel.Name,
                        Address = hotel.Address,
                        CityId = hotel.CityId,
                        CityName = city.Name,
                        //aquele mesmo erro
                        State = city.State
                    }
                },
            };
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var user = _context.Users.First(u => u.Email == email);
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if(booking == null || user.UserId != booking.UserId) return null!;
            var room = _context.Rooms.First(r => r.RoomId == booking.RoomId);
            var hotel = _context.Hotels.First(h => h.HotelId == room.HotelId);
            var city = _context.Cities.First(c => c.CityId == hotel.CityId);
            return new BookingResponse{
                BookingId = booking.BookingId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = new RoomDto{
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto{
                        HotelId = hotel!.HotelId,
                        Name = hotel.Name,
                        Address = hotel.Address,
                        CityId = hotel.CityId,
                        CityName = city.Name,
                        //aquele mesmo erro
                        State = city.State
                    }
                },
            };
        }

        public Room GetRoomById(int RoomId)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == RoomId);
            if (room == null) return null!;
            return room;
        }

    }

}