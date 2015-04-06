using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput.Native;

using System.Diagnostics;
using System.Runtime.InteropServices;
using Gma.UserActivityMonitor;
using WindowsInput;

using System.Reflection;
using System.Web;
using System.Runtime;

namespace League_Auto_Key_Presser
{
    public partial class Form1 : Form
    {
        Timer timer = null;
        bool controlPressed = false;
        bool optionPressed = false;
        bool key9Pressed = false;

        bool keyQPressed = false;
        bool keyWPressed = false;
        bool keyEPressed = false;
        bool keyRPressed = false;

        bool pressingSpell1 = false;
        bool pressingSpell2 = false;
        bool pressingSpell3 = false;
        bool pressingSpell4 = false;
        bool pressingActive = false;

        long pressingSpell1LastTime = 0;
        long pressingSpell2LastTime = 0;
        long pressingSpell3LastTime = 0;
        long pressingSpell4LastTime = 0;
        long pressingActiveLastTime = 0;

        int keypressTime = 10;

        double pressSpell1Interval = 0.05 * 1000000000/100;
        double pressSpell2Interval = 0.4 * 1000000000/100;
        double pressSpell3Interval = 0.3 * 1000000000/100;
        double pressSpell4Interval = 0.5 * 1000000000/100;
        double pressActiveInterval = 0.35 * 1000000000/100;

        bool midPressSpell1 = false;
        bool midPressSpell2 = false;
        bool midPressSpell3 = false;
        bool midPressSpell4 = false;
        bool midPressActive1 = false;
        bool midPressActive2 = false;

        InputSimulator simulator = new InputSimulator();

        public Form1()
        {
            InitializeComponent();
            HookManager.KeyDown += HookManager_KeyDown;
            HookManager.KeyUp += HookManager_KeyUp;

            timer = new Timer();
            timer.Tick +=timer_Tick;
            timer.Interval = 8;
            timer.Start();
    
            pressingSpell1LastTime = DateTime.Now.Ticks;
            pressingSpell2LastTime = DateTime.Now.Ticks;
            pressingSpell3LastTime = DateTime.Now.Ticks;
            pressingSpell4LastTime = DateTime.Now.Ticks;
            pressingActiveLastTime = DateTime.Now.Ticks;
        }

        void timer_Tick(object sender, System.EventArgs e)
        {
            if (pressingSpell1) {
                long elapsedTime = DateTime.Now.Ticks - pressingSpell1LastTime;
                if (elapsedTime >= pressSpell1Interval) {
                    tapSpell1();
                    pressingSpell1LastTime = DateTime.Now.Ticks;
                }
            }
            if (pressingSpell2) {
                long elapsedTime = DateTime.Now.Ticks - pressingSpell2LastTime;
                if (elapsedTime >= pressSpell2Interval) {
                    tapSpell2();
                    pressingSpell2LastTime = DateTime.Now.Ticks;
                }
            }
            if (pressingSpell3) {
                long elapsedTime = DateTime.Now.Ticks - pressingSpell3LastTime;
                if (elapsedTime >= pressSpell3Interval) {
                    tapSpell3();
                    pressingSpell3LastTime = DateTime.Now.Ticks;
                }
            }
            if (pressingSpell4) {
                long elapsedTime = DateTime.Now.Ticks - pressingSpell4LastTime;
                if (elapsedTime >= pressSpell4Interval) {
                    tapSpell4();
                    pressingSpell4LastTime = DateTime.Now.Ticks;
                }
            }
            if (pressingActive) {
                long elapsedTime = DateTime.Now.Ticks - pressingActiveLastTime;
                if (elapsedTime >= pressActiveInterval) {
                    tapActive1();
                    Timer t = new Timer();
                    t.Interval = 1;
                    t.Tick += (o, e2) => { tapActive2(); t.Stop(); };
                    t.Start();
            
                    pressingActiveLastTime = DateTime.Now.Ticks;
                }
            }
        }

