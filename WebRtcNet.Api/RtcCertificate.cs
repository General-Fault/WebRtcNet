using System;
using System.Collections.Generic;

namespace WebRtcNet;

/// <summary>
/// Represents a DTLS fingerprint entry.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcdtlsfingerprint-dictionary"/>
public sealed record RtcDtlsFingerprint(string Algorithm, string Value);

/// <summary>
/// Represents a certificate that can be used by a peer connection.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtccertificate-interface"/>
public abstract class RtcCertificate
{
	/// <summary>
	/// Gets the certificate expiration time.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtccertificate-expires"/>
	public abstract DateTime Expires { get; }

	/// <summary>
	/// Gets the DTLS fingerprints for this certificate.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtccertificate-getfingerprints"/>
	public abstract IReadOnlyList<RtcDtlsFingerprint> GetFingerprints();
}
