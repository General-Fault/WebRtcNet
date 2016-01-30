using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace WebRtcNet
{
    /// <summary>
    /// Constraints for the MediaTrack. 
    /// <seealso href="http://www.w3.org/TR/mediacapture-streams/#media-track-constraints"/>
    /// <seealso href="http://tools.ietf.org/html/draft-alvestrand-constraints-resolution-03"/>
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

        public abstract class Constraint
        {
            protected Constraint(string key)
            {
                Key = key;
            }

            public string Key { get; }
            public abstract string ValueString { get; }
        }

        public class Constraint<T> : Constraint
        {
            public T Value { get; }

            public override string ValueString  => Value.ToString();

            public Constraint(Constraint constraint)
                : base(constraint.Key)
            {
                Contract.Requires<ArgumentNullException>(constraint != null);

                //cooerce the unknown value type
                Value = (T)Convert.ChangeType(constraint.ValueString, typeof(T));
            }

            public Constraint(string key, T value) : base(key)
            {
                Value = value;
            }
        }

        public Constraint<T> FindConstraint<T>(string key, out bool mandatory, T defaultVal)
        {
            if (Mandatory.Any(c => c.Key == key))
            {
                mandatory = true;
                var constraint = Mandatory.First(c => c.Key == key);
                return constraint as Constraint<T> ?? new Constraint<T>(constraint);
            }

            mandatory = false;
            if (Optional.Any(c=>c.Key == key))
            {
                var constraint = Optional.First(c => c.Key == key);
                return constraint as Constraint<T> ?? new Constraint<T>(constraint);
            }

            return null;
        }

        public IEnumerable<Constraint> Mandatory => _mandatory;

        public IEnumerable<Constraint> Optional => _optional;

        public void AddMandatory<T>(string key, T value)
        {
            _mandatory.Add(new Constraint<T>(key, value));
        }

        public void SetMandatory<T>(string key, T value)
        {
            _mandatory.RemoveAll(new Predicate<Constraint>(c => c.Key == key));
            _mandatory.Add(new Constraint<T>(key, value));
        }

        public void AddOptional<T>(string key, T value)
        {
            _optional.Add(new Constraint<T>(key, value));
        }

        public void SetOptional<T>(string key, T value)
        {
            _optional.RemoveAll(new Predicate<Constraint>(c => c.Key == key));
            _optional.Add(new Constraint<T>(key, value));
        }
    };


    public class MediaStreamConstraints
    {
        public MediaStreamConstraints(bool audio, bool video)
        {
            Audio = audio;
            AudioConstraints = null;

            Video = video;
            VideoConstraints = null;
        }

        public MediaStreamConstraints(MediaConstraints audio, bool video)
        {
            Audio = true;
            AudioConstraints = audio;

            Video = video;
            VideoConstraints = null;
        }

        public MediaStreamConstraints(bool audio, MediaConstraints video)
        {
            Audio = audio;
            AudioConstraints = null;

            Video = true;
            VideoConstraints = video;
        }

        public MediaStreamConstraints(MediaConstraints audio, MediaConstraints video)
        {
            Audio = true;
            AudioConstraints = audio;

            Video = true;
            VideoConstraints = video;
        }


        public bool Audio { get; set; }
        public MediaConstraints AudioConstraints { get; set; }

        public bool Video { get; set; }
        public MediaConstraints VideoConstraints { get; set; }
    }
}
