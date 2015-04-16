using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fase05
{
    public interface ISerializer
    {
        void encode();
        void decode();
    }
}