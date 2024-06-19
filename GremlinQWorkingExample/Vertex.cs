using System.ComponentModel.DataAnnotations;

namespace GremlinQWorkingExample
{
    public class Vertex
    {
        public Vertex() { }

        public string? Id { get; set; }

        public string? TenantId { get; set; }

        [EnumDataType(typeof(VertexType))]
        public VertexType VertexType { get; set; }


    }
}
