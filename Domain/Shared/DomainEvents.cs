using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    /// <summary>
    /// Simpel statisk event-dispatcher.
    /// Bruges til at registrere og kalde handlers for domænehændelser.
    /// </summary>
    public static class DomainEvents
    {
        // Liste af asynkrone handlers, registreret i runtime.
        private static readonly List<Func<IDomainEvent, Task>> _handlers = new();

        /// <summary>
        /// Registrer en handler (bruges typisk i opstarten).
        /// </summary>
        public static void Register(Func<IDomainEvent, Task> handler)
            => _handlers.Add(handler);

        /// <summary>
        /// Udløs en event – kalder alle registrerede handlers.
        /// </summary>
        public static async Task RaiseAsync(IDomainEvent domainEvent)
        {
            foreach (var handler in _handlers)
                await handler(domainEvent);
        }
    }

}