
Vue.component('digsite-item', {
  props: ['item'],
  data: function() {
      return {
          tooltipShowing: false,
          tooltipX: '0px',
          tooltipY: '0px'
      }
  },
  methods: {
    showTooltip: function(event) {
        if (this.item.description === "" || this.item.description === undefined)
        {
            return
        }

        this.tooltipShowing = true
        this.tooltipX = (event.clientX + 20) + 'px'
        this.tooltipY = (event.clientY + 20) + 'px'
    },
    hideTooltip: function(event) {
        this.tooltipShowing = false
    }
  },
  template: `
    <span class="item" v-on:mousemove="showTooltip" v-on:mouseout="hideTooltip" >
        <span v-if="item.isEquipped">({{ item.slotName }}) </span>
        <span class="item-name">{{ item.name }}</span> 
        <span v-if="item.currentCooldown > 0">
            <i class="fa fa-clock"/> {{ item.currentCooldown }}
        </span>
        <span v-if="item.upgradeLevel > 1">(V{{ item.upgradeLevel }})</span>
        <div v-if="tooltipShowing" :style="{ position:'fixed', left: tooltipX, top: tooltipY }" class="item-description-tooltip">
          <span v-if="item.slotName != 'None'">({{ item.slotName }})</span> 
          {{ item.description }} 
        </div>
    </span>
    `
})