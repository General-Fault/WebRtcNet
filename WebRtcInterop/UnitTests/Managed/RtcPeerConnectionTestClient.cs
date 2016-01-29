//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WebRtcInterop;
//using WebRtcNet;
//using NUnit.Framework;

//namespace WebRtcInterop.UnitTests
//{
//    internal interface ISignallClient
//    {
//        event EventHandler<SdpMessageEventArgs> SdpMessageReceived;
//        event EventHandler<IceMessageEventArgs> IceMessageReceived;

//        void SendSdpMessage(RtcSdpType type, string msg);
//        void SendIceMessage(string sdpMid, int sdpMlineIndex, string msg);
//    };

//    internal class SdpMessageEventArgs : EventArgs
//    {
//        public RtcSdpType Type;
//        public string Msg;

//        public SdpMessageEventArgs(RtcSdpType type, string msg)
//        {
//            Type = type;
//            Msg = msg;
//        }
//    }

//    internal class IceMessageEventArgs : EventArgs
//    {
//        public string SdpMid;
//        public int SdpMlineIndex;
//        public string Msg;

//        IceMessageEventArgs(string sdpMid, int sdpMlineIndex, string msg)
//        {
//            SdpMid = sdpMid;
//            SdpMlineIndex = sdpMlineIndex;
//            Msg = msg;
//        }
//    }

//    internal class TestSignalClient : ISignallClient
//    {
//        private Action<string, int, string> _iceMessageHandler;
//        private Action<RtcSdpType, string> _sdpMessageHandler;

//        public TestSignalClient(Action<string, int, string> iceMessageHandler, Action<RtcSdpType, string> sdpMessageHandler)
//        {
//            _iceMessageHandler = iceMessageHandler;
//            _sdpMessageHandler = sdpMessageHandler;
//        }

//        public event EventHandler<IceMessageEventArgs> IceMessageReceived;
//        public event EventHandler<SdpMessageEventArgs> SdpMessageReceived;

//        public void SendIceMessage(string sdpMid, int sdpMlineIndex, string msg)
//        {
//            _iceMessageHandler(sdpMid, sdpMlineIndex, msg);
//        }

//        public void SendSdpMessage(RtcSdpType type, string msg)
//        {
//            _sdpMessageHandler(type, msg);
//        }
//    }



//    internal class RtcPeerConnectionTestClient
//    {
//        private string _id;
//        private ISignallClient _signalClient;
//        private RtcPeerConnection _peerConnection;

//        private MediaConstraints _sessionDescriptionConstraints;

//        private readonly Dictionary<string, IVideoRenderer> _fakeVideoRenderers = new Dictionary<string, IVideoRenderer>();
//        private readonly List<IVideoRenderer> _removedFakeVideoRenderers = new List<IVideoRenderer>();
        
//        public static RtcPeerConnectionTestClient CreateClient(string id, MediaConstraints constraints)
//        {
//            RtcPeerConnectionTestClient client = new RtcPeerConnectionTestClient(id);

//            if (!client.Init(constraints))
//            {
//                return null;
//            }

//            return client;
//        }

//        internal RtcPeerConnectionTestClient(string id)
//        {
//            _id = id;
//        }


//        public virtual async Task NegotiateAsync(bool audio = true, bool video = true)
//        {
//            RtcSessionDescription offer;
//            offer = await CreateOfferAsync();

//            if (offer.Sdp.GetContentByName("audio"))
//            {
//                offer.Sdp.GetContentByName("audio").rejected = !audio;
//            }
//            if (offer.Sdp.GetContentByName("video"))
//            {
//                offer.Sdp.GetContentByName("video").rejected = !video;
//            }

//            string sdp = offer.ToString();
//            await SetLocalDescriptionAsync(offer);
//            _signalClient.SendSdpMessage(RtcSdpType.Offer, sdp);
//        }


//        protected virtual void OnIceMessageReceived(object sender, IceMessageEventArgs args)
//        {
//            Console.WriteLine($"INFO {_id} OnIceMessageReceived: SdpMid={args.SdpMid}, SdpMlineIndex={args.SdpMlineIndex}, Msg={args.Msg}");

//            RtcIceCandidate candidate = CreateIceCandidate(sdpMid, sdpMlineIndex, msg, null);
//            Assert.That(_peerConnection.AddIceCandidate(candidate), Is.True);
//        }

