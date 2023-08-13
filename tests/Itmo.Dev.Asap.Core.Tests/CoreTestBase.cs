using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Platform.Testing;
using Moq;
using Xunit.Abstractions;

namespace Itmo.Dev.Asap.Core.Tests;

public abstract class CoreTestBase : TestBase
{
    protected CoreTestBase(ITestOutputHelper? output = null) : base(output)
    {
        AuthorizationServiceMock = new Mock<IAuthorizationService>();
    }

    protected Mock<IAuthorizationService> AuthorizationServiceMock { get; }
}