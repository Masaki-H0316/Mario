using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DxExample.Example
{
    internal class Kinoko
    {
        /// <summary>
        /// キノコ画像
        /// </summary>
        private DrawableObject kinoko;

        /// <summary>
        /// キノコの初期位置
        /// </summary>
        private int kinokoMovePosX = 500;

        /// <summary>
        /// キノコが進んでいるかどうか
        /// </summary>
        bool isKinokoMove = true;

        public Kinoko(string imageKinoko)
        {
            // キノコの画像をメモリに読み込んでおく(描画の度に読み込むと遅いので)。
            this.kinoko = new DrawableObject(imageKinoko);

            this.kinoko.Scale = 0.2;
            this.kinoko.PositionX = 500;
            this.kinoko.PositionY = Convert.ToInt32(600 - (this.kinoko.Height * this.kinoko.Scale / 2));
        }

        /// <summary>
        /// 描画情報の更新
        /// </summary>
        public void Update()
        {
            // キノコ進むフラグがtrueかどうか
            if (isKinokoMove == true)
            {
                // X座標をインクリメント
                kinokoMovePosX += 5;
            }
            else
            {
                // X座標をデクリメント
                kinokoMovePosX += -5;
            }

            // X座標が0以下かどうか
            if (kinokoMovePosX <= 0)
            {
                // キノコ進むフラグをtrueにする
                isKinokoMove = true;
            }

            // X座標が1280以上かどうか
            else if (kinokoMovePosX >= 1280)
            {
                // キノコ進むフラグをfalseにする
                isKinokoMove = false;
            }

            // X座標をキノコに設定する
            this.kinoko.PositionX = kinokoMovePosX;
        }

        /// <summary>
        /// 描画関数
        /// </summary>
        public void Draw()
        {
            this.kinoko.Draw();
        }
    }
}
