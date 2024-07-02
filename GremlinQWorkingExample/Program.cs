// See https://aka.ms/new-console-template for more information

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using GremlinQWorkingExample;
using static ExRam.Gremlinq.Core.GremlinQuerySource;
using Edge = GremlinQWorkingExample.Edge;
using Vertex = GremlinQWorkingExample.Vertex;

Console.WriteLine("Hello, World!");

string dburl = "";
string db = "";
string graph = "";
string auth = "";

var _g = g
    .UseCosmosDb<Vertex, Edge>(configurator => configurator
        .At(new Uri(dburl))
        .OnDatabase(db)
        .OnGraph(graph)
        .WithPartitionKey(x => x.TenantId!)
        .AuthenticateBy(auth)
        .UseNewtonsoftJson())
    .ConfigureEnvironment(env => env.UseModel(GraphModel.FromBaseTypes<Vertex, Edge>().AddAssemblies(typeof(Node).Assembly)));


var node1 = await _g
    .AddV(new AnotherNodeType()
    {
        Name = "OOI 1",
        TenantId = "123",
        VertexType = VertexType.ObjectOfInterest,
    })
    .FirstAsync();

var node2 = await _g
    .AddV(new Node()
    {
        Name = "OOI 1",
        TenantId = "123",
        VertexType = VertexType.ObjectOfInterest,
    })
    .FirstAsync();

var node3 = await _g
    .AddV(new Node()
    {
        Name = "OOI 1",
        TenantId = "123",
        VertexType = VertexType.ObjectOfInterest,
    })
    .FirstAsync();


await _g
    .V<AnotherNodeType>(node1.Id!)
    .AddE<AnotherEdgeType>()
    .To(__ => __
        .V<Node>(node2.Id))
    .FirstAsync();

await _g
    .V<Node>(node2.Id!)
    .AddE<Edge>()
    .To(__ => __
        .V<Node>(node3.Id))
    .FirstAsync();



var paths = await _g.V<AnotherNodeType>(node1.Id)
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

var query = _g.V<AnotherNodeType>(node1.Id)
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

_g = g
    .UseCosmosDb<Vertex, Edge>(configurator => configurator
        .At(new Uri(dburl))
        .OnDatabase(db)
        .OnGraph(graph)
        .WithPartitionKey(x => x.TenantId!)
        .AuthenticateBy(auth)
        .UseNewtonsoftJson());

paths = await _g.V<AnotherNodeType>(node1.Id)
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

query = _g.V<AnotherNodeType>(node1.Id)
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

var node4 = await _g
    .AddV(new Node()
    {
        Name = "OOI 1",
        TenantId = "123",
        VertexType = VertexType.ObjectOfInterest,
    })
    .FirstAsync();

Console.Read();