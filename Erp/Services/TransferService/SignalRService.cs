using Microsoft.AspNetCore.SignalR.Client;

namespace Erp.Services.TransferService
{
    public class SignalRService
    {
        private readonly IConfiguration _configuration;
        public HubConnection? HubConnection { get; private set; }
        private readonly Dictionary<string, List<Delegate>> _handlers = new();

        public event Action OnConnected;

        public SignalRService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ConnectAsync(string User)
        {
            if (HubConnection is not null && HubConnection.State == HubConnectionState.Connected)
                return;

            var baseUrl = _configuration["BaseUrlApi"];
            var url = $"{baseUrl}/user?user={User}";

            HubConnection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            await HubConnection.StartAsync();
            OnConnected?.Invoke();
            await HubConnection.SendAsync("Init");
        }

        // Subscribe to events
        //public void On<T>(string methodName, Action<T> handler)
        //{
        //    if (!_handlers.TryGetValue(methodName, out var list))
        //    {
        //        list = new List<Delegate>();
        //        _handlers[methodName] = list;

        //        // Register the handler with HubConnection
        //        HubConnection?.On<T>(methodName, (arg) =>
        //        {
        //            if (_handlers.ContainsKey(methodName))
        //            {
        //                foreach (var h in _handlers[methodName])
        //                {
        //                    ((Action<T>)h).Invoke(arg);
        //                }
        //            }
        //        });
        //    }

        //    list.Add(handler);
        //}

        // Unsubscribe from events
        //public void Off<T>(string methodName, Action<T> handler)
        //{
        //    if (_handlers.TryGetValue(methodName, out var list))
        //    {
        //        // Remove the handler from the list
        //        list.Remove(handler);

        //        // If there are no handlers left, you can choose to stop listening to this event
        //        if (list.Count == 0)
        //        {
        //            _handlers.Remove(methodName);
        //            // Optionally, unregister from the HubConnection
        //            // HubConnection?.Remove(methodName); <-- Note: This would require custom implementation to support this.
        //        }
        //    }
        //}

        // Subscribe to events
        public void On(string methodName, Delegate handler)
        {
            var paramTypes = handler.Method.GetParameters().Select(p => p.ParameterType).ToArray();

            HubConnection.On(methodName, paramTypes, (args) =>
            {
                handler.DynamicInvoke(args);
                return Task.CompletedTask;
            });
        }

        // Unsubscribe from events
        public void Off(string methodName, Delegate handler)
        {
            if (_handlers.TryGetValue(methodName, out var list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                {
                    _handlers.Remove(methodName);
                    // SignalR does not provide a built-in 'Off' API directly,
                    // but we clean our local handler list so it won't be called.
                }
            }
        }
    }
}
