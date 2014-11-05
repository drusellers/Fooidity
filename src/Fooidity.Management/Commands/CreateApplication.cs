namespace Fooidity.Management.Commands
{
    using System;


    public interface CreateApplication
    {
        Guid CommandId { get; }

        DateTime Timestamp { get; }

        string UserId { get; }

        string OrganizationId { get; }

        string ApplicationName { get; }
    }
}