namespace FleetTracker.Domain.Common;

public abstract class SoftDeletableEntity : BaseEntity
{
    public bool IsDeleted { get; protected set; }

    protected void MarkDeleted(DateTime utcNow)
    {
        IsDeleted = true;
        MarkModified(utcNow);
    }
}
