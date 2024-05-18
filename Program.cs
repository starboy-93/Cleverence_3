namespace Cleverence_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    public static class Server
    {
        private static int count = 0; //переменная для хранения значения.
        private static readonly SemaphoreSlim readLock = new SemaphoreSlim(10, 10); //семафор для управления параллельным доступом к чтению count.
        private static readonly SemaphoreSlim writeLock = new SemaphoreSlim(1, 1); //семафор для управления последовательным доступом к записи count.

        public static async Task<int> GetCountAsync() //асинхронный метод для безопасного чтения
        {
            await readLock.WaitAsync(); //ожидание доступа к семафору для чтения
            try
            {
                return count;
            }
            finally
            {
                readLock.Release(); //освобождение семафорора чтения
            }
        }

        public static async Task AddToCountAsync(int value) //асинхронный метод для безопасной записи
        {
            await writeLock.WaitAsync(); //ожидание доступа к семафору для записи
            try
            {
                //cимуляция длительной операции добавления
                await Task.Delay(1000);
                count += value;
            }
            finally
            {
                writeLock.Release();  //освобождение семафорора записи
            }
        }

    }
}
