'use strict';

    var cachedCamera, cachedMicrophone, cachedSpeaker, savedDevices;
    var bSurvey = false;

    var Application, client, myConversation, endpointSip, conferenceUri, chatService, videoService, username, timer;
    var token, discoverlink, meetingInfo;
    var peopleSlots = ["", "", "", "", "", "", ""];
    var videoSlots = [true, false, false, false, false, false, false];

    var registeredListeners = registeredListeners || [];
    registeredListeners.forEach(function (listener) {
        listener.dispose();
    });
    registeredListeners = [];

    var videoService, audioService, conversation;
    var fullscreen = false;

    if (getParameterByName("name") != "") {
        username = getParameterByName("name");
    } else {
        username = $("#Customer").val();
    }

    conferenceUri = $("#OnlineMeetingUri").val();

    $(window).bind('beforeunload', function () {
        SignOut();
    });

    Skype.initialize({ apiKey: config.apiKey }, function (api) {
        window.skypeWebAppCtor = api.application;
        window.skypeWebApp = new api.application();
        client = window.skypeWebApp;
        Init();
    }, function (err) {
        alert('some error occurred: ' + err);
    });


    function Init() {
        $('#menuHangup').click(function () {
            SignOut();
        });

        token = 'Bearer ' + $("#AnonymousToken").val();
        discoverlink = $("#DiscoverUri").val();
        conferenceUri = $("#OnlineMeetingUri").val();
        username = "Anonymous Join User";
        var options = {
            name: username,
            cors: true,
            root: { user: discoverlink },
            token: token
        }
        ShowButton("btn-power");
        client.signInManager.signIn(options)
       .then(function () {
           setupListeners();
           setupDevices();
           joinConference();

       }, function (error) {
           // if something goes wrong in either of the steps above,
           // display the error message
           //console.error(error);
           alert(error);
       });
    }

    function joinConference() {
        var conv, dfd;
        myConversation = client.conversationsManager.getConversationByUri(conferenceUri);
        myConversation.videoService.start();
        setCam();
    }

    function setupListeners() {
        var person;

        var addedListener = client.conversationsManager.conversations.added(function (conversation) {

            var chatService, dfdChatAccept, audioService, dfdAudioAccept, videoService, dfdVideoAccept, selfParticipant, name, timerId;

            chatService = conversation.chatService;
            audioService = conversation.audioService;
            videoService = conversation.videoService;
            //  Participants added to the conversation
            conversation.participants.added(function (p) {
                var target;
                p.video.state.changed(function (newState, reason, oldState) {
                    //person = p.displayName();
                    onChanged(p.displayName(), newState, reason, oldState);

                    // a convenient place to set the video stream container 
                    if (newState == 'Connected') {
                        p.video.channels(0).stream.state.changed(function (ns, r, os) {
                            onChanged(p.displayName(), newState, reason, oldState);
                        });

                        // setTimeout is a workaround
                        setTimeout(function () {
                            person = p.displayName();
                            target = getNextVideoElement(person);
                            p.video.channels(0).stream.source.sink.container.set(document.getElementById(target)).then(function () {
                                setTimeout(function () {
                                    var targetName = target + "Name";
                                    $("#commChannel").append("adding " + person + " to " + targetName + "<br/>");
                                    $("#" + target).show();
                                    $("#" + targetName).html(person).show();
                                    p.video.channels(0).isStarted(true);
                                }, 0)
                            });
                        }, 6000);
                    }


                }); //end state changed


            });

            //  Self is handled differently from other participants - we know we will always hit this first
            conversation.selfParticipant.video.state.changed(function (newState, reason, oldState) {
                var selfChannel, target;
                person = conversation.selfParticipant.displayName();
                onChanged(person, newState, reason, oldState);
                if (newState == 'Connected') {
                    target = "oVideo0"; //me is always 0 slot
                    var targetName = target + "Name";
                    $("#oVideo0Name").html(person).show();
                    $("#" + target).show();
                    selfChannel = conversation.selfParticipant.video.channels(0);
                    selfChannel.stream.source.sink.container.set(document.getElementById(target)).then(function () {
                        selfChannel.isStarted(true);
                    });
                }
            });

        });
        registeredListeners.push(addedListener);
    }

    function SignOut() {
        // start signing out
        myConversation.leave();
        client.signInManager.signOut()
        .then(function () {
            $("#oVideo0").hide();
            $("#oVideo0Name").hide();
        }, function (error) {
            // or a failure
            alert(error || 'Cannot sign out');
        });
    }

    function onChanged(name, newState, reason, oldState) {
        $("#commChannel").append(name + ': ' + newState + ' <-- ' + oldState + '  Reason: ' + reason + "<br/>");
        if (newState == 'Disconnected') {
            //When someone disconnects we have mini video slots at the top 
            //that we need to reset for new entrance
            resetPeopleSlot(name);
        }
    }

    function resetPeopleSlot(name) {
        for (var i = 0; i < peopleSlots.length; i++) {
            if (peopleSlots[i] == name) {
                videoSlots[i] = false;
                peopleSlots[i] = "";
                var nameSlot = "#oVideo" + i.toString() + "Name";
                $(nameSlot).html("");
                $(nameSlot).hide();
            }
        }
    }

    function getVideoSlot() {
        for (var i = 0; i < videoSlots.length; i++) {
            if (videoSlots[i] == false) {
                videoSlots[i] = true;
                return i;
            }
        }
        return videoSlots.length;

    }

    function getNextVideoElement(name) {
        var i = getVideoSlot();
        peopleSlots[i] = name;
        var nextSlot = "oVideo" + i.toString();
        return nextSlot;
    }

    function setCam() {
        var selectedCameraId = $('#cams').val();
        setCookie("camera", selectedCameraId, 10);
        console.log("camera: " + selectedCameraId);
        var filteredCameras = client.devicesManager.cameras().filter(function (cam) {
            return cam.id() == selectedCameraId;
        });
        if (filteredCameras.length > 0) {
            client.devicesManager.selectedCamera.set(filteredCameras[0]);
        }

    }

    function getParameterByName(name) {
        //Get Query Strings
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }
