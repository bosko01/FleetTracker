namespace FleetTracker.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAtUtc { get; protected set; }
    public DateTime? ModifiedAtUtc { get; protected set; }

    protected void MarkModified(DateTime utcNow) => ModifiedAtUtc = utcNow;
}
