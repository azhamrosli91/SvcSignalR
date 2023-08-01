using Microsoft.AspNetCore.SignalR;

namespace SvcSignalR.Hubs
{
    public class Counting : Hub
    {
        private static int count;
        //public async Task HitCounter()
        //{
        //    count++;

        //    await Clients.All.SendAsync("Counting", count);
        //}
        public override Task OnConnectedAsync()
        {
            count++;
            Clients.All.SendAsync("Counting", count);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            count--;
            Clients.All.SendAsync("Counting", count);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
