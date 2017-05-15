namespace $ApplicationName$.Api.$Entity$
{
    using Improving.MediatR;
    using Improving.Highway.Data.Scope.Concurrency;
    using Entities;

    [RelativeOrder(5), StopOnFailure]
    public class $Entity$Concurency : CheckConcurrency<$Entity$, $Entity$Data>
    {
    }
}
