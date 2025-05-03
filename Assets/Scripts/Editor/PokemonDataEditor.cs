using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.IO;

[CustomEditor(typeof(PokemonData))]
public class PokemonDataEditor : Editor
{
    private PokemonData pokemonDataSet;
    private SerializedProperty abilitiesProp;

    private string[] subclassNames;
    private Type[] subclassTypes;
    private int selectedIndex = 0;
    private bool addAsHidden = false;

    // Sprite animation variables
    private Sprite[] frontAnimationFrames;
    private Sprite[] backAnimationFrames;
    private Sprite[] frontShinyAnimationFrames;
    private Sprite[] backShinyAnimationFrames;
    private double lastTime;
    private int currentFrame;
    private const double frameRate = 1.0 / 15.0;

    // Shiny toggle
    private bool showShiny = false;

    private void EditorUpdate()
    {
        UpdateFrame(); // Advance frame if needed
    }

    private void OnEnable()
    {
        EditorApplication.update += EditorUpdate;

        pokemonDataSet = (PokemonData)target;
        abilitiesProp = serializedObject.FindProperty("possibleAbilities");

        // Get Ability subclasses
        subclassTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(Ability)) && !t.IsAbstract)
            .ToArray();

        subclassNames = subclassTypes.Select(t => t.Name).ToArray();

        AutoAssignSprites();
    }

    private void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Sprites", EditorStyles.boldLabel);

        showShiny = EditorGUILayout.Toggle("Show Shiny", showShiny);

        GUILayout.BeginHorizontal(); // Start a horizontal layout for sprites
        DrawSpriteSection("Front", showShiny ? frontShinyAnimationFrames : frontAnimationFrames);
        DrawSpriteSection("Back", showShiny ? backShinyAnimationFrames : backAnimationFrames);
        GUILayout.EndHorizontal(); // End the horizontal layout

        EditorGUILayout.Space(10);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("pokemonName"), new GUIContent("Pokemon Name"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("classification"), new GUIContent("Classification"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pokedex_entry"), new GUIContent("Pokedex Entry"));

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Abilities", EditorStyles.boldLabel);

        var sortedAbilities = pokemonDataSet.possibleAbilities
            .OrderBy(a => a.isHidden)
            .ThenBy(a => a.AbilityName)
            .ToList();

        DrawAbilityList(sortedAbilities);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("----------------------------------------------------", EditorStyles.label);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Add New Ability", EditorStyles.boldLabel);
        selectedIndex = EditorGUILayout.Popup("Ability Type", selectedIndex, subclassNames);
        addAsHidden = EditorGUILayout.Toggle("Is Hidden Ability", addAsHidden);

        if (GUILayout.Button("Add Ability"))
        {
            var newAbility = Activator.CreateInstance(subclassTypes[selectedIndex]) as Ability;
            newAbility.isHidden = addAsHidden;

            var nameProp = subclassTypes[selectedIndex].GetProperty("Name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            nameProp?.SetValue(newAbility, subclassTypes[selectedIndex].Name);

            pokemonDataSet.possibleAbilities.Add(newAbility);
            EditorUtility.SetDirty(pokemonDataSet);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSpriteSection(string label, Sprite[] spriteFrames)
    {
        if (spriteFrames == null || spriteFrames.Length == 0)
            return;

        GUILayout.BeginVertical(); // Start a vertical layout for each sprite

        if (spriteFrames.Length > currentFrame)
        {
            var sprite = spriteFrames[currentFrame];
            Texture2D tex = sprite.texture;
            Rect spriteRect = sprite.rect;

            float texWidth = tex.width;
            float texHeight = tex.height;

            Rect uv = new Rect(
                spriteRect.x / texWidth,
                spriteRect.y / texHeight,
                spriteRect.width / texWidth,
                spriteRect.height / texHeight
            );

            Rect drawArea = GUILayoutUtility.GetRect(128, 128, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            GUI.DrawTextureWithTexCoords(drawArea, tex, uv);

            // Adding "Front" or "Back" under the sprite
            string description = label; // Just use the label ("Front" or "Back")
            EditorGUILayout.LabelField(description, EditorStyles.wordWrappedMiniLabel);
        }

        GUILayout.EndVertical(); // End the vertical layout
    }

    private void DrawAbilityList(System.Collections.Generic.List<Ability> abilities)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            var ability = abilities[i];
            int index = pokemonDataSet.possibleAbilities.IndexOf(ability);
            SerializedProperty element = abilitiesProp.GetArrayElementAtIndex(index);

            string label = !string.IsNullOrEmpty(ability.AbilityName)
                ? ability.AbilityName + (ability.isHidden ? " (Hidden)" : "")
                : $"Ability {i + 1}";

            string description = !string.IsNullOrEmpty(ability.AbilityDescription)
                ? ability.AbilityDescription
                : "No description available.";

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(element, new GUIContent(label), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(description, EditorStyles.wordWrappedMiniLabel);

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                pokemonDataSet.possibleAbilities.Remove(ability);
                EditorUtility.SetDirty(pokemonDataSet);
                break;
            }

            EditorGUILayout.Space(8);
        }
    }

    private void AutoAssignSprites()
    {
        string baseName = pokemonDataSet.name;
        string folderPath = $"Assets/Sprites/Pokemon/{baseName}/";

        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning($"Sprite folder not found: {folderPath}");
            return;
        }

        string[] files = Directory.GetFiles(folderPath, "*.png");

        var frontList = new System.Collections.Generic.List<Sprite>();
        var frontShinyList = new System.Collections.Generic.List<Sprite>();
        var backList = new System.Collections.Generic.List<Sprite>();
        var backShinyList = new System.Collections.Generic.List<Sprite>();

        foreach (string file in files)
        {
            UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(file);

            foreach (var asset in assets)
            {
                if (asset is Sprite sprite)
                {
                    string spriteName = sprite.name;

                    if (spriteName.StartsWith($"{baseName}_f_s"))
                        frontShinyList.Add(sprite);
                    else if (spriteName.StartsWith($"{baseName}_f"))
                        frontList.Add(sprite);
                    else if (spriteName.StartsWith($"{baseName}_b_s"))
                        backShinyList.Add(sprite);
                    else if (spriteName.StartsWith($"{baseName}_b"))
                        backList.Add(sprite);
                }
            }
        }

        frontAnimationFrames = frontList.OrderBy(s => s.name).ToArray();
        frontShinyAnimationFrames = frontShinyList.OrderBy(s => s.name).ToArray();
        backAnimationFrames = backList.OrderBy(s => s.name).ToArray();
        backShinyAnimationFrames = backShinyList.OrderBy(s => s.name).ToArray();

        Debug.Log($"Loaded sprites for {baseName}: " +
            $"Front ({frontAnimationFrames.Length}), Front Shiny ({frontShinyAnimationFrames.Length}), " +
            $"Back ({backAnimationFrames.Length}), Back Shiny ({backShinyAnimationFrames.Length})");
    }

    private void UpdateFrame()
    {
        double time = EditorApplication.timeSinceStartup;
        if (time - lastTime > frameRate)
        {
            lastTime = time;
            currentFrame++;

            int maxFrameCount = new[] {
                frontAnimationFrames?.Length ?? 0,
                frontShinyAnimationFrames?.Length ?? 0,
                backAnimationFrames?.Length ?? 0,
                backShinyAnimationFrames?.Length ?? 0
            }.Max();

            if (maxFrameCount > 0)
                currentFrame = currentFrame % maxFrameCount;

            Repaint();
        }
    }
}
