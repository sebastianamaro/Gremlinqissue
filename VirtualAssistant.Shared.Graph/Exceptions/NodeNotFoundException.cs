using VirtualAssistant.Shared.Graph.Models;

namespace VirtualAssistant.Shared.Graph.Exceptions
{
    public class NodeNotFoundException : Exception
    {
        public NodeNotFoundException(string objectOfInterestId, VertexType vertexType) : base($"The {Enum.GetName(typeof(VertexType), vertexType)} with ID: {objectOfInterestId} was not found.") { }
    }
}
