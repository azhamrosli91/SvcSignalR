using libMasterObject;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace SvcSignalR.Hubs
{
    public class Notification : Hub
    {
        public async Task AddToCompany(string companyName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, companyName);

            var usr = Context.UserIdentifier;

            await Clients.Group(companyName).SendAsync("CompanyNotification", $"{Context.ConnectionId} has joined the group {companyName}.");
        }

        public async Task RemoveFromCompany(string companyName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, companyName);

            await Clients.Group(companyName).SendAsync("CompanyNotification", $"{Context.ConnectionId} has left the group {companyName}.");
        }

        public async Task StartLogin(SignalR_Connection value)
        {
            if (!string.IsNullOrWhiteSpace(value.CompanyName)) {
                value.GroupID = String.Concat(value.CompanyName.Trim(), value.CompanyID.ToString()).Trim();
                await Groups.AddToGroupAsync(Context.ConnectionId, value.GroupID);   
            }
            await Clients.Client(Context.ConnectionId).SendAsync("startLoginAccept", JsonConvert.SerializeObject(value));
        }
        public async Task SentNotification(SignalR_Notification value)
        {
            if (string.IsNullOrWhiteSpace(value.GroupID) && !string.IsNullOrWhiteSpace(value.CompanyName))
            { 
                value.GroupID = String.Concat(value.CompanyName.Trim(), value.CompanyID.ToString()).Trim();
            }
            if (!string.IsNullOrWhiteSpace(value.GroupID)) {
                await Clients.Group(value.GroupID).SendAsync("sendNotificationAccept", JsonConvert.SerializeObject(value));
            }
        }
    }
}
