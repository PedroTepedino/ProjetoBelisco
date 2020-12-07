using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteAnimationImporter
{
    public class SpriteAnimation
    {
        public List<SpriteFrame> frames = new List<SpriteFrame>();
        public string name;
        public bool useDefaultSampleRate = true;
        public int sampleRate = 60;
        public bool loop = true;
        public bool generateClip = true;

        public SpriteAnimation(string name)
        {
            this.name = name;
        }
    }
}