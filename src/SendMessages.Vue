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
        <div class="container mt-2" id="MessageServiceSelection">
            <b-card header="Messaging Service" class="mt-2">
                <p class="card-text">
                    Select the Azure messaging service you would like to use:
                </p>
                <select id="messageServiceChoice" @change="messageServiceChanged" class="mb-3" size="sm">
                    <option selected></option>
                    <option v-for="service in messageServicesList" :key="service.value" :value="service.value">{{service.text}}</option>
                </select>
            </b-card>
        </div>
        <div class="container mt-2" id="egMessageServiceSettings" hidden>
            <b-card id="egSettingsCard" header="Event Grid Settings">
                <b-form inline>
                    <label class="mr-sm-2">Event Grid Endpoint:</label>
                    <b-form-input id="eventgridSettingsEndpoint"
                        name="egEndpoint"
                        type="text" @change="messageServiceSettingsChanged"></b-form-input>
                </b-form>
                <b-form inline>
                    <label class="mr-sm-2">Event Grid Key:</label>
                    <b-form-input id="eventgridSettingsKey"
                        name="egKey"
                        type="text" @change="messageServiceSettingsChanged"></b-form-input>
                </b-form>
            </b-card>
        </div>
        <div class="container mt-2" id="ehMessageServiceSettings" hidden>
            <b-card id="ehSettings" header="Event Hub Settings">
                <b-form inline>
                    <label class="mr-sm-2">Event Hub Connection String:</label>
                    <b-form-input id="eventhubSettingConnectionString"
                        name="ehConnectionString"
                        type="text" @change="messageServiceSettingsChanged"></b-form-input>
                </b-form>
                <b-form inline>
                    <label class="mr-sm-2">Event Hub Name:</label>
                    <b-form-input id="eventhubSettingEventHub"
                        name="ehEventHub"
                        type="text" @change="messageServiceSettingsChanged"></b-form-input>
                </b-form>
            </b-card>
        </div>
        <div class="container mt-2" id="sbMessageServiceSettings" hidden>
            <b-card id="sbSettings" header="Service Bus Settings">
                <b-form inline>
                    <label class="mr-sm-2">Service Bus Connection String:</label>
                    <b-form-input id="servicebusConnectionString"
                        name="sbConnectionString"
                        type="text" @change="messageServiceSettingsChanged"></b-form-input>
                </b-form>
                <b-form inline>
                    <label class="mr-sm-2">Service Bus Queue:</label>
                    <b-form-input id="servicebusQueue"
                        name="sbQueue"
                        type="text" @change="messageServiceSettingsChanged"></b-form-input>
                </b-form>
            </b-card>
        </div>
        <div class="container mt-2" id="sqMessageServiceSettings" hidden>
            <b-card id="sbSettings" header="Storage Queue Settings">
                <b-form inline>
                    <label class="mr-sm-2">Storage Queue Connection String:</label>
                    <b-form-input id="storagequeueConnectionString"
                        name="sqConnectionString"
                        type="text" @change="messageServiceSettingsChanged"></b-form-input>
                </b-form>
                <b-form inline>
                    <label class="mr-sm-2">Storage Queue Name:</label>
                    <b-form-input id="storagequeueName"
                        name="sqName"
                        type="text" @change="messageServiceSettingsChanged"></b-form-input>
                </b-form>
            </b-card>
        </div>
        <div class="container mt-2" id="MessageSendSettings" hidden>
            <b-card id="MessageSendSettingsCard" header="Generate Messages">
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
        </div>
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
        egSettingsCollapse: true,
        messageServicesList: [
            {text: "Event Grid", value: "eventgrid"},
            {text: "Event Hub", value: "eventhub"},
            {text: "Service Bus", value: "servicebus"},
            {text: "Storage Queue", value: "storagequeue"}
        ],
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
      messageServiceChanged: function(test) {
        this.messageService = messageServiceChoice.value
        Array.prototype.slice.call(document.querySelectorAll('div[id$="MessageServiceSettings"]')).forEach( function(el) {
            el.hidden = true;
        });
        switch (this.messageService) {
            case "eventgrid":
                document.getElementById('egMessageServiceSettings').hidden = false;
                break;
            case "eventhub":
                document.getElementById('ehMessageServiceSettings').hidden = false;
                break;
            case "servicebus":
                document.getElementById('sbMessageServiceSettings').hidden = false;
                break;
            case "storagequeue":
                document.getElementById('sqMessageServiceSettings').hidden = false;
                break;    
        }
      },
      messageServiceSettingsChanged: function() {
        var selector = this.messageService + 'Settings'
        var hasBlanks = [];
        Array.prototype.slice.call(document.querySelectorAll('*[id^='+selector+']')).forEach(function(el) {el.value == '' ? hasBlanks.push(true): hasBlanks.push(false);});
        if (hasBlanks.indexOf(true) == -1) {
            console.log("no more blanks");
            document.getElementById('MessageSendSettings').hidden = false;
        } else {
            console.log("blanks still");
            document.getElementById('MessageSendSettings').hidden = true;
        }
    }
  }
}
</script>

<style scoped>
</style>