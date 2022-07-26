using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebRtcNet.Media
{
    /// <Summary>
    /// A .Net implementation of the MediaDevices Interface (W3C Candidate Recommendation Draft 16 June 2022)
    /// </Summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#mediadevices"/>
    public interface IMediaDevices
    {
        /// <summary>
        /// The set of availab emedia devices has changed. The current list devices can be retrieved with the 
        /// EnumerateDevices() method.
        /// </summary>
        /// <see href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadevices-ondevicechange"/>
        event EventHandler OnDeviceChange;

        /// <summary>
        /// Collects information about the User Agent's available media input and output devices.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadevices-enumeratedevices"/>
        /// <returns>A task that when completed has a list of MediaDeviceInfo objects representing available devices.</returns>
        Task<IEnumerable<MediaDeviceInfo>> EnumerateDevices();

        #region 10.2 MediaDevices Interface Extensions
        /// <summary>
        /// The GetSupportedConstraints method is provided to allow the application to determine which constraints are 
        /// recognized. Applications may need this information to use required constraints reliably or get predictable 
        /// results from combinatory logic in advanced constraints.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadevices-getsupportedconstraints"/>
        /// <returns>A dictionary whose members are the constrainable properties known to the User Agent.</returns>
        MediaTrackSupportedConstraints GetSupportedConstraints();

        /// <summary>
        /// The GetUserMedia method uses constraints to help select an appropriate source for a track and configure it.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadevices-getusermedia"/>
        /// <param name="constraints">Used to instruct the User Agent what sort of IMediaStreamTracks to include in the MediaStream</param>
        /// <returns>an IMediaStream with tracks that conform to the contraints provided.</returns>
        Task<IMediaStream> GetUserMedia(MediaStreamConstraints constraints);

        #endregion //10.2 MediaDevices Interface Extensions
    }
}
