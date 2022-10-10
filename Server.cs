using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServerBounce
{
    // Class organizes/packages related responsibilities and behaviors and data.
    public class MinecraftServer
    {
        private Process _serverProcess;
        private Timer _timer;
        //properties
        public string RunningDirectory { get; }
        public string ProcessName { get; }
        public string ExeFile { get; }
        public string Arguments { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="runningDirectory"></param>
        /// <param name="processName"></param>
        /// <param name="exeFile"></param>
        /// <param name="arguments"></param>
        public MinecraftServer(string runningDirectory, string processName, string exeFile, string arguments)
        {
            // save the info
            RunningDirectory = runningDirectory;
            ProcessName = processName;
            ExeFile = exeFile;
            Arguments = arguments;

            // change directory
            Directory.SetCurrentDirectory(runningDirectory);

            // kill running server process
            Process targetProcess = GetServerProcess();
            if (targetProcess != null)
            {
                targetProcess.Kill();
                targetProcess.WaitForExit();
            }

            // start timer to keep it running
            _timer = new Timer(2000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Start();
        }

        /// <summary>
        /// Timer event code - called every time the timer goes off
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Process targetProcess = GetServerProcess();

            if (targetProcess == null)
            {
                StartServer();
            }
        }

        /// <summary>
        /// Returns the process of running server or null if not running.
        /// </summary>
        /// <returns></returns>
        public Process GetServerProcess()
        {
            var processes = Process.GetProcesses().OrderBy(p1 => p1.ProcessName);
            return processes.FirstOrDefault(p1 => p1.ProcessName.Contains(ProcessName));
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        public void StartServer()
        {
            _serverProcess = new Process();
            _serverProcess.StartInfo.FileName = ExeFile;
            _serverProcess.StartInfo.Arguments = Arguments;
            _serverProcess.StartInfo.RedirectStandardOutput = false;
            _serverProcess.StartInfo.RedirectStandardInput = true;
            _serverProcess.StartInfo.UseShellExecute = false;
            if (!_serverProcess.Start())
            {
                Console.WriteLine(ExeFile + " failed to start!");
                return;
            }

            _serverProcess.StandardInput.WriteLine("/help");
        }

        public void SendMessage(string message)
        {
            _serverProcess.StandardInput.WriteLine("/say " + message);
        }

        public void Stop()
        {
            _serverProcess.StandardInput.WriteLine("/stop");
        }
    }
}
