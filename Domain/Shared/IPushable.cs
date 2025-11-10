using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    public interface IPublishable
    {
        void Publish();
        IReadOnlyCollection<object> DomainEvents { get; }
        void ClearDomainEvents();
    }
}

