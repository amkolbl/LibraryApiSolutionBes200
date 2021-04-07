using LibraryApi.Domain;
using LibraryApi.Models.Reservations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace LibraryApi.Services
{
    public class EfReservationsLookup : ILookupReservations, IReservationCommands
    {
        private readonly LibraryDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;
        private readonly IProcessReservations _reservationProcessor;

        public EfReservationsLookup(LibraryDataContext context, IMapper mapper, MapperConfiguration config, IProcessReservations reservationProcessor)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _reservationProcessor = reservationProcessor;
        }

        public async Task<GetReservationSummaryResponseItem> AddReservationAsync(PostReservationRequest request)
        {
            var reservation = new BookReservation
            {
                For = request.For,
                BookIds = request.Books,
                Status = ReservationStatus.Pending
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            await _reservationProcessor.AddWorkAsync(reservation);
            return _mapper.Map<GetReservationSummaryResponseItem>(reservation);
        }

        public async Task<GetReservationSummaryResponse> GetAllReservationsAsync()
        {
            var response = new GetReservationSummaryResponse();

            var books = await _context.Reservations
                .ProjectTo<GetReservationSummaryResponseItem>(_config)
                .ToListAsync();

            response.Data = books;
            return response;
        }

        public async Task<GetReservationSummaryResponseItem> GetByIdAsync(int id)
        {
            var response = await _context.Reservations
                .Where(r => r.Id == id)
                .ProjectTo<GetReservationSummaryResponseItem>(_config)
                .SingleOrDefaultAsync();

                /* This is replaced with the "ProjectTo" above
                .Where(r => r.Id == id)
                .Select(r => new GetReservationSummaryResponseItem
                {
                    Id = r.Id,
                    For = r.For,
                    BookIds = r.BookIds,
                    Status = r.Status
                }).SingleOrDefaultAsync(); */

            return response;
        }
    }
}
