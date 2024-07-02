// See https://aka.ms/new-console-template for more information

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using GremlinQNotWorkingExample;
using GremlinQNotWorkingExample.Model;
using GremlinQNotWorkingExample2;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

Console.WriteLine("Hello, World!");

//This example needs that you already run the GremlinQNotWorkingExample.
//The reason for this, is that 
// Without this line     .ConfigureEnvironment(env=> env.UseModel(GraphModel.FromBaseTypes<Vertex, Edge>().AddAssemblies(typeof(Node).Assembly)))
//You cannot add nodes, because of the "Is not part of the model" error, which you will get if that line is not present.

//Enter here the ID of the Node1 from the GremlinQNotWorkingExample

var _g = g
    .UseCosmosDb<Vertex, Edge>(configurator => configurator
        .At(new Uri("wss://virtualassistantdb.gremlin.cosmos.azure.com:443/"))
        .OnDatabase("virtualassistantdb")
        .OnGraph("samaro")
        .WithPartitionKey(x => x.TenantId!)
        .AuthenticateBy("O9fop5MCJmNOg7gGBGDfJWRK891jOPYALsPO7dbE1RNJPZnNT2ctJCC0yXsBSs8tgYNY0gKgBRIIACDbfSlKAA==")
        .UseNewtonsoftJson());


//var node1 = await _g
//    .AddV(new AnotherNodeType()
//    {
//        Name = "OOI 1",
//        TenantId = "123",
//        VertexType = VertexType.ObjectOfInterest,
//    })
//    .FirstAsync();

//var node2 = await _g
//    .AddV(new Node()
//    {
//        Name = "OOI 1",
//        TenantId = "123",
//        VertexType = VertexType.ObjectOfInterest,
//    })
//    .FirstAsync();

//var node3 = await _g
//    .AddV(new Node()
//    {
//        Name = "OOI 1",
//        TenantId = "123",
//        VertexType = VertexType.ObjectOfInterest,
//    })
//    .FirstAsync();


//await _g
//    .V<AnotherNodeType>(node1.Id!)
//    .AddE<AnotherEdgeType>()
//    .To(__ => __
//        .V<Node>(node2.Id))
//    .FirstAsync();

//await _g
//    .V<Node>(node2.Id!)
//    .AddE<Edge>()
//    .To(__ => __
//        .V<Node>(node3.Id))
//    .FirstAsync();


//var startingQuery = _g.V<AnotherNodeType>()
//    .Not(__ => __.In<Edge>());


var paths = await _g.V<AnotherNodeType>("ea7bce49-526e-417f-b352-3969178be7b6")
    .As("AnotherNodeType")
    .Cast<object>()
    .Out<AnotherEdgeType>()
    .As("start")
    .Cast<Vertex>()
    .OutE<Edge>()
    .As("interaction")
    .InV<Node>()
    .As("end")
    .Path().ToArrayAsync();

var query = _g.V<AnotherNodeType>("ea7bce49-526e-417f-b352-3969178be7b6")
    .As("AnotherNodeType")
    .Cast<object>()
    .Out<AnotherEdgeType>()
    .As("start")
    .Cast<Vertex>()
    .OutE<Edge>()
    .As("interaction")
    .InV<Node>()
    .As("end")
    .Path().Debug();

Console.WriteLine(query);
Console.Read();