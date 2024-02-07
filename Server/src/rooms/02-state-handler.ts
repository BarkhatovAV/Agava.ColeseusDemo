import { Room, Client } from "colyseus";
import { Schema, type, MapSchema, ArraySchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("string") login = "";
    @type("boolean") isWhitePlayer;
}

export class State extends Schema {
    @type({ map: Player }) players = new MapSchema<Player>();
    @type([ "string" ]) playersId = new ArraySchema<string>();

    createPlayer(sessionId: string, login) {
        const player = new Player();
        player.login = login;

        this.players.set(sessionId, player);
        this.playersId.push(sessionId);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    TrySetSides(){
        const playersSchema = this.players;
        const playersId = this.playersId;

        if(playersSchema.size == 2){
            playersSchema.get(playersId[0]).isWhitePlayer = false;
            playersSchema.get(playersId[1]).isWhitePlayer = true;
        }
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 2;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("moved", (client, data) => {
            this.broadcast("moved", JSON.stringify({
                sessionID: client.sessionId,
                id: data.id,
                targetMapWidthPosition: data.targetMapWidthPosition,
                targetMapLengthPosition: data.targetMapLengthPosition}), {except: client});
        });

        this.onMessage("capture", (client, data) => {
            this.broadcast("capture", JSON.stringify({
                sessionID: client.sessionId,
                id: data.id,
                widthWayPoints: data.widthWayPoints,
                lengthWayPoints: data.lengthWayPoints}), {except: client});
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data) {
        this.state.createPlayer(client.sessionId, data.login);
        this.state.TrySetSides();

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