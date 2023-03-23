//using System;

using StructureMap;

namespace Receiver1
{
    internal class StructureMapServiceProvider
    {
        private Container container;

        public StructureMapServiceProvider(Container container)
        {
            this.container = container;
        }
    }
}