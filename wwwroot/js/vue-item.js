
Vue.component('digsite-item', {
  props: ['item'],
  template: `
    <span class="item">
        <span v-if="item.isEquipped">({{ item.slotName }}) </span>
        <span class="item-name">{{ item.name }}</span> 
        <span v-if="item.cooldown > 0">
            <i class="fa fa-clock"/> {{ item.cooldown }}
        </span>
    </span>
    `
})