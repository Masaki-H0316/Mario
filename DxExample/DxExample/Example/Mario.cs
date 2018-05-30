using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;

namespace DxExample.Example
{
    /// <summary>
    /// マリオの描画情報クラス
    /// </summary>
    internal class Mario
    {
        /// <summary>
        /// 立ちマリオ
        /// </summary>
        private DrawableObject mario;

        /// <summary>
        /// 歩きマリオ1
        /// </summary>
        private DrawableObject marioWalk1;

        /// <summary>
        /// 歩きマリオ2
        /// </summary>
        private DrawableObject marioWalk2;

        /// <summary>
        /// ジャンプマリオ
        /// </summary>
        private DrawableObject marioJump;

        /// <summary>
        /// 歩き画像カウンター
        /// </summary>
        private int walkCounter = 0;

        /// <summary>
        /// 描画情報の仲介
        /// </summary>
        private DrawableObject drawMario;

        /// <summary>
        /// 歩き状態フラグ
        /// </summary>
        private bool isWalking = false;

        /// <summary>
        /// サイズ変更フラグ
        /// </summary>
        bool isBig = false;

        /// <summary>
        /// ジャンプ補助
        /// </summary>
        JumpHelper jump = new JumpHelper();
        
        /// <summary>
        /// マリオ
        /// </summary>
        /// <param name="imageName1">立ちマリオ画像</param>
        /// <param name="imageName2">歩きマリオ画像1</param>
        /// <param name="imageName3">歩きマリオ画像2</param>
        /// <param name="imageName4">ジャンプマリオ画像</param>
        public Mario(string imageName1, string imageName2, string imageName3, string imageName4)
        {
            // マリオの画像をメモリに読み込んでおく(描画の度に読み込むと遅いので)。
            this.mario = new DrawableObject(imageName1);
            this.marioWalk1 = new DrawableObject(imageName2);
            this.marioWalk2 = new DrawableObject(imageName3);
            this.marioJump = new DrawableObject(imageName4);

            this.mario.Scale = 0.2;
            this.mario.PositionX = 100;
            this.mario.PositionY = Convert.ToInt32(600 - (this.mario.Height * this.mario.Scale / 2));

            this.marioWalk1.Scale = 0.2;
            this.marioWalk1.PositionX = 100;
            this.marioWalk1.PositionY = Convert.ToInt32(600 - (this.marioWalk1.Height * this.marioWalk1.Scale / 2));

            this.marioWalk2.Scale = 0.2;
            this.marioWalk2.PositionX = 100;
            this.marioWalk2.PositionY = Convert.ToInt32(600 - (this.marioWalk2.Height * this.marioWalk2.Scale / 2));

            this.marioJump.Scale = 0.2;
            this.marioJump.PositionX = 100;
            this.marioJump.PositionY = Convert.ToInt32(600 - (this.marioJump.Height * this.marioWalk1.Scale / 2));
        }

