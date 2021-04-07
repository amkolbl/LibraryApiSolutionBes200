using LibraryApi.Domain;
using LibraryApi.Models.Reservations;
using RabbitMqUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class RabbitMqReservationProcessor : IProcessReservations
    {
        private readonly IRabbitManager _manager;

        public RabbitMqReservationProcessor(IRabbitManager manager)
        {
            _manager = manager;
        }

        public Task AddWorkAsync(BookReservation reservation)
        {
            //TODO: What should our "Plan B" be?
            _manager.Publish(reservation, "", "direct", "reservations");
            return Task.CompletedTask;
        }
    }
}
