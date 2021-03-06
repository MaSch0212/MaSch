using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaSch.Native.Windows.Input
{
    public class KeyListener
    {
        public event Action<Keys>? KeyPressed;

        private readonly bool[] _keyStates = new bool[255];
        private readonly int _interval;
        private Task? _listener;
        private bool _stopRequested;

        public KeyListener(int checkInterval = 200)
        {
            StartListener();
            _interval = checkInterval;
        }

        public void StopListener()
        {
            _stopRequested = true;
        }

        public void StartListener()
        {
            while (_listener != null && _listener.Status != TaskStatus.Running)
                Task.Delay(50);
            _listener = Task.Run(new Action(DoListenerAction));
        }

        public async void StartListenerAsync()
        {
            await Task.Run(() => StartListener());
        }

        public bool IsKeyPressed(Keys key)
        {
            return _keyStates[(int)key];
        }

        public static bool IsControlKey(Keys key)
        {
            return key == Keys.ControlKey || key == Keys.LControlKey || key == Keys.RControlKey;
        }

        public static bool IsShiftKey(Keys key)
        {
            return key == Keys.ShiftKey || key == Keys.LShiftKey || key == Keys.RShiftKey;
        }

        private void DoListenerAction()
        {
            while (!_stopRequested)
            {
                for (int i = 1; i < 255; i++)
                {
                    if (User32.GetAsyncKeyState(i) == -32767)
                    {
                        if (!_keyStates[i])
                        {
                            _keyStates[i] = true;
                            KeyPressed?.Invoke((Keys)i);
                        }
                    }
                    else
                    {
                        _keyStates[i] = false;
                    }
                }

                Task.Delay(_interval);
            }
        }
    }
}
