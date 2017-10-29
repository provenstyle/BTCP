namespace BibleTraining.Web.UI
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.ExceptionHandling;
    using Castle.Core.Logging;
    using Miruken.Mediate;

    /// <summary>
    /// Logs any exception that happened outside of the MediatR pipeline
    /// </summary>
    public class UnhandledExceptionLogger : IExceptionLogger
    {
        private readonly ILogger _logger;

        public UnhandledExceptionLogger(ILogger logger)
        {
            _logger = logger;
        }

        public async Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            if (!Equals(context.Exception?.Data[Stage.Logging], true))
                _logger.Error("Exception unhandled by Mediate", context.Exception);
        }
    }
}