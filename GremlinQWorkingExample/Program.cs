// See https://aka.ms/new-console-template for more information

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using GremlinQNotWorkingExample;
using GremlinQWorkingExample;
using static ExRam.Gremlinq.Core.GremlinQuerySource;
using Edge = GremlinQWorkingExample.Edge;
using Vertex = GremlinQWorkingExample.Vertex;

Console.WriteLine("Hello, World!");

var _g = g
    .UseCosmosDb<Vertex, Edge>(configurator => configurator
        .At(new Uri("wss://virtualassistantdb.gremlin.cosmos.azure.com:443/"))
        .OnDatabase("virtualassistantdb")
        .OnGraph("samaro")
        .WithPartitionKey(x => x.TenantId!)
        .AuthenticateBy("O9fop5MCJmNOg7gGBGDfJWRK891jOPYALsPO7dbE1RNJPZnNT2ctJCC0yXsBSs8tgYNY0gKgBRIIACDbfSlKAA==")
        .UseNewtonsoftJson());

var node1 = await _g
    .AddV(new Node()
    {
        Name = "OOI 1",
        TenantId = "123",
        VertexType = VertexType.ObjectOfInterest,
    })
    .FirstAsync();


Console.WriteLine(node1.Id);

Console.Read();