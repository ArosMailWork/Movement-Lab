using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    #region SerializedProperty
    SerializedProperty WeaponType;
    SerializedProperty FireType;

    SerializedProperty FireRate;
    SerializedProperty Damage;

    bool isGun;

    private void OnEnable()
    {
        WeaponType = serializedObject.FindProperty("weaponType");
        FireType = serializedObject.FindProperty("fireType");

        FireRate = serializedObject.FindProperty("FireRate");
        Damage = serializedObject.FindProperty("Damage");
    }
    #endregion

    public override void OnInspectorGUI()
    {
        Weapon _weapon = (Weapon)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(WeaponType);

        //isGun = EditorGUILayout.BeginFoldoutHeaderGroup(isGun, "Gun Stats");
        if (_weapon.weaponType == Weapon.WeaponType.Gun)
        {
            EditorGUILayout.PropertyField(FireType);
            EditorGUILayout.PropertyField(FireRate);
            EditorGUILayout.PropertyField(Damage);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
