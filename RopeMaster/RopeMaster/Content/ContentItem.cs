﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Glitch.Engine.Content
{
    /// <summary>
    /// Basic content item
    /// </summary>
    public abstract class ContentItem
    {
        public string AssetName { get; set; }
        public string AssetPath { get; set; }
        public bool LoadOnStartup { get; set; }
        public bool IsLoaded { get; set; }

        public override string ToString()
        {
            // For Debug
            return AssetName + ": " + AssetPath;
        }
    }

    public sealed class Texture2DContentItem : ContentItem
    {
        public Texture2D Texture { get; set; }
        public bool IsEmptyTexture { get; set; }
    }

    public sealed class Model3DContentItem : ContentItem
    {
        public Model Model3D { get; set; }
    }

    public sealed class SoundEffectContentItem : ContentItem
    {
        public SoundEffect SoundEffect { get; set; }
    }

    public sealed class FontContentItem : ContentItem
    {
        public SpriteFont Font { get; set; }
        public bool IsDefaultFont { get; set; }
    }

    public sealed class SongContentItem : ContentItem
    {
        public Song Song { get; set; }
        public string Artist { get; set; }
        public string Name { get; set; }
    }
}
