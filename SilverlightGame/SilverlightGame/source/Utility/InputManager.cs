using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Reflection;

namespace SilverlightGame.Utility
{
    public class InputManager
    {
        private Dictionary<Key, bool> nowPressed = new Dictionary<Key, bool>();
        private Dictionary<Key, bool> oldPressed = new Dictionary<Key, bool>();

        private Dictionary<Key, bool> down = new Dictionary<Key, bool>();
        private Dictionary<Key, bool> up = new Dictionary<Key, bool>();
        private Dictionary<Key, bool> trigger = new Dictionary<Key, bool>();
        private Dictionary<Key, bool> release = new Dictionary<Key, bool>();
        private Key[] keys;
        private Key[] Keys {
            get { return keys; }
        }

        private bool nowMouseLPressd;
        private bool oldMouseLPressd;
        private bool mouseLDown;
        private bool mouseLUp;
        private bool mouseLTrigger;
        private bool mouseLRelease;

        private Point oldMousePosition;
        private Point nowMousePosition;
        private Point mouseMove;

        private int mouseWheel;
        private int nowMouseWheel;

        private FrameworkElement targetElement = null;

        public void Initialize(FrameworkElement target)
        {
            CreateKeys();
            Clear();
            targetElement = target;
            targetElement.KeyDown += KeyDown;
            targetElement.KeyUp += KeyUp;
            targetElement.MouseLeftButtonDown += MouseLeftDown;
            targetElement.MouseLeftButtonUp += MouseLeftUp;
            targetElement.MouseMove += MouseMove;
            targetElement.MouseWheel += MouseWheel;
            targetElement.MouseLeave += MouseLeave;
            targetElement.LostFocus += LostFocus;
        }

        public void Destroy()
        {
            Clear();
            targetElement.KeyDown -= KeyDown;
            targetElement.KeyUp -= KeyUp;
            targetElement.MouseLeftButtonDown -= MouseLeftDown;
            targetElement.MouseLeftButtonUp -= MouseLeftUp;
            targetElement.MouseMove -= MouseMove;
            targetElement.MouseWheel -= MouseWheel;
            targetElement.MouseLeave -= MouseLeave;
            targetElement.LostFocus -= LostFocus;
            targetElement = null;
        }

        private void CreateKeys()
        {
            FieldInfo[] fieldInfos = typeof(Key).GetFields(BindingFlags.Public | BindingFlags.Static);
            keys = new Key[fieldInfos.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = (Key)fieldInfos[i].GetValue(null);
            }
        }

        public void Clear() {
            Key[] keys = Keys;
            foreach (Key key in keys)
            {
                nowPressed[key] = false;
                oldPressed[key] = false;
                down[key] = false;
                up[key] = false;
                trigger[key] = false;
                release[key] = false;
            }

            nowMouseLPressd = false;
            oldMouseLPressd = false;
            mouseLDown = false;
            mouseLUp = false;
            mouseLTrigger = false;
            mouseLRelease = false;

            mouseMove.X = 0;
            mouseMove.Y = 0;
            mouseWheel = 0;
            nowMouseWheel = 0;
        }

        public void Update(double dt)
        {
            Key[] keys = Keys;
            foreach (Key key in keys)
            {
                down[key] = nowPressed[key];
                up[key] = !nowPressed[key];
                trigger[key] = nowPressed[key] & (oldPressed[key] == false);
                release[key] = oldPressed[key] & (nowPressed[key] == false);

                oldPressed[key] = nowPressed[key];
            }

            mouseLDown = nowMouseLPressd;
            mouseLUp = !nowMouseLPressd;
            mouseLTrigger = nowMouseLPressd & (oldMouseLPressd == false);
            mouseLRelease = oldMouseLPressd & (nowMouseLPressd == false);
    
            oldMouseLPressd = nowMouseLPressd;

            mouseMove.X = nowMousePosition.X - oldMousePosition.X;
            mouseMove.Y = nowMousePosition.Y - oldMousePosition.Y;
            oldMousePosition = nowMousePosition;

            mouseWheel = nowMouseWheel;
            nowMouseWheel = 0;
        }

        public bool isDown(Key key) {
            return down[key];
        }
        public bool isUp(Key key)
        {
            return up[key];
        }
        public bool isTrigger(Key key)
        {
            return trigger[key];
        }
        public bool isRelease(Key key)
        {
            return release[key];
        }

        public bool isMouseLDown()
        {
            return mouseLDown;
        }
        public bool isMouseLUp()
        {
            return mouseLUp;
        }
        public bool isMouseLTrigger()
        {
            return mouseLTrigger;
        }
        public bool isMouseLRelease()
        {
            return mouseLRelease;
        }

        public Point MousePosition()
        {
            return oldMousePosition;
        }
        public Point MouseMove()
        {
            return mouseMove;
        }
        public int MouseWheel()
        {
            return mouseWheel;
        }


        private void KeyDown(object sender, KeyEventArgs e)
        {
            nowPressed[e.Key] = true;
        }
        private void KeyUp(object sender, KeyEventArgs e)
        {
            nowPressed[e.Key] = false;
        }
        private void MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            nowMouseLPressd = true;
        }
        private void MouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            nowMouseLPressd = false;
        }
        private void MouseMove(object sender, MouseEventArgs e)
        {
            nowMousePosition = e.GetPosition(null);
        }
        private void MouseLeave(object sender, MouseEventArgs e)
        {
            nowMouseLPressd = false;
        }
        private void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            nowMouseWheel += e.Delta;
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            Clear();
            MyLog.WriteLine("FocusLost");
        }
    }
}