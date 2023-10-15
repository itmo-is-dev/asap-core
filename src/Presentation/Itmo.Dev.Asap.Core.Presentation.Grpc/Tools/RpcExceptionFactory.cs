using Grpc.Core;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Tools;

public static class RpcExceptionFactory
{
    public static RpcException UnexpectedOperationResult
        => new RpcException(new Status(StatusCode.Internal, "Operation finished with unexpected result"));
}