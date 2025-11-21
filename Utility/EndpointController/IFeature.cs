using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndpointController
{
    public interface IFeature
    {
        static abstract void Map(IEndpointRouteBuilder app);
    }
}
