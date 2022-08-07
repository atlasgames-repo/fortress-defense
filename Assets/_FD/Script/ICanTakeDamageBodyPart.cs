using UnityEngine;
using System.Collections;


public enum BODYPART {NONE, HEAD, BODY, LEG_LEFT, LEG_RIGHT, KNEE_LEFT, KNEE_RIGHT}
public interface ICanTakeDamageBodyPart {
    void TakeDamage(float damage, Vector2 force, Vector2 hitPosition, GameObject instigator, WeaponEffect weaponEffect = null, float pushBackPercent = 0, float knockDownRagdollPercent = 0, float shockPercent = 0);
}
