using OpenTelemetry;
using OpenTelemetry.Logs;
using System.Collections.Concurrent;

namespace Throttling
{
    internal class MyCompositeProcessor : CompositeProcessor<LogRecord>
    {
        private IEnumerable<BaseProcessor<LogRecord>> _processors;
        public MyCompositeProcessor(IEnumerable<BaseProcessor<LogRecord>> processors):base(processors)
        {
            _processors = processors;
        }
        public override void OnEnd(LogRecord data)
        {
            if (data != null)
            {
                
                foreach (var _processor in _processors)
                {
                    if (_processor.GetType() == typeof(ThrottlingProcessor))
                    {
                       if(((ThrottlingProcessor) _processor).skip)
                        {
                            break;
                        }
                    }
                        _processor.OnEnd(data); 
                  
                    
                }
               
             
            }
           

        }
    }
}
