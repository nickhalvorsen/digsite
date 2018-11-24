
'use strict';



var app = new Vue({
    el: '#app',
    data: {
        gameState: {
            isLoaded: false,
            playerId: 1001,
        },
        playerState: {
            money: -1,
        },
        digState: {
            hasDigState: false,
            depth: -1,
            fuel: -1,
            isPaused: true,
            nearbyMonsterState: { }
        },
        uiState: {
            isAwaitingServerResponse: true,
            logMessages: ''
        },
        itemState : { } 
    },
    methods: {
        load: function () {
            console.log('loading data from server...')

            connection.invoke('GameLoadData', this.gameState.playerId).catch(function (err) {
                return console.error(err.toString())
            })
        },
        updateGameData: function(payload) {
            this.gameState.isLoaded = true
            if (payload.playerState !== null) {
                console.log("player state: ")
                console.log(payload.playerState)
                this.playerState = payload.playerState
            }
            if (payload.digState !== null) {
                console.log("dig state: ")
                console.log(payload.digState)
                this.digState = payload.digState
            }
            if (payload.itemState !== null) {
                console.log("item state: ")
                console.log(payload.itemState)
                this.itemState = payload.itemState
            }
        },
        startDigging: function () {
            console.log('start')
            connection.invoke('StartDigging', this.gameState.playerId).catch(function (err) {
                return console.error(err.toString())
            })
        },
        stopDigging: function () {
            console.log('stop')

            connection.invoke('StopDigging', this.gameState.playerId).catch(function (err) {
                return console.error(err.toString())
            });
        },
        addMessage: function (message) {
            this.uiState.logMessages += '\n' + message

            var textarea = document.querySelector('.messages')
            textarea.scrollTop = textarea.scrollHeight
        },
        equipItem: function(item) {
            console.log(item)
        },
        unequipItem: function() {

        }
    }
})


var connection = new signalR.HubConnectionBuilder().withUrl('/digHub').build();

connection.start()
    .catch(function (err) {
        return console.error(err.toString())
    }).then(function () {
        app.load()
    })

connection.on('ReceiveGameUpdate', function(payload) {
    app.updateGameData(payload);
})

connection.on('ReceiveGameLogMessage', function(message) {
    app.addMessage(message);
})