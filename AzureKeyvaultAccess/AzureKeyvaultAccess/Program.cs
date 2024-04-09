using System;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

string secretName = "<<ConnectionString>>";
string keyVaultName = "<<keyvault Name>>";
var kvUri = "<<KeyvautlURI>>";
SecretClientOptions options = new SecretClientOptions()
{
    Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
};

var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential(), options);

Console.WriteLine("Retrieving your secret from " + keyVaultName + ".");

KeyVaultSecret secret = client.GetSecret(secretName);

Console.Write("Input the value of your secret > ");
string secretValue = Console.ReadLine();

Console.Write("Creating a secret in " + keyVaultName + " called '" + secretName + "' with the value '" + secretValue + "' ...");

client.SetSecret(secretName, secretValue);

Console.WriteLine(" done.");

Console.WriteLine("Forgetting your secret.");
secretValue = "";
Console.WriteLine("Your secret is '" + secretValue + "'.");

Console.WriteLine("Retrieving your secret from " + keyVaultName + ".");

KeyVaultSecret secret1 = client.GetSecret(secretName);

Console.WriteLine("Your secret is '" + secret1.Value + "'.");

Console.Write("Deleting your secret from " + keyVaultName + " ...");

client.StartDeleteSecret(secretName);

System.Threading.Thread.Sleep(5000);
Console.WriteLine(" done.");