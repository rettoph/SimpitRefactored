using KerbalSimpit.Proxy.SerialThroughTCP;
using System.Text.Json;

SimpitProxyConfiguration[] configuration = JsonSerializer.Deserialize<SimpitProxyConfiguration[]>(File.ReadAllText("simpit.proxy.config.json")) ?? Array.Empty<SimpitProxyConfiguration>();
SimpitProxy[] proxies = configuration.Select(x => new SimpitProxy(x)).ToArray();

await Task.WhenAll(proxies.Select(x => x.RunAsync()));