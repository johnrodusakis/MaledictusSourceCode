using Maledictus.Enemy;
using Maledictus.Utilities;
using UnityEngine;

namespace Maledictus.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(EnemySO))]
    public class EnemySOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EnemySO enemySO = (EnemySO)target;

            EditorUtilities.Vertical(() =>
            {
                EditorUtilities.Horizontal(() =>
                {
                    enemySO.Figure = EditorUtilities.SpriteFieldNoLabel(enemySO.Figure, 200, 300);

                    EditorUtilities.Space(25f);

                    EditorUtilities.Vertical(() =>
                    {
                        var labelWidth = 100f;

                        enemySO.Name = EditorUtilities.TextField("Name", enemySO.Name, labelWidth);
                        enemySO.Category = EditorUtilities.EnumField("Category", enemySO.Category, labelWidth);
                        enemySO.Stage = EditorUtilities.EnumField("Stage", enemySO.Stage, labelWidth);
                        enemySO.Rarity = EditorUtilities.EnumField("Rarity", enemySO.Rarity, labelWidth);

                        EditorUtilities.Space(10f);

                        enemySO.Type1 = EditorUtilities.EnumField("Type 1", enemySO.Type1, labelWidth);
                        enemySO.Type2 = EditorUtilities.EnumField("Type 2", enemySO.Type2, labelWidth);

                        EditorUtilities.Space(10f);

                        enemySO.Discovery.Value = EditorUtilities.EnumField("Discovery", enemySO.Discovery.Value, labelWidth);

                        EditorUtilities.Space(15f);
                    });
                });

                GUILayout.Space(-10f);
                enemySO.Description = EditorUtilities.DescriptionField("Description", enemySO.Description);
            });

            if (GUI.changed)
            {
                EditorUtility.SetDirty(enemySO);
            }
        }
    }
}