//        protected virtual void OnSdpMessageReceived(object sender, SdpMessageEventArgs args)
//        {
//            Console.WriteLine($"INFO {_id} OnSdpMessageReceived: Type={args.Type}, Msg={args.Msg}");

//            msg = FilterIncomingSdpMessage(msg);
//            if (type == RtcSdpType.Offer)
//            {
//                HandleIncomingOffer(msg);
//            }
//            else
//            {
//                HandleIncomingAnswer(msg);
//            }
//        }

//        // PeerConnectionObserver callbacks.
//        protected virtual void OnSignalingChange(RtcSignalingState newState)
//        {
//            Console.WriteLine($"INFO {_id} OnSignalingChange: {newState}");

//            Assert.That(_peerConnection.SignalingState, Is.EqualTo(newState));
//        }

//        protected virtual void OnAddStream(MediaStream mediaStream)
//        {
//            Console.WriteLine($"INFO {_id} OnAddStream: {mediaStream?.Id ?? "NA"} - Video={mediaStream?.GetVideoTracks().Count()}, Audio={mediaStream?.GetAudioTracks().Count()}");

//            foreach (var track in mediaStream.GetVideoTracks())
//            {
//                Assert.That(_fakeVideoRenderers.ContainsKey(track.Id), Is.False);
//                _fakeVideoRenderers.Add(track.Id, new FakeVideoRenderer(track));
//            }
//        }


//        protected virtual void OnRemoveStream(MediaStream mediaStream)
//        {
//            Console.WriteLine($"INFO {_id} OnRemoveStream: {mediaStream?.Id ?? "NA"} - Video={mediaStream?.GetVideoTracks().Count()}, Audio={mediaStream?.GetAudioTracks().Count()}");
//        }

//        protected virtual void OnRenegotiationNeeded()
//        {
//            Console.WriteLine($"INFO {_id} OnRenegotiationNeeded:");
//        }

//        protected virtual void OnIceConnectionChange(RtcIceConnectionState newState)
//        {
//            Console.WriteLine($"INFO {_id} OnIceConnectionChange: {newState}");
//            Assert.That(_peerConnection.IceConnectionState, Is.EqualTo(newState));
//        }

//        protected virtual void OnIceGatheringChange(RtcGatheringState newState)
//        {
//            Console.WriteLine($"INFO {_id} OnIceGatheringChange: {newState}");
//            Assert.That(_peerConnection.IceGatheringState, Is.EqualTo(newState));
//        }

//        protected virtual void OnIceCandidate(RtcIceCandidate candidate)
//        {
//            Console.WriteLine($"INFO {_id} OnIceCandidate: {candidate}");

//            string iceSdp = candidate.ToString();

//            if (_signalClient == null)
//            {
//                // Remote party may be deleted.
//                return;
//            }
//            _signalClient.SendIceMessage(candidate.SdpMid, candidate.SdpMLineIndex, iceSdp);
//        }

//        // MediaStream events
//        void OnAddTrack(object sender, IMediaStreamTrack track)
//        {
//            _fakeVideoRenderers.Add(track.Id, new FakeVideoTrackRenderer(track));
//        }

//        void OnRemoveTrack(object sender, IMediaStreamTrack track)
//        { 
//            // Remove renderers for tracks that were removed.
//            var toRemove = (from rendererMap in _fakeVideoRenderers where rendererMap.Key == track.Id select rendererMap);

//            foreach (var rm in toRemove)
//            {
//                _fakeVideoRenderers.Remove(toRemove.Key);
//                _removedFakeVideoRenderers.Range(toRemove.Value);
//            }
//        }

//        public MediaConstraints VideoConstraints { get; set; }

//        void AddMediaStream(bool audio, bool video)
//        {
//            var streamLabel = $"StreamLabelBase {_peerConnection.GetLocalStreams().Count()}";
//            var stream = Media.GetUserMedia(_sessionDescriptionConstraints);

//            _peerConnection.AddStream(stream);
//        }

//        int NumberOfLocalMediaStreams() => _peerConnection.LocalStreams.Count();

//        bool SessionActive() => _peerConnection.SignalingState == RtcSignalingState.Stable;

//        // Automatically add a stream when receiving an offer, if we don't have one.
//        // Defaults to true.
//        void SetAutoAddStream(bool autoAddStream)
//        {
//            _autoAddStream = autoAddStream;
//        }