        void HookManager_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey) {
                controlPressed = false;
            }
            if (e.KeyCode == Keys.LMenu) {
                optionPressed = false;
            }
            if (e.KeyCode == Keys.D9) {
                    key9Pressed = false;
            }
            if (e.KeyCode == Keys.Q) { //Q
                    keyQPressed = false;
            }
            if (e.KeyCode == Keys.W) { //W
                    keyWPressed = false;
            }
            if (e.KeyCode == Keys.E) { //E
                    keyEPressed = false;
            }
            if (e.KeyCode == Keys.R) { //R
                    keyRPressed = false;
            }
            if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.LMenu || e.KeyCode == Keys.D9 || e.KeyCode == Keys.Q || 
                e.KeyCode == Keys.W || e.KeyCode == Keys.E || e.KeyCode == Keys.R) {
                runLogicPress();
            }
        }

        void HookManager_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey)
            {
                controlPressed = true;
            }
            if (e.KeyCode == Keys.LMenu)
            {
                optionPressed = true;
            }
            if (e.KeyCode == Keys.D9)
            {
                key9Pressed = true;
            }
            if (e.KeyCode == Keys.Q)
            { //Q
                keyQPressed = true;
            }
            if (e.KeyCode == Keys.W)
            { //W
                keyWPressed = true;
            }
            if (e.KeyCode == Keys.E)
            { //E
                keyEPressed = true;
            }
            if (e.KeyCode == Keys.R)
            { //R
                keyRPressed = true;
            }
            if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.LMenu || e.KeyCode == Keys.D9 || e.KeyCode == Keys.Q ||
                e.KeyCode == Keys.W || e.KeyCode == Keys.E || e.KeyCode == Keys.R)
            {
                runLogicPress();
            }
        }
        void runLogicPress() {
            if (controlPressed && optionPressed && key9Pressed) {
                pressingActive = !pressingActive;
                key9Pressed = false;
            }
            pressingSpell1 = keyQPressed;
            pressingSpell2 = keyWPressed;
            pressingSpell3 = keyEPressed;
            pressingSpell4 = keyRPressed;
        }

        void tapSpell1() {
            if (midPressSpell1 == false) {
                pressSpell1();
                Timer t = new Timer();
                t.Interval = keypressTime;
                t.Tick += (o, e) => { releaseSpell1(); t.Stop(); };
                t.Start();
            }
        }
        void tapSpell2() {
            if (midPressSpell2 == false) {
                pressSpell2();
                Timer t = new Timer();
                t.Interval = keypressTime;
                t.Tick += (o, e) => { releaseSpell2(); t.Stop(); };
                t.Start();
            }
        }
        void tapSpell3() {
            if (midPressSpell3 == false) {
                pressSpell3();
                Timer t = new Timer();
                t.Interval = keypressTime;
                t.Tick += (o, e) => { releaseSpell3(); t.Stop(); };
                t.Start();
            }
        }
        void tapSpell4() {
            if (midPressSpell4 == false) {
                pressSpell4();
                Timer t = new Timer();
                t.Interval = keypressTime;
                t.Tick += (o, e) => { releaseSpell4(); t.Stop(); };
                t.Start();
            }
        }
        void tapActive1() {
            if (midPressActive1 == false) {
                pressActive1();
                Timer t = new Timer();
                t.Interval = keypressTime;
                t.Tick += (o, e) => { releaseActive1(); t.Stop(); };
                t.Start();
           }
        }
        void tapActive2() {
            if (midPressActive2 == false) {
                pressActive2();
                Timer t = new Timer();
                t.Interval = keypressTime;
                t.Tick += (o, e) => { releaseActive2(); t.Stop(); };
                t.Start();
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 SendInput(UInt32 nInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] pInputs, Int32 cbSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private const uint WM_CHAR = 0x0102;
        private const int VK_RETURN = 0x0D;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        void pressSpell1() {
            midPressSpell1 = true;
            Process[] processes = Process.GetProcessesByName("League of Legends");
            Console.WriteLine(Process.GetCurrentProcess());
            List<Process> processesList = new List<Process>();
            //simulator.Keyboard.KeyDown(VirtualKeyCode.VK_Z);
            //simulator.Keyboard.KeyPress(VirtualKeyCode.VK_Z);
            //KeyboardSimulator. SimulateKeyDown(VirtualKeyCode.SHIFT);
            //KeyboardEmulator.SendKeyDown(KeyCode.KEY_Z);
            //KeyboardManager.HoldKey((Int16)KeyCode.KEY_Z);
            //KeyboardEmulator.SimulateKeyPress((UInt16)KeyCode.KEY_Z);
            Console.WriteLine("Processes: " + processesList.Count);
            for (int i = 0; i < processesList.Count; i++)
            {
                Console.WriteLine("Sending");
                SendMessage(processesList[i].MainWindowHandle, WM_KEYDOWN, new IntPtr(VK_RETURN), IntPtr.Zero);
                SendMessage(processesList[i].MainWindowHandle, WM_KEYUP, new IntPtr(VK_RETURN), IntPtr.Zero);
            
            }
            SendMessage(Process.GetCurrentProcess().MainWindowHandle, WM_KEYDOWN, new IntPtr(VK_RETURN), IntPtr.Zero);
            SendMessage(Process.GetCurrentProcess().MainWindowHandle, WM_KEYUP, new IntPtr(VK_RETURN), IntPtr.Zero);
        }
        void releaseSpell1() {
            midPressSpell1 = false;
            //simulator.Keyboard.KeyUp(VirtualKeyCode.VK_Z);
            //KeyboardEmulator.SendKeyUp(KeyCode.KEY_Z);
            //KeyboardManager.ReleaseKey((Int16)KeyCode.KEY_Z);
        }
        void pressSpell2() {
            midPressSpell2 = true;
            simulator.Keyboard.KeyDown(VirtualKeyCode.VK_X);
            //KeyboardEmulator.SendKeyDown(KeyCode.KEY_X);
        }
        void releaseSpell2() {
            midPressSpell2 = false;
            simulator.Keyboard.KeyUp(VirtualKeyCode.VK_X);
            //KeyboardEmulator.SendKeyUp(KeyCode.KEY_X);
        }
        void pressSpell3() {
            midPressSpell3 = true;
            simulator.Keyboard.KeyDown(VirtualKeyCode.VK_C);
            //KeyboardEmulator.SendKeyDown(KeyCode.KEY_C);
        }
        void releaseSpell3() {
            midPressSpell3 = false;
            simulator.Keyboard.KeyUp(VirtualKeyCode.VK_C);
            //KeyboardEmulator.SendKeyUp(KeyCode.KEY_C);
        }
        void pressSpell4() {
            midPressSpell4 = true;
            simulator.Keyboard.KeyDown(VirtualKeyCode.VK_V);
            //KeyboardEmulator.SendKeyDown(KeyCode.KEY_V);
        }
        void releaseSpell4() {
            midPressSpell4 = false;
            simulator.Keyboard.KeyUp(VirtualKeyCode.VK_V);
            //KeyboardEmulator.SendKeyUp(KeyCode.KEY_V);
        }
        void pressActive1() {
            midPressActive1 = true;
            simulator.Keyboard.KeyDown(VirtualKeyCode.OEM_2);
            //.KeyboardSimulator.KeyboardEmulator.SendKeyDown(KeyCode.KEY_2);
        }
        void releaseActive1() {
            midPressActive1 = false;
            simulator.Keyboard.KeyUp(VirtualKeyCode.OEM_2);
            //KeyboardEmulator.SendKeyUp(KeyCode.KEY_2);
        }
    void pressActive2() {
        midPressActive2 = true;
        simulator.Keyboard.KeyDown(VirtualKeyCode.OEM_3);
        //KeyboardEmulator.SendKeyDown(KeyCode.KEY_1);
    }
        void releaseActive2() {
            midPressActive2 = false;
            simulator.Keyboard.KeyUp(VirtualKeyCode.OEM_3);
            //KeyboardEmulator.SendKeyUp(KeyCode.KEY_1);
        }
    }
}
