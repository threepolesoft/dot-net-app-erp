using AppBO.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text.Encodings.Web;

namespace RestAPI.Hubs
{
    public class UserHub: Hub
    {
        private static ConcurrentDictionary<string, List<string>> clientConnections = new();
        private static ConcurrentDictionary<string, string> clientAndroid = new();
        private static ConcurrentDictionary<string, string> clientWeb = new();
        public override async Task<Task> OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["user"]; // assuming ?user=username
            var platrorm = Context.GetHttpContext()?.Request.Query["platrorm"].ToString(); // assuming ?platrorm=adroid or ios or web
            if (!string.IsNullOrEmpty(userId))
            {
                // Ensure that the userId exists in the dictionary
                if (!clientConnections.ContainsKey(userId))
                {
                    clientConnections[userId] = new List<string>();
                }

                // Add the current connectionId for the user
                clientConnections[userId].Add(Context.ConnectionId);

                if (!string.IsNullOrEmpty(platrorm))
                {
                    if (platrorm == "android")
                    {
                        clientAndroid[userId] = Context.ConnectionId;
                    }
                    else if (platrorm == "web")
                    {
                        clientWeb[userId] = Context.ConnectionId;
                    }
                }
                await Clients.All.SendAsync("ActiveUser", clientConnections, clientAndroid, clientWeb);
            }
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            var user = clientConnections.FirstOrDefault(x => x.Value.Contains(Context.ConnectionId)).Key;
            if (!string.IsNullOrEmpty(user))
            {
                // Remove the connectionId from the list of connections for this user
                //clientConnections[user].Remove(user);

                clientConnections.TryRemove(user, out _);
                clientAndroid.TryRemove(user, out _);
                clientWeb.TryRemove(user, out _);

                // If there are no more connections for this user, remove the user from the dictionary
                //if (clientConnections[user].Count == 0)
                //{
                //    clientConnections.TryRemove(user, out _);
                //}

                await Clients.All.SendAsync("ActiveUser", clientConnections, clientAndroid, clientWeb);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Init()
        {
            await Clients.All.SendAsync("ActiveUser", clientConnections, clientAndroid, clientWeb);
        }

        public async Task SendSettings(string user, string settings)
        {
            // Send message to all connected clients of the user
            if (clientConnections.TryGetValue(user, out var connectionIds))
            {
                // Send message to each connectionId associated with the user
                foreach (var connectionId in connectionIds)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveSettings", settings);
                }
            }
        }
    }
}
