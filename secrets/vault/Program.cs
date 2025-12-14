using Vault;
using Vault.Client;
using Vault.Model;

var address = "http://127.0.0.1:8200";
var config = new VaultConfiguration(address);

var vaultClient = new VaultClient(config);
vaultClient.SetToken("myroot");

try
{
    var secretData = new Dictionary<string, string>
    {
        { "clientId", "teste" },
        { "clientSecret", "teste2" }
    };

    // Write a secret
    var kvRequestData = new KvV2WriteRequest(secretData);

    vaultClient.Secrets.KvV2Write("mypath", kvRequestData, kvV2MountPath: "kv");

    // Read a secret
    var resp = vaultClient.Secrets.KvV2Read("mypath", kvV2MountPath: "kv");
    Console.WriteLine(resp.Data.Data);
}
catch (VaultApiException e)
{
    Console.WriteLine("Failed to read secret with message {0}", e.Message);
}