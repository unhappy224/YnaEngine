using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yna.Engine.Storage;

namespace Yna.Engine
{
    [Serializable]
    public class YnBootstrap
    {
        public static string ConfigurationFolder = "";
        public static string ConfigurationFilename = "settings.yna";

        // Rendering
        public int Width { get; set; }
        public int Height { get; set; }
        public string Title { get; set; }
        public bool Fullscreen { get; set; }
        public int ReferenceWidth { get; set; }
        public int ReferenceHeight { get; set; }

        // Audio
        public float SoundVolume { get; set; }
        public float MusicVolume { get; set; }
        public bool SoundEnabled { get; set; }
        public bool MusicEnabled { get; set; }

        // Input
        public bool KeyboardEnabled { get; set; }
        public bool MouseEnabled { get; set; }
        public Vector2 MouseSensitivity { get; set; }
        public bool GamepadEnabled { get; set; }
        public Vector2 GamepadSensitivity { get; set; }
        public bool TouchEnabled { get; set; }
        public Vector2 TouchSensitivity { get; set; }

        public YnBootstrap()
        {
            Width = 800;
            Height = 600;
            ReferenceWidth = Width;
            ReferenceHeight = Height;
            Title = "YnGame";
            Fullscreen = false;

            SoundEnabled = true;
            SoundVolume = 1.0f;
            MusicEnabled = true;
            MusicVolume = 1.0f;

            KeyboardEnabled = true;
            MouseEnabled = true;
            MouseSensitivity = Vector2.One;
            GamepadEnabled = true;
            GamepadSensitivity = Vector2.One;
            TouchEnabled = true;
            TouchSensitivity = Vector2.One;
        }

        public bool Load()
        {
            var config = StorageManager.Instance.Load<YnBootstrap>(ConfigurationFolder, ConfigurationFilename);
            
            if (config != null)
            {
                Width = config.Width;
                Height = config.Height;
                ReferenceWidth = config.ReferenceWidth;
                ReferenceHeight = config.ReferenceHeight;
                Title = config.Title;
                Fullscreen = config.Fullscreen;

                SoundEnabled = config.SoundEnabled;
                SoundVolume = config.SoundVolume;
                MusicEnabled = config.MusicEnabled;
                MusicVolume = config.MusicVolume;

                KeyboardEnabled = config.KeyboardEnabled;
                MouseEnabled = config.MouseEnabled;
                MouseSensitivity = config.MouseSensitivity;
                GamepadEnabled = config.GamepadEnabled;
                GamepadSensitivity = config.GamepadSensitivity;
                TouchEnabled = config.TouchEnabled;
                TouchSensitivity = config.TouchSensitivity;
            }

            return config != null;
        }

        public void Apply()
        {
            YnScreen.Setup(Width, Height, ReferenceWidth, ReferenceHeight, true);
            YnScreen.IsFullScreen = Fullscreen;
            YnG.Game.Window.Title = Title;

            YnG.AudioManager.SoundEnabled = SoundEnabled;
            YnG.AudioManager.SoundVolume = SoundVolume;
            YnG.AudioManager.MusicEnabled = MusicEnabled;
            YnG.AudioManager.MusicVolume = MusicVolume;

            YnG.Mouse.Enabled = MouseEnabled;
            YnG.Mouse.Sensitivity = MouseSensitivity;
            YnG.Gamepad.Enabled = GamepadEnabled;
            YnG.Gamepad.Sensitivity = GamepadSensitivity;
            YnG.Touch.Enabled = TouchEnabled;
            YnG.Touch.Sensitivity = TouchSensitivity;
        }

        public void Save(bool needUpdate)
        {
            if (needUpdate)
            {
                Width = YnScreen.Width;
                Height = YnScreen.Height;
                ReferenceWidth = YnScreen.ReferenceWidth;
                ReferenceHeight = YnScreen.ReferenceHeight;
                Title = YnG.Game.Window.Title;
                Fullscreen = YnScreen.IsFullScreen;

                SoundEnabled = YnG.AudioManager.SoundEnabled;
                SoundVolume = YnG.AudioManager.SoundVolume;
                MusicEnabled = YnG.AudioManager.MusicEnabled;
                MusicVolume = YnG.AudioManager.MusicVolume;

                KeyboardEnabled = YnG.Keys.Enabled;
                MouseEnabled = YnG.Mouse.Enabled;
                MouseSensitivity = YnG.Mouse.Sensitivity;
                GamepadEnabled = YnG.Gamepad.Enabled;
                GamepadSensitivity = YnG.Gamepad.Sensitivity;
                TouchEnabled = YnG.Touch.Enabled;
                TouchSensitivity = YnG.Touch.Sensitivity;
            }

             StorageManager.Instance.Save<YnBootstrap>(ConfigurationFolder, ConfigurationFilename, this);
        }

        public void LoadCLIParameters(string[] args, bool needApply, bool needSave)
        {
            int size = args.Length;

            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                    ParseCLIParameter(args[i]);
            }

            if (needApply)
                Apply();

            if (needSave)
                Save(false);
        }

        private void ParseCLIParameter(string param)
        {
            string[] temp = param.Split(new char[] { '=' });
            string name = temp[0];
            string value = temp[1];

            switch (name)
            {
                // Rendering
                case "screen.width": Width = int.Parse(value); break;
                case "screen.height": Height = int.Parse(value); break;
                case "screen.fullscreen": Fullscreen = bool.Parse(value); break;

                // Audio
                case "sound.enabled": SoundEnabled = bool.Parse(value); break;
                case "sound.volume": SoundVolume = float.Parse(value); break;
                case "music.enabled": MusicEnabled = bool.Parse(value); break;
                case "music.volume": MusicVolume = float.Parse(value); break;

                // Input
                case "keyboard.enabled": KeyboardEnabled = bool.Parse(value); break;
                case "mouse.enabled": MouseEnabled = bool.Parse(value); break;
                case "mouse.sensitivity": MouseSensitivity = new Vector2(float.Parse(value)); break;
                case "gamepad.enabled": GamepadEnabled = bool.Parse(value); break;
                case "gamepad.sensitivity": GamepadSensitivity = new Vector2(float.Parse(value)); break;
                case "touch.enabled": TouchEnabled = bool.Parse(value); break;
                case "touch.sensitivity": TouchSensitivity = new Vector2(float.Parse(value)); break;
                default: break;
            }
        }
    }
}
