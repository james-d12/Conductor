namespace Conductor.Engine.Domain.User;

public readonly record struct UserId(Guid Value)
{
    public UserId() : this(Guid.NewGuid())
    {
    }
}