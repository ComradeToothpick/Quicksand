using ProtoBuf;

namespace Quicksand.network
{
    [ProtoContract]
    public class ConfigMessage
    {
        [ProtoMember (1)]
        public float stillSinkSpeed = 0.002f;
        [ProtoMember (2)]
        public float maxSinkSpeed = 0.007f;
        [ProtoMember (3)]
        public float climbSpeed = 0.004f;
        [ProtoMember (4)]
        public float maxHorizontalSpeed = 0.05f;
        [ProtoMember (5)]
        public float friction = 0.99f;
        [ProtoMember (6)]
        public float struggleDrain = 20f;
        [ProtoMember (7)]
        public bool blindEnabled = true;
    }

    [ProtoContract]
    public class ConfigRequest
    {
        [ProtoMember (1)]
        public bool sendToClient = true;
    }
}