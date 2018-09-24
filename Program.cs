
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace colorswitchdesktop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          static_class.font_arcena=  static_class.AddFont("ARCENA.ttf");
            Application.Run(new Form1());
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
    public class static_class
    {
        public static byte[] GetFontResources(string name)
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            string NAME = "colorswitchdesktop.Fonts." + name;
            using (Stream stream = ass.GetManifestResourceStream(NAME))
            {
                using (BinaryReader bnr = new BinaryReader(stream))
                {
                    return bnr.ReadBytes((int)stream.Length);
                }
            }
        }


        //\\\\\\\\\\\\\\\\\\
        ///////
        //////=============
        /////
        ////   [] N T 


        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

        static public FontFamily AddFont(string name)
        {
            byte[] fontfile = GetFontResources(name);
            int dataLength = fontfile.Length;
            IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);
            Marshal.Copy(fontfile, 0, ptrData, dataLength);
            uint cFonts = 0;
            AddFontMemResourceEx(ptrData, (uint)fontfile.Length, IntPtr.Zero, ref cFonts);
            PrivateFontCollection pfc = new PrivateFontCollection();

                pfc.AddMemoryFont(ptrData, dataLength);
            Marshal.FreeCoTaskMem(ptrData);
            return pfc.Families[0];

        }
      public  static FontFamily font_arcena;
    }
}
