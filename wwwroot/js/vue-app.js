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
            console.log("received game update payload")
            this.gameState.isLoaded = true
            if (payload.playerState !== null) {
                console.log("player state: ")
                console.log(payload.playerState)
                this.playerState = payload.playerState
            }
            if (payload.digState !== null) {
                console.log("dig state: ")
                console.log(payload.digState)
                this.digState = payload.digState;
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
        equipItem: function(equippedPlayerItemId) {
            console.log("equip")
            console.log(equippedPlayerItemId)
            var unequippedPlayerItemId = this.getUnequipItemId(equippedPlayerItemId)

            connection.invoke('equipItem', this.gameState.playerId, equippedPlayerItemId, unequippedPlayerItemId).catch(function (err) {
                return console.error(err.toString())
            });
        },
        getUnequipItemId(playerItemId) {
            var thisPlayerItemSlot = null
            for (var i = 0; i < this.itemState.length; i++) {
                if (this.itemState[i].playerItemId === playerItemId) {
                    thisPlayerItemSlot = this.itemState[i].itemSlotId
                }
            }

            console.log("thisplayeritemslot:")
            console.log(thisPlayerItemSlot)

            if (thisPlayerItemSlot === null) {
                return null
            }

            for (var i = 0; i < this.itemState.length; i++) {
                console.log("checking:")
                console.log(this.itemState[i].playerItemId)
                console.log(this.itemState[i].itemSlotId)
                if (this.itemState[i].itemSlotId === thisPlayerItemSlot && this.itemState[i].isEquipped && playerItemId !== this.itemState[i].playerItemId) {
                    console.log("yep")
                    return this.itemState[i].playerItemId
                }
            }

            return null
        },
        unequipItem: function(unequippedPlayerItemId) {
            connection.invoke('equipItem', this.gameState.playerId, null, unequippedPlayerItemId).catch(function (err) {
                return console.error(err.toString())
            });
        },
        returnToSurface: function() {
            connection.invoke('returnToSurface', this.gameState.playerId).catch(function (err) {
                return console.error(err.toString())
            });
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
    app.updateGameData(payload)
})

connection.on('ReceiveGameLogMessage', function(message) {
    app.addMessage(message)
})