// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using MyClient.Service;

var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
var address = new EndpointAddress(new Uri("https://localhost:44364/Service.svc"));
var client = new ServiceClient(binding, address);
client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication =
    new X509ServiceCertificateAuthentication
    {
        CertificateValidationMode = X509CertificateValidationMode.None,
        RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck
    };

client.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
// Create a local user and fill in the data here. This is using a user named "wcftest" with the password "wcftest". 
client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("wcftest", "wcftest", Environment.MachineName);
Console.WriteLine("Calling WCF Method GetData");
var ret = await client.GetDataAsync(5);
Console.WriteLine("Return of GetData: {0}", ret);
