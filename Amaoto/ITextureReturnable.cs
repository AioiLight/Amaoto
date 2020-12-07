namespace Amaoto
{
    /// <summary>
    /// 何かのテクスチャを取得できる仕組みを提供します。
    /// </summary>
    interface ITextureReturnable
    {
        /// <summary>
        /// テクスチャを取得する。
        /// </summary>
        /// <returns>テクスチャ。</returns>
        Texture GetTexture();
    }
}
