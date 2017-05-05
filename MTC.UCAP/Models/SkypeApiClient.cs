using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Rtc.Internal.Platform.ResourceContract;
using Microsoft.SfB.PlatformService.SDK.ClientModel;
using Microsoft.SfB.PlatformService.SDK.Common;
using RemoteAdvisor.UCAP;

namespace RemoteAdvisor.UCAP
{
    public class SkypeApiClient
    {
        private ApplicationEndpoint _applicationEndpoint;
        private ClientPlatformSettings _platformSettings;
        private FakeEventChannel _eventChannel;
        public StringLogger Logger;
        private LoggingContext _loggingContext;
        private readonly Guid _aadClientId;
        private readonly string _aadClientSecret;
        private readonly SipUri _applicationEndpointId;
        private bool _isInitialized;

        #region ctor
        public SkypeApiClient()
        {
            Logger = new StringLogger();
            _loggingContext = new LoggingContext(Guid.NewGuid());
            _isInitialized = false;
            if (!Guid.TryParse(System.Configuration.ConfigurationManager.AppSettings["ida:clientid"].ToString(), out _aadClientId))
                throw new InvalidDataException("AAD_ClientId must be a GUID");
            _aadClientSecret = System.Configuration.ConfigurationManager.AppSettings["ida:clientsecret"];
            _applicationEndpointId = new SipUri(System.Configuration.ConfigurationManager.AppSettings["ida:endpointid"]);
        }
        #endregion

        #region InitializeEndPointAsync
        private async Task InitializeEndpointAsync()
        {
            _platformSettings = new ClientPlatformSettings(_aadClientSecret, _aadClientId);
            var platform = new ClientPlatform(_platformSettings, Logger);

            _eventChannel = new FakeEventChannel();
            var endpointSettings = new ApplicationEndpointSettings(_applicationEndpointId);
            _applicationEndpoint = new ApplicationEndpoint(platform, endpointSettings, _eventChannel);

            _applicationEndpoint.InitializeAsync(null).GetAwaiter().GetResult();
            _applicationEndpoint.InitializeApplicationAsync(null).GetAwaiter().GetResult();

            if (_applicationEndpoint.Application != null)
                _isInitialized = true;
        }
        #endregion

        #region CreateSkypeAdhocMeetingAsync
        public async Task<MeetingInfo> CreateSkypeAdhocMeetingAsync(MeetingInput input)
        {
            if (!_isInitialized)
                await InitializeEndpointAsync();

            //setup meeting request

            AdhocMeetingCreationInput inputInfo = new AdhocMeetingCreationInput(input.Subject, AccessLevel.Everyone);

            var newMeeting = await _applicationEndpoint.Application.CreateAdhocMeetingAsync(_loggingContext, inputInfo);

            MeetingInfo meetingInfo = new MeetingInfo(); //convert to common class

            meetingInfo.Subject = newMeeting.Subject;
            meetingInfo.OnlineMeetingUri = newMeeting.OnlineMeetingUri;
            meetingInfo.JoinUrl = newMeeting.JoinUrl;
            return meetingInfo;

        }
        #endregion

        #region GetAnonymousTokenAsync
        public async Task<SkypeAnonTokenInfo> GetAnonymousTokenAsync(MeetingInput meetingInput)
        {
            if (!_isInitialized)
                await InitializeEndpointAsync();

            string origins = System.Configuration.ConfigurationManager.AppSettings["ida:AllowedOrigins"];
            string meetingId = meetingInput.MeetingIdentifier;
            var tokenInfo = await _applicationEndpoint.Application.GetAnonApplicationTokenForMeetingAsync(
                _loggingContext,
                meetingId,
                origins,
                Guid.NewGuid().ToString());


            return new SkypeAnonTokenInfo
            {
                AnonToken = tokenInfo.AuthToken,
                DiscoverUrl = tokenInfo.AnonymousApplicationsDiscoverUri.ToString(),
                ConferenceUri = meetingId
            };
        }
        #endregion

        #region CreateSkypeMeetingAndTokenAsync
        public async Task<MeetingInfo> CreateSkypeMeetingAndTokenAsync(MeetingInput input)
        {
            if (!_isInitialized)
                await InitializeEndpointAsync();
            string origins = System.Configuration.ConfigurationManager.AppSettings["ida:AllowedOrigins"];

            AdhocMeetingCreationInput inputInfo = new AdhocMeetingCreationInput(input.Subject, AccessLevel.Everyone);

            var newMeeting = await _applicationEndpoint.Application.CreateAdhocMeetingAsync(_loggingContext, inputInfo);

            var tokenInfo = await _applicationEndpoint.Application.GetAnonApplicationTokenForMeetingAsync(
                _loggingContext, 
                newMeeting.JoinUrl, 
                origins, 
                Guid.NewGuid().ToString());

            MeetingInfo meetingInfo = new MeetingInfo(); //convert to common class
            meetingInfo.Subject = newMeeting.Subject;
            meetingInfo.OnlineMeetingUri = newMeeting.OnlineMeetingUri;
            meetingInfo.JoinUrl = newMeeting.JoinUrl;

            return meetingInfo;
        }
        #endregion

    }
}
