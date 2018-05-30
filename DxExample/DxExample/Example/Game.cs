namespace DxExample.Example
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using DxLibDLL;

    /// <summary>
    /// マリオを表示するだけのプログラム。
    /// </summary>
    internal class Game
    {
        /// <summary>
        /// ウィンドウのサイズ。
        /// </summary>
        private readonly DX.RECT windowRect;

        /// <summary>
        /// FPS マネージャ。
        /// </summary>
        private readonly FPSManager fpsManager;

        /// <summary>
        /// マリオの画像のハンドル。
        /// </summary>
        private readonly int marioImageHandle;

        /// <summary>
        /// マリオの画像の幅。
        /// </summary>
        private readonly int marioImageWidth;

        /// <summary>
        /// マリオの画像の高さ。
        /// </summary>
        private readonly int marioImageHeight;

        /// <summary>
        /// キノコの画像のハンドル。
        /// </summary>
        private readonly int kinoko_ImageHandle;

        /// <summary>
        /// キノコの画像の幅。
        /// </summary>
        private readonly int kinoko_ImageWidth;

        /// <summary>
        /// キノコの画像の高さ。
        /// </summary>
        private readonly int kinoko_ImageHeight;

        /// <summary>
        /// 立ちマリオ画像の描画
        /// </summary>
        private DrawableObject marioStand;

        /// <summary>
        /// 歩きマリオ1画像の描画
        /// </summary>
        private DrawableObject marioWalk1;

        /// <summary>
        /// 歩きマリオ2画像の描画
        /// </summary>
        private DrawableObject marioWalk2;

        /// <summary>
        /// ジャンプマリオ画像の描画
        /// </summary>
        private DrawableObject marioJump;

        /// <summary>
        /// 歩き画像切り替えカウンター
        /// </summary>
        private int walkCounter = 0;

        /// <summary>
        /// マリオの描画情報
        /// </summary>
        private Mario mario;

        /// <summary>
        /// キノコの描画情報
        /// </summary>
        private Kinoko kinoko;

        /// <summary>
        /// 乱数生成用
        /// </summary>
        private static Random rnd = new Random();

        /// <summary>
        /// ランダム用カウンタ
        /// </summary>
        private int randCounter = 0;

        /// <summary>
        /// ランダムの動き決定値
        /// </summary>
        private int randValue = 0;

        /// <summary>
        /// ウィンドウモードで表示するかどうかを指定して、DxExample.Example.Mario クラスの新しいインスタンスを作成します。
        /// </summary>
        /// <param name="isWindowMode">ウィンドウモードで表示する場合は true。フルスクリーンで表示する場合は false。</param>
        public Game(bool isWindowMode)
        {
            // とりあえず 60 FPS で動かす。
            this.fpsManager = new FPSManager(120);

            // ウィンドウモードかフルスクリーンで起動するかを設定。
            DX.ChangeWindowMode(isWindowMode ? DX.TRUE : DX.FALSE);

            // 画面サイズをとりあえず 1280x800 にする。
            DX.SetWindowSize(1280, 800);

            DX.SetGraphMode(1280, 800, 32);

            // DX ライブラリの初期化。
            if (DX.DxLib_Init() < 0)
            {
                throw new Exception("DX ライブラリの初期化に失敗しました。");
            }

            // ウィンドウサイズを取得する。
            int windowWidth;
            int windowHeight;
            DX.GetWindowSize(out windowWidth, out windowHeight);
            this.windowRect = new DX.RECT { top = 0, left = 0, right = windowWidth, bottom = windowHeight };

            // 描画先を裏側にする。
            // 裏側で描き終えたら、表面と反転して画面に表示する。
            // (表側に書いてしまうと書いている途中に表示されてしまい、中途半端な状態で描画されてしまうことがある。)
            // 「ダブルバッファリング」で検索。
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);
        }

        /// <summary>
        /// FPS を描画する必要があるかどうかを示す値を取得または設定します。
        /// </summary>
        public bool IsFPSWriteRequired { get; set; }

        /// <summary>
        /// 実行します。
        /// </summary>
        /// <returns>例外が発生した場合は発生した例外を返します。正常に終了した場合は null。</returns>
        public Exception Run()
        {
            try
            {
                this.IsFPSWriteRequired = true;
                this.Loop();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
            finally
            {
                DX.DxLib_End();
            }
        }

        /// <summary>
        /// メインループ
        /// </summary>
        private void Loop()
        {
            mario = new Mario("mario.png", "mariowalk1.png", "mariowalk2.png", "marioJump.png");
            kinoko = new Kinoko("kinoko.png");
            uint fpsColor = DX.GetColor(255, 255, 0);

            while (DX.ProcessMessage() != -1)
            {
                //Escapeキー取得
                int escape = DX.CheckHitKey(DX.KEY_INPUT_ESCAPE);

                // Escapeが押されているか
                if (escape == DX.TRUE)
                {
                    break;
                }

                // 画面クリア。
                DX.ClearDrawScreen();

                 //FPS 更新。
                this.fpsManager.Update();

                // FPS を描画する必要があるかどうか。
                if (this.IsFPSWriteRequired)
                {
                    // FPSの数値を描画
                    DX.DrawString(0, 0, ((int)this.fpsManager.CurrentFPS).ToString(), fpsColor);
                }

                //キノコの描画情報更新
                //this.kinoko.Update();

                // マリオのランダムな動きを決定
                if (randCounter == 0)
                {
                    randValue = rnd.Next(0, 50) % 5;
                }
                
                //マリオの描画情報更新(true:auto, false:手動)
                this.mario.Update(false, randValue);

                // カウントアップ
                randCounter++;

                // 一定期間は同じ動きをする
                if (this.randCounter >= 50)
                {
                    this.randCounter = 0;
                }
                
                //キノコの描画
                this.kinoko.Draw();

                // マリオの描画(スター(？))
                //if (this.randCounter % 10 != 0 || this.randCounter % 5 != 0)
                //{
                //    this.mario.Draw();
                //}

                // マリオの描画(ノーマル)
                this.mario.Draw();

                // 画面の裏側と表側を反転する。(ゲームは次の描画を裏にして、それと入れ替える形で描画しているため)
                DX.ScreenFlip();

                //　fpsManager 作成時の FPS にあうように Sleep する。
                this.fpsManager.Wait();
            }
        }

        /// <summary>
        /// 大きいマリオのサイズを設定
        /// </summary>
        private void BigMario()
        {
            this.ChangeMarioSize(0.5);
        }

        /// <summary>
        /// 小さいマリオのサイズを設定
        /// </summary>
        private void SmallMario()
        {
            this.ChangeMarioSize(0.2);
        }

        /// <summary>
        /// マリオのサイズ変更
        /// </summary>
        /// <param name="scale">double</param>
        private void ChangeMarioSize(double scale)
        {
            this.marioStand.Scale = scale;
            this.marioStand.PositionY = Convert.ToInt32(600 - (this.marioImageHeight * this.marioStand.Scale / 2));

            this.marioWalk1.Scale = scale;
            this.marioWalk1.PositionY = Convert.ToInt32(600 - (this.marioImageHeight * this.marioWalk1.Scale / 2));

            this.marioWalk2.Scale = scale;
            this.marioWalk2.PositionY = Convert.ToInt32(600 - (this.marioImageHeight * this.marioWalk2.Scale / 2));

            this.marioJump.Scale = scale;
            this.marioJump.PositionY = Convert.ToInt32(600 - (this.marioImageHeight * this.marioJump.Scale / 2));
        }

        /// <summary>
        /// ジャンプ時の高さ設定
        /// </summary>
        /// <param name="height">int</param>
        private void JumpMario(int height)
        {
            this.marioJump.PositionY += height;
        }
    }
}
