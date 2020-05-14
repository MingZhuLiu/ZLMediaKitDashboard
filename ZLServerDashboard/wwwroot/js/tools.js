function isEmpty(obj) {
    if (typeof obj == "undefined" || obj == null || obj == "") {
        return true;
    } else {
        return false;
    }
}

function clog(obj) {
    console.log(obj);
}

function getFormatDate() {
    var date = new Date();
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var hour = date.getHours();
    if (hour >= 0 && hour <= 9) {
        hour = "0" + hour;
    }

    var min = date.getMinutes();
    if (min >= 0 && min <= 9) {
        min = "0" + min;
    }

    var sec = date.getSeconds();
    if (sec >= 0 && sec <= 9) {
        sec = "0" + sec;
    }

    var currentDate = date.getFullYear() + month + strDate
        + hour + min + sec + date.getMilliseconds();
    return currentDate;
}

function guid() {
    return getFormatDate()+'xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function warningMsg(msg) {
    layer.msg(msg, { icon: 3 });
}

function successMsg(msg, time) {
    if (time == null)
        layer.msg(msg, { icon: 6 });
    else
        layer.msg(msg, { icon: 6, time: time * 1000 });

}

function errorMsg(msg) {
    layer.msg(msg, { icon: 5 });
}

function confirmMsg(msg, yes, no) {

    //var index = layer.confirm(msg, {
    //    icon: 3, title: '警告', btn: ['确定', '取消']
    //    , function(index, layero) {
    //        debugger
    //        console.log('yessss');
    //        yes(index, layero);
    //    }, function(index) {
    //        debugger
    //        console.log('nooooo');
    //        no(index, layero);
    //    }
    //});
    var id = guid();
   var form= layer.open({
        type: 1
        , title: false //不显示标题栏
        , closeBtn: false
        , area: '300px;'
        , shade: 0.8
        , id: id //设定一个id，防止重复弹出
        , btn: ['确定', '取消']
        , btnAlign: 'c'
        , moveType: 1 //拖拽模式，0或者1
        , content: '<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">' + msg + '</div>'
        , yes: function (index, layero) {
            yes(index, layero);
        }
        , btn2: function (index, layero) {
            no(index, layero);
        }
    });
    return form;
}