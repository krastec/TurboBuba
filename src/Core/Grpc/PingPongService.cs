using Grpc.Core;
using PingPong;
using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.Grpc
{
    public class PingPongService : PingPong.PingPong.PingPongBase
    {
        public override Task<PongReply> SendPing(PingRequest request, ServerCallContext context)
        {
            var incoming = request.Message ?? "";
            // можно логировать или делать что-то ещё
            var reply = new PongReply { Message = "Pong: " + incoming };
            return Task.FromResult(reply);
        }
    }
}
