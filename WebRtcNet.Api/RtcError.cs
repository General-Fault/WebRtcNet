using System;

namespace WebRtcNet
{
    /// <summary>
    /// Describes the cause of an RtcError
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcerrordetailtype-enum"/>
    public enum RtcErrorDetailType
    {
        /// <summary>
        /// The data channel has failed.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerrordetailtype-data-channel-failure"/>
        DataChannelFailure,

        /// <summary>
        /// The DTLS negotiation has failed or the connection has been terminated with a fatal error. The message contains
        /// information relating to the nature of error. If a fatal DTLS alert was received, the
        /// <see cref="RtcError.ReceivedAlert"/> is set to the value of the DTLS alert received. If a fatal DTLS alert was
        /// sent, the <see cref="RtcError.SentAlert"/> property is set to the value of the DTLS alert sent.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerrordetailtype-dtls-failure"/>
        DtlsFailure,

        /// <summary>
        /// The <see cref="IRtcDtlsTransport">RTCDtlsTransport's</see> remote certificate did not match any of the
        /// fingerprints provided in the SDP. If the remote peer cannot match the local certificate against the provided
        /// fingerprints, this error is not generated. Instead a "bad_certificate" (42) DTLS alert might be received from the
        /// remote peer, resulting in a <see cref="DtlsFailure"/>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerrordetailtype-fingerprint-failure"/>
        FingerprintFailure,

        /// <summary>
        /// The SCTP negotiation has failed or the connection has been terminated with a fatal error. The
        /// <see cref="RtcError.SctpCauseCode"/> property is set to the SCTP cause code.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerrordetailtype-sctp-failure"/>
        SctpFailure,

        /// <summary>
        /// The SDP syntax is not valid. The <see cref="RtcError.SdpLineNumber"/> property is set to the line number in the
        /// SDP where the syntax error was detected.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerrordetailtype-sdp-syntax-error"/>
        SdpSyntaxError,

        /// <summary>
        /// The hardware encoder resources required for the requested operation are not available.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerrordetailtype-hardware-encoder-not-available"/>
        HardwareEncoderNotAvailable,

        /// <summary>
        /// The hardware encoder does not support the provided parameters.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerrordetailtype-hardware-encoder-error"/>
        HardwareEncoderError
    };


    /// <summary>
    /// Some operations throw or fire RTCError. This is an extension of Exception that carries additional WebRTC-specific
    /// information.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcerror-interface"/>
    public class RtcError : Exception
    {
        public RtcError(RtcErrorDetailType errorDetail, long? sdpLineNumber, long? sctpCauseCode, ulong? receivedAlert, ulong? sentAlert)
        {
            ErrorDetail = errorDetail;
            SdpLineNumber = sdpLineNumber;
            SctpCauseCode = sctpCauseCode;
            ReceivedAlert = receivedAlert;
            SentAlert = sentAlert;
        }

        /// <summary>
        /// The WebRTC-specific error code for the type of error that occurred.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerror-errordetail"/>
        public RtcErrorDetailType ErrorDetail { get; }

        /// <summary>
        /// If <see cref="ErrorDetail"/> is <see cref="RtcErrorDetailType.SdpSyntaxError"/> this is the line number where the
        /// error was detected (the first line has line number 1).
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerror-sdplinenumber"/>
        public long? SdpLineNumber { get; }

        /// <summary>
        /// If <see cref="ErrorDetail"/> is <see cref="RtcErrorDetailType.SctpFailure"/> this is the SCTP cause code of the
        /// failed SCTP negotiation. 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerror-sctpcausecode"/>
        public long? SctpCauseCode { get; }

        /// <summary>
        /// If <see cref="ErrorDetail"/> is <see cref="RtcErrorDetailType.DtlsFailure"/> his is the SCTP cause code of the failed SCTP
        /// negotiation.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerror-receivedalert"/>
        public ulong? ReceivedAlert { get; }

        /// <summary>
        /// If <see cref="ErrorDetail"/> is <see cref="RtcErrorDetailType.DtlsFailure"/> and a fatal DTLS alert was sent, this is the
        /// value of the DTLS alert sent.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcerror-sentalert"/>
        public ulong? SentAlert { get; }
    };

    /// <summary>
    /// The RtcErrorEvent is defined for cases when an RtcError is raised as an event.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcerrorevent-interface"/>
    public class RtcErrorEventArgs : EventArgs
    {
        public RtcErrorEventArgs(RtcError error)
        {
            Error = error;
        }

        public RtcError Error { get; }
    }
}