using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstrumentationSdk
{
    public class TestHttpServer:IDisposable
    {
        private readonly HttpListener _listener;
        private readonly Task _httpListenerTask;
        private readonly AutoResetEvent initialized = new(false);
        public TestHttpServer(Action<HttpListenerContext> action, string port , string hostName)
        {
            this._listener = new HttpListener();

            this._listener.Prefixes.Add($"http://{hostName}:{port}/");
            this._listener.Start();

        
            this._httpListenerTask = new Task(async () =>
            {
                while (true)
                {
                    var ctxTask = this._listener.GetContextAsync();

                    this.initialized.Set();
                    action(await ctxTask);
                }
            });

        }
        public void Dispose()
        {
            try
            {
                Console.WriteLine("Disposing");
                this._listener.Stop();
                this._httpListenerTask.Wait();
            }
            catch (Exception)
            {

            }
        }

        public void StartListener()
        {
            this._httpListenerTask.Start();

            this.initialized.WaitOne();
        }

        public void StopListening()
        {
            this._listener.Stop();
            
        }
    }
}
