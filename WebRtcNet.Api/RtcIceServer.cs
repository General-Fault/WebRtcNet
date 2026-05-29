using System.Collections.Generic;
using System.Linq;

namespace WebRtcNet;

/// <summary>
/// </summary>
/// <seealso href="http://www.w3.org/TR/webrtc/#rtciceserver-type"/>
public sealed record RtcIceServer
{
	//TODO: Implement Ice definition parsing from string
	//RtcIceServer(string  definition);

	/// <summary>
	/// Create an RtcIceServer from a list of urls, and optionally a username and credential (password).
	/// </summary>
	/// <param name="urls">STUN or TURN URIs as defined in <see href="https://tools.ietf.org/html/rfc7064">[RFC7064]</see> and 
	/// <see href="https://tools.ietf.org/html/rfc7065">[RFC7065]</see> or other URI types.</param>
	/// <param name="userName">If this RtcIceServer object represents a TURN server, then this attribute specifies the username to use with that TURN server.</param>
	/// <param name="credential">If this RtcIceServer object represents a TURN server, then this attribute specifies the credential to use with that TURN server.</param>
	public RtcIceServer(IEnumerable<string> urls, string? userName = null, string? credential = null)
	{
		Urls = urls?.ToArray() ?? [];
		UserName = userName ?? string.Empty;
		Credential = credential ?? string.Empty;
	}

	/// <summary>
	/// Create an RtcIceServer from a url, and optionally a username and credential (password).
	/// </summary>
	/// <param name="url">STUN or TURN URI as defined in <see href="https://tools.ietf.org/html/rfc7064">[RFC7064]</see> and 
	/// <see href="https://tools.ietf.org/html/rfc7065">[RFC7065]</see> or other URI types.</param>
	/// <param name="username">If this RtcIceServer object represents a TURN server, then this attribute specifies the username to use with that TURN server.</param>
	/// <param name="credential">If this RtcIceServer object represents a TURN server, then this attribute specifies the credential to use with that TURN server.</param>
	public RtcIceServer(string url, string? username = null, string? credential = null)
	{
		Urls = [url];
		UserName = username ?? string.Empty;
		Credential = credential ?? string.Empty;
	}

	/// <summary>
	/// STUN or TURN URI(s) as defined in <see href="https://tools.ietf.org/html/rfc7064">[RFC7064]</see> and 
	/// <see href="https://tools.ietf.org/html/rfc7065">[RFC7065]</see> or other URI types.
	/// </summary>
	public IReadOnlyList<string> Urls { get; set; } = [];

	/// <summary>
	/// If this RtcIceServer object represents a TURN server, then this attribute specifies the username to use with that TURN server.
	/// </summary>
	public string UserName { get; set; } = string.Empty;

	/// <summary>
	/// If this RtcIceServer object represents a TURN server, then this attribute specifies the credential to use with that TURN server.
	/// </summary>
	public string Credential { get; set; } = string.Empty;
};