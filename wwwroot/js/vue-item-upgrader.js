
Vue.component('digsite-item-upgrader', {
  props: ['items', 'playerId'],
  data: function() {
    return { 
      selectedItem1: { 
        playerItemId: -1,
        itemId: -1 
      },
      selectedItem2: { 
        playerItemId: -1,
        itemId: -1 
      },
      defaultSelectedItem: {
        playerItemId: -1,
        itemId: -1 
      },
      lastAssigned: 2
    }
  },
  methods: {
    selectItem: function(clickedItem) {
      // unequip slot 1
      if (clickedItem.playerItemId === this.selectedItem1.playerItemId) {
        this.selectedItem1 = -1
        return
      }
      
      // unequip slot 2
      if (clickedItem.playerItemId === this.selectedItem2.playerItemId) {
        this.selectedItem2 = -1
        return
      }

      // select different item 
      if (clickedItem.itemId !== this.selectedItem1.itemId) {
        this.selectedItem1 = clickedItem
        this.selectedItem2 = this.defaultSelectedItem
        return
      }

      // select same item with single already selected
      if (clickedItem.itemId === this.selectedItem1.itemId
        && this.selectedItem2.playerItemId === -1) { 
          this.selectedItem2 = clickedItem
          return
      }

      // select same item with two already selected
      if (clickedItem.itemId === this.selectedItem1.itemId
        && this.selectedItem1.itemId === this.selectedItem2.itemId) {

          if (this.lastAssigned === 2)
          {
            this.selectedItem1 = clickedItem
            this.lastAssigned = 1
            return
          }
          
          if (this.lastAssigned === 1)
          {
            this.selectedItem2 = clickedItem
            this.lastAssigned = 2
            return
          }
      }
    },
    submitUpgrade: function() {
      if (this.selectedItem1.playerItemId === -1 || this.selectedItem2.playerItemId === -1) {
        return
      }

      console.log(this.playerId)

      connection.invoke('upgradeItem', this.playerId, this.selectedItem1.playerItemId, this.selectedItem2.playerItemId).catch(function (err) {
          return console.error(err.toString())
      });

      this.selectedItem1 = this.defaultSelectedItem; 
      this.selectedItem2 = this.defaultSelectedItem; 
    },
    getUpgraderItemCssClass: function(item) {

      var selectedClass = false
      var clickableClass = false
      var nonClickableClass = false
    

        if (item.isAwaitingServerResponse) {
            return  {
                'item': true
                ,'itemPendingServerUpdate': true
            }
        }

        if (item.playerItemId === this.selectedItem1.playerItemId
          || item.playerItemId === this.selectedItem2.playerItemId) {
            selectedClass = true
        }

        if (item.itemSlotId > 1) {
          clickableClass = true
        }
        else {
          nonClickableClass = true
        }

        return {
            'item': true
            , 'selected': selectedClass
            , 'non-clickable': nonClickableClass
            , 'clickable': clickableClass
        }
    },
  },
  template: `
  <div class="item-upgrader">
    <h3>item upgrader</h3>
    <button v-on:click="submitUpgrade" :disabled="this.selectedItem2.playerItemId === -1 || this.selectedItem1.playerItemId === -1">submit upgrade</button>
    <div v-for="item in items">
        <div v-bind:class="getUpgraderItemCssClass(item)" v-on:click="selectItem(item)">
            <digsite-item :item="item"/>
        </div>
    </div>

  </div>

    `
})