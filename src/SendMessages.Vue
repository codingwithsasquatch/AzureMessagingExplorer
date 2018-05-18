<template>
  <div>
    <b-container fluid>
        <b-jumbotron fluid>
                <h1 class="lead text-centered">Azure Event Generator</h1>
            <div class="row text-center">
                <p class="lead text-muted">A simple tool for generating sample events in Azure messaging services</p>
            </div>
        </b-jumbotron>
    </b-container>
    <b-container>
        <b-card header="Messaging Service">
            <p class="card-text">
                Select the Azure messaging service you would like to use:
            </p>
            <b-btn v-b-toggle="selected+'Collapse'" @click="window.alert('ding')">test</b-btn>
            <b-form-select id="messageService" v-model="selected" class="mb-3" size="sm" >
                    <option value="eventgrid">Event Grid</option>
                    <option value="eventhub">Event Hub</option>
                    <option value="servicebus">Service Bus</option>
                    <option value="storagequeue">Storage Queue</option>
            </b-form-select>
            <p>{{selected}}</p>
            <p>{{selected+'Collapse'}}</p>
        </b-card>
        <b-collapse id="eventgridCollapse" accordion="settingsAccordion" class="mt-2">
            <b-card id="egSettingsCard" header="Event Grid Settings">
                <b-form inline>
                    <label class="mr-sm-2">Event Grid Endpoint:</label>
                    <b-form-input id="egEndpoint"
                        name="egEndpoint"
                        type="text"></b-form-input>
                </b-form>
                <b-form inline>
                    <label class="mr-sm-2">Event Grid Key:</label>
                    <b-form-input id="egKey"
                        name="egKey"
                        type="text"></b-form-input>
                </b-form>
            </b-card>
        </b-collapse>
        <b-collapse id="ehSettingsCollapse" accordion="settingsAccordion" class="mt-2">
            <b-card id="ehSettings" header="Event Hub Settings">
                <b-form inline>
                    <label class="mr-sm-2">Event Hub Connection String:</label>
                    <b-form-input id="ehConnectionString"
                        name="ehConnectionString"
                        type="text"></b-form-input>
                </b-form>
                <b-form inline>
                    <label class="mr-sm-2">Event Hub Name:</label>
                    <b-form-input id="ehEventHub"
                        name="ehEventHub"
                        type="text"></b-form-input>
                </b-form>
            </b-card>
        </b-collapse>
        <b-collapse id="sbSettingsCollapse" accordion="settingsAccordion" class="mt-2">
            <b-card id="sbSettings" header="Service Bus Settings">
                <b-form inline>
                    <label class="mr-sm-2">Service Bus Connection String:</label>
                    <b-form-input id="sbConnectionString"
                        name="sbConnectionString"
                        type="text"></b-form-input>
                </b-form>
                <b-form inline>
                    <label class="mr-sm-2">Service Bus Queue:</label>
                    <b-form-input id="sbQueue"
                        name="sbQueue"
                        type="text"></b-form-input>
                </b-form>
            </b-card>
        </b-collapse>
        <b-collapse id="sqSettingsCollapse" accordion="settingsAccordion" class="mt-2">
            <b-card id="sbSettings" header="Storage Queue Settings">
                <b-form inline>
                    <label class="mr-sm-2">Storage Queue Connection String:</label>
                    <b-form-input id="sqConnectionString"
                        name="sqConnectionString"
                        type="text"></b-form-input>
                </b-form>
                <b-form inline>
                    <label class="mr-sm-2">Storage Queue Name:</label>
                    <b-form-input id="sqName"
                        name="sqName"
                        type="text"></b-form-input>
                </b-form>
            </b-card>
        </b-collapse>
        <b-collapse id="GenerateMessages" visible>
            <b-card id="MessageSendSettings" header="Generate Messages">
                <div class="panel panel-default">
                    <div class="panel-heading">Now let's pick our scenario and send some messages:</div>
                    <div class="panel-body">
                        <li v-for="scen in messageScenarioList" :key="scen.value">
                            <input type="radio" name="messageScenarioSelect" :key="scen.value" :value="scen.value" :checked="scen.checked">{{scen.text}}
                        </li>
                        <br>
                        <p id="messageScenarioDescription">{{messageScenarioList[1].description}}</p>
                        <label>Message duration:</label>
                        <select id="messageDuration">
                            <option v-for="dur in messageDurationList" :key="dur.value" :value="dur.value">{{dur.text}}</option>
                        </select><br>
                        <label>Message frequency: (per second)</label>
                        <select id="messageFrequencyList" v-model="messageFrequency">
                            <option v-for="freq in messageFrequencyList" :key="freq.value" :value="freq.value">{{freq.text}}</option>
                        </select><br>
                        <input class="btn btn-primary" id="startButton" type="button" value="Start" />
                        <input class="btn btn-primary" id="stopButton" type="button" value="Stop" disabled/>
                    </div>
                </div>
            </b-card>
        </b-collapse>
    </b-container>
  </div>
</template>

<script>

export default {
  data() {
    return {
        messageService: null,
        messageScenario: "simple",
        messageDuration: 1,
        messageFrequency: 1,
        selected: null,
        egSettingsCollapse: true,
        messageScenarioList: [
            {text: "Simple", value:"simple", description: "Simple enumerated messages", checked: true},
            {text:"Ninja Battle", value:"ninjaattack", description: "Epic battle between good and bad ninjas played out through sensors on the battlefield"}
        ],
        messageDurationList: [
            {text:"1 minute", value:"1"},
            {text:"2 minute", value:"2"},
            {text:"3 minute", value:"3"},
            {text:"4 minute", value:"4"},
            {text:"5 minute", value:"5"},  
        ],
        messageFrequencyList: [
            {text:"1", value:"1"},
            {text:"5", value:"5"},
            {text:"10", value:"10"},
            {text:"100", value:"100"},
            {text:"1000", value:"1000"},
        ]
    }
  },
  methods: {
      messageServiceChanged: function() {
          switch (messageService.value) {
              case "eventgrid":
                v-b-toggle.eventgridCollapse
                //egSettingsCollapse.visible
          }
      }
  }
}
</script>

<style scoped>
</style>