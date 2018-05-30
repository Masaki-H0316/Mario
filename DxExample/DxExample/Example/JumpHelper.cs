namespace DxExample.Example
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// ジャンプのヘルパクラス。
    /// </summary>
    internal class JumpHelper
    {
        /// <summary>
        /// 列挙子。
        /// </summary>
        private IEnumerator<int> enumerator = null;

        /// <summary>
        /// ジャンプ中かどうかを示す値を取得します。
        /// </summary>
        public bool IsInJumping { get { return this.enumerator != null; } }

        /// <summary>
        /// ジャンプの補正値を取得します。
        /// </summary>
        /// <returns>ジャンプの補正値。</returns>
        public int Next()
        {
            if (this.enumerator == null)
            {
                // ジャンプ中じゃない場合は int.MinValue を返す。
                return int.MinValue;
            }

            if (!this.enumerator.MoveNext())
            {
                // 次の値がない場合も int.MinValue を返す。
                this.enumerator.Dispose();
                this.enumerator = null;
                return int.MinValue;
            }

            return this.enumerator.Current;
        }

        /// <summary>
        /// ジャンプを開始します。
        /// </summary>
        public void StartJump()
        {
            this.enumerator = this.Jump();
        }

        /// <summary>
        /// ジャンプの補正値を計算する列挙子を返します。
        /// </summary>
        /// <returns>列挙子。</returns>
        private IEnumerator<int> Jump()
        {
            for (int i = 0; i < 180; i += 6)
            {
                var rad = (double)i / 180 * Math.PI;

                System.Diagnostics.Debug.WriteLine("[{0}]: rad: {1} ret: {2}", i, rad, Math.Sin(rad));
                yield return (int)(Math.Sin(rad) * 100 * -2);
            }
        }
    }
}
