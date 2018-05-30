namespace DxExample
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// 1 秒あたりのフレーム数を管理する機能を提供します。
    /// </summary>
    internal class FPSManager
    {
        /// <summary>
        /// 1 秒あたりのフレーム数。
        /// </summary>
        public readonly int FPS;

        /// <summary>
        /// タイマ変わり。
        /// </summary>
        private readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// フレーム数用カウンタ。
        /// </summary>
        private int frames;

        /// <summary>
        /// FPS を指定して、DxExample.FPSManager クラスの新しいインスタンスを作成します。
        /// </summary>
        /// <param name="fps">1 秒あたりのフレーム数。</param>
        public FPSManager(int fps)
        {
            if (fps <= 0)
            {
                throw new ArgumentException("fps に 0 以上は設定できません。");
            }

            // 高解像力パフォーマンスカウンタがサポートされていない場合はエラー。
            if (!Stopwatch.IsHighResolution)
            {
                throw new Exception("まともな PC 使って");
            }

            this.FPS = fps;
            this.frames = 0;
            this.CurrentFPS = 0.0;
        }

        /// <summary>
        /// 現在の FPS を取得します。
        /// </summary>
        public double CurrentFPS { get; private set; }

        /// <summary>
        /// FPS 情報を更新します。1 フレーム毎に呼び出す必用があります。
        /// </summary>
        public void Update()
        {
            if (this.frames == 0)
            {
                this.stopwatch.Restart();
            }
            else if (this.frames >= this.FPS)
            {
                this.CurrentFPS = 1000.0 / (this.stopwatch.ElapsedMilliseconds / (double)this.FPS);
                this.frames = 0;
                this.stopwatch.Restart();
            }

            ++this.frames;
        }

        /// <summary>
        /// コンストラクタで指定された FPS を超えないように待機します。
        /// </summary>
        public void Wait()
        {
            var waitMilliseSeconds = (this.frames * 1000 / this.FPS) - this.stopwatch.ElapsedMilliseconds;
            if (waitMilliseSeconds > 0)
            {
                Thread.Sleep((int)waitMilliseSeconds);
            }
        }
    }
}
