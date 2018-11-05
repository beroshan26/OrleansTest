using Interfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains
{
    public class PersonGrain : Grain, IPerson
    {
        public Task SayHelloAsync(string message)
        {
            string name = this.GetPrimaryKeyString();
            Console.WriteLine($"{name} says: {message}");

            return TaskDone.Done;
        }
    }
}
