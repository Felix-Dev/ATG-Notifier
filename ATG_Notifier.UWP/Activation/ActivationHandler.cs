using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UWP.Activation
{
    // For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.md
    public abstract class ActivationHandler
    {
        // TODO: Prevent null
        public string Name { get; set; } = "";

        public abstract bool CanHandle(object args);

        public abstract Task HandleAsync(object args);
    }

    public abstract class ActivationHandler<T> : ActivationHandler
        where T : class
    {
        public override async Task HandleAsync(object args)
        {
            await HandleInternalAsync(args as T);
        }

        public override bool CanHandle(object args)
        {
            return args is T && CanHandleInternal(args as T);
        }

        protected abstract Task HandleInternalAsync(T args);

        protected virtual bool CanHandleInternal(T args)
        {
            return true;
        }
    }
}
