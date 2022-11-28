using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTest : MonoBehaviour
{
    public ArrowProjectile arrow;
    public Transform firePostion;
    public int damageMin, damageMax, force;
    public float gravityScale, criticalMultiplier, angle;
    public Transform target;
    public WeaponEffect weaponEffect = new WeaponEffect();
    public bool isCritical, is_manual_enable;
    public bool shoot;
    // Start is called before the first frame update
    void Start()
    {

    }
    public float Vector2ToAngle(Vector2 vec2)
    {
        var angle = Mathf.Atan2(vec2.y, vec2.x) * Mathf.Rad2Deg;
        return angle;
    }
    public static Vector2 AngleToVector2(float degree)
    {
        Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, degree) * Vector2.right);

        return dir;
    }
    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            shoot = false;
            Vector2 fromPosition = firePostion.position;
            float beginAngle = Vector2ToAngle((Vector2)target.position - fromPosition);
            ArrowProjectile _tempArrow;
            _tempArrow = Instantiate(arrow, fromPosition, Quaternion.identity);
            _tempArrow.Init(damageMin, damageMax, force * AngleToVector2(beginAngle + angle), gravityScale, isCritical, weaponEffect, _owner: this.gameObject);
        }
        Debug.DrawLine(firePostion.position, target.position, Color.red);
    }
}
