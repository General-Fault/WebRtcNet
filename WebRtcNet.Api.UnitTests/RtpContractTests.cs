using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using WebRtcNet.Media;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class RtpContractTests
{
	[Test]
	public void RtcRtpEncodingParameters_Defaults_Codec_MaxFramerate_And_Rid_To_Null()
	{
		var parameters = new RtcRtpEncodingParameters();

		Assert.IsNull(parameters.Codec);
		Assert.IsNull(parameters.MaxFramerate);
		Assert.IsNull(parameters.Rid);
	}

	[Test]
	public void RtcRtpEncodingParameters_Inherits_RtcRtpCodingParameters()
	{
		Assert.IsTrue(typeof(RtcRtpCodingParameters).IsAssignableFrom(typeof(RtcRtpEncodingParameters)));
	}

	[Test]
	public void RtcRtpCodecParameters_Inherits_RtcRtpCodec()
	{
		Assert.IsTrue(typeof(RtcRtpCodec).IsAssignableFrom(typeof(RtcRtpCodecParameters)));
		Assert.IsTrue(typeof(RtcRtpCodec).IsAssignableFrom(typeof(RtcRtpCodecCapability)));
	}

	[Test]
	public void RtcRtpSender_Exposes_Updated_SetParameters_Overload()
	{
		var method = typeof(RtcRtpSender).GetMethod(
			nameof(RtcRtpSender.SetParameters),
			new[] { typeof(RtcRtpSendParameters), typeof(RtcSetParameterOptions) });

		Assert.IsNotNull(method);
	}

	[Test]
	public void RtcRtpSender_Uses_Nullable_Track_And_Transport_Properties()
	{
		var trackProperty = typeof(RtcRtpSender).GetProperty(nameof(RtcRtpSender.Track));
		var transportProperty = typeof(RtcRtpSender).GetProperty(nameof(RtcRtpSender.Transport));

		Assert.IsNotNull(trackProperty);
		Assert.IsNotNull(transportProperty);
		Assert.AreEqual(2, GetNullableFlag(trackProperty!.GetMethod!.ReturnParameter));
		Assert.AreEqual(2, GetNullableFlag(transportProperty!.GetMethod!.ReturnParameter));
	}

	[Test]
	public void RtcRtpSender_Dtmf_Is_Nullable()
	{
		var property = typeof(RtcRtpSender).GetProperty(nameof(RtcRtpSender.Dtmf));

		Assert.IsNotNull(property);
		Assert.AreEqual(2, GetNullableFlag(property!.GetMethod!.ReturnParameter));
	}

	[Test]
	public void RtcRtpSender_ReplaceTrack_Allows_Nullable_WithTrack()
	{
		var method = typeof(RtcRtpSender).GetMethod(nameof(RtcRtpSender.ReplaceTrack), new[] { typeof(MediaStreamTrack) });

		Assert.IsNotNull(method);
		Assert.AreEqual(2, GetNullableFlag(method!.GetParameters()[0]));
	}

	[Test]
	public void RtcRtpTransceiver_Uses_Shared_RtpCodec_Type_For_Codec_Preferences()
	{
		var method = typeof(RtcRtpTransceiver).GetMethod(
			nameof(RtcRtpTransceiver.SetCodecPreferences),
			new[] { typeof(IEnumerable<RtcRtpCodec>) });

		Assert.IsNotNull(method);
	}

	[Test]
	public void RtcRtpReceiver_Exposes_JitterBufferTarget_Property()
	{
		var property = typeof(RtcRtpReceiver).GetProperty(nameof(RtcRtpReceiver.JitterBufferTarget));

		Assert.IsNotNull(property);
		Assert.AreEqual(typeof(TimeSpan?), property!.PropertyType);
		Assert.IsTrue(property.CanRead);
		Assert.IsTrue(property.CanWrite);
	}

	[Test]
	public void RtcRtpReceiver_Transport_Is_Nullable()
	{
		var property = typeof(RtcRtpReceiver).GetProperty(nameof(RtcRtpReceiver.Transport));

		Assert.IsNotNull(property);
		Assert.AreEqual(2, GetNullableFlag(property!.GetMethod!.ReturnParameter));
	}

	[Test]
	public void RtcRtpTransceiver_Mid_Is_Nullable()
	{
		var property = typeof(RtcRtpTransceiver).GetProperty(nameof(RtcRtpTransceiver.Mid));

		Assert.IsNotNull(property);
		Assert.AreEqual(2, GetNullableFlag(property!.GetMethod!.ReturnParameter));
	}

	[Test]
	public void RtpSender_And_Receiver_Expose_Static_GetCapabilities()
	{
		var senderMethod = typeof(RtcRtpSender).GetMethod(nameof(RtcRtpSender.GetCapabilities), new[] { typeof(MediaStreamTrackKind) });
		var receiverMethod = typeof(RtcRtpReceiver).GetMethod(nameof(RtcRtpReceiver.GetCapabilities), new[] { typeof(MediaStreamTrackKind) });

		Assert.IsNotNull(senderMethod);
		Assert.IsNotNull(receiverMethod);
		Assert.AreEqual(typeof(RtcRtpCapabilities), senderMethod!.ReturnType);
		Assert.AreEqual(typeof(RtcRtpCapabilities), receiverMethod!.ReturnType);
		Assert.AreEqual(2, GetNullableFlag(receiverMethod.ReturnParameter));
	}

	[Test]
	public void RtcRtpCodec_Defaults_Optional_Values()
	{
		var codec = new RtcRtpCodec();

		Assert.AreEqual(string.Empty, codec.MimeType);
		Assert.IsNull(codec.Channels);
		Assert.AreEqual(string.Empty, codec.SdpFmtpLine);
	}

	private static byte? GetNullableFlag(ICustomAttributeProvider provider)
	{
		var nullableFlag = GetAttributeFlag(provider, "System.Runtime.CompilerServices.NullableAttribute");
		if (nullableFlag.HasValue)
		{
			return nullableFlag;
		}

		if (provider is ParameterInfo parameterInfo)
		{
			nullableFlag = GetAttributeFlag(parameterInfo.Member, "System.Runtime.CompilerServices.NullableContextAttribute");
			if (nullableFlag.HasValue)
			{
				return nullableFlag;
			}

			if (parameterInfo.Member.DeclaringType is not null)
			{
				return GetAttributeFlag(parameterInfo.Member.DeclaringType, "System.Runtime.CompilerServices.NullableContextAttribute");
			}
		}

		if (provider is MemberInfo memberInfo)
		{
			nullableFlag = GetAttributeFlag(memberInfo, "System.Runtime.CompilerServices.NullableContextAttribute");
			if (nullableFlag.HasValue)
			{
				return nullableFlag;
			}

			if (memberInfo.DeclaringType is not null)
			{
				return GetAttributeFlag(memberInfo.DeclaringType, "System.Runtime.CompilerServices.NullableContextAttribute");
			}
		}

		return null;
	}

	private static byte? GetAttributeFlag(ICustomAttributeProvider provider, string attributeTypeFullName)
	{
		var attributes = GetCustomAttributes(provider);
		foreach (var attribute in attributes)
		{
			if (attribute.AttributeType.FullName != attributeTypeFullName ||
				attribute.ConstructorArguments.Count == 0)
			{
				continue;
			}

			return GetFirstByte(attribute.ConstructorArguments[0]);
		}

		return null;
	}

	private static IList<CustomAttributeData> GetCustomAttributes(ICustomAttributeProvider provider)
	{
		if (provider is ParameterInfo parameterInfo)
		{
			return CustomAttributeData.GetCustomAttributes(parameterInfo);
		}

		return CustomAttributeData.GetCustomAttributes((MemberInfo)provider);
	}

	private static byte? GetFirstByte(CustomAttributeTypedArgument argument)
	{
		if (argument.ArgumentType == typeof(byte))
		{
			return (byte)argument.Value!;
		}

		if (argument.Value is IReadOnlyCollection<CustomAttributeTypedArgument> flags)
		{
			foreach (var flag in flags)
			{
				if (flag.Value is byte flagValue)
				{
					return flagValue;
				}
			}
		}

		return null;
	}
}