//        void SetSignallingMessageReceiver(SignalingMessageReceiver signalingMessageReceiver)
//        {
//            _signalingMessageReceiver = signalingMessageReceiver;
//        }

//        //void EnableVideoDecoderFactory()
//        //{
//        //    _videoDecoderFactoryEnabled = true;
//        //    _fakeVideoDecoderFactory.AddSupportedVideoCodecType(
//        //        webrtc.kVideoCodecVP8);
//        //}

//        void IceRestart()
//        {
//            _sessionDescriptionConstraints.SetMandatory(MediaConstraints.IceRestart, true);
//            ExpectIceRestart = true;
//        }

//        bool ExpectIceRestart { get; set; }

//        void SetReceiveAudioVideo(bool audio, bool video)
//        {
//            SetReceiveAudio(audio);
//            SetReceiveVideo(video);
//            Assert.That(audio, Is.EqualTo(CanReceiveAudio());
//            Assert.That(video, Is.EqualTo(CanReceiveVideo());
//        }

//        void SetReceiveAudio(bool audio)
//        {
//            if (audio && CanReceiveAudio()) return;
//            _sessionDescriptionConstraints.SetMandatory(MediaConstraints.OfferToReceiveAudio, audio);
//        }

//        void SetReceiveVideo(bool video)
//        {
//            if (video && CanReceiveVideo()) return;
//            _sessionDescriptionConstraints.SetMandatory(MediaConstraints.OfferToReceiveVideo, video);
//        }

//        void RemoveMsidFromReceivedSdp(bool remove) { _removeMsId = remove; }

//        void RemoveSdesCryptoFromReceivedSdp(bool remove) { _removeSdes = remove; }

//        void RemoveBundleFromReceivedSdp(bool remove) { _removeBundle = remove; }

//        bool CanReceiveAudio()
//        {
//            bool value;
//            if (_sessionDescriptionConstraints.Mandatory
//                                       MediaraintsInterface.kOfferToReceiveAudio,
//                                       value, null))
//            {
//                return value;
//            }
//            return true;
//        }

//        bool CanReceiveVideo()
//        {
//            bool value;
//            if (webrtc.Findraint(_sessionDescriptionConstraints,
//                                       MediaraintsInterface.kOfferToReceiveVideo,
//                                       value, null))
//            {
//                return value;
//            }
//            return true;
//        }

//        void OnIceComplete()
//        {
//            Console.WriteLine("INFO) << _id << "OnIceComplete"; }


//  void OnDataChannel(DataChannelInterface data_channel)  {
//                Console.WriteLine("INFO) << _id << "OnDataChannel";


//                _dataChannel = data_channel;
//                data_observer_.reset(new MockDataChannelObserver(data_channel));
//            }

//            void CreateDataChannel()
//        {
//                _dataChannel = _peerConnection.CreateDataChannel(kDataChannelLabel, null);
//                ASSERT_TRUE(_dataChannel.get() != null);
//                data_observer_.reset(new MockDataChannelObserver(_dataChannel));
//            }

//            webrtc.AudioTrackInterface > CreateLocalAudioTrack(
//                   string streamLabel)
//{
//                MediaConstraints raints;
//                // Disable highpass filter so that we can get all the test audio frames.
//                raints.AddMandatory(MediaraintsInterface.kHighpassFilter, false);
//                webrtc.AudioSourceInterface > source =
//                    _peerConnectionfactory_.CreateAudioSource(raints);
//                // TODO(perkj): Test audio source when it is implemented. Currently audio
//                // always use the default input.
//                string label = streamLabel + kAudioTrackLabelBase;
//                return _peerConnectionfactory_.CreateAudioTrack(label, source);
//            }

//            webrtc.VideoTrackInterface > CreateLocalVideoTrack(
//                   string streamLabel)
//{
//                // Set max frame rate to 10fps to reduce the risk of the tests to be flaky.
//                MediaConstraints source_raints = _videoConstraints;
//                source_raints.SetMandatoryMaxFrameRate(10);

//                cricket.FakeVideoCapturer fake_capturer =
//                    new webrtc.FakePeriodicVideoCapturer();
//                _videoCapturers.push_back(fake_capturer);
//                webrtc.VideoSourceInterface > source =
//                    _peerConnectionfactory_.CreateVideoSource(fake_capturer,
//                                                                source_raints);
//                string label = streamLabel + kVideoTrackLabelBase;
//                return _peerConnectionfactory_.CreateVideoTrack(label, source);
//            }

