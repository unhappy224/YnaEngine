using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Yna.Engine.Resources
{
    public class ResourceHelper
    {
        public static Assembly GetAssembly()
        {
#if WINDOWS_STOREAPP
            return typeof(ResourceHelper).GetTypeInfo().Assembly;
#else
            return Assembly.GetExecutingAssembly();
#endif
        }

        public static Effect LoadEffect(string name)
        {
            string suffix = "ogl";
#if DIRECTX
            suffix = "dx11";
#endif
            var stream = GetAssembly().GetManifestResourceStream(String.Format("Yna.Engine.Resources.Effects.{0}.{1}.mgfxo", name, suffix));

            byte[] shaderCode;

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                shaderCode = ms.ToArray();
            }

            return new Effect(YnG.GraphicsDevice, shaderCode);
        }

        public static Texture2D LoadTexture(string name)
        {
            var stream = GetAssembly().GetManifestResourceStream("Yna.Engine.Resources.Controller." + name);

            return Texture2D.FromStream(YnG.GraphicsDevice, stream);
        }
    }

}
