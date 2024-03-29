﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DxLibDLL;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Amaoto
{
    /// <summary>
    /// テクスチャ。
    /// </summary>
    public class Texture : IDisposable, ITextureReturnable
    {
        /// <summary>
        /// テクスチャを生成します。
        /// </summary>
        public Texture()
        {
            Rotation = 0.0;
            ScaleX = 1.0;
            ScaleY = 1.0;
            Opacity = 1.0;
            ReferencePoint = ReferencePoint.TopLeft;
        }

        /// <summary>
        /// 画像ファイルからテクスチャを生成します。
        /// </summary>
        /// <param name="fileName">ファイル名。</param>
        public Texture(string fileName)
            : this()
        {
            ID = DX.LoadGraph(fileName);
            if (ID != -1)
            {
                IsEnable = true;
            }
            FileName = fileName;
        }

        /// <summary>
        /// DXLibのグラフィックハンドルから生成します。
        /// </summary>
        /// <param name="handle">DxLibのグラフィックハンドル。</param>
        public Texture(int handle)
            : this()
        {
            ID = handle;
            if (ID != -1)
            {
                IsEnable = true;
            }
            FileName = null;
        }

        /// <summary>
        /// ビットマップからテクスチャを生成します。
        /// </summary>
        /// <param name="bitmap">ビットマップ。</param>
        public Texture(Bitmap bitmap)
            : this()
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                var buf = ms.ToArray();

                unsafe
                {
                    fixed (byte* p = buf)
                    {
                        DX.SetDrawValidGraphCreateFlag(DX.TRUE);
                        DX.SetDrawValidAlphaChannelGraphCreateFlag(DX.TRUE);

                        ID = DX.CreateGraphFromMem((IntPtr)p, buf.Length);

                        DX.SetDrawValidGraphCreateFlag(DX.FALSE);
                        DX.SetDrawValidAlphaChannelGraphCreateFlag(DX.FALSE);
                    }
                }
            }
        }

        ~Texture()
        {
            if (IsEnable)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (DX.DeleteGraph(ID) != -1)
            {
                IsEnable = false;
            }
        }

        /// <summary>
        /// 描画します。
        /// </summary>
        /// <param name="x">X座標。</param>
        /// <param name="y">Y座標。</param>
        /// <param name="rectangle">描画範囲。</param>
        /// <param name="drawOrigin">描画基準点。指定するとReferencerPointを無視する。</param>
        /// <param name="reverseX">横方向に反転描画するか。</param>
        /// <param name="reverseY">縦方向に反転描画するか。</param>
        public void Draw(double x, double y, Rectangle? rectangle = null, Point? drawOrigin = null, bool reverseX = false, bool reverseY = false)
        {
            var origin = new Point();
            var isDefinedRect = rectangle.HasValue;
            DX.GetGraphSize(ID, out var width, out var height);
            if (rectangle == null)
            {
                // rectangleが引数として渡されなかった場合、
                // 元画像のすべてを表示するためにここで全領域のrectangleを生成する。
                rectangle = new Rectangle(0, 0, width, height);
            }
            switch (ReferencePoint)
            {
                case ReferencePoint.TopLeft:
                    origin.X = 0;
                    origin.Y = 0;
                    break;

                case ReferencePoint.TopCenter:
                    origin.X = rectangle.Value.Width / 2;
                    origin.Y = 0;
                    break;

                case ReferencePoint.TopRight:
                    origin.X = rectangle.Value.Width;
                    origin.Y = 0;
                    break;

                case ReferencePoint.CenterLeft:
                    origin.X = 0;
                    origin.Y = rectangle.Value.Height / 2;
                    break;

                case ReferencePoint.Center:
                    origin.X = rectangle.Value.Width / 2;
                    origin.Y = rectangle.Value.Height / 2;
                    break;

                case ReferencePoint.CenterRight:
                    origin.X = rectangle.Value.Width;
                    origin.Y = rectangle.Value.Height / 2;
                    break;

                case ReferencePoint.BottomLeft:
                    origin.X = 0;
                    origin.Y = rectangle.Value.Height;
                    break;

                case ReferencePoint.BottomCenter:
                    origin.X = rectangle.Value.Width / 2;
                    origin.Y = rectangle.Value.Height;
                    break;

                case ReferencePoint.BottomRight:
                    origin.X = rectangle.Value.Width;
                    origin.Y = rectangle.Value.Height;
                    break;

                default:
                    origin.X = 0;
                    origin.Y = 0;
                    break;
            }

            if (drawOrigin.HasValue)
            {
                origin = drawOrigin.Value;
            }

            var blendParam = (int)(Opacity * 255);
            DX.SetDrawBlendMode(DXLibUtil.GetBlendModeConstant(BlendMode), blendParam);

            if (ScaleX != 1.0 || ScaleY != 1.0)
            {
                DX.SetDrawMode(DX.DX_DRAWMODE_BILINEAR);
            }
            else
            {
                DX.SetDrawMode(DX.DX_DRAWMODE_NEAREST);
            }

            if (!isDefinedRect)
            {
                if (ScaleX == 1.0 && ScaleY == 1.0)
                {
                    DX.DrawRotaGraph2F(
                        // 座標
                        (float)x,
                        (float)y,
                        // 描画基準点
                        origin.X,
                        origin.Y,
                        1.0f,
                        // 回転角度
                        Rotation,
                        ID,
                        DX.TRUE,
                        reverseX ? DX.TRUE : DX.FALSE,
                        reverseY ? DX.TRUE : DX.FALSE);
                }
                else
                {
                    DX.DrawRotaGraph3F(
                        // 座標
                        (float)x,
                        (float)y,
                        // 描画基準点
                        origin.X,
                        origin.Y,
                        // 拡大率
                        ScaleX,
                        ScaleY,
                        // 回転角度
                        Rotation,
                        ID,
                        DX.TRUE,
                        reverseX ? DX.TRUE : DX.FALSE,
                        reverseY ? DX.TRUE : DX.FALSE);
                }
            }
            else
            {
                if (ScaleX == 1.0 && ScaleY == 1.0)
                {
                    DX.DrawRectRotaGraph2F(
                        // 座標
                        (float)x,
                        (float)y,
                        // 元画像座標
                        rectangle.Value.X,
                        rectangle.Value.Y,
                        // 元画像幅・高さ
                        rectangle.Value.Width,
                        rectangle.Value.Height,
                        // 描画基準点
                        origin.X,
                        origin.Y,
                        // 拡大率
                        1.0f,
                        // 回転角度
                        Rotation,
                        ID,
                        DX.TRUE,
                        reverseX ? DX.TRUE : DX.FALSE,
                        reverseY ? DX.TRUE : DX.FALSE);
                }
                else
                {
                    DX.DrawRectRotaGraph3F(
                        // 座標
                        (float)x,
                        (float)y,
                        // 元画像座標
                        rectangle.Value.X,
                        rectangle.Value.Y,
                        // 元画像幅・高さ
                        rectangle.Value.Width,
                        rectangle.Value.Height,
                        // 描画基準点
                        origin.X,
                        origin.Y,
                        // 拡大率
                        ScaleX,
                        ScaleY,
                        // 回転角度
                        Rotation,
                        ID,
                        DX.TRUE,
                        reverseX ? DX.TRUE : DX.FALSE,
                        reverseY ? DX.TRUE : DX.FALSE);
                }
            }

            DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
        }

        /// <summary>
        /// テクスチャをPNGファイルに出力します。
        /// </summary>
        /// <param name="path">保存先。</param>
        public void SaveAsPNG(string path)
        {
            DX.SaveDrawValidGraphToPNG(ID, 0, 0, TextureSize.Width, TextureSize.Height, path, 0);
        }

        /// <summary>
        /// テクスチャを取得する。
        /// </summary>
        /// <returns>テクスチャ。</returns>
        public Texture GetTexture()
        {
            return this;
        }

        /// <summary>
        /// 有効かどうか。
        /// </summary>
        public bool IsEnable { get; private set; }

        /// <summary>
        /// 合成モード。
        /// </summary>
        public BlendMode BlendMode { get; set; }

        /// <summary>
        /// ファイル名。
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 不透明度。
        /// </summary>
        public double Opacity { get; set; }

        /// <summary>
        /// ID。
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 角度(弧度法)。
        /// </summary>
        public double Rotation { get; set; }

        /// <summary>
        /// 描画基準点。
        /// </summary>
        public ReferencePoint ReferencePoint { get; set; }

        /// <summary>
        /// 拡大率X。
        /// </summary>
        public double ScaleX { get; set; }

        /// <summary>
        /// 拡大率Y。
        /// </summary>
        public double ScaleY { get; set; }

        /// <summary>
        /// テクスチャのサイズを返します。
        /// </summary>
        public Size TextureSize
        {
            get
            {
                DX.GetGraphSize(ID, out var width, out var height);
                return new Size(width, height);
            }
        }

        /// <summary>
        /// 拡大率を考慮した、描画されるときのサイズ。
        /// </summary>
        public Size ActualSize
        {
            get
            {
                var s = TextureSize;
                return new Size((int)(ScaleX * s.Width), (int)(ScaleY * s.Height));
            }
        }
    }

    /// <summary>
    /// 合成モード。
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BlendMode
    {
        /// <summary>
        /// なし
        /// </summary>
        None,

        /// <summary>
        /// 加算合成
        /// </summary>
        Add,

        /// <summary>
        /// 減算合成
        /// </summary>
        Subtract
    }

    /// <summary>
    /// 描画基準点。
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReferencePoint
    {
        /// <summary>
        /// 左上
        /// </summary>
        TopLeft,

        /// <summary>
        /// 中央上
        /// </summary>
        TopCenter,

        /// <summary>
        /// 右上
        /// </summary>
        TopRight,

        /// <summary>
        /// 左中央
        /// </summary>
        CenterLeft,

        /// <summary>
        /// 中央
        /// </summary>
        Center,

        /// <summary>
        /// 右中央
        /// </summary>
        CenterRight,

        /// <summary>
        /// 左下
        /// </summary>
        BottomLeft,

        /// <summary>
        /// 中央下
        /// </summary>
        BottomCenter,

        /// <summary>
        /// 右下
        /// </summary>
        BottomRight
    }
}