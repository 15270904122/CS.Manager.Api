using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CS.Manager.EasyNetQ.Consumer
{
    public interface IBaseConsumer
    {
        void InitSubscribe();
    }
}
