<template>
  <div>
    <b-container fluid>
        <b-jumbotron fluid>
            <h1 class="lead text-centered">Azure Event Viewer</h1>
            <h3 class="lead centered text-muted">A simple tool for generating sample events in Azure messaging services</h3>
            <p class="centered">some sort of info printed here</p>
            <div class="row centered">
                <b-button variant="primary" v-on:click="start">Start</b-button>
                <b-button variant="danger" v-on:click="stop">Stop</b-button>
            </div>
        </b-jumbotron>
    </b-container>
    <!-- <b-container>
      <b-jumbotron header="Azure Event Viewer" leader="">
          <p>some sort of info printed here</p>
          <b-form-group
              id="fieldset1"
              description="Connection String"
              label="Enter your connection string"
              label-for="connectionstring"
          >
            <b-form-input id="connectionstring" v-model="connectionstring"></b-form-input>
          </b-form-group>
          <b-form-group
              id="fieldset2"
              description="Queue Name"
              label="Enter your queue name"
              label-for="queue"
          >
            <b-form-input id="queue" v-model="queue"></b-form-input>
          </b-form-group>
          <b-button variant="primary" v-on:click="start">Start</b-button>
          <b-button variant="danger" v-on:click="stop">Stop</b-button>
      </b-jumbotron>
    </b-container> -->
    <b-container>
        <div class="container mt-2">
            <ol>
                <event-item
                    v-for="item in eventList"
                    v-bind:event="item"
                    v-bind:key="item.id">
                </event-item>
            </ol>
        </div>
    </b-container>
  </div>
</template>
<script>
import EventItem from './EventItem.vue'

export default {
  components: {
    EventItem
  },
  data: function() {
    return {
        connection: null,
        connectionstring: null,
        connectionInfo: null,
        lastMessage: null,
        messageCount: 0,
        queue: 'queue',
        eventList: [ ],
    }
  },
  methods: {
    getSignalRConnectionInfo: function() {
      let xhttp = new XMLHttpRequest()
      xhttp.open("GET", "https://signalrbroker.azurewebsites.net/api/negotiate", false)
      xhttp.setRequestHeader("Content-type", "application/json")
      xhttp.send()
      return JSON.parse(xhttp.response)
    },
    start: function() {
        this.connectionInfo = this.getSignalRConnectionInfo();
        this.connection = new this.$signalR.HubConnectionBuilder()
            .withUrl(this.connectionInfo.endpoint, {accessTokenFactory: () => this.connectionInfo.accessKey})
            .configureLogging(this.$signalR.LogLevel.Information)
            .build();
        this.connection.start();
        this.connection.on('notify', data => {
            this.messageCount++
            this.lastMessage = new Date().toLocaleString();
            this.eventList.unshift({ id: this.messageCount.toString(), time: this.lastMessage, data: data});

        });
    },
    stop: function() { this.connection.stop() }
  }
}
</script>

<style scoped></style>