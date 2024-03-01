using MediatR;
using SwiftLink.Domain.Entities;

namespace SwiftLink.Domain.Events;

public class LinkVisitedEvent(LinkVisit linkVisit) : INotification
{
    public LinkVisit LinkVisit { get; } = linkVisit;
}