//            DataChannelInterface data_channel() { return _dataChannel; }
//            MockDataChannelObserver data_observer()
//        {
//                return data_observer_.get();
//            }

//            webrtc.PeerConnectionInterface _peerConnection { return _peerConnection.get(); }

//            void StopVideoCapturers()
//        {
//                for (List<cricket.VideoCapturer>.iterator it =
//                         _videoCapturers.begin();
//                     it != _videoCapturers.end(); ++it)
//                {
//                    (it).Stop();
//                }
//            }

//            bool AudioFramesReceivedCheck(int number_of_frames)
//{
//                return number_of_frames <= _fakeAudioCaptureModule.frames_received();
//            }

//            int audio_frames_received()
//        {
//                return _fakeAudioCaptureModule.frames_received();
//            }

//            bool VideoFramesReceivedCheck(int number_of_frames)
//{
//                if (_videoDecoderFactoryEnabled)
//                {
//                    List<FakeWebRtcVideoDecoder> decoders
//                       = _fakeVideoDecoderFactory.decoders();
//                    if (decoders.empty())
//                    {
//                        return number_of_frames <= 0;
//                    }

//                    for (FakeWebRtcVideoDecoder decoder : decoders)
//                    {
//                        if (number_of_frames > decoder.GetNumFramesReceived())
//                        {
//                            return false;
//                        }
//                    }
//                    return true;
//                }
//                else {
//                    if (_fakeVideoRenderers.empty())
//                    {
//                        return number_of_frames <= 0;
//                    }

//                    for (autopair : _fakeVideoRenderers)
//                    {
//                        if (number_of_frames > pair.second.num_rendered_frames())
//                        {
//                            return false;
//                        }
//                    }
//                    return true;
//                }
//            }

//            int video_frames_received()
//        {
//                int total = 0;
//                if (_videoDecoderFactoryEnabled)
//                {
//                    List<FakeWebRtcVideoDecoder> decoders =
//                       _fakeVideoDecoderFactory.decoders();
//                    for (FakeWebRtcVideoDecoder decoder : decoders)
//                    {
//                        total += decoder.GetNumFramesReceived();
//                    }
//                }
//                else {
//                    for (auto pair : _fakeVideoRenderers)
//                    {
//                        total += pair.second.num_rendered_frames();
//                    }
//                    for (auto renderer : _removedFakeVideoRenderers)
//                    {
//                        total += renderer.num_rendered_frames();
//                    }
//                }
//                return total;
//            }

//            // Verify the CreateDtmfSender interface
//            void VerifyDtmf()
//        {
//                DummyDtmfObserver > observer(new DummyDtmfObserver());
//                DtmfSenderInterface > dtmf_sender;

//                // We can't create a DTMF sender with an invalid audio track or a non local
//                // track.
//                EXPECT_TRUE(_peerConnection.CreateDtmfSender(null) == null);
//                webrtc.AudioTrackInterface > non_localtrack(
//                    _peerConnectionfactory_.CreateAudioTrack("dummy_track", null));
//                EXPECT_TRUE(_peerConnection.CreateDtmfSender(non_localtrack) == null);

//                // We should be able to create a DTMF sender from a local track.
//                webrtc.AudioTrackInterface localtrack =
//                    _peerConnection.LocalStreams.at(0).GetAudioTracks()[0];
//                dtmf_sender = _peerConnection.CreateDtmfSender(localtrack);
//                EXPECT_TRUE(dtmf_sender.get() != null);
//                dtmf_sender.RegisterObserver(observer.get());

//                // Test the DtmfSender object just created.
//                EXPECT_TRUE(dtmf_sender.CanInsertDtmf());
//                EXPECT_TRUE(dtmf_sender.InsertDtmf("1a", 100, 50));

//                // We don't need to verify that the DTMF tones are actually sent out because
//                // that is already covered by the tests of the lower level components.

//                EXPECT_TRUE_WAIT(observer.completed(), kMaxWaitMs);
//                List<string> tones;
//                tones.push_back("1");
//                tones.push_back("a");
//                tones.push_back("");
//                observer.Verify(tones);

//                dtmf_sender.UnregisterObserver();
//            }

