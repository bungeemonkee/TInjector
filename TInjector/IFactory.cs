using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TInjector
{
    public interface IFactory<out T>
        where T : class
    {
        T Make(IRequest request);
    }
}
