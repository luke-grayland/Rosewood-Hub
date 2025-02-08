using Microsoft.AspNetCore.SignalR;

namespace RosewoodHub;

public class DeviceHub(ILogger<DeviceHub> logger) : Hub
{
    public async Task RegisterDevice(string deviceId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, deviceId);
    }

    //Called by desktop app
    public async Task SendCommand(string deviceId, string command)
    {
        Console.WriteLine($"Command received from {deviceId}: {command}");
        
        await Clients.All.SendAsync("ReceiveCommand", command);
    }

    //Called when device sends data
    public async Task SendDeviceData(string deviceId, string data)
    {
        Console.WriteLine($"Command received from {deviceId}: {data}");
        await Clients.All.SendAsync("ReceiveDeviceData", deviceId, data);
    }
    
    public override async Task OnConnectedAsync()
    {
        logger.LogInformation($"Client connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogWarning($"Client disconnected: {Context.ConnectionId}. Reason: {exception?.Message ?? "No exception"}");
        await base.OnDisconnectedAsync(null);
    }
}