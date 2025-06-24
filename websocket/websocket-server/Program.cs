var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(10)
};

app.UseWebSockets(webSocketOptions);
app.MapGet("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        
        Console.WriteLine("Cliente conectado");

        var buffer = new byte[1024 * 4];
        while (webSocket.State == System.Net.WebSockets.WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            
            if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Fechando", CancellationToken.None);
                Console.WriteLine("Cliente desconectado");
            }
            else
            {
                var received = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Mensagem recebida: {received}");

                var response = System.Text.Encoding.UTF8.GetBytes($"Eco: {received}");
                await webSocket.SendAsync(new ArraySegment<byte>(response), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});

app.Run();