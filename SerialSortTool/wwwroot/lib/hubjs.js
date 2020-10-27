//连接signalr
function SignalRConnect(url, qs, op) {
    var query = [];
    for (var k in qs) {
        var v = qs[k];
        query.push(k + '=' + v);
    }
    var queryString = query.join('&');

    var connection = new signalR.HubConnectionBuilder().withUrl(url + (queryString ? '?' + queryString : ''))
        .build();

    connection.on("received", function (text) {
        if (op.received)
            op.received(text);
    });
    var connect = function () {
        console.log(`连接中...${new Date()}`);
        connection.start().then(function () {
            console.log('连接成功');
        }).catch(function (err) {
            if (connection.state === 0) {
                setTimeout(connect, 3000);
            }
        });
    };
    connection.onclose(function () {
        if (op.close)
            op.close();
        setTimeout(connect, 3000);
    });
    connect();
}

function dthub(key, callback) {
    var token = (JSON.parse(localStorage.getItem(key)) || {}).str;
    var url = `/dthub?RequestName=${key}&token=${token}`;
    SignalRConnect(url, {}, {
        received: callback
    });
}

function serialHub(portName, callback) {
    var url = `http://localhost:8081/serial?group=${portName}`;
    SignalRConnect(url, {}, {
        received: callback
    });
}