// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Trace;
using System.Collections;
using System.Diagnostics;

public class Program
{

    private static readonly ActivitySource activitySource = new ActivitySource("OpenTelemetry.FirstActivitySource");

    private static async Task Main(string[] args)
    {
        #region notusedcode
        //ActivityListener activityListener = new ActivityListener()
        //{
        //  ActivityStarted = a =>
        //  {
        //      Console.WriteLine( $"ParentId : {a.ParentId} TraceId:{a.TraceId} SpanId: {a.SpanId}");
        //  },
        //    ShouldListenTo = ((a) => { return true; })
        //};

        //ActivitySource.AddActivityListener(activityListener);   
        #endregion

        SetTraceProvider();


        #region Demo1
        //using (var activity = activitySource.StartActivity("MyFirstActivity"))
        //{
        //    activity?.SetTag("Name", "Sunil");
        //    activity?.SetTag("WhatisMyCurrentActivity", Activity.Current?.SpanId);
        //}

        //    using (var activity = activitySource.StartActivity("MyFirstActivity"))
        //{

        //    activity?.SetTag("Name", "Datla");
        //    activity?.SetTag("WhatisMyCurrentActivity", Activity.Current?.SpanId);

        //    using (var childactivity = activitySource.StartActivity("Child Activity"))
        //    {
        //        childactivity?.SetTag("Foo", "Bar");
        //        childactivity?.AddEvent(new ActivityEvent("Bar is closed", DateTime.Now));
        //        childactivity?.SetTag("WhatisMyCurrentActivity", Activity.Current?.SpanId);

        //    }

        //}

        #endregion

        #region Demo2

        //var childactivity2 = activitySource.StartActivity("Child Activity");
        //childactivity2?.SetTag("Demo2", "Demo2Value");

        //childactivity2?.Stop();

        #endregion

        #region Demo3

        //using (var activity = Activity.Current)
        //{
        //    activity?.SetTag("Name", "Sunil");

        //    using (var childactivity = Activity.Current)// activitySource.StartActivity("Child Activity"))
        //    {
        //        childactivity?.SetTag("Foo", "Bar");
        //        childactivity?.AddEvent(new ActivityEvent("Bar is closed", DateTime.Now));

        //    }

        //}
        #endregion

        Console.Read();
    }

   private static void SetTraceProvider()
    {
        Sdk.CreateTracerProviderBuilder()
            .AddSource("*")
            .AddConsoleExporter()
            .Build();
    }
}
