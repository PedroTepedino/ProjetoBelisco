using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteAnimationImporter
{
    public class AsepriteImporter : AnimationImporter
    {
        [System.Serializable]
        protected class AsepriteMetaData
        {
            [System.Serializable]
            public class FrameRect
            {
                public int x, y, w, h;
            }

            [System.Serializable]
            public class Frame
            {
                public FrameRect frame;
                public int duration;
            }

            [System.Serializable]
            public class FrameTag
            {
                public string name;
                public int from, to;
                public string direction;
            }

            [System.Serializable]
            public class Size
            {
                public int w, h;
            }

            [System.Serializable]
            public class Meta
            {
                public FrameTag[] frameTags;
                public Size size;
            }

            public Frame[] frames;
            public Meta meta;
        }

        private AsepriteMetaData m_metaData = null;
        private const string k_orphanAnimName = "Others";

        /// <summary>
        /// Translates animation meta data file to our representation of list of animations and frames
        /// </summary>
        public override void Load(TextAsset metaData, Texture2D atlas)
        {
            if (metaData == null)
            {
                m_metaData = null;
                m_animations = new SpriteAnimation[0];
                m_frames = new SpriteFrame[0];
                return;
            }

            try
            {
                m_metaData = JsonUtility.FromJson<AsepriteMetaData>(metaData.text);
            } catch(System.ArgumentException)
            {
                Debug.LogWarning(metaData.name + " is not a valid aseprite animation meta data file");
                return;
            }

            var animations = new List<SpriteAnimation>();
            m_frames = new SpriteFrame[m_metaData.frames.Length];

            // fill m_frames
            for (int i = 0; i < m_frames.Length; i++)
            {
                var frame = m_metaData.frames[i].frame;
                var spriteFrame = new SpriteFrame(i.ToString(), i, new Rect(frame.x, atlas.height - frame.y - 1, frame.w, frame.h));
                spriteFrame.duration = m_metaData.frames[i].duration;
                m_frames[i] = spriteFrame;
            }

            // fill m_animations
            for (int i = 0; i < m_metaData.meta.frameTags.Length; i++)
            {
                var metaAnim = m_metaData.meta.frameTags[i];
                var anim = new SpriteAnimation(metaAnim.name);
                for (int j = 0; j < m_frames.Length; j++)
                    if (j >= metaAnim.from && j <= metaAnim.to)
                        anim.frames.Add(m_frames[j]);
                animations.Add(anim);
            }

            // create animation for frames that don't belong to any clip
            var orphanFrames = new List<SpriteFrame>();
            foreach(var frame in m_frames)
            {
                bool isOrphan = true;
                foreach(var anim in animations)
                {
                    if(anim.frames.Contains(frame))
                    {
                        isOrphan = false;
                        break;
                    }
                }
                if(isOrphan)
                    orphanFrames.Add(frame);
            }
            if(orphanFrames.Count > 0)
            {
                var others = new SpriteAnimation(k_orphanAnimName);
                others.frames = orphanFrames;
                others.generateClip = false;
                animations.Add(others);
            }

            m_animations = animations.ToArray();
        }
    }
}