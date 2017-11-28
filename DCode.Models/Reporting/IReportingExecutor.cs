using System;


namespace DCode.Models.Reporting
{
    public interface IReportingExecutor : IDisposable
    {
        void Invoke();
    }
}
