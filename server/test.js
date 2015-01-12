//var ipaddress = process.env.OPENSHIFT_NODEJS_IP || "127.0.0.1";
//var port = process.env.OPENSHIFT_NODEJS_PORT || 8080;


var ipaddress = "192.168.0.11";
var port = 8080;

var app = require('http').createServer(handler);
var io = require('socket.io');
var fs = require('fs');

app.listen(port, ipaddress,function(){
});


// Chargement du fichier index.html affiché au client
function handler(req, res) {
    fs.readFile('./index.html', 'utf-8', function(error, content) {
        res.writeHead(200, {"Content-Type": "text/html"});
        res.end(content);
    });
}

// Chargement de socket.io
var io = require('socket.io').listen(app);

// Quand on client se connecte, on le note dans la console
io.sockets.on('connection', function (socket) {
        socket.emit('message', 'Vous êtes bien connecté !');
});

io.sockets.on('test', function(socket){
	socket.broadcast.emit('testresponse', 'test réussi');
});

