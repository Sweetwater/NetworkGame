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

        private FrameworkElement targetElement = null;

        public void Initialize(FrameworkElement target)
        {
            CreateKeys();
            Clear();
            targetElement = target;
            targetElement.KeyDown += new KeyEventHandler(KeyDown);
            targetElement.KeyUp += new KeyEventHandler(KeyUp);
            targetElement.LostFocus += new RoutedEventHandler(LostFocus);
        }

        public void Destroy()
        {
            Clear();
            targetElement.KeyDown -= new KeyEventHandler(KeyDown);
            targetElement.KeyUp -= new KeyEventHandler(KeyUp);
            targetElement.LostFocus -= new RoutedEventHandler(LostFocus);
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


        private void KeyDown(object sender, KeyEventArgs e)
        {
            nowPressed[e.Key] = true;
        }
        private void KeyUp(object sender, KeyEventArgs e)
        {
            nowPressed[e.Key] = false;
        }
        private void LostFocus(object sender, RoutedEventArgs e)
        {
            Clear();
        }
    }
}