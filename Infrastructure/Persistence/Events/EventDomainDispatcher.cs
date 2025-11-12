using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

using Domain.Shared;

namespace Infrastructure.Persistence.Events;

public class EventDomainDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public EventDomainDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent)
    {
        // Send eventet gennem MediatR
        await _mediator.Publish(domainEvent);
    }
}