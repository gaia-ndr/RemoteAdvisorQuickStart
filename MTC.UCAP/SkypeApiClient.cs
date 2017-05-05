using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Rtc.Internal.Platform.ResourceContract;
using SkypeApi.Models;
using MTC.RemoteAdvisor.Common;

namespace MTC.UCAP
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

        private Dictionary<string, string> skypeAuthSettings = new Dictionary<string, string>()
                                    {
                                        {"AAD_ClientId", "d54ca66c-9278-46d2-a5dc-f5f74a25c91f"},
                                        {"AAD_ClientSecret", "u1IrBLMrug27nNPLJJHNGINfdVRCWBsYcDSqYoBVktM="},
                                        {"ApplicationEndpointId", "sip:dallasucap@mtcdallas.com"}
                                    };

        public SkypeApiClient()
        {
            if (!Guid.TryParse(skypeAuthSettings["AAD_ClientId"], out _aadClientId))
                throw new InvalidDataException("AAD_ClientId must be a GUID");
            _aadClientSecret = skypeAuthSettings["AAD_ClientSecret"];
            _applicationEndpointId = new SipUri(skypeAuthSettings["ApplicationEndpointId"]);
            Logger = new StringLogger();
            _loggingContext = new LoggingContext(Guid.NewGuid());
            _isInitialized = false;
        }

        private void InitializeEndpoint()
        {
            _platformSettings = new ClientPlatformSettings(null, _aadClientId, null, _aadClientSecret, true);
            var platform = new ClientPlatform(_platformSettings, Logger);

            _eventChannel = new FakeEventChannel();
            var endpointSettings = new ApplicationEndpointSettings(_applicationEndpointId);
            _applicationEndpoint = new ApplicationEndpoint(platform, endpointSettings, _eventChannel);

            _applicationEndpoint.InitializeAsync(null).GetAwaiter().GetResult();
            _applicationEndpoint.InitializeApplicationAsync(null).GetAwaiter().GetResult();
            
            if (_applicationEndpoint.Application != null)
                _isInitialized = true;
        }

        public MeetingOutput ScheduleAdhocMeeting(MeetingInput input)
        {
            AdhocMeetingInput adhocMeetingInput = input.ToAdhocMeetingInput();
            if (!_isInitialized)
                InitializeEndpoint();
            var adhocMeeting = _applicationEndpoint.Application.GetAdhocMeetingResourceAsync(_loggingContext, adhocMeetingInput).Result;
            return adhocMeeting.ToMeetingOutput();
        }

        public SkypeAnonTokenInfo GetAnonymousToken(AnonymousApplicationTokenInput tokenInput)
        {
            if(!_isInitialized)
                InitializeEndpoint();
            var anonToken = _applicationEndpoint.Application.GetAnonApplicationTokenAsync(_loggingContext, tokenInput).Result;
            return new SkypeAnonTokenInfo
                   {
                       AnonToken = anonToken.AuthToken,
                       DiscoverUrl = anonToken.AnonymousApplicationsDiscover.Href
                   };
        }
    }
}
