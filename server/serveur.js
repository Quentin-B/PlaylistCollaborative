var express = require('express');
var http = require('http');

var PORT = process.env.OPENSHIFT_NODEJS_PORT || 8080;
var IPADDRESS = process.env.OPENSHIFT_NODEJS_IP || "127.0.0.1";

var musicdata;

var ANDROID = "android_";
var TABLE = "surface_"

// Setup a very simple express application.
app = express();
// The client path is for client specific code.
app.use('/client', express.static(__dirname + '/client'));
// The common path is for shared code: used by both client and server.
app.use('/common', express.static(__dirname + '/common'));
// The root path should serve the client HTML.
app.get('/', function (req, res) {
    res.sendfile(__dirname + '/index.html');
});

// Our express application functions as our main listener for HTTP requests
// in this example which is why we don't just invoke listen on the app object.
server = require('http').createServer(app);
server.listen(PORT, IPADDRESS);


// socket.io augments our existing HTTP server instance.
io = require('socket.io').listen(server);

// Called on a new connection from the client. The socket object should be
// referenced for future communication with an explicit client.
io.sockets.on('connection', function (socket) {

  socket.on('echo', function(data) {
    socket.emit('echo back', data);
  });

  socket.on(ANDROID + 'getmusic', function(message){
    if(!musicdata){
        socket.emit(ANDROID + 'nomusic','musiques non disponibles')
    } else {
        socket.emit(ANDROID + 'music', musicdata);
    }
  });

  socket.on(TABLE + 'sendmusic',function(data){
    musicdata = data;
    socket.broadcast.emit(ANDROID + 'music', musicdata);
  });

  socket.on(ANDROID + 'plus',function(data)){
    socket.broadcast.emit(TABLE + 'plus',data);
  }

  socket.on(ANDROID + 'moins',function(data)){
    socket.broadcast.emit(TABLE + 'moins', data)
  }

});



