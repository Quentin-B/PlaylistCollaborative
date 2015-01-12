var io = require('socket.io').listen(80); 

    io.on('connection', function (socket) {
      socket.emit('news', { hello: 'world' });
      socket.on('test', function (data) {
        console.log(data);
        socket.emit('message', {msg: data})
      });
});