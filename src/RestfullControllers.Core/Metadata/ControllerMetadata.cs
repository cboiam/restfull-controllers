using System.Linq;
using System.Collections.Generic;
using System;

namespace RestfullControllers.Core.Metadata
{
    public class ControllerMetadata
    {
        public Type Controller { get; internal set; }
        public string Template { get; internal set; }
        public IEnumerable<ActionMetadata> Actions { get; internal set; }
    }
}