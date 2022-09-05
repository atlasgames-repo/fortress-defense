using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetSelector : MonoBehaviour
{
    private SmartEnemyGrounded enemy;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<SmartEnemyGrounded>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(UpdateEnum());
    }

    // Update is called once per frame
    IEnumerator UpdateEnum()
    {
        while (this.gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(0.01f);
            if (enemy.is_targeted)
            {
                sprite.enabled = true;
            }
            else
            {
                sprite.enabled = false;
            }
        }
    }
}
