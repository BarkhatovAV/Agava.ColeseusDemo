import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("string") login = ""
    @type("uint32") isPlayerTurnReady;
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, login) {
        const player = new Player();
        player.login = login;
        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 2;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("spawn", (client, data) => {
            this.broadcast("spawn", JSON.stringify({
                sessionID: client.sessionId, 
                id: data.id, 
                x: data.x, 
                z: data.z}), {except: client});
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data) {
        // client.send("hello", "world");
        this.state.createPlayer(client.sessionId, data.login);
        console.log(data.login);
    }

    onLeave (client) {
        console.log(client.sessionId, "left!");
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }

}
