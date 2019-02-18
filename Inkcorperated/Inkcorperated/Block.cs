using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Inkcorperated
{
    enum BlockType
    {
        Basic, Speed, Bouncy
    };

    class Block : Drawable
    {
        BlockType blockType;

        public BlockType Type
        {
            get
            {
                return blockType;
            }
        }

        public Block(Rectangle bounds, Texture2D texture, BlockType bType) : base(bounds, texture)
        {
            blockType = bType;
        }
    }
}
