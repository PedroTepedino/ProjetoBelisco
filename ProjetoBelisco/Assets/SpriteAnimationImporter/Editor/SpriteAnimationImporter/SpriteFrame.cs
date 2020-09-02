using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteAnimationImporter
{
    public class SpriteFrame
    {
        public Rect rect;
        public int duration = 100;
        public string name;
        public int id;

        public SpriteFrame(string name, int id, Rect rect)
        {
            this.name = name;
            this.rect = rect;
            this.id = id;
        }

        /// <summary>
        /// Returns a trimmed rectangle of this sprite frame.
        /// </summary>
        /// <param name="atlas">Spritesheet containing this sprite frame</param>
        /// <param name="trimWidth">Should we trim in width?</param>
        /// <param name="trimHeight">Should we trim in height?</param>
        /// <returns></returns>
        public Rect GetTrimmed(Texture2D atlas, bool trimWidth, bool trimHeight)
        {
            int left = (int)rect.x;
            int top = (int)rect.y;
            int width = (int)rect.width;
            int height = (int)rect.height;
            int right = left + width;
            int bottom = top - height;
            if (trimWidth)
            {
                // trim left
                bool stopTrimming = false;
                while (left < right && !stopTrimming)
                {
                    for (int i = 0; i < height; i++)
                    {
                        if (atlas.GetPixel(left, top - i).a > 0.0f)
                        {
                            stopTrimming = true;
                            break;
                        }
                    }
                    if (!stopTrimming)
                        left++;
                }
                // trim right
                stopTrimming = false;
                while (right > left && !stopTrimming)
                {
                    for (int i = 0; i < height; i++)
                    {
                        if (atlas.GetPixel(right - 1, top - i).a > 0.0f)
                        {
                            stopTrimming = true;
                            break;
                        }
                    }
                    if (!stopTrimming)
                        right--;
                }
                // refresh width
                width = right - left;
            }
            if (trimHeight)
            {
                // trim top
                bool stopTrimming = false;
                while (top > bottom && !stopTrimming)
                {
                    for (int i = 0; i < width; i++)
                    {
                        if (atlas.GetPixel(left + i, top).a > 0.0f)
                        {
                            stopTrimming = true;
                            break;
                        }
                    }
                    if (!stopTrimming)
                        top--;
                }
                // trim bottom
                stopTrimming = false;
                while (bottom < top && !stopTrimming)
                {
                    for (int i = 0; i < width; i++)
                    {
                        if (atlas.GetPixel(left + i, bottom + 1).a > 0.0f)
                        {
                            stopTrimming = true;
                            break;
                        }
                    }
                    if (!stopTrimming)
                        bottom++;
                }
            }

            return new Rect(
                left,
                top,
                right - left,
                top - bottom
            );
        }

        /// <summary>
        /// Computes a new pivot make sprite stay in place relatively to it's non-trimmed counterpart.
        /// </summary>
        /// <param name="trimmed">trimmed rectangle</param>
        /// <param name="defaultPivot">non-trimmed pivot</param>
        /// <param name="adjustX">should compute new pivot X?</param>
        /// <param name="adjustY">should compute new pivot Y?</param>
        /// <returns></returns>
        public Vector2 GetTrimmedPivot(Rect trimmed, Vector2 defaultPivot, bool adjustX, bool adjustY)
        {
            Vector2 adjustedPivot = defaultPivot;
            if(adjustX)
            {
                float originalX = rect.x + rect.width * defaultPivot.x;
                adjustedPivot.x = (originalX - trimmed.x) / trimmed.width;
            }
            if(adjustY)
            {
                float originalY = rect.y - rect.height + rect.height * defaultPivot.y;
                adjustedPivot.y = (originalY - trimmed.y + trimmed.height) / trimmed.height;
            }
            return adjustedPivot;
        }
    }
}
