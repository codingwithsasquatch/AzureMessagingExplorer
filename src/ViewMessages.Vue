<template>
  <div>
    <b-container>
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
    </b-container>
    <b-container>
        <b-card-group>
            <event-item 
                v-for="item in eventList"
                v-bind:event="item"
                v-bind:key="item.id">
            </event-item>
        </b-card-group>
    </b-container>
  </div>
</template>
"/static/js/azure-storage.queue.min.js"
<script>
const Queue = require('./azure-storage.queue.min.js')

import EventItem from './EventItem.vue'

export default {
  data() {
    return {
        connectionstring: 'connectionstring',
        queue: 'queue',
        eventList: [ ],
    }
  },
  methods: {
      start: function() {startEventListener();},
      stop: function() {stopEventListener();}
  }
}


var currentBackoff = 0;
var maximumBackoff = 10;
var stopListening = false;
var queueService;
var encoder = new Queue.QueueMessageEncoder.TextBase64QueueMessageEncoder();
var timer;

function startEventListener() {
    queueService = Queue.createQueueService(app.connectionstring);
    getEvents();
}

function stopEventListener() {
    stopListening = true;
}

function getEvents() {
    if (stopListening != true) {
        queueService.getMessages(app.queue, {numOfMessages:32},function(error, messages, response) {
            if(!error){
                if (messages.length>0) {
                    // Reset backoff
                    currentBackoff = 0;

                    for (message in messages) {
                        app.eventList.push(JSON.parse(messages[message].messageText));
                        queueService.deleteMessage(app.queue, messages[message].messageId, messages[message].popReceipt, function(error, response) {
                            if (!error) {
                                //message deleted
                            }
                        });
                    }
                } else {
                    if (currentBackoff < maximumBackoff)
                    {
                        currentBackoff++;
                    }
                }
                timer = setTimeout(getEvents,currentBackoff*1000);
            }
        });
        stopListening = false;
    }
}
</script>

<style scoped></style>