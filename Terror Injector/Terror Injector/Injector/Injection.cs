using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Terror_Injector.Injector
{
    public class Injection
    {
        private static readonly IntPtr INTPTR_ZERO = (IntPtr)0;

        private static Injection _instance;

        public bool Injectionsuccsess;

        public static Injection GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Injection();
                }
                return _instance;
            }
        }

        #region WinAPI DllImports

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        #endregion

        private unsafe bool bInject(uint pToBeInjected, string sDllPath)
        {
            bool Result = false;
            string caption = string.Empty;

            IntPtr intPtr = OpenProcess(1082u, 1, pToBeInjected);
            if (intPtr == INTPTR_ZERO)
            {
                caption = "Error hndProc";
            }
            else
            {
                IntPtr procAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                if (procAddress == INTPTR_ZERO)
                {
                    caption = "Error lpLLAddress";
                }
                else
                {
                    IntPtr intPtr2 = VirtualAllocEx(intPtr, (IntPtr)(void*)null, (IntPtr)sDllPath.Length, 12288u, 64u);
                    if (intPtr2 == INTPTR_ZERO)
                    {
                        caption = "Error lpAddress";
                    }
                    else
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(sDllPath);
                        if (WriteProcessMemory(intPtr, intPtr2, bytes, (uint)bytes.Length, 0) == 0)
                        {
                            caption = "Error WriteProcessMemory";
                        }
                        else
                        {
                            if (CreateRemoteThread(intPtr, (IntPtr)(void*)null, 0u, procAddress, intPtr2, 0u, (IntPtr)(void*)null) == INTPTR_ZERO)
                            {
                                caption = "Error CreateRemoteThread";
                            }
                            else
                            {
                                Result = true;
                            }
                        }
                    }
                }
            }

            if (!Result) {
                MessageBox.Show(caption, "Terror Installer");
            }

            CloseHandle(intPtr);
            return Result;
        }

        public DllInjectionResult Inject(string sProcName, string sDllPath)
        {
            DllInjectionResult Result;

            if (!File.Exists(sDllPath))
            {
                Result = DllInjectionResult.DllNotFound;
            }
            else
            {
                uint num = 0u;
                Process[] processes = Process.GetProcesses();

                for (int i = 0; i < processes.Length; i++)
                {
                    if (processes[i].ProcessName == sProcName)
                    {
                        num = (uint)processes[i].Id;
                        break;
                    }
                }

                if (num == 0)
                {
                    Injectionsuccsess = false;

                    Result = DllInjectionResult.ProcessNotFound;
                }
                else
                {
                    Injectionsuccsess = bInject(num, sDllPath);

                    if (Injectionsuccsess)
                    {
                        Result = DllInjectionResult.Success;
                    }
                    else
                    {
                        Result = DllInjectionResult.InjectionFailed;
                    }
                }
            }

            return Result;
        }
    }
}
