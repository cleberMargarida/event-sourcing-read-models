#nullable disable
namespace Application.FunctionalTests.Clients
{
    public static class TaskExtensions
    {
        public static async Task<T> WithDelay<T>(this Task<T> task, TimeSpan delay)
        {
            await Task.Delay(delay);
            return await task;
        }

        public static async Task WithDelay(this Task task, TimeSpan delay)
        {
            await Task.Delay(delay);
            await task;
        }

        public static async Task<T> WithDelay<T>(this Task<T> task, int millisecondsDelay)
        {
            await Task.Delay(millisecondsDelay);
            return await task;
        }

        public static async Task WithDelay(this Task task, int millisecondsDelay)
        {
            await Task.Delay(millisecondsDelay);
            await task;
        }
    }
}