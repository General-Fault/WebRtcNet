using System;

namespace WebRtcNet.Media
{
    /// <summary>
    /// The InputDeviceInfo interface gives access to the capabilities of the input device it represents.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-inputdeviceinfo"/>
    public class InputDeviceInfo :  MediaDeviceInfo
    {
        internal InputDeviceInfo(string deviceId, MediaDeviceKind kind, string label, string groupId) : base(deviceId, kind, label, groupId)
        {
        }

        /// <summary>
        /// Returns a MediaTrackCapabilities object describing the primary audio or video track of a device's MediaStream 
        /// (according to its kind value), in the absence of any user-supplied constraints.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-inputdeviceinfo-getcapabilities"/>
        public MediaTrackCapabilities GetCapabilities()
        {
            throw new NotImplementedException();
        }
    }
}
