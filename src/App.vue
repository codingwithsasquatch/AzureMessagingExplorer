<template>
  <div id="app">
    <b-navbar toggleable="md" type="dark" variant="info">
      <b-navbar-toggle target="nav_collapse"></b-navbar-toggle>
      <b-navbar-brand href="#">Azure Messaging Explorer</b-navbar-brand>
      <b-collapse is-nav id="nav_collapse">
        <b-navbar-nav>
          <b-nav-item @click="activate_section(1)" :class="{ active : active_section == 1 }" href="#">Send</b-nav-item>
          <b-nav-item @click="activate_section(2)" :class="{ active : active_section == 2 }" href="#">View</b-nav-item>
          <b-nav-item @click="activate_section(3)" :class="{ active : active_section == 3 }" href="#" disabled>Manage</b-nav-item>
        </b-navbar-nav>
        <!-- Right aligned nav items -->
        <b-navbar-nav class="ml-auto">
          <b-button @click="login('aad')">Login</b-button>
          <b-button @click="getAuthResponse('aad')">Get Auth</b-button>
          <b-nav-item-dropdown right>
            <!-- Using button-content slot -->
            <template slot="button-content">
              <em>Pete</em>
            </template>
            <b-dropdown-item href="#">Profile</b-dropdown-item>
            <b-dropdown-item href="#">Signout</b-dropdown-item>
          </b-nav-item-dropdown>
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
    <div>
      <send-messages v-show="active_section == 1"></send-messages>
      <view-messages v-show="active_section == 2"></view-messages>
    </div>
  </div>
</template>

<script>

//import Login from './Login.vue'
let hello = require('hellojs')

hello.init({
  aad: {
    name: 'Azure Active Directory',
    
    oauth: {
      version: 2,
      auth: 'https://login.microsoftonline.com/common/oauth2/v2.0/authorize',
      grant: 'https://login.microsoftonline.com/common/oauth2/v2.0/token'
    },

    // Authorization scopes
    scope: {
      // you can add as many scopes to the mapping as you want here
      profile: 'user.read',
      offline_access: ''
    },

    scope_delim: ' ',

    login: function(p) {
      if (p.qs.response_type === 'code') {
        // Let's set this to an offline access to return a refresh_token
        p.qs.access_type = 'offline_access';
      }
    },

    base: 'https://www.graph.microsoft.com/v1.0/',

    get: {
      me: 'me'
    },

    xhr: function(p) {
      if (p.method === 'post' || p.method === 'put') {
        toJSON(p);
      }
      else if (p.method === 'patch') {
        hello.utils.extend(p.query, p.data);
        p.data = null;
      }

      return true;
    },

    // Don't even try submitting via form.
    // This means no POST operations in <=IE9
    form: false
  }
})

function login(network) {
	// By defining response type to code, the OAuth flow that will return a refresh token to be used to refresh the access token
	// However this will require the oauth_proxy server
	hello(network).login({display: 'popup'}, (s) => {alert(JSON.stringify(s, true, 2))}).then(function() {
	alert('You are signed in to AAD');

	// Change button to Sign Out 
	var authButton = document.getElementById('auth');
	authButton.innerHTML = 'logout';
	authButton.setAttribute('onclick', 'logout("aad");');

}, function(e) {
	alert('Signin error: ' + e.error.message);
});
}

hello.init({
	aad : `fb513dd1-d662-478d-a897-13f687903ba1`
},{
	redirect_uri: './redirect.html',
	scope: 'User.read'
});

function logout(network) {
	// Removes all sessions, need to call AAD endpoint to do full logout
	hello(network).logout({force: true}, (s) => {alert(JSON.stringify(s, true, 2))}).then(function() {
	alert('You have Signed Out of  AAD');

	// Change button to Sign in
	var authButton = document.getElementById('auth');
	authButton.innerHTML = "Login AAD";
	authButton.setAttribute('onclick', 'login("aad");');

}, function(e) {
	alert('Sign out error: ' + e.error.message);
});
}

function getAuthResponse(network) {
  return hello.getAuthResponse(network);
}

import SendMessages from './SendMessages.vue'
import ViewMessages from './ViewMessages.vue'

export default {
  name: 'app',
  data () {
    return {
      active_section: 1
    }
  },
  components: {
    SendMessages,
    ViewMessages
  }, 
  methods: {
    login: (network) => {
      login(network)
    },
    logout: () => {
      logout(network)
    },
    getAuthResponse: (network) => {
      alert(JSON.stringify(hello.getAuthResponse(network)));
      return hello.getAuthResponse(network)
    },
    activate_section: function(section) {
        this.active_section = section;
    }
  }
}
</script>

<style>
#app {
  font-family: Roboto, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

.container-fluid {
  padding: 0px
}

a.nav-link.active {
  font-weight:600
}

h1, h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}
</style>