//            // Verifies that the SessionDescription have rejected the appropriate media
//            // content.
//            void VerifyRejectedMediaInSessionDescription()
//        {
//                ASSERT_TRUE(_peerConnection.remote_description() != null);
//                ASSERT_TRUE(_peerConnection.local_description() != null);
//                cricket.SessionDescription remote_desc =
//                   _peerConnection.remote_description().description();
//                cricket.SessionDescription local_desc =
//                   _peerConnection.local_description().description();

//                ContentInfo remote_audio_content = GetFirstAudioContent(remote_desc);
//                if (remote_audio_content)
//                {
//                    ContentInfo audio_content =
//                       GetFirstAudioContent(local_desc);
//                    EXPECT_EQ(CanReceiveAudio(), !audio_content.rejected);
//                }

//                ContentInfo remote_video_content = GetFirstVideoContent(remote_desc);
//                if (remote_video_content)
//                {
//                    ContentInfo video_content =
//                       GetFirstVideoContent(local_desc);
//                    EXPECT_EQ(CanReceiveVideo(), !video_content.rejected);
//                }
//            }

//            void VerifyLocalIceUfragAndPassword()
//        {
//                ASSERT_TRUE(_peerConnection.local_description() != null);
//                cricket.SessionDescription desc =
//                   _peerConnection.local_description().description();
//                cricket.ContentInfoscontents = desc.contents();

//                for (size_t index = 0; index < contents.size(); ++index)
//                {
//                    if (contents[index].rejected)
//                        continue;
//                    cricket.TransportDescription transport_desc =
//                       desc.GetTransportDescriptionByName(contents[index].name);

//                    std.map<int, IceUfragPwdPair>._iterator ufragpair_it =
//                        _iceUfragPwd.find(static_cast<int>(index));
//                    if (ufragpair_it == _iceUfragPwd.end())
//                    {
//                        ASSERT_FALSE(ExpectIceRestart());
//                        _iceUfragPwd[static_cast<int>(index)] =
//                            IceUfragPwdPair(transport_desc.ice_ufrag, transport_desc.ice_pwd);
//                    }
//                    else if (ExpectIceRestart())
//                    {
//                        IceUfragPwdPairufrag_pwd = ufragpair_it.second;
//                        EXPECT_NE(ufrag_pwd.first, transport_desc.ice_ufrag);
//                        EXPECT_NE(ufrag_pwd.second, transport_desc.ice_pwd);
//                    }
//                    else {
//                        IceUfragPwdPairufrag_pwd = ufragpair_it.second;
//                        EXPECT_EQ(ufrag_pwd.first, transport_desc.ice_ufrag);
//                        EXPECT_EQ(ufrag_pwd.second, transport_desc.ice_pwd);
//                    }
//                }
//            }

//            int GetAudioOutputLevelStats(webrtc.MediaStreamTrackInterface track)
//{
//                MockStatsObserver >
//                    observer(new rtc.RefCountedObject<MockStatsObserver>());
//                EXPECT_TRUE(_peerConnection.GetStats(
//                    observer, track, PeerConnectionInterface.kStatsOutputLevelStandard));
//                EXPECT_TRUE_WAIT(observer.called(), kMaxWaitMs);
//                EXPECT_NE(0, observer.timestamp());
//                return observer.AudioOutputLevel();
//            }

//            int GetAudioInputLevelStats()
//        {
//                MockStatsObserver >
//                    observer(new rtc.RefCountedObject<MockStatsObserver>());
//                EXPECT_TRUE(_peerConnection.GetStats(
//                    observer, null, PeerConnectionInterface.kStatsOutputLevelStandard));
//                EXPECT_TRUE_WAIT(observer.called(), kMaxWaitMs);
//                EXPECT_NE(0, observer.timestamp());
//                return observer.AudioInputLevel();
//            }

//            int GetBytesReceivedStats(webrtc.MediaStreamTrackInterface track)
//{
//                MockStatsObserver >
//                observer(new rtc.RefCountedObject<MockStatsObserver>());
//                EXPECT_TRUE(_peerConnection.GetStats(
//                    observer, track, PeerConnectionInterface.kStatsOutputLevelStandard));
//                EXPECT_TRUE_WAIT(observer.called(), kMaxWaitMs);
//                EXPECT_NE(0, observer.timestamp());
//                return observer.BytesReceived();
//            }

