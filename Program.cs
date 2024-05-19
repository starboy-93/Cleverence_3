using System.Diagnostics;
using System.Threading;

namespace Cleverence_3
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Выберите пункт, который хотите выполнить: \n1. Прочитать переменную \n2. Изменить значение переменной \n3. Выйти из программы");
                var choice = GetIntFromUser();
                switch (choice)
                {
                    case 1:
                        await Console.Out.WriteLineAsync($"Текущее значение переменной count: {await Server.GetCountAsync()}\n");
                        break;
                    case 2:
                        await Console.Out.WriteLineAsync("Введите значение, которое необходимо добавить к переменной count:");
                        var temp = GetIntFromUser();
                        await Server.AddToCountAsync(temp);
                        await Console.Out.WriteLineAsync($"Новое значение переменной count: {await Server.GetCountAsync()}\n");
                        break;
                    case 3:
                        await Console.Out.WriteLineAsync("Вы выбрали завершение программы.");
                        Process.GetCurrentProcess().Kill();
                        break;
                    default:
                        await Console.Out.WriteLineAsync("Вашего выбора нет в списке! Попробуйте еще раз.\n");
                        break;
                }
            }
        }

        private static int GetIntFromUser()
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int num))
                {
                    return num;
                }
                else
                {
                    Console.Write("Ошибка ввода. Пожалуйста, введите корректное число: \n");
                }
            }
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


        public static async Task AddToCountAsync(int value)
        {
            await writeLock.WaitAsync();
            try
            {
                //симуляция долгой записи
                await Task.Delay(1000);
                count += value;
            }
            finally
            {
                writeLock.Release();
            }
        }
    }
}
