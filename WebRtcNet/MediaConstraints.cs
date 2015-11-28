using System;
using System.Collections.Generic;

namespace WebRtcNet
{

    /// <summary>
    /// Constraints for the MediaTrack. 
    /// <seealso cref="http://www.w3.org/TR/mediacapture-streams/#media-track-constraints"/>
    /// <seealso cref="http://tools.ietf.org/html/draft-alvestrand-constraints-resolution-03"/>
    /// </summary>
    public class MediaConstraints
    {
        private readonly List<Constraint> _mandatory;
        private readonly List<Constraint> _optional;

        #region Constraint Names
        // Constraint keys used by a local video source.
        // Specified by draft-alvestrand-constraints-resolution-00b
        public static string MinAspectRatio = @"minAspectRatio";
        public static string MaxAspectRatio = @"maxAspectRatio";
        public static string MaxWidth = @"maxWidth";
        public static string MinWidth = @"minWidth";
        public static string MaxHeight = @"maxHeight";
        public static string MinHeight = @"minHeight";
        public static string MaxFrameRate = @"maxFrameRate";
        public static string MinFrameRate = @"minFrameRate";

        // Constraint keys used by a local audio source.
        public static string EchoCancellation = @"echoCancellation";

        // These keys are google specific.
        public static string GoogEchoCancellation = @"googEchoCancellation";

        public static string ExtendedFilterEchoCancellation = @"googEchoCancellation2";
        public static string DAEchoCancellation = @"googDAEchoCancellation";
        public static string AutoGainControl = @"googAutoGainControl";
        public static string ExperimentalAutoGainControl = @"googAutoGainControl2";
        public static string NoiseSuppression = @"googNoiseSuppression";
        public static string ExperimentalNoiseSuppression = @"googNoiseSuppression2";
        public static string HighpassFilter = @"googHighpassFilter";
        public static string TypingNoiseDetection = @"googTypingNoiseDetection";
        public static string AudioMirroring = @"googAudioMirroring";
        public static string AecDump = @"audioDebugRecording";

        // Google-specific constraint keys for a local video source
        public static string NoiseReduction = @"googNoiseReduction";

        // Constraint keys for CreateOffer / CreateAnswer
        // Specified by the W3C PeerConnection spec
        public static string OfferToReceiveVideo = @"OfferToReceiveVideo";
        public static string OfferToReceiveAudio = @"OfferToReceiveAudio";
        public static string VoiceActivityDetection = @"VoiceActivityDetection";
        public static string IceRestart = @"IceRestart";

        // These keys are google specific.
        public static string UseRtpMux = @"googUseRtpMUX";

        // Constraints values.
        public static string ValueTrue = @"true";
        public static string ValueFalse = @"false";

        // PeerConnection constraint keys.
        // Temporary pseudo-constraints used to enable DTLS-SRTP
        public static string EnableDtlsSrtp = @"DtlsSrtpKeyAgreement";  // Enable DTLS-SRTP

        // Temporary pseudo-constraints used to enable DataChannels
        public static string EnableRtpDataChannels = @"RtpDataChannels";  // Enable RTP DataChannels

        // Google-specific constraint keys.
        // Temporary pseudo-constraint for enabling DSCP through JS.
        public static string EnableDscp = @"googDscp";

        // Constraint to enable IPv6 through JS.
        public static string EnableIPv6 = @"googIPv6";

        // Temporary constraint to enable suspend below min bitrate feature.
        public static string EnableVideoSuspendBelowMinBitrate = @"googSuspendBelowMinBitrate";
        public static string NumUnsignalledRecvStreams = @"googNumUnsignalledRecvStreams";

        // Constraint to enable combined audio+video bandwidth estimation.
        public static string CombinedAudioVideoBwe = @"googCombinedAudioVideoBwe";
        public static string ScreencastMinBitrate = @"googScreencastMinBitrate";
        public static string CpuOveruseDetection = @"googCpuOveruseDetection";
        public static string CpuUnderuseThreshold = @"googCpuUnderuseThreshold";
        public static string CpuOveruseThreshold = @"googCpuOveruseThreshold";

        // Low cpu adaptation threshold for relative standard deviation of encode time.
        public static string CpuUnderuseEncodeRsdThreshold = @"googCpuUnderuseEncodeRsdThreshold";

        // High cpu adaptation threshold for relative standard deviation of encode time.
        public static string CpuOveruseEncodeRsdThreshold = @"googCpuOveruseEncodeRsdThreshold";
        public static string CpuOveruseEncodeUsage = @"googCpuOveruseEncodeUsage";
        public static string HighStartBitrate = @"googHighStartBitrate";
        public static string PayloadPadding = @"googPayloadPadding";
        #endregion Constraint Names

        public MediaConstraints(IEnumerable<Constraint> mandatory, IEnumerable<Constraint> optional)
        {
            _mandatory = new List<Constraint>(mandatory);
            _optional = new List<Constraint>(optional);
        }

        public struct Constraint
        {
            public readonly string Key;
            public string Value;

            public Constraint(string key, string value)
            {
                Key = key;
                Value = value;
            }
        }

        public IList<Constraint> Mandatory
        {
            get { return _mandatory; }
        }

        public IList<Constraint> Optional
        {
            get { return _optional; }
        }

        void AddMandatory<T>(string key, T value)
        {
            _mandatory.Add(new Constraint(key, value.ToString()));
        }

        void SetMandatory<T>(string key, T value)
        {
            _mandatory.RemoveAll(new Predicate<Constraint>(c => c.Key == key));
            _mandatory.Add(new Constraint(key, value.ToString()));

        }

        void AddOptional<T>(string key, T value)
        {
            _optional.Add(new Constraint(key, value.ToString()));
        }

        void SetOptional<T>(string key, T value)
        {
            _optional.RemoveAll(new Predicate<Constraint>(c => c.Key == key));
            _optional.Add(new Constraint(key, value.ToString()));
        }
    };
}
