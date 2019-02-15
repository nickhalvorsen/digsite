
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
            <digsite-item :item="item" v-on:click="selectItem"/>
        </div>
    </div>

  </div>

    `
})