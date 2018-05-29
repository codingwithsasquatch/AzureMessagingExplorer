using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureMessagingExplorer
{
    public static class EventGridTrigger
    {
        [FunctionName("EventGridTrigger")]
        [Disable("EventGridTrigger_Disabled")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequestMessage req, 
                                [SignalR(HubName = "broadcast")]IAsyncCollector<SignalRMessage> signalRMessages, 
                                TraceWriter log)
        {
            var requestContent = await req.Content.ReadAsStringAsync();

            // Retrieve event type value from the header and return a bad
            // request code if it is missing.
            if (!req.Headers.TryGetValues("Aeg-Event-Type", out var headerValues))
                return req.CreateResponse(HttpStatusCode.BadRequest, "Missing event type in the header.");
            var eventTypeHeaderValue = headerValues.FirstOrDefault();

            // Echo back the validation code if the event type
            // is for a subscription validation.
            if (eventTypeHeaderValue == "SubscriptionValidation")
            {
                var events = JsonConvert.DeserializeObject<EventGridEvent[]>(requestContent);
                if (events.FirstOrDefault()?.Data is JObject dataObject)
                {
                    return req.CreateResponse(HttpStatusCode.OK,
                        new {validationResponse = dataObject["validationCode"]});
                }

                req.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid request");
            }
            else if (eventTypeHeaderValue == "Notification")
            {
                // Write the event to a storage queue
                log.Info(requestContent);
                try
                {
                    await signalRMessages.AddAsync(new SignalRMessage()
                    {
                        Target = "notify",
                        Arguments = new object[] { JsonConvert.DeserializeObject<EventGridEvent[]>(requestContent) }
                    });
                }
                catch (Exception e)
                {
                    log.Info(e.ToString());
                    return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error with storage queue");
                }

                return req.CreateResponse(HttpStatusCode.OK);
            }

            // Return a bad request if the event type is missing from the header.
            return req.CreateResponse(HttpStatusCode.BadRequest, "Missing event type in the header.");
        }
    }
}
