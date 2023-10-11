using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Audio;

namespace CalamityWeaponRemake.Common.SoundEffects
{
    public static class SoundAction
    {
        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="pos">声音播放的位置</param>
        /// <param name="sound">要播放的声音样式（SoundStyle）</param>
        /// <param name="volume">声音的音量</param>
        /// <param name="pitch">声音的音调</param>
        /// <param name="pitchVariance">音调的变化范围</param>
        /// <param name="maxInstances">最大实例数，允许同时播放的声音实例数量</param>
        /// <param name="soundLimitBehavior">声音限制行为，用于控制当达到最大实例数时的行为</param>
        /// <returns>返回声音实例的索引</returns>
        public static SlotId SoundPlayer(
            Vector2 pos,
            SoundStyle sound,
            float volume = 1,
            float pitch = 1,
            float pitchVariance = 1,
            int maxInstances = 1,
            SoundLimitBehavior soundLimitBehavior = SoundLimitBehavior.ReplaceOldest
            )
        {
            sound = sound with
            {
                Volume = volume,
                Pitch = pitch,
                PitchVariance = pitchVariance,
                MaxInstances = maxInstances,
                SoundLimitBehavior = soundLimitBehavior
            };

            SlotId sid = SoundEngine.PlaySound(sound, pos);
            return sid;
        }

        /// <summary>
        /// 更新声音位置
        /// </summary>
        public static void PanningSound(Vector2 pos, SlotId sid)
        {
            if (!SoundEngine.TryGetActiveSound(sid, out var activeSound)) return;
            else activeSound.Position = pos;
        }
    }
}
