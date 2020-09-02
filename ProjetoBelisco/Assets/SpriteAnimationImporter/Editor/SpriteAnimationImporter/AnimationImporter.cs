using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteAnimationImporter
{
    public abstract class AnimationImporter
    {
        public enum ImporterType
        {
            Aseprite
        }

        protected SpriteAnimation[] m_animations = new SpriteAnimation[0];
        protected SpriteFrame[] m_frames = new SpriteFrame[0];

        public SpriteAnimation[] Animations { get { return m_animations; } }
        public SpriteFrame[] Frames { get { return m_frames; } }

        public abstract void Load(TextAsset metaData, Texture2D atlas);

        public void SetFrameNames(string prefix, bool useTagNames)
        {
            foreach (var animation in m_animations)
            {
                for(int i = 0; i < animation.frames.Count; i++)
                {
                    string name = "";
                    if (prefix != null)
                        name = prefix;
                    if (useTagNames)
                        name += char.ToUpper(animation.name[0]) + animation.name.Substring(1);
                    name += "_" + i;
                    animation.frames[i].name = name;
                }
            }
        }

        public static AnimationImporter ImporterFactory(ImporterType importerType)
        {
            switch(importerType)
            {
                case ImporterType.Aseprite:
                    return new AsepriteImporter();
                default:
                    break;
            }
            throw new System.NotSupportedException("Importer " + importerType.ToString() + " is not recognized");
        }
    }
}