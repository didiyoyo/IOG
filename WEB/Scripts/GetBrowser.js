function getBrowserInfo() {
    var Sys = {};
    var ua = navigator.userAgent.toLowerCase();
    var re = /(msie|firefox|chrome|opera|version).*?([\d.]+)/;
    var m = ua.match(re);
    Sys.browser = m[1].replace(/version/, "'safari");
    Sys.ver = m[2];
    return Sys;
}
// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

function WriteLog(id, value, operate, mark) {
    var start;
    var end;
    var duration = 0;
    start = new Date().Format("yyyy-MM-dd hh:mm:ss");
    end = new Date().Format("yyyy-MM-dd hh:mm:ss");//用户退出时间 
    AddBehaviorRecord(start, end, operate, mark, id, value);
    //$(window).bind('beforeunload', function (e) {
    //    end = new Date().Format("yyyy-MM-dd hh:mm:ss");//用户退出时间 
    //    AddBehaviorRecord( start, end, operate, mark, id, value);
    //});
} 


function AddBehaviorRecord(sdate, edate, operate, mark, field_id, field_value) { 
    var BrowserInfo = navigator.userAgent.toLowerCase();
    $.ajax({
        type: 'POST',
        async: false,//这块至关重要，用$post默认是true
        url: '/IO/ClickLog/Behavior_Record',
        data: { url: document.location.href, sdate: sdate, edate: edate, ip: ip, agent: BrowserInfo, operate: operate, mark: mark, field_id: field_id, field_value: field_value }
    });
}

