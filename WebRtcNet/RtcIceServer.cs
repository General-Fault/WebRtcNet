using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebRtcNet
{

    /// <summary>
    /// <seealso cref="http://www.w3.org/TR/webrtc/#rtciceserver-type"/>
    /// </summary>
    public class RtcIceServer
    {
        //TODO: Implement Ice definition parsing from string
        //RtcIceServer(string  definition);

        public RtcIceServer(IEnumerable<string> urls, string userName, string credential)
        {
            Urls = urls;
            UserName = userName;
            Credential = credential;
        }

        /// <summary>
        /// STUN or TURN URI(s) as defined in <seealso cref="https://tools.ietf.org/html/rfc7064">[RFC7064]</seealso> and 
        /// <seealso cref="https://tools.ietf.org/html/rfc7065">[RFC7065]</seealso> or other URI types.
        /// </summary>
        public IEnumerable<string> Urls { get; }

        /// <summary>
        /// If this RTCIceServer object represents a TURN server, then this attribute specifies the username to use with that TURN server.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// If this RTCIceServer object represents a TURN server, then this attribute specifies the credential to use with that TURN server.
        /// </summary>
        public string Credential { get; }
    };
}
