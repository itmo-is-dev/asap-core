﻿namespace Itmo.Dev.Asap.Core.Common.Exceptions;

public class UpdateUsernameFailedException : DomainException
{
    public UpdateUsernameFailedException(string? message) : base(message) { }
}