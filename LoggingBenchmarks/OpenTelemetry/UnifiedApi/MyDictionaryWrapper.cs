using System.Collections.Generic;

namespace LoggingBenchmarks.Loggers
{
    public class MyDictionaryWrapper 
    {
        private KeyValuePair<string, string> _kvpair;
        public MyDictionaryWrapper(KeyValuePair<string,string> kvpair) {

            _kvpair= kvpair;
        }

        public override string ToString()
        {
            return _kvpair.Key.ToString() + ":" + _kvpair.Value.ToString();
        }

    }
}
