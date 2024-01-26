import { Room, Client } from "colyseus";
import { Schema, type, MapSchema, ArraySchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("string") login = "";
    @type("boolean") isWhitePlayer = false;
    @type ("boolean") isTurnReady = false;
}

export class State extends Schema {
    @type({ map: Player }) players = new MapSchema<Player>();
    @type([ "string" ]) playersId = new ArraySchema<string>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, login) {
        const player = new Player();
        player.login = login;

        this.players.set(sessionId, player);
        this.playersId.push(sessionId);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }
    //2
    changeTurnReady(sessionId: string, value: boolean){
        const player = this.players.get(sessionId);
        player.isTurnReady = value;
    }

    TrySetSides(){
        const playersSchema = this.players;
        const playersId = this.playersId;

        if(playersSchema.size == 2){
            playersSchema.get(playersId.shift()).isWhitePlayer = true;
            playersSchema.get(playersId.shift()).isWhitePlayer = false;
        }
    }

    setIsWhite(){
        this.players[0].isWhitePlayer = true;
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
        //3
        this.onMessage("isWhitePlayer", (client, value) => {
            //this.state.setSide(client.sessionId, value) 
        });

        this.onMessage("isTurnReady", (client, value) => {
            this.state.changeTurnReady(client.sessionId, value)
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data) {
        ///Я бы тут проверял, сколько уже игроков в комнате и рандомно выдавал бы сторону, но можно ли узнать обо всех, кто есть в комнате и использовать генератор псевдо случайных чисел?
        // client.send("hello", "world");

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