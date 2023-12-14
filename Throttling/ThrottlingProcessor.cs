using OpenTelemetry;
using OpenTelemetry.Logs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Throttling
{
    internal class ThrottlingProcessor : BaseProcessor<LogRecord>
    {
        private static ConcurrentDictionary<string, int> categorynames = new ConcurrentDictionary<string, int>();
        private static int counter = 0;
        internal bool skip = false;
        public ThrottlingProcessor()
        {
        }
        public override void OnEnd(LogRecord data)
        {
            if (null != data) { 
           
                counter++;
                categorynames.GetOrAdd(data?.CategoryName, counter);
                categorynames[data.CategoryName] = counter;
            }

            if (categorynames.TryGetValue(data.CategoryName, out var categorycount) && categorycount > 50)
            {
                skip = true;
            }
                Console.WriteLine($"ThrottlingProcessor {counter} - OnEnd");
               
            }
            
        }
    }

    internal class SecondProcessor : BaseProcessor<LogRecord>
    {
        public SecondProcessor()
        {
        }
        public override void OnEnd(LogRecord data)
        {
            Console.WriteLine("SecondProcessor - OnEnd");
            base.OnEnd(data);
        }
    }

