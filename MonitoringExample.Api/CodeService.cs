using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace MonitoringExample.Api
{
    public class CodeService : ICodeService
    {
        public void Execute(Action action, Action<ExceptionDispatchInfo> onFailure = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (onFailure == null)
                    throw;
                else
                {
                    onFailure(ExceptionDispatchInfo.Capture(ex));
                }
            }
        }

        public async Task ExecuteAsync(Func<Task> action, Func<ExceptionDispatchInfo, Task> onFailure = null)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                if (onFailure == null)
                    throw;
                else
                {
                    await onFailure(ExceptionDispatchInfo.Capture(ex));
                }
            }
        }
    }

    public interface ICodeService
    {
        void Execute(Action action, Action<ExceptionDispatchInfo> onFailure = null);
        Task ExecuteAsync(Func<Task> action, Func<ExceptionDispatchInfo, Task> onFailure = null);
    }
}
