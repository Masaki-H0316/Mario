namespace DxExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// メインクラス
    /// </summary>
    internal static class Program
    {
        const byte OK = 0x0;
        const byte Doku = 0x1;
        const byte Nemuri = 0x2;
        const byte Yakedo = 0x4;
        const byte Mahi = 0x8;
        const byte Koori = 0x10;
        const byte Konran = 0x20;
        const byte DokuKesi = 0x7F;

        private static void OutputStatus(byte status)
        {
            System.Diagnostics.Debug.WriteLine("------------------------------------------");
            if ((status & Doku) == Doku)
                System.Diagnostics.Debug.WriteLine("毒");
            if ((status & Nemuri) == Nemuri)
                System.Diagnostics.Debug.WriteLine("眠り");
            if ((status & Yakedo) == Yakedo)
                System.Diagnostics.Debug.WriteLine("やけど");
            if ((status & Mahi) == Mahi)
                System.Diagnostics.Debug.WriteLine("マヒ");
            if ((status & Koori) == Koori)
                System.Diagnostics.Debug.WriteLine("氷");
            if ((status & Konran) == Konran)
                System.Diagnostics.Debug.WriteLine("混乱");
        }
        private static void Test()
        {
            byte status = 0;
            status = (byte)(status | Doku);
            OutputStatus(status);
            status = (byte)(status | Nemuri);
            OutputStatus(status);
            status = (byte)(status & ~Doku);
            OutputStatus(status);
        }
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //Test();
            //return;
            var breakout = new Example.Game(true);
            var ex = breakout.Run();
            if (ex != null)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
