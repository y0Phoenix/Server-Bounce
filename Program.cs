using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerBounce
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(@"C:\Users\Admin\Desktop\weepwoopzoneMinecraftServer");
            DateTime bounceTime = DateTime.ParseExact("14:45", "HH:mm", null);
            bool restarting = false;
            string filename = @"C:\Program Files (x86)\Common Files\Oracle\Java\javapath\java.exe";
            while (true)
            {
                Process p = new Process();

                if (restarting == false)
                {
                    restarting = true;
                    bool doesExist = File.Exists(filename);
                    if (!doesExist)
                    {
                        Console.WriteLine(filename + " does not exist!");
                        return;
                    }

                    var processes = Process.GetProcesses().OrderBy(p1 => p1.ProcessName);

                    Process targetProcess = targetProcess = processes.FirstOrDefault(p1 => p1.ProcessName.Contains("java")); // MainWindowTitle == "Minecraft server");
                    if (targetProcess != null)
                    {
                        targetProcess.Kill();
                        targetProcess.WaitForExit();
                    }

                    p.StartInfo.FileName = filename;
                    p.StartInfo.Arguments = @"-server -XX:+UseParNewGC -XX:+CMSIncrementalPacing -XX:ParallelGCThreads=4 -XX:+AggressiveOpts -Xms4G -Xmx4G -jar fabric-server-launch.jar ";
                    p.StartInfo.RedirectStandardOutput = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.UseShellExecute = false;
                    if (!p.Start())
                    {
                        Console.WriteLine(filename + " failed to start!");
                        return;
                    }

                    p.StandardInput.WriteLine("/help");
                }

                while (DateTime.Now.Hour != bounceTime.Hour || DateTime.Now.Minute != bounceTime.Minute)
                {
                    restarting = false;
                    Thread.Sleep(1000);
                }

                if (restarting == false)
                {
                    p.StandardInput.WriteLine("/say Shutting down in 5 minutes...");

                    Thread.Sleep(300000);
                    
                    p.StandardInput.WriteLine("/say Shutting down in 10 seconds...");

                    Thread.Sleep(10000);

                    p.StandardInput.WriteLine("/stop");

                    p.WaitForExit();

                    Thread.Sleep(5000);
                }
            }
        }

        static void DadsShowing()
        { 
            //Server myStars = new Server();
            //myStars.i = 10;
            //Server myFroggie = new Server();
            //myFroggie.ForBestFriends = true;
            //var newTypeThingy = myFroggie.Run();
            //Console.WriteLine(myFroggie == newTypeThingy);
            //Console.WriteLine(myStars == newTypeThingy);

            //Server friends = new Server();
            //Server bestFriends = new Server();
            //friends.i = 10;
            //bestFriends.ForBestFriends = true;
        }
    }

    class Server
    {
        public bool ForBestFriends = false;

        public int i = 5;
        public Server Run()
        {
            Console.WriteLine("I'm alive!");

            var keypressed = Console.ReadKey();

            Console.WriteLine("You pressed the " + keypressed.KeyChar + " key! Bye!");

            Console.WriteLine("Press any key to exist...");

            Console.ReadKey();

            return this;
        }

    }
}
