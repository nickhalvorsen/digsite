
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
            nearbyMonsters: { }
        },
        uiState: {
            isAwaitingServerResponse: true,
            logMessages: ''
        }
    },
    methods: {
        load: function () {
            console.log('load')
            connection.invoke('RequestPlayerState', this.gameState.playerId).catch(function (err) {
                return console.error(err.toString())
            })

            connection.invoke('RequestDigState', this.gameState.playerId).catch(function (err) {
                return console.error(err.toString())
            })
            connection.invoke('RequestNearbyMonsterState', this.gameState.playerId).catch(function (err) {
                return console.error(err.toString())
            })
        },
        receivePlayerState: function (playerState) {
            console.log("player state:")
            console.log(playerState)
            this.playerState.money = playerState.money
            this.gameState.isLoaded = true
        },
        receiveDigState: function(digState) {
            console.log("dig state: ")
            console.log(digState)
            if (digState === null) {
                this.digState.hasDigState = false
            }
            else {
                this.digState.hasDigState = true
                this.digState.depth = digState.depth
                this.digState.isPaused = digState.isPaused
                this.digState.fuel = digState.fuel
            }
        },
        receiveNearbyMonsterState: function(nearbyMonsterState) {
            console.log("nearby monster state: ")
            console.log(nearbyMonsterState)

            this.digState.nearbyMonsters = nearbyMonsterState
        },
        start: function () {
            console.log('start')
            this.uiState.isAwaitingServerResponse = true
            connection.invoke('StartDigging', this.gameState.playerId).catch(function (err) {
                return console.error(err.toString())
            })
        },
        stop: function () {
            this.uiState.isAwaitingServerResponse = true;
            console.log('stop')

            connection.invoke('StopDigging', this.gameState.playerId).catch(function (err) {
                return console.error(err.toString())
            });
        },
        foundItem: function(thing, value) {
            this.playerState.money += value;
            this.addMessage("you found a " + thing)
        },
        addMessage: function (message) {
            this.uiState.logMessages += '\n'
            this.uiState.logMessages += message

            var textarea = document.querySelector('.messages')
            textarea.scrollTop = textarea.scrollHeight
        }
    }
})



var connection = new signalR.HubConnectionBuilder().withUrl('/digHub').build();

connection.on('Find', function (thing, value) {
    app.foundItem(thing, value)
})

connection.start()
    .catch(function (err) {
        return console.error(err.toString())
    }).then(function () {
        app.load()
    })

connection.on('ReceivePlayerState', function (data) {
    app.receivePlayerState(data)
    app.$data.uiState.isAwaitingServerResponse = false
})


connection.on('ReceiveDigState', function (data) {
    app.receiveDigState(data)
    app.$data.uiState.isAwaitingServerResponse = false
})

connection.on('ReceiveNearbyMonsterState', function (data) {
    app.receiveNearbyMonsterState(data)
    app.$data.uiState.isAwaitingServerResponse = false
})
