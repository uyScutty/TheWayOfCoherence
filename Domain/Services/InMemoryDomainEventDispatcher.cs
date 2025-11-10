using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Shared;

namespace Domain.Services
{
    /// <summary>
    /// Simpel in-memory event dispatcher.
    /// Kan senere udskiftes med MediatR eller en rigtig Event Bus.
    /// </summary>
    public class InMemoryDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IEnumerable<IDomainEventHandler<IDomainEvent>> _handlers;

        public InMemoryDomainEventDispatcher(IEnumerable<IDomainEventHandler<IDomainEvent>> handlers)
        {
            _handlers = handlers;
        }

        public async Task DispatchAsync(IDomainEvent domainEvent)
        {
            var matchingHandlers = _handlers
                .Where(h => h.GetType()
                .GetInterfaces()
                .Any(i => i.IsGenericType &&
                          i.GetGenericArguments()[0] == domainEvent.GetType()));

            foreach (var handler in matchingHandlers)
                await handler.HandleAsync(domainEvent);
        }
    }
}

