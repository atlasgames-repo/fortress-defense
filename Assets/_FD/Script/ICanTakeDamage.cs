using UnityEngine;
using System.Collections;

public interface ICanTakeDamage {

    //void TakeDamageEnhance (float damage, Vector2 force, GameObject instigator, Vector2 hitPosition, BulletFeature bulletType = BulletFeature.Normal);

    //void TakeDamage(float damage, Vector2 force, GameObject instigator);
    void TakeDamage(float damage, Vector2 force, Vector2 hitPoint, GameObject instigator, BODYPART bodyPart = BODYPART.NONE, WeaponEffect weaponEffect = null, WEAPON_EFFECT forceEffect = WEAPON_EFFECT.NONE);
}