//            int GetBytesSentStats(webrtc.MediaStreamTrackInterface track)
//{
//                MockStatsObserver >
//                observer(new rtc.RefCountedObject<MockStatsObserver>());
//                EXPECT_TRUE(_peerConnection.GetStats(
//                    observer, track, PeerConnectionInterface.kStatsOutputLevelStandard));
//                EXPECT_TRUE_WAIT(observer.called(), kMaxWaitMs);
//                EXPECT_NE(0, observer.timestamp());
//                return observer.BytesSent();
//            }

//            int GetAvailableReceivedBandwidthStats()
//        {
//                MockStatsObserver >
//                    observer(new rtc.RefCountedObject<MockStatsObserver>());
//                EXPECT_TRUE(_peerConnection.GetStats(
//                    observer, null, PeerConnectionInterface.kStatsOutputLevelStandard));
//                EXPECT_TRUE_WAIT(observer.called(), kMaxWaitMs);
//                EXPECT_NE(0, observer.timestamp());
//                int bw = observer.AvailableReceiveBandwidth();
//                return bw;
//            }

//            string GetDtlsCipherStats()
//        {
//                MockStatsObserver >
//                    observer(new rtc.RefCountedObject<MockStatsObserver>());
//                EXPECT_TRUE(_peerConnection.GetStats(
//                    observer, null, PeerConnectionInterface.kStatsOutputLevelStandard));
//                EXPECT_TRUE_WAIT(observer.called(), kMaxWaitMs);
//                EXPECT_NE(0, observer.timestamp());
//                return observer.DtlsCipher();
//            }

//            string GetSrtpCipherStats()
//        {
//                MockStatsObserver >
//                    observer(new rtc.RefCountedObject<MockStatsObserver>());
//                EXPECT_TRUE(_peerConnection.GetStats(
//                    observer, null, PeerConnectionInterface.kStatsOutputLevelStandard));
//                EXPECT_TRUE_WAIT(observer.called(), kMaxWaitMs);
//                EXPECT_NE(0, observer.timestamp());
//                return observer.SrtpCipher();
//            }

//            int rendered_width()
//        {
//                EXPECT_FALSE(_fakeVideoRenderers.empty());
//                return _fakeVideoRenderers.empty() ? 1 :
//                    _fakeVideoRenderers.begin().second.width();
//            }

//            int rendered_height()
//        {
//                EXPECT_FALSE(_fakeVideoRenderers.empty());
//                return _fakeVideoRenderers.empty() ? 1 :
//                    _fakeVideoRenderers.begin().second.height();
//            }

//            size_t number_of_remote_streams()
//        {
//                if (!_peerConnection)
//                    return 0;
//                return _peerConnection.remote_streams().count();
//            }

//            StreamCollectionInterface remote_streams()
//        {
//                if (!_peerConnection)
//                {
//                    ADD_FAILURE();
//                    return null;
//                }
//                return _peerConnection.remote_streams();
//            }

//            StreamCollectionInterface local_streams()
//        {
//                if (!_peerConnection)
//                {
//                    ADD_FAILURE();
//                    return null;
//                }
//                return _peerConnection.LocalStreams;
//            }

//            RtcSignalingState signaling_state()
//        {
//                return _peerConnection.signaling_state();
//            }

//            RtcIceConnectionState ice_connection_state()
//        {
//                return _peerConnection.ice_connection_state();
//            }

//            RtcIceGatheringState ice_gathering_state()
//        {
//                return _peerConnection.ice_gathering_state();
//            }

//private:
//  class DummyDtmfObserver : public DtmfSenderObserverInterface {
//   public:
//    DummyDtmfObserver() : completed_(false) { }

//        // Implements DtmfSenderObserverInterface.
//        void OnToneChange(string tone)
//        {
//            tones_.push_back(tone);
//            if (tone.empty())
//            {
//                completed_ = true;
//            }
//        }

//        void Verify(List<string> tones)
//        {
//            ASSERT_TRUE(tones_.size() == tones.size());
//            EXPECT_TRUE(std.equal(tones.begin(), tones.end(), tones_.begin()));
//        }

//        bool completed() { return completed_; }

//        private:
//    bool completed_;
//        List<string> tones_;
//    };

//    explicit PeerConnectionTestClient(string id) : _id(id) { }

