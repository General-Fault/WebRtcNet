using System.Text.Json.Serialization;

namespace BasicVideoChat.Signaling;

internal enum SignalingMessageType
{
	Offer,
	Answer,
	Candidate,
	Bye
}

internal class SignalingMessage
{
	[JsonPropertyName("type")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public SignalingMessageType Type { get; set; }

	[JsonPropertyName("sdp")]
	public string? Sdp { get; set; }

	[JsonPropertyName("candidate")]
	public string? Candidate { get; set; }

	[JsonPropertyName("sdpMid")]
	public string? SdpMid { get; set; }

	[JsonPropertyName("sdpMLineIndex")]
	public ushort? SdpMLineIndex { get; set; }
}
