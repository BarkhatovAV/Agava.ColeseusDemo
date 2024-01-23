import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("string") login = "";
    //1
    @type ("boolean") isTurnReady = false;
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
    //2
    changeTurnReady(sessionId: string, value: boolean){
        const player = this.players.get(sessionId);
        player.isTurnReady = value;
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 2;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        //broadcast и except: client нужны только когда сообщение отправляется и сразу обрабатывается у клиента
        this.onMessage("spawn", (client, data) => {
            this.broadcast("spawn", JSON.stringify({
                sessionID: client.sessionId, 
                id: data.id, 
                x: data.x, 
                z: data.z}), {except: client});
        });

        this.onMessage("moved", (client, data) => {
            console.log("data.id", data.id);
            console.log("data.targetMapWidthPosition", data.targetMapWidthPosition);
            console.log("data.targetMapLengthPosition", data.targetMapLengthPosition);
            this.broadcast("moved", JSON.stringify({
                sessionID: client.sessionId,
                id: data.id,
                targetMapWidthPosition: data.targetMapWidthPosition,
                targetMapLengthPosition: data.targetMapLengthPosition}), {except: client});
        });
        //3
        this.onMessage("isTurnReady", (client, value) => {
            this.state.changeTurnReady(client.sessionId, value)
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