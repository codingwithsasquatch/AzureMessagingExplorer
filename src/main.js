import Vue from 'vue'
import BootstrapVue from "bootstrap-vue"
import App from './App.vue'
import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap-vue/dist/bootstrap-vue.css"

//const HelloVue = require('./hellovue.js')

Vue.use(BootstrapVue)

new Vue({
  el: '#app',
  render: h => h(App)
})