using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class EfSqlBooksData :ILookupBooks, IBookCommands
    {

        private readonly LibraryDataContext _context;
        private readonly MapperConfiguration _config;
        private readonly IMapper _mapper;

        public EfSqlBooksData(LibraryDataContext context, MapperConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        public async Task<GetBookDetailsResponse> GetBookByIdAsync(int id)
        {
            return await _context.AvailableBooks
                .Where(b => b.Id == id)
                .ProjectTo<GetBookDetailsResponse>(_config)
                .SingleOrDefaultAsync();
        }

        public async Task<GetBooksSummaryResponse> GetBooksByGenreAsync(string genre)
        {
            var query = _context.AvailableBooks;
            if (genre != null)
            {
                query = query.Where(b => b.Genre == genre);
            };

            var data = await query.ProjectTo<BookSummaryItem>(_config).ToListAsync();

            var response = new GetBooksSummaryResponse
            {
                Data = data,
                GenreFilter = genre
            };

            return response;
        }

        public async Task RemoveBookAsync(int id)
        {

            var book = await _context.AvailableBooks.SingleOrDefaultAsync(b => b.Id == id);
            if (book != null)
            {
                //_context.Books.Remove(book);
                book.IsAvailable = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
