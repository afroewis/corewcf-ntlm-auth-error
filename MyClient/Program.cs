// See https://aka.ms/new-console-template for more information

using System.ServiceModel.Security;
using MyClient.Service;

Console.WriteLine("Hello, World!");

var x = new ServiceClient();
x.ClientCredentials.ServiceCertificate.SslCertificateAuthentication =
    new X509ServiceCertificateAuthentication()
    {
        CertificateValidationMode = X509CertificateValidationMode.None,
        RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck
    };
Console.WriteLine("Calling WCF Method GetData");
var ret = await x.GetDataAsync(5);
Console.WriteLine("Return of GetData: {0}", ret);
