
Vue.component('digsite-item', {
  props: ['item'],
  template: `
    <span class="item">
        <span v-if="item.isEquipped">({{ item.slotName }}) </span>
        <span class="item-name">{{ item.name }}</span> 
        <span v-if="item.currentCooldown > 0">
            <i class="fa fa-clock"/> {{ item.currentCooldown }}
        </span>
    </span>
    `
})