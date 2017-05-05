var cameras = [];
var microphones = [];
var speakers = [];

function setupDevices() {
    //Setup Devices 

    if (getCookie("defaultDevices")) {
        //get cached devices
        savedDevices = JSON.parse(decodeURIComponent(getCookie("defaultDevices")))
        cachedCamera = savedDevices.camera;
        cachedMicrophone = savedDevices.microphone;
        cachedSpeaker = savedDevices.speaker;
    } else {
        //setup new object to cache
        savedDevices = new Object();
        savedDevices.camera = "";
        savedDevices.microphone = "";
        savedDevices.speaker = "";
    }

    //1) Subscribe to each media
    client.devicesManager.speakers.subscribe();
    client.devicesManager.microphones.subscribe();
    client.devicesManager.cameras.subscribe();
    console.log(client.devicesManager.cameras);

    // observe .speakers
    client.devicesManager.speakers.added(function (ad) {
        speakers.push(ad);
        var html = "<li  id='" + ad.id() + "' role='presentation'><a role='menuitem' tabindex='-1' href='#' onclick=\"setSpeaker('" + ad.id() + "','" + ad.name() + "')\"><span id='s-" + ad.id() + "'class='spkr-item'></span>" + ad.name() + "</a></li>";
        $("#spks").append(html);
        if (cachedMicrophone != null) {
            if (cachedSpeaker == ad.name()) {
                setSpeaker(ad.id(), ad.name());
            }
        }
        if (speakers.length > 1) {
            ShowButton("btn-speakers");
        }

    });
    client.devicesManager.speakers.removed(function (ad) {
        $('#spks option[value="' + ad.id() + '"]')[0].remove();
    });

    // observe .microphones
    client.devicesManager.microphones.added(function (ad) {
        microphones.push(ad);
        var html = "<li  id='" + ad.id() + "' role='presentation'><a role='menuitem' tabindex='-1' href='#' onclick=\"setMicrophone('" + ad.id() + "','" + ad.name() + "')\"><span id='s-" + ad.id() + "'class='cam-item'></span>" + ad.name() + "</a></li>";
        $("#mics").append(html);
        if (cachedMicrophone != null) {
            if (cachedMicrophone == ad.name()) {
                setMicrophone(ad.id(), ad.name());
            }
        }
        if (microphones.length > 1) {
            ShowButton("btn-microphone");
        }

    });
    client.devicesManager.microphones.removed(function (ad) {
        $('#mics option[value="' + ad.id() + '"]')[0].remove();
    });

    // observe .cameras
    client.devicesManager.cameras.added(function (vd) {
        cameras.push(vd);

        console.log("camera added: " + vd.name);
        var html = "<li  id='" + vd.id() + "' role='presentation'><a role='menuitem' tabindex='-1' href='#' onclick=\"setCamera('" + vd.id() + "','" + vd.name() + "')\"><span id='s-" + vd.id() + "'class='cam-item'></span>" + vd.name() + "</a></li>";
        $("#cams").append(html);
        if (cachedCamera != null) {
            if (cachedCamera == vd.name()) {
                setCamera(vd.id(), vd.name());
            }
        }
        if (cameras.length > 1) {
            ShowButton("btn-camera");
        }
    });
    client.devicesManager.cameras.removed(function (vd) {
        $('#cams option[value="' + vd.name() + '"]')[0].remove();
    });

    // observe .selected*
    client.devicesManager.selectedSpeaker.changed(function (ad) {
        var obj = new Object();
        obj.id = ad.id();
        obj.name = ad.name();
        obj.type = ad.type();
        savedDevices.speaker = obj;
        console.log("speaker changed:  " + ad.name());
    });
    client.devicesManager.selectedMicrophone.changed(function (ad) {
        var obj = new Object();
        obj.id = ad.id();
        obj.name = ad.name();
        obj.type = ad.type();
        savedDevices.microphone = obj;
        console.log("microphone changed:  " + ad.name());
    });
    client.devicesManager.selectedCamera.changed(function (vd) {
        var obj = new Object();
        obj.id = vd.id();
        obj.name = vd.name();
        obj.type = vd.type();
        savedDevices.camera = obj;
        console.log("camera changed:  " + vd.name());
        console.log(vd);
    });
}
function setCamera(cameraId, cameraName) {

    $(".cam-item").removeClass('glyphicon');
    $(".cam-item").removeClass('glyphicon-check');
    var el = "#s-" + cameraId;
    $(el).addClass('glyphicon');
    $(el).addClass('glyphicon-check');
    var filteredCameras = client.devicesManager.cameras().filter(function (cam) {
        return cam.name() == cameraName;
    });
    if (filteredCameras.length > 0) {
        client.devicesManager.selectedCamera.set(filteredCameras[0]);
    }

}
function setSpeaker(speakerId, speakerName) {
    $(".spkr-item").removeClass('glyphicon');
    $(".spkr-item").removeClass('glyphicon-check');
    var el = "#s-" + speakerId;
    $(el).addClass('glyphicon');
    $(el).addClass('glyphicon-check');
    var filteredSpeakers = client.devicesManager.speakers().filter(function (spkr) {
        return spkr.name() == speakerName;
    });
    if (filteredSpeakers.length > 0) {
        client.devicesManager.selectedSpeaker.set(filteredSpeakers[0]);
    }
}
function setMicrophone(microphoneId, microphoneName) {
    $(".mic-item").removeClass('glyphicon');
    $(".mic-item").removeClass('glyphicon-check');
    var el = "#s-" + microphoneId;
    $(el).addClass('glyphicon');
    $(el).addClass('glyphicon-check');
    var filteredMicrophones = client.devicesManager.microphone().filter(function (mic) {
        return mic.name() == speakerName;
    });
    if (filteredMicrophones.length > 0) {
        client.devicesManager.selectedMicrophone.set(filteredMicrophones[0]);
    }

}
function ShowButton(element) {
    $("#"+element).removeClass('display');
    $("#"+element).css('display', 'inline-block');
}