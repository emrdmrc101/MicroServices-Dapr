using Core.Domain.KeyVault;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;

namespace Core.Consul;

public class VaultService
{
    public static void SetVaultSecrets(WebApplicationBuilder? builder)
    {
        var configurationBuilder = (IConfigurationBuilder)builder.Configuration;
        var configuration = (IConfiguration)builder.Configuration;

        var vaultSettings = configuration.GetSection("Vault").Get<VaultSettings>();

        var authMethod = new TokenAuthMethodInfo(vaultSettings?.Token);
        var vaultClientSettings = new VaultClientSettings(vaultSettings?.Address, authMethod);

        var vaultClient = new VaultClient(vaultClientSettings);

        var sysBackend = vaultClient.V1.System;
        var sealStatus = sysBackend.GetSealStatusAsync().Result;
        if (sealStatus.Sealed)
        {
            var unsealResponse = sysBackend.UnsealAsync(vaultSettings?.UnSealKey).Result;
            if (unsealResponse.Sealed)
                throw new Exception("Vault is still sealed");
        }

        var configPrefix = "";
        if (builder.Environment.IsDevelopment())
            configPrefix = "Local";

        if (!string.IsNullOrWhiteSpace(vaultSettings?.ConfigName))
        {
            Secret<SecretData> secret =
                vaultClient.V1.Secrets.KeyValue.V2
                    .ReadSecretAsync($"{configPrefix}{vaultSettings?.ConfigName}", mountPoint: "secret")
                    .Result;

            configurationBuilder.AddInMemoryCollection(
                secret.Data.Data.Select(kvp => new KeyValuePair<string, string?>(kvp.Key, kvp.Value?.ToString()))
            );
        }

        Secret<SecretData> commonSecret =
            vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync($"{configPrefix}CommonConfig", mountPoint: "secret")
                .Result;

        configurationBuilder.AddInMemoryCollection(
            commonSecret.Data.Data.Select(kvp => new KeyValuePair<string, string?>(kvp.Key, kvp.Value?.ToString()))
        );
    }
}