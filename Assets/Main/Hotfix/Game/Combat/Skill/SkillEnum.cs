using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hotfix.Skill
{
    public enum SkillCast
    {
        ActiveSkill = 0,
        Passive = 1,
    }

    public enum SkillTargetSelect
    {
        Manual = 0,
        Collision = 1,
        Condition = 2,
        Custom = 3,
    }

    public enum SkillAreaClass
    {
        Circle = 0,
        Rect = 1,
        Sector = 2,
    }

    public enum SkillTargetFaction
    {
        Self = 0,
        SelfTeam = 1,
        EnemyTeam = 2,
    }

    public enum SkillEffectiveTarget
    {
        Self = 0,
        SkillTarget = 1,
    }

    public enum DamageClass
    {
        Physic = 0,
        Magic = 1,
        Real = 2,
    }

    public enum SkillEffectClass
    {
        None = 0,
        Damage = 1,
        Heal = 2,
        Buff = 3,
        Debuff = 4,
        Disperse = 5,
        Shield = 6,
        Tag = 7,
    }

    public enum AcrionControlClass
    {
        None = 0,
        Wind = 1 << 1,
        Silence = 1 << 2,
        Charm = 1 << 3,
        Fear = 1 << 4,
        Move = 1 << 5,
    }
}
