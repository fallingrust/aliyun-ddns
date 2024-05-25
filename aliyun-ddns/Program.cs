namespace aliyun_ddns
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            while (true)
            {
                await Task.Delay(1000);
                Console.WriteLine(DateTime.Now);
            }
        }
    }
}
