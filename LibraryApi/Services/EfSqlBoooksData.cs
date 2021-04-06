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
    public class EfSqlBoooksData :ILookupBooks
    {

        private readonly LibraryDataContext _context;
        private readonly MapperConfiguration _config;
        private readonly IMapper _mapper;

        public EfSqlBoooksData(LibraryDataContext context, MapperConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
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
    }
}