        /// <summary>
        /// 描画情報の更新
        /// </summary>
        public void Update(bool autoFlag, int randValue)
        {
            // キーの取得
            int left = DX.CheckHitKey(DX.KEY_INPUT_A);
            int right = DX.CheckHitKey(DX.KEY_INPUT_D);            
            int big = DX.CheckHitKey(DX.KEY_INPUT_B);
            int small = DX.CheckHitKey(DX.KEY_INPUT_V);
            int space = DX.CheckHitKey(DX.KEY_INPUT_SPACE);

            // オートで移動するマリオ
            if (autoFlag == true)
            {
                switch(randValue)
                {
                    // 左移動
                    case 0:
                        left = 1;
                        right = 0;
                        break;
                    // 右移動
                    case 1:
                        left = 0;
                        right = 1;
                        break;
                    // ジャンプ
                    case 2:
                        space = DX.TRUE;
                        break;
                    // 大きくなる
                    case 3:
                        big = DX.TRUE;
                        break;
                    // 小さくなる
                    case 4:
                        small = DX.TRUE;
                        break;
                }                
            }

            // ジャンプが押されているか
            if (space == DX.TRUE)
            {
                // ジャンプ中ではないか
                if (!jump.IsInJumping)
                {
                    //ジャンプ
                    jump.StartJump();
                }
            }
            
            // 左が押されているか
            if (left > 0 && right == 0)
            {
                this.mario.PositionX -= 10;

                // 画像は右向きなので左側へ移動したら反転させる。
                this.mario.ReverseRequired = DX.TRUE;

                this.marioWalk1.PositionX -= 10;

                // 画像は右向きなので左側へ移動したら反転させる。
                this.marioWalk1.ReverseRequired = DX.TRUE;

                this.marioWalk2.PositionX -= 10;

                // 画像は右向きなので左側へ移動したら反転させる。
                this.marioWalk2.ReverseRequired = DX.TRUE;

                this.marioJump.PositionX -= 10;

                // 画像は右向きなので左側へ移動したら反転させる。
                this.marioJump.ReverseRequired = DX.TRUE;

                isWalking = true;
            }

            // 右が押されているか
            else if (right > 0 && left == 0)
            {
                this.mario.PositionX += 10;

                // 左に向いている可能性があるので反転しないように設定する。
                this.mario.ReverseRequired = DX.FALSE;

                this.marioWalk1.PositionX += 10;

                // 左に向いている可能性があるので反転しないように設定する。
                this.marioWalk1.ReverseRequired = DX.FALSE;

                this.marioWalk2.PositionX += 10;

                // 左に向いている可能性があるので反転しないように設定する。
                this.marioWalk2.ReverseRequired = DX.FALSE;

                this.marioJump.PositionX += 10;

                // 左に向いている可能性があるので反転しないように設定する。
                this.marioJump.ReverseRequired = DX.FALSE;

                isWalking = true;
            }
            else
            {
                // 歩き状態フラグをfalseにする
                isWalking = false;
            }

            // Bが押下されたかどうか
            if (big == DX.TRUE)
            {
                isBig = true;
            }

            // Vが押下されたかどうか
            if (small == DX.TRUE)
            {
                isBig = false;
            }

            // 大きくするフラグがtrueかどうか
            if (isBig == true)
            {
                this.BigMario();
            }
            else
            {
                this.SmallMario();
            }

            // ジャンプ状態かどうか
            if (jump.IsInJumping)
            {
                var v = jump.Next();

                // ジャンプが終わったら int.MinValue を返してくるので、ジャンプ中のみ補正値を追加する。
                if (v != int.MinValue)
                {
                    // ジャンプ中の補正値を追加する
                    this.JumpMario(v);
                }

                // ジャンプのマリオを描画する
                this.drawMario = this.marioJump;
            }
            else
            {
                // 歩き状態かどうか
                if (isWalking == true)
                {
                    // カウンターが0<=10かどうか
                    if (this.walkCounter >= 0 && this.walkCounter <= 3)
                    {
                        // 歩きマリオ1を描画する
                        this.drawMario = this.marioWalk1;
                    }

                    // カウンターが11<=20かどうか
                    else if (this.walkCounter >= 4 && this.walkCounter <= 6)
                    {
                        // 歩きマリオ2を描画する
                        this.drawMario = this.marioWalk2;
                    }
                    else
                    {
                        // 歩きマリオ2を描画する。カウンターを0にする                        
                        this.drawMario = this.marioWalk2;
                        this.walkCounter = 0;
                    }
                }
                else
                {
                    // 立ちマリオを描画する
                    this.drawMario = this.mario;
                }
            }

            // カウンターを＋1する
            ++this.walkCounter;
        }

        /// <summary>
        /// 描画関数
        /// </summary>
        public void Draw()
        {
            this.drawMario.Draw();
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
            this.mario.Scale = scale;
            this.mario.PositionY = Convert.ToInt32(600 - (this.mario.Height * this.mario.Scale / 2));

            this.marioWalk1.Scale = scale;
            this.marioWalk1.PositionY = Convert.ToInt32(600 - (this.marioWalk1.Height * this.marioWalk1.Scale / 2));

            this.marioWalk2.Scale = scale;
            this.marioWalk2.PositionY = Convert.ToInt32(600 - (this.marioWalk2.Height * this.marioWalk2.Scale / 2));

            this.marioJump.Scale = scale;
            this.marioJump.PositionY = Convert.ToInt32(600 - (this.marioJump.Height * this.marioJump.Scale / 2));
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
