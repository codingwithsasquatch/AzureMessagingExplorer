import Vue from 'vue'
import BootstrapVue from "bootstrap-vue"
import App from './App.vue'
import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap-vue/dist/bootstrap-vue.css"

const HelloJs = require('hellojs/dist/hello.all.min.js')
const VueHello = require('vue-hellojs')

HelloJs.init({
  google: 'GOOGLE_APP_CLIENT_ID',
  facebook: 'FACEBOOK_APP_CLIENT_ID',
  microsoft: 'MICROSOFT_CLIENT_ID'
}, {
  redirect_uri: 'authcallback/'
});

Vue.use(BootstrapVue, VueHello, HelloJs)

new Vue({
  el: '#app',
  render: h => h(App)
})
