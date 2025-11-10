using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    /// <summary>
    /// Generisk interface for event-handlere,
    /// som reagerer på hændelser i domænet.
    /// </summary>
    public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent evt);
    }
}
