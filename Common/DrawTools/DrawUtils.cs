using CalamityWeaponRemake.Common.AuxiliaryMeans;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Common.DrawTools
{
    /// <summary>
    /// 绘制工具
    /// </summary>
    public static class DrawUtils
    {
        #region 普通绘制工具
        /// <summary>
        /// 获取与纹理大小对应的矩形框
        /// </summary>
        /// <param name="value">纹理对象</param>
        public static Rectangle GetRec(Texture2D value)
        {
            return new Rectangle(0, 0, value.Width, value.Height);
        }
        /// <summary>
        /// 获取与纹理大小对应的矩形框
        /// </summary>
        /// <param name="value">纹理对象</param>
        /// <param name="Dx">X起点</param>
        /// <param name="Dy">Y起点</param>
        /// <param name="Sx">宽度</param>
        /// <param name="Sy">高度</param>
        /// <returns></returns>
        public static Rectangle GetRec(Texture2D value, int Dx, int Dy, int Sx, int Sy)
        {
            return new Rectangle(Dx, Dy, Sx, Sy);
        }
        /// <summary>
        /// 获取与纹理大小对应的矩形框
        /// </summary>
        /// <param name="value">纹理对象</param>
        /// <param name="frameCounter">帧索引</param>
        /// <param name="frameCounterMax">总帧数，该值默认为1</param>
        /// <returns></returns>
        public static Rectangle GetRec(Texture2D value, int frameCounter, int frameCounterMax = 1)
        {
            int singleFrameY = value.Height / frameCounterMax;
            return new Rectangle(0, singleFrameY * frameCounter, value.Width, singleFrameY);
        }
        /// <summary>
        /// 获取与纹理大小对应的缩放中心
        /// </summary>
        /// <param name="value">纹理对象</param>
        /// <returns></returns>
        public static Vector2 GetOrig(Texture2D value)
        {
            return new Vector2(value.Width, value.Height) * 0.5f;
        }
        /// <summary>
        /// 获取与纹理大小对应的缩放中心
        /// </summary>
        /// <param name="value">纹理对象</param>
        /// <param name="ScaleOrig">整体缩放体积偏移</param>
        /// <returns></returns>
        public static Vector2 GetOrig(Texture2D value, float ScaleOrig)
        {
            return new Vector2(value.Width, value.Height) * ScaleOrig;
        }
        /// <summary>
        /// 获取与纹理大小对应的缩放中心
        /// </summary>
        /// <param name="value">纹理对象</param>
        /// <param name="ScaleX">X方向收缩系数</param>
        /// <param name="ScaleY">Y方向收缩系数</param>
        /// <returns></returns>
        public static Vector2 GetOrig(Texture2D value, float ScaleX, float ScaleY)
        {
            return new Vector2(value.Width * ScaleX, value.Height * ScaleY);
        }
        /// <summary>
        /// 获取与纹理大小对应的缩放中心
        /// </summary>
        /// <param name="value">纹理对象</param>
        /// <param name="frameCounter">帧索引</param>
        /// <param name="frameCounterMax">总帧数，该值默认为1</param>
        /// <returns></returns>
        public static Vector2 GetOrig(Texture2D value, int frameCounter, int frameCounterMax = 1)
        {
            float singleFrameY = value.Height / frameCounterMax;
            return new Vector2(value.Width * 0.5f, singleFrameY / 2);
        }
        /// <summary>
        /// 对帧数索引进行走表
        /// </summary>
        /// <param name="frameCounter"></param>
        /// <param name="intervalFrame"></param>
        /// <param name="Maxframe"></param>
        public static void ClockFrame(ref int frameCounter, int intervalFrame, int maxFrame)
        {
            if (Main.fpsCount % intervalFrame == 0) frameCounter++;
            if (frameCounter > maxFrame) frameCounter = 0;
        }
        /// <summary>
        /// 对帧数索引进行走表
        /// </summary>
        /// <param name="frameCounter"></param>
        /// <param name="intervalFrame"></param>
        /// <param name="Maxframe"></param>
        /// <param name="startCounter"></param>
        public static void ClockFrame(ref double frameCounter, int intervalFrame, int maxFrame, int startCounter = 0)
        {
            if (Main.fpsCount % intervalFrame == 0) frameCounter++;
            if (frameCounter > maxFrame) frameCounter = startCounter;
        }
        /// <summary>
        /// 将世界位置矫正为适应屏幕的画布位置
        /// </summary>
        /// <param name="entity">传入目标实体</param>
        /// <returns></returns>
        public static Vector2 WDEpos(Entity entity)
        {
            return AiBehavior.GetEntityCenter(entity) - Main.screenPosition;
        }
        /// <summary>
        /// 将世界位置矫正为适应屏幕的画布位置
        /// </summary>
        /// <param name="pos">绘制目标的世界位置</param>
        /// <returns></returns>
        public static Vector2 WDEpos(Vector2 pos)
        {
            return pos - Main.screenPosition;
        }
        /// <summary>
        /// 获取纹理实例，类型为 Texture2D
        /// </summary>
        /// <param name="texture">纹理路径</param>
        /// <returns></returns>
        public static Texture2D GetT2DValue(string texture)
        {
            return ModContent.Request<Texture2D>(texture).Value;
        }

        /// <summary>
        /// 获取纹理实例，类型为 Asset<Texture2D>
        /// </summary>
        /// <param name="texture">纹理路径</param>
        /// <returns></returns>
        public static Asset<Texture2D> GetT2DAsset(string texture)
        {
            return ModContent.Request<Texture2D>(texture);
        }

        #endregion

        #region 高级绘制工具

        private static readonly FieldInfo shaderTextureField = typeof(MiscShaderData).GetField("_uImage1", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly FieldInfo shaderTextureField2 = typeof(MiscShaderData).GetField("_uImage2", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly FieldInfo shaderTextureField3 = typeof(MiscShaderData).GetField("_uImage3", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// 任意设置 <see cref=" SpriteBatch "/> 的 <see cref=" BlendState "/>。
        /// </summary>
        /// <param name="spriteBatch">绘制模式</param>
        /// <param name="blendState">要使用的混合状态</param>
        public static void SetBlendState(this SpriteBatch spriteBatch, BlendState blendState)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, blendState, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        /// <summary>
        /// 将 <see cref="SpriteBatch"/> 的 <see cref="BlendState"/> 重置为典型的 <see cref="BlendState.AlphaBlend"/>。
        /// </summary>
        /// <param name="spriteBatch">绘制模式</param>
        public static void ResetBlendState(this SpriteBatch spriteBatch) => spriteBatch.SetBlendState(BlendState.AlphaBlend);

        /// <summary>
        /// 将 <see cref="SpriteBatch"/> 重置为无效果的UI画布状态，在大多数情况下，这个适合结束一段在UI中的绘制
        /// </summary>
        /// <param name="spriteBatch">绘制模式</param>
        public static void ResetUICanvasState(this SpriteBatch spriteBatch)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
        }

        /// <summary>
        /// 使用反射来设置 _uImage1。它的底层数据是私有的，唯一可以公开更改它的方式是通过一个只接受原始纹理路径的方法。
        /// </summary>
        /// <param name="shader">着色器</param>
        /// <param name="texture">要使用的纹理</param>
        public static void SetShaderTexture(this MiscShaderData shader, Asset<Texture2D> texture) => shaderTextureField.SetValue(shader, texture);

        /// <summary>
        /// 使用反射来设置 _uImage2。它的底层数据是私有的，唯一可以公开更改它的方式是通过一个只接受原始纹理路径的方法。
        /// </summary>
        /// <param name="shader">着色器</param>
        /// <param name="texture">要使用的纹理</param>
        public static void SetShaderTexture2(this MiscShaderData shader, Asset<Texture2D> texture) => shaderTextureField2.SetValue(shader, texture);

        /// <summary>
        /// 使用反射来设置 _uImage3。它的底层数据是私有的，唯一可以公开更改它的方式是通过一个只接受原始纹理路径的方法。
        /// </summary>
        /// <param name="shader">着色器</param>
        /// <param name="texture">要使用的纹理</param>
        public static void SetShaderTexture3(this MiscShaderData shader, Asset<Texture2D> texture) => shaderTextureField3.SetValue(shader, texture);

        #endregion

        #region 具体内容绘制工具

        /// <summary>
        /// 绘制虫洞效果，给定纹理和参数
        /// </summary>
        /// <param name="texture">用于绘制的纹理</param>
        /// <param name="spriteBatch">用于绘制的精灵批处理对象</param>
        /// <param name="position">绘制位置</param>
        /// <param name="scaleMultiplier">缩放倍数</param>
        /// <param name="alphaMultiplier">透明度倍数</param>
        /// <param name="rotationMultiplier">旋转倍数</param>
        /// <param name="inWorld">指示位置是否在游戏世界中，若为 true 则使用屏幕位置偏移</param>
        public static void DrawWormhole(Texture2D texture, SpriteBatch spriteBatch, Vector2 position, float scaleMultiplier = 1.0f, float alphaMultiplier = 0.5f, float rotationMultiplier = 1.0f, bool inWorld = true)
        {
            float layers = 20;
            float maxScale = 25;
            float rotation = Main.GlobalTimeWrappedHourly * -rotationMultiplier;
            for (int i = 1; i < layers; i++)
            {
                float scale = maxScale - i * maxScale / layers;
                Color colour = new(scale / maxScale, scale / maxScale, scale / maxScale);
                rotation *= 0.8f;
                spriteBatch.Draw(texture, position - (inWorld ? Main.screenPosition : Vector2.Zero), null, Color.Red * (i / layers) * alphaMultiplier, rotation, new Vector2(32, 32), scale * scaleMultiplier, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// 绘制特殊虫子的各个部分
        /// </summary>
        /// <param name="spriteBatch">用于绘制的精灵批处理对象</param>
        /// <param name="npc">虫子NPC实例</param>
        /// <param name="head">头部纹理</param>
        /// <param name="body">身体纹理</param>
        /// <param name="tail">尾部纹理</param>
        /// <param name="drawColor">绘制颜色</param>
        /// <param name="segmentCount">段数</param>
        /// <param name="segmentPos">各段位置的数组</param>
        /// <param name="segmentRot">各段旋转角度的数组</param>
        /// <param name="specialSecond">特殊第二部分纹理（可选）</param>
        /// <param name="glowmaskHead">头部发光蒙版纹理（可选）</param>
        /// <param name="glowmaskBody">身体发光蒙版纹理（可选）</param>
        /// <param name="glowmaskTail">尾部发光蒙版纹理（可选）</param>
        /// <param name="glowmaskSpecialSecond">特殊第二部分发光蒙版纹理（可选）</param>
        public static void DrawSpecialWorm(SpriteBatch spriteBatch,
            NPC npc,
            Texture2D head,
            Texture2D body,
            Texture2D tail,
            Color drawColor,
            int segmentCount,
            Vector2[] segmentPos,
            float[] segmentRot,
            Texture2D specialSecond = null,
            Texture2D glowmaskHead = null,
            Texture2D glowmaskBody = null,
            Texture2D glowmaskTail = null,
            Texture2D glowmaskSpecialSecond = null)
        {
            Texture2D texture = head;
            Texture2D glowmask = glowmaskHead;

            // 绘制虫子头部
            spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(texture.Width / 2, texture.Height / 2), npc.scale, SpriteEffects.None, 0);

            // 绘制头部发光蒙版（如果有）
            if (glowmask != null)
            {
                spriteBatch.Draw(glowmask, npc.Center - Main.screenPosition, null, Color.White, npc.rotation, new Vector2(glowmask.Width / 2, glowmask.Height / 2), npc.scale, SpriteEffects.None, 0);
            }

            // 循环绘制虫子的各个部分
            for (int i = 0; i < segmentCount; i++)
            {
                if (i == segmentCount - 1)
                {
                    texture = tail;
                    glowmask = glowmaskTail;
                }
                else if (specialSecond != null && i == 0)
                {
                    texture = specialSecond;
                    glowmask = glowmaskSpecialSecond;
                }
                else
                {
                    texture = body;
                    glowmask = glowmaskBody;
                }

                // 绘制虫子的身体部分
                spriteBatch.Draw(texture, segmentPos[i] - Main.screenPosition, null, Lighting.GetColor((int)segmentPos[i].X / 16, (int)segmentPos[i].Y / 16), segmentRot[i] + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), npc.scale, SpriteEffects.None, 0);

                // 绘制身体部分的发光蒙版（如果可用）
                if (glowmask != null)
                {
                    spriteBatch.Draw(glowmask, segmentPos[i] - Main.screenPosition, null, Color.White, segmentRot[i] + 1.57f, new Vector2(glowmask.Width / 2, glowmask.Height / 2), npc.scale, SpriteEffects.None, 0);
                }
            }
        }


        #endregion
    }
}
