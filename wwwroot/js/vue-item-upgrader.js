
Vue.component('digsite-item-upgrader', {
  props: ['items'],
  data: function() {
  },
  methods: {
  },
  template: `
  <div class="item-upgrader">

    <div v-for="item in items">
        <div>
            <digsite-item :item="playerItem"/>
        </div>
    </div>

  </div>


    <span class="item" v-on:mousemove="showTooltip" v-on:mouseout="hideTooltip" >
        <span v-if="item.isEquipped">({{ item.slotName }}) </span>
        <span class="item-name">{{ item.name }}</span> 
        <span v-if="item.currentCooldown > 0">
            <i class="fa fa-clock"/> {{ item.currentCooldown }}
        </span>
        <div v-if="tooltipShowing" :style="{ position:'fixed', left: tooltipX, top: tooltipY }" class="item-description-tooltip">
          <span v-if="item.slotName != 'None'">({{ item.slotName }})</span> {{ item.description }}
        </div>
    </span>
    `
})