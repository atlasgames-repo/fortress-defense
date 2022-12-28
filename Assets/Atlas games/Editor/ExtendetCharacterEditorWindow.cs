using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
public class ExtendetCharacterEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;


    protected void DrawProperties(SerializedProperty prop, bool drawChildren)
    {
        string lastPropPath = string.Empty;
        foreach (SerializedProperty p in prop)
        {
            if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(p, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) { continue; }
                lastPropPath = p.propertyPath;
                EditorGUILayout.PropertyField(p, drawChildren);
            }
        }
    }
    protected void DrawProperties(GameObject enemy, SerializedProperty prop, bool drawChildren)
    {
        LoadEnemy(enemy);
        DrawProperties(prop, drawChildren);
    }
    protected void LoadEnemy(GameObject enemy)
    {
        CharacterData obj = serializedObject.targetObject as CharacterData;
        SmartEnemyGrounded player = enemy.GetComponent<SmartEnemyGrounded>();
        switch (player.attackType)
        {
            case ATTACKTYPE.MELEE:
                EnemyMeleeAttack meleeattack = TryGetComponent<EnemyMeleeAttack>(enemy);
                obj.enemyMeleeAttack.criticalPercent = meleeattack.criticalPercent;
                obj.enemyMeleeAttack.dealDamage = meleeattack.dealDamage;
                obj.enemyMeleeAttack.meleeRate = meleeattack.meleeRate;
                obj.enemyMeleeAttack.radiusCheck = meleeattack.radiusCheck;
                obj.enemyMeleeAttack.targetLayer = meleeattack.targetLayer;
                break;
            case ATTACKTYPE.RANGE:
                EnemyRangeAttack rangeattack = TryGetComponent<EnemyRangeAttack>(enemy);
                obj.enemyRangeAttack.aimTarget = rangeattack.aimTarget;
                obj.enemyRangeAttack.aimTargetOffset = rangeattack.aimTargetOffset;
                obj.enemyRangeAttack.damage = rangeattack.damage;
                obj.enemyRangeAttack.detectDistance = rangeattack.detectDistance;
                break;
            case ATTACKTYPE.THROW:
                EnemyThrowAttack throwattack = TryGetComponent<EnemyThrowAttack>(enemy);
                obj.enemyThrowAttack.damage = throwattack.damage;
                obj.enemyThrowAttack.addTorque = throwattack.addTorque;
                obj.enemyThrowAttack.angleThrow = throwattack.angleThrow;
                obj.enemyThrowAttack.onlyAttackTheFortrest = throwattack.onlyAttackTheFortrest;
                obj.enemyThrowAttack.radiusDetectPlayer = throwattack.radiusDetectPlayer;
                obj.enemyThrowAttack.targetPlayer = throwattack.targetPlayer;
                obj.enemyThrowAttack.throwForceMax = throwattack.throwForceMax;
                obj.enemyThrowAttack.throwForceMin = throwattack.throwForceMin;
                obj.enemyThrowAttack.throwRate = throwattack.throwRate;
                break;
            default:
                break;
        }
        obj.enemyName = enemy.name;
        obj.expMin = player.GetComponent<GiveExpWhenDie>().expMin;
        obj.expMax = player.GetComponent<GiveExpWhenDie>().expMax;
        obj.smartEnemyGrounded.attackType = player.attackType;
        obj.smartEnemyGrounded.gravity = player.gravity;
        obj.smartEnemyGrounded.health = player.health;
        obj.smartEnemyGrounded.spawnDelay = player.spawnDelay;
        obj.smartEnemyGrounded.startBehavior = player.startBehavior;
        obj.smartEnemyGrounded.useGravity = player.useGravity;
        obj.smartEnemyGrounded.walkSpeed = player.walkSpeed;


        obj.attakType = player.attackType;

        serializedObject = new SerializedObject(obj);
        Apply();
        AssetDatabase.Refresh();
    }
    protected void Apply()
    {
        serializedObject.ApplyModifiedProperties();
    }
    protected void Save(GameObject enemy)
    {
        CharacterData obj = serializedObject.targetObject as CharacterData;
        SmartEnemyGrounded player = enemy.GetComponent<SmartEnemyGrounded>();

        switch (obj.attakType)
        {
            case ATTACKTYPE.MELEE:
                EnemyMeleeAttack meleeattack = TryGetComponent<EnemyMeleeAttack>(enemy);
                meleeattack.criticalPercent = obj.enemyMeleeAttack.criticalPercent;
                meleeattack.dealDamage = obj.enemyMeleeAttack.dealDamage;
                meleeattack.meleeRate = obj.enemyMeleeAttack.meleeRate;
                meleeattack.radiusCheck = obj.enemyMeleeAttack.radiusCheck;
                meleeattack.targetLayer = obj.enemyMeleeAttack.targetLayer;
                break;
            case ATTACKTYPE.RANGE:
                EnemyRangeAttack rangeattack = TryGetComponent<EnemyRangeAttack>(enemy);
                rangeattack.aimTarget = obj.enemyRangeAttack.aimTarget;
                rangeattack.aimTargetOffset = obj.enemyRangeAttack.aimTargetOffset;
                rangeattack.damage = obj.enemyRangeAttack.damage;
                rangeattack.detectDistance = obj.enemyRangeAttack.detectDistance;
                break;
            case ATTACKTYPE.THROW:
                EnemyThrowAttack throwattack = TryGetComponent<EnemyThrowAttack>(enemy);
                throwattack.damage = obj.enemyThrowAttack.damage;
                throwattack.addTorque = obj.enemyThrowAttack.addTorque;
                throwattack.angleThrow = obj.enemyThrowAttack.angleThrow;
                throwattack.onlyAttackTheFortrest = obj.enemyThrowAttack.onlyAttackTheFortrest;
                throwattack.radiusDetectPlayer = obj.enemyThrowAttack.radiusDetectPlayer;
                throwattack.targetPlayer = obj.enemyThrowAttack.targetPlayer;
                throwattack.throwForceMax = obj.enemyThrowAttack.throwForceMax;
                throwattack.throwForceMin = obj.enemyThrowAttack.throwForceMin;
                throwattack.throwRate = obj.enemyThrowAttack.throwRate;
                break;
            default:
                break;
        }

        enemy.name = obj.enemyName;
        player.GetComponent<GiveExpWhenDie>().expMin = obj.expMin;
        player.GetComponent<GiveExpWhenDie>().expMax = obj.expMax;
        player.attackType = obj.smartEnemyGrounded.attackType;
        player.gravity = obj.smartEnemyGrounded.gravity;
        player.health = obj.smartEnemyGrounded.health;
        player.spawnDelay = obj.smartEnemyGrounded.spawnDelay;
        player.startBehavior = obj.smartEnemyGrounded.startBehavior;
        player.useGravity = obj.smartEnemyGrounded.useGravity;
        player.walkSpeed = obj.smartEnemyGrounded.walkSpeed;



        player.attackType = obj.attakType;
        Apply();

        EditorUtility.SetDirty(enemy.gameObject);
        PrefabUtility.RecordPrefabInstancePropertyModifications(enemy.gameObject);
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    }
    T TryGetComponent<T>(GameObject value)
    {
        bool found = value.TryGetComponent<T>(out T component);
        if (!found)
        {
            EditorUtility.DisplayDialog("Error", $"Wrong attackType was found: {typeof(T)}", "Ok", "Exit");
            GetWindow(typeof(CharacterDataEditorWindow)).Close();
            throw new System.Exception($"Wrong attackType was found: {typeof(T)}");
        }

        return component;
    }
}
