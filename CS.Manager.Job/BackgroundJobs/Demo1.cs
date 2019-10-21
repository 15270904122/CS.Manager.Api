using CS.Manager.Infrastructure.Jobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CS.Manager.Job.BackgroundJobs
{
    public class Demo1Args
    {
        public string Name { get; set; }
    }

    public class Demo1 : Job<Demo1Args>
    {
        public override async Task Execute(Demo1Args args)
        {
            throw new Exception("错误1");
        }
    }
}
