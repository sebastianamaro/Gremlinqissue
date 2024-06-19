﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistant.Shared.Graph.Models;

namespace GremlinQNotWorkingExample
{
    public class Node : Vertex
    {
        public string Id { get; set; }

        public string TenantId { get; set; }

        public string Name { get; set; }
    }
}
