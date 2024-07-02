// See https://aka.ms/new-console-template for more information

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using GremlinQNotWorkingExample;
using GremlinQNotWorkingExample.Model;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

// ReSharper disable once InvalidXmlDocComment
/**
 * Here you have the 3 issues pinned down. 
 *
 * Issue #1: If I use this line     .ConfigureEnvironment(env => env.UseModel(GraphModel.FromBaseTypes<Vertex, Edge>().AddAssemblies(typeof(Node).Assembly))) to avoid Issues #2 & #3
 * I get no results in the query, even though I should, and the reason is that even when I use .OutE<Edge> it is generating the query as .outE('AnotherEdgeType'). I understand because that is the first type that inherits from Edge and lives in this assembly.
 *https://github.com/Gremlinq/ExRam.Gremlinq/issues/1627 Here I pointed out what debugging brought me to, but I can be wrong
 *
 * Issue #2: If I dont use the line I described above, the .Path() method returns objects of type ExRam.Gremlinq.Support.NewtonsoftJson.DynamicObjectConverterFactory.DynamicObjectConverter<object>.DynamicDictionary
 *
 * Issue #3: If I dont use the line I described above, the AddV method fails with an exception System.ArgumentException: 'GremlinQNotWorkingExample.Node is not part of the model.'
   at ExRam.Gremlinq.Core.GraphElementModelExtensions.GetMetadata(IGraphElementModel model, Type elementType) in /_/src/Core/Extensions/GraphElementModelExtensions.cs:line 9
   at ExRam.Gremlinq.Core.GraphElementModelCache.GraphElementModelCacheImpl.<>c__DisplayClass4_0.<GetLabel>b__2(Type type) in /_/src/Core/Cache/GraphElementModelCache.cs:line 29
   at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
   at System.Linq.Enumerable.TryGetFirst[TSource](IEnumerable`1 source, Boolean& found)
   at System.Linq.Enumerable.FirstOrDefault[TSource](IEnumerable`1 source)
   at ExRam.Gremlinq.Core.GraphElementModelCache.GraphElementModelCacheImpl.<>c.<GetLabel>b__4_0(Type closureType, IGraphElementModel closureModel) in /_/src/Core/Cache/GraphElementModelCache.cs:line 26
   at System.Collections.Concurrent.ConcurrentDictionary`2.GetOrAdd[TArg](TKey key, Func`3 valueFactory, TArg factoryArgument)
   at ExRam.Gremlinq.Core.GraphElementModelCache.GraphElementModelCacheImpl.GetLabel(Type type) in /_/src/Core/Cache/GraphElementModelCache.cs:line 23
   at ExRam.Gremlinq.Core.GremlinQuery`4.<>c__5`1.<AddV>b__5_0(FinalContinuationBuilder`1 builder, TVertex vertex) in /_/src/Core/Queries/GremlinQuery.cs:line 117
   at ExRam.Gremlinq.Core.ContinuationBuilder`2.<>c__7`2.<Build>b__7_0(TOuterQuery outer, TAnonymousQuery _, ContinuationFlags _, ValueTuple`2 state) in /_/src/Core/Queries/ContinuationBuilder/ContinuationBuilder.cs:line 35
   at ExRam.Gremlinq.Core.ContinuationBuilder`2.With[TState,TResult](Func`5 continuation, TState state) in /_/src/Core/Queries/ContinuationBuilder/ContinuationBuilder.cs:line 38
   at ExRam.Gremlinq.Core.ContinuationBuilder`2.Build[TNewQuery,TState](Func`3 builderTransformation, TState state) in /_/src/Core/Queries/ContinuationBuilder/ContinuationBuilder.cs:line 34
   at ExRam.Gremlinq.Core.GremlinQuery`4.AddV[TVertex](TVertex vertex) in /_/src/Core/Queries/GremlinQuery.cs:line 114
   at ExRam.Gremlinq.Core.GremlinQuery`4.ExRam.Gremlinq.Core.IStartGremlinQuery.AddV[TVertex](TVertex vertex) in /_/src/Core/Queries/GremlinQuery.explicit.cs:line 235
   at Program.<<Main>$>d__0.MoveNext() in C:\Users\SebastianAmaro\source\repos\Gremlinqissue\GremlinQNotWorkingExample\Program.cs:line 144
   at Program.<Main>(String[] args)


If you check the other
 */
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