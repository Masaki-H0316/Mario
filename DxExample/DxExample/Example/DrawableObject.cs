using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;

namespace DxExample.Example
{
    /// <summary>
    /// 描画クラス
    /// </summary>
    internal class DrawableObject
    {
        /// <summary>
        /// 画像のハンドル
        /// </summary>
        private int imageHandle;

        /// <summary>
        /// X座標の値
        /// </summary>
        private int positionX;

        /// <summary>
        /// Y座標の値
        /// </summary>
        private int positionY;

        /// <summary>
        /// 画像の反転
        /// </summary>
        private int reverseRequired;

        /// <summary>
        /// 画像の倍率
        /// </summary>
        private double scale;

        /// <summary>
        /// 画像の幅
        /// </summary>
        private int width;

        /// <summary>
        /// 画像の高さ
        /// </summary>
        private int height;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName">string</param>
        public DrawableObject(string fileName)
        {
            this.imageHandle = 0;
            this.positionX = 0;
            this.positionY = 0;
            this.reverseRequired = 0;
            this.scale = 1;

            // 画像をメモリに読み込んでおく(描画の度に読み込むと遅いので)。
            this.imageHandle = DX.LoadGraph(fileName);

            // 画像のサイズを取得する。
            DX.GetGraphSize(this.imageHandle, out width, out height);
        }

        /// <summary>
        /// X座標プロパティ
        /// </summary>
        public int PositionX
        {
            get
            {
                return this.positionX;
            }

            set
            {
                // 左移動の上限を設ける
                if (value <= -10)
                {
                    this.positionX = 1280;
                }
                else if (value > 1280)
                {
                    this.positionX = -10;
                }
                else
                {
                    this.positionX = value;
                }
            }
        }

        /// <summary>
        /// Y座標プロパティ
        /// </summary>
        public int PositionY
        {
            get
            {
                return this.positionY;
            }

            set
            {
                this.positionY = value;
            }
        }

        /// <summary>
        /// 画像の反転プロパティ
        /// </summary>
        public int ReverseRequired
        {
            get
            {
                return this.reverseRequired;
            }

            set
            {
                this.reverseRequired = value;
            }
        }

        /// <summary>
        /// 画像の倍率プロパティ
        /// </summary>
        public double Scale
        {
            get
            {
                return this.scale;
            }

            set
            {
                if (value <= 0)
                {
                    this.scale = 0.1;
                }
                else
                {
                    this.scale = value;
                }
            }
        }

        /// <summary>
        /// 画像の幅を取得する。
        /// </summary>
        public int Width
        {
            get
            {
                return this.width;
            }
        }

        /// <summary>
        /// 画像の高さを取得する。
        /// </summary>
        public int Height
        {
            get
            {
                return this.height;
            }
        }

        /// <summary>
        /// 描画関数
        /// </summary>
        public void Draw()
        {
            // 画像がでかすぎるので、1/2(=0.5) くらいで表示する。
            DX.DrawRotaGraph(this.positionX, this.positionY, this.scale, 0, this.imageHandle, DX.TRUE, this.reverseRequired);
        }
    }
}
