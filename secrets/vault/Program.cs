using Vault;
using Vault.Client;
using Vault.Model;

var address = "http://127.0.0.1:8200";
var config = new VaultConfiguration(address);

var vaultClient = new VaultClient(config);
vaultClient.SetToken("myroot");

try
{
    await StoreKeyValueTestAsync();
    await Task.Delay(200);
    
    await StoreCubbyholeAsync();
    // await Task.Delay(200);
}
catch (VaultApiException e)
{
    Console.WriteLine("Failed to read secret with message {0}\n\n{1}", e.Message, e);
}

return;

async Task StoreKeyValueTestAsync()
{
    Console.WriteLine("Key value test...");
    
    var secretData = new Dictionary<string, string>
    {
        { "clientId", "teste" },
        { "clientSecret", "teste2" }
    };

    // Write a secret
    var kvRequestData = new KvV2WriteRequest(secretData);
    
    await vaultClient.Secrets.KvV2WriteAsync("mypath/mysecret", kvRequestData, kvV2MountPath: "secret");
    
    // Read a secret
    var resp = await vaultClient.Secrets.KvV2ReadAsync("mypath/mysecret", kvV2MountPath: "secret");
    Console.WriteLine(resp.Data.Data);
    Console.WriteLine();
}

async Task StoreCubbyholeAsync()
{
    Console.WriteLine("Cubby hole test...");
    var resp = await vaultClient.Secrets.CubbyholeReadAsync("teste");
    Console.WriteLine(resp.Data);
    Console.WriteLine();
}
