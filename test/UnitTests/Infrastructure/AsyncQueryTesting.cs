namespace UnitTests.Infrastructure
{
    using System;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using Rhino.Mocks;
    using Rhino.Mocks.Interfaces;

    public static class AsyncQueryTesting
    {
        public interface IAsyncQueryable<out T> : IQueryable<T>, IDbAsyncEnumerable<T> { }

        public static IQueryable<T> TestAsync<T>(this IQueryable<T> queryable)
        {
            var asyncProvider = new TestDbAsyncQueryProvider<T>(queryable.Provider);
            var mockQueryable = MockRepository.GenerateStub<IAsyncQueryable<T>>();
            mockQueryable.Stub(q => q.GetAsyncEnumerator())
                .Return(() => new TestDbAsyncEnumerator<T>(queryable.GetEnumerator()));
            mockQueryable.Stub(q => q.Provider).Return(asyncProvider);
            mockQueryable.Stub(q => q.ElementType).Return(queryable.ElementType);
            mockQueryable.Stub(q => q.Expression).Return(queryable.Expression);
            mockQueryable.Stub(q => q.GetEnumerator()).Return(queryable.GetEnumerator);
            return mockQueryable;
        }

        public static IMethodOptions<T> Return<T>(this IMethodOptions<T> opts, Func<T> factory)
        {
            opts.Return(default(T));    // required for Rhino.Mocks on non-void methods
            opts.WhenCalled(mi => mi.ReturnValue = factory());
            return opts;
        }
    }
}