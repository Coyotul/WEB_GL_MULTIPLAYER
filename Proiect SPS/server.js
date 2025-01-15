const WebSocket = require('ws');
const wss = new WebSocket.Server({ port: 8080 });

let players = {};
let nextPlayerId = 1;

wss.on('connection', (ws) => {
    const playerId = nextPlayerId++;
    players[playerId] = ws;
    console.log(`Jucătorul cu ID-ul ${playerId} s-a conectat`);

    // Trimite ID-ul jucătorului la clientul conectat
    ws.send(JSON.stringify({ type: 'connected', playerId: playerId }));

    // Trimite lista de jucători deja conectați către clientul curent
    for (let id in players) {
        if (id != playerId) {
            const positionMessage = {
                type: 'player_data',
                playerId: id,
                position: { x: 0, y: 0, z: 0 } // Poziția default
            };
            ws.send(JSON.stringify(positionMessage));
            console.log(`Trimitem poziția jucătorului ${id} către jucătorul ${playerId}`);
        }
    }

    // Când primim un mesaj de la client (poziția acestuia)
    ws.on('message', (message) => {
        console.log(`Mesaj primit de la jucătorul ${playerId}:`);
        const data = JSON.parse(message);
        
        // Dacă este un mesaj cu poziția unui jucător
        if (true) {
            // Trimitem poziția acestui jucător către toți ceilalți jucători
            for (let id in players) {
                if (id != playerId) {
                    const positionMessage = {
                        type: 'position',
                        playerId: playerId,
                        position: data.position
                    };
                    players[id].send(JSON.stringify(positionMessage));
                    console.log(`Trimitem poziția jucătorului ${playerId} către jucătorul ${id}`);
                }
            }
        }
    });

    // La deconectarea unui client
    ws.on('close', () => {
        console.log(`Jucătorul cu ID-ul ${playerId} s-a deconectat`);
        delete players[playerId];

        // Trimitem tuturor clienților că un jucător s-a deconectat
        for (let id in players) {
            const disconnectMessage = {
                type: 'player_disconnected',
                playerId: playerId
            };
            players[id].send(JSON.stringify(disconnectMessage));
        }
    });
});

console.log("Serverul WebSocket rulează pe portul 8080");