//    bool Init(
//           MediaraintsInterface raints,
//           PeerConnectionFactory.Options options,
//        webrtc.DtlsIdentityStoreInterface> dtls_identity_store)
//    {
//        EXPECT_TRUE(!_peerConnection);
//        EXPECT_TRUE(!_peerConnectionfactory_);
//        _allocatorFactory = webrtc.FakePortAllocatorFactory.Create();
//        if (!_allocatorFactory)
//        {
//            return false;
//        }
//        _fakeAudioCaptureModule = FakeAudioCaptureModule.Create();

//        if (_fakeAudioCaptureModule == null)
//        {
//            return false;
//        }
//        _fakeVideoDecoderFactory = new FakeWebRtcVideoDecoderFactory();
//        _fakeVideoEncoderFactory = new FakeWebRtcVideoEncoderFactory();
//        _peerConnectionfactory_ = webrtc.CreatePeerConnectionFactory(
//            rtc.Thread.Current(), rtc.Thread.Current(),
//            _fakeAudioCaptureModule, _fakeVideoEncoderFactory,
//            _fakeVideoDecoderFactory);
//        if (!_peerConnectionfactory_)
//        {
//            return false;
//        }
//        if (options)
//        {
//            _peerConnectionfactory_.SetOptions(options);
//        }
//        _peerConnection = CreatePeerConnection(
//            _allocatorFactory.get(), raints, std.move(dtls_identity_store));
//        return _peerConnection.get() != null;
//    }

//    webrtc.PeerConnectionInterface> CreatePeerConnection(
//        webrtc.PortAllocatorFactoryInterface factory,
//           MediaraintsInterface raints,
//        webrtc.DtlsIdentityStoreInterface> dtls_identity_store)
//    {
//        // CreatePeerConnection with IceServers.
//        RtcIceServers ice_servers;
//        RtcIceServer ice_server;
//        ice_server.uri = "stun:stun.l.google.com:19302";
//        ice_servers.push_back(ice_server);

//        return _peerConnectionfactory_.CreatePeerConnection(
//            ice_servers, raints, factory, std.move(dtls_identity_store),
//            this);
//    }

//    void HandleIncomingOffer(string msg)
//    {
//        Console.WriteLine("INFO) << _id << "HandleIncomingOffer ";
//        if (NumberOfLocalMediaStreams() == 0  _autoAddStream)
//    {
//            // If we are not sending any streams ourselves it is time to add some.
//            AddMediaStream(true, true);
//        }
//        SessionDescriptionInterface > desc(
//            webrtc.CreateSessionDescription("offer", msg, null));
//        EXPECT_TRUE(DoSetRemoteDescription(desc.release()));
//        SessionDescriptionInterface > answer;
//        EXPECT_TRUE(DoCreateAnswer(answer.use()));
//        string sdp;
//        EXPECT_TRUE(answer.ToString(sdp));
//        EXPECT_TRUE(DoSetLocalDescription(answer.release()));
//        if (_signalingMessageReceiver)
//        {
//            _signalingMessageReceiver.ReceiveSdpMessage(
//                webrtc.SessionDescriptionInterface.kAnswer, sdp);
//        }
//    }

//    void HandleIncomingAnswer(string msg)
//    {
//        Console.WriteLine("INFO) << _id << "HandleIncomingAnswer";


//        SessionDescriptionInterface > desc(
//            webrtc.CreateSessionDescription("answer", msg, null));
//        EXPECT_TRUE(DoSetRemoteDescription(desc.release()));
//    }

//    bool DoCreateOfferAnswer(SessionDescriptionInterface desc,
//                             bool offer)
//    {
//        MockCreateSessionDescriptionObserver >
//            observer(new rtc.RefCountedObject<
//                MockCreateSessionDescriptionObserver>());
//        if (offer)
//        {
//            _peerConnection.CreateOffer(observer, _sessionDescriptionConstraints);
//        }
//        else {
//            _peerConnection.CreateAnswer(observer, _sessionDescriptionConstraints);
//        }
//        EXPECT_EQ_WAIT(true, observer.called(), kMaxWaitMs);
//        desc = observer.release_desc();
//        if (observer.result()  ExpectIceRestart())
//    {
//            EXPECT_EQ(0u, (desc).candidates(0).count());
//        }
//        return observer.result();
//    }

//    bool DoCreateOffer(SessionDescriptionInterface desc)
//    {
//        return DoCreateOfferAnswer(desc, true);
//    }

//    bool DoCreateAnswer(SessionDescriptionInterface desc)
//    {
//        return DoCreateOfferAnswer(desc, false);
//    }

