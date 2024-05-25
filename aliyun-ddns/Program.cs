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
                Console.WriteLine();
                Console.WriteLine(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"config","config.json")));
            }
        }
    }
}
