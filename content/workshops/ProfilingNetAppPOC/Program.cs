// See https://aka.ms/new-console-template for more information


using ProfilingNetAppPOC;
using ProfilingNetAppPOC.Asyncronous;
using ProfilingNetAppPOC.Instrumentation;
using ProfilingNetAppPOC.ObjectAllocation;
using ProfilingNetAppPOC.ProfilingInDebugMode;

IShowProfiling showProfilingResult = new SortingList();
#region ProfilingDebugMode 
//
showProfilingResult.showProfiling();

#endregion


#region ProfilingAsyncCode

//
showProfilingResult = new TeachingInSchool();
//showProfilingResult.showProfiling();

#endregion



#region ProfilingObjectAllocation

//
showProfilingResult = new FlyweightClient();
//showProfilingResult.showProfiling();

#endregion


#region ProfilingInstrumentation

//
showProfilingResult = new Fibonacci();
//showProfilingResult.showProfiling();//

#endregion