//    bool DoSetLocalDescription(SessionDescriptionInterface desc)
//    {
//        MockSetSessionDescriptionObserver >
//                observer(new rtc.RefCountedObject<
//                    MockSetSessionDescriptionObserver>());
//        Console.WriteLine("INFO) << _id << "SetLocalDescription ";


//        _peerConnection.SetLocalDescription(observer, desc);
//        // Ignore the observer result. If we wait for the result with
//        // EXPECT_TRUE_WAIT, local ice candidates might be sent to the remote peer
//        // before the offer which is an error.
//        // The reason is that EXPECT_TRUE_WAIT uses
//        // rtc.Thread.Current().ProcessMessages(1);
//        // ProcessMessages waits at least 1ms but processes all messages before
//        // returning. Since this test is synchronous and send messages to the remote
//        // peer whenever a callback is invoked, this can lead to messages being
//        // sent to the remote peer in the wrong order.
//        // TODO(perkj): Find a way to check the result without risking that the
//        // order of sent messages are changed. Ex- by posting all messages that are
//        // sent to the remote peer.
//        return true;
//    }

//    bool DoSetRemoteDescription(SessionDescriptionInterface desc)
//    {
//        MockSetSessionDescriptionObserver >
//            observer(new rtc.RefCountedObject<
//                MockSetSessionDescriptionObserver>());
//        Console.WriteLine("INFO) << _id << "SetRemoteDescription ";


//        _peerConnection.SetRemoteDescription(observer, desc);
//        EXPECT_TRUE_WAIT(observer.called(), kMaxWaitMs);
//        return observer.result();
//    }

//    // This modifies all received SDP messages before they are processed.
//    void FilterIncomingSdpMessage(string sdp)
//    {
//        if (_removeMsId)
//        {
//            char kSdpSsrcAttribute[] = "a=ssrc:";
//            RemoveLinesFromSdp(kSdpSsrcAttribute, sdp);
//            char kSdpMsidSupportedAttribute[] = "a=msid-semantic:";
//            RemoveLinesFromSdp(kSdpMsidSupportedAttribute, sdp);
//        }
//        if (_removeBundle)
//        {
//            char kSdpBundleAttribute[] = "a=group:BUNDLE";
//            RemoveLinesFromSdp(kSdpBundleAttribute, sdp);
//        }
//        if (_removeSdes)
//        {
//            char kSdpSdesCryptoAttribute[] = "a=crypto";
//            RemoveLinesFromSdp(kSdpSdesCryptoAttribute, sdp);
//        }
//    }


//    //IRtcPortAllocatorFactory _allocatorFactory;
//    IRtcPeerConnection _peerConnection;

//    bool _autoAddStream = true;

//    Dictionary<int, Tuple<string, string>> _iceUfragPwd = new Dictionary<int, Tuple<string, string>>;
//    bool _expectIceRestart = false;

//    // Needed to keep track of number of frames sent.
//    FakeAudioCaptureModule _fakeAudioCaptureModule;
//    // Needed to keep track of number of frames received.
//    Dictionary<string, Tuple<IMediaStreamTrack, FakeVideoTrackRenderer>> _fakeVideoRenderers = new Dictionary<string, Tuple<IMediaStreamTrack, FakeVideoTrackRenderer>>();

//    // Needed to ensure frames aren't received for removed tracks.
//    List<FakeVideoTrackRenderer> _removedFakeVideoRenderers;

//    // Needed to keep track of number of frames received when external decoder
//    // used.
//    //FakeWebRtcVideoDecoderFactory _fakeVideoDecoderFactory = null;
//    //FakeWebRtcVideoEncoderFactory _fakeVideoEncoderFactory = null;
//    bool _videoDecoderFactoryEnabled = false;
//    MediaConstraints _videoConstraints;

//    // For remote peer communication.
//    SignalingMessageReceiver _signalingMessageReceiver = null;

//    // Store references to the video capturers we've created, so that we can stop
//    // them, if required.
//    //List<RtcVideoCapturer> _videoCapturers;

//    MediaConstraints _sessionDescriptionConstraints;
//    bool _removeMsId = false;  // True if MSID should be removed in received SDP.
//    bool _removeBundle = false;  // True if bundle should be removed in received SDP.
//    bool _removeSdes = false;  // True if a=crypto should be removed in received SDP.

//    IRtcDataChannel _dataChannel;
//};
//}
