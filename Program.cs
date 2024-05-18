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
        private static int count = 0; //переменная для хранения значения
        private static readonly object lockObject = new object(); //объект для блокировки операций чтения и записи.

        public static int GetCount() //метод для безопасного чтения значения count с использованием блокировки.
        {
            lock (lockObject)
            {
                return count;
            }
        }

        public static void AddToCount(int value) //метод для безопасного добавления значения к count с блокировкой
        {
            lock (lockObject) //сама блокировка операций чтения и записи
            {
                //симуляция длительной операции добавления
                Thread.Sleep(1000);
                count += value;
            }
        }
    }
}
