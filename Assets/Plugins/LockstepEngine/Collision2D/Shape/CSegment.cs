// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using Lockstep.Math;
using Lockstep.UnsafeCollision2D;

namespace Lockstep.Collision2D {
    public class CSegment : CBaseShape {
        public override int TypeId => (int) EShape2D.Segment;
        public LVector2 pos1;
        public LVector2 pos2;
    }
}