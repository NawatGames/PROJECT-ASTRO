using System.IO;
using Menus.Pause;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

// Aplica o visual de terminal CRT no PauseCanvas: fonte VT323 com brilho, tela verde-escura,
// cursor ">" na seleção, cabeçalho e rodapé, e o overlay de scanlines com a animação de ligar.
// Roda uma vez pelo menu Tools; depois disso dá pra ajustar tudo direto no prefab.
public static class CrtPauseStyleBuilder
{
    private const string PrefabPath = "Assets/Prefabs/PauseCanvas.prefab";
    private const string FontPath = "Assets/Fonts/VT323/VT323-Regular.ttf";
    private const string FontAssetPath = "Assets/Fonts/VT323/VT323-Regular SDF.asset";
    private const string GlowMaterialPath = "Assets/Fonts/VT323/VT323-Regular SDF Glow.mat";
    private const string CrtMaterialFolder = "Assets/Materials/UI";
    private const string CrtMaterialPath = CrtMaterialFolder + "/CrtScreen.mat";
    private const string OverlayName = "CrtOverlay";

    private const float MenuFontSize = 26f;
    private const float CursorWidth = 14f;
    private const float ScreenTextSize = 38f;
    private const float ScreenPadding = 60f;

    private static readonly Color Phosphor = new Color32(0x6D, 0xFF, 0xA6, 0xFF);
    private static readonly Color ScreenColor = new Color32(0x03, 0x12, 0x0A, 0xFF);
    private static readonly Color BezelColor = new Color32(0x1F, 0x5A, 0x36, 0xFF);

    [MenuItem("Tools/Astro/Aplicar estilo CRT na pausa")]
    private static void Apply()
    {
        var fontAsset = GetOrCreateFontAsset();
        var glowMaterial = GetOrCreateGlowMaterial(fontAsset);
        var crtMaterial = GetOrCreateCrtMaterial();
        if (!fontAsset || !glowMaterial || !crtMaterial) return;

        var root = PrefabUtility.LoadPrefabContents(PrefabPath);
        try
        {
            if (root.transform.Find(OverlayName))
            {
                Debug.LogWarning($"[CRT] {PrefabPath} já tem o estilo CRT aplicado. Nada foi alterado.");
                return;
            }

            Restyle(root, fontAsset, glowMaterial, crtMaterial);
            PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            Debug.Log($"[CRT] Estilo aplicado em {PrefabPath}.");
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(root);
        }
    }

    private static void Restyle(GameObject root, TMP_FontAsset fontAsset, Material glowMaterial, Material crtMaterial)
    {
        var canvas = root.transform;
        var screen = (RectTransform)canvas.Find("Background");
        var mainPanel = canvas.Find("MainPanel");

        screen.GetComponent<Image>().color = ScreenColor;
        canvas.Find("MenuBorder").GetComponent<Image>().color = BezelColor;
        mainPanel.Find("MenuBorder").gameObject.SetActive(false);
        mainPanel.Find("Title").gameObject.SetActive(false);

        foreach (var text in root.GetComponentsInChildren<TMP_Text>(true))
        {
            text.font = fontAsset;
            text.fontSharedMaterial = glowMaterial;
        }

        Selectable firstItem = null;
        foreach (Transform item in mainPanel.Find("Container"))
        {
            var button = item.GetComponent<Button>();
            if (!firstItem) firstItem = button;
            RestyleMenuItem(item, fontAsset, glowMaterial);
        }

        var headerLeft = CreateScreenText("CrtHeaderLeft", screen, "NAV // EM ESPERA", fontAsset, glowMaterial,
            new Vector2(0, 1), new Vector2(ScreenPadding, -ScreenPadding * 0.66f), TextAlignmentOptions.TopLeft, 0.6f);
        var headerRight = CreateScreenText("CrtHeaderRight", screen, "ASTRO-7", fontAsset, glowMaterial,
            new Vector2(1, 1), new Vector2(-ScreenPadding, -ScreenPadding * 0.66f), TextAlignmentOptions.TopRight, 0.6f);
        var footer = CreateScreenText("CrtFooter", screen, "AGUARDANDO OPERADOR", fontAsset, glowMaterial,
            new Vector2(0, 0), new Vector2(ScreenPadding, ScreenPadding * 0.66f), TextAlignmentOptions.BottomLeft, 0.7f);
        footer.gameObject.AddComponent<CrtBlinkingSuffix>();

        var menu = mainPanel.gameObject.AddComponent<CrtMenu>();
        var menuSo = new SerializedObject(menu);
        menuSo.FindProperty("firstSelected").objectReferenceValue = firstItem;
        var extras = menuSo.FindProperty("showWithMenu");
        extras.arraySize = 3;
        extras.GetArrayElementAtIndex(0).objectReferenceValue = headerLeft.gameObject;
        extras.GetArrayElementAtIndex(1).objectReferenceValue = headerRight.gameObject;
        extras.GetArrayElementAtIndex(2).objectReferenceValue = footer.gameObject;
        menuSo.ApplyModifiedPropertiesWithoutUndo();

        CreateOverlay(canvas, screen, crtMaterial);
    }

    private static void RestyleMenuItem(Transform item, TMP_FontAsset fontAsset, Material glowMaterial)
    {
        Object.DestroyImmediate(item.GetComponent<ButtonHandler>());
        item.Find("Background").GetComponent<Image>().color = Color.clear;

        var label = item.GetComponentInChildren<TMP_Text>(true);
        label.color = Phosphor;
        label.fontSize = MenuFontSize;
        label.rectTransform.offsetMin += new Vector2(CursorWidth, 0);

        var cursor = NewUiObject("Cursor", item);
        cursor.anchorMin = new Vector2(0, 0);
        cursor.anchorMax = new Vector2(0, 1);
        cursor.pivot = new Vector2(0, 0.5f);
        cursor.anchoredPosition = Vector2.zero;
        cursor.sizeDelta = new Vector2(CursorWidth, 0);
        var cursorText = cursor.gameObject.AddComponent<TextMeshProUGUI>();
        cursorText.text = ">";
        cursorText.font = fontAsset;
        cursorText.fontSharedMaterial = glowMaterial;
        cursorText.fontSize = MenuFontSize;
        cursorText.color = Phosphor;
        cursorText.alignment = TextAlignmentOptions.MidlineLeft;
        cursorText.raycastTarget = false;

        var menuItem = item.gameObject.AddComponent<CrtMenuItem>();
        var so = new SerializedObject(menuItem);
        so.FindProperty("label").objectReferenceValue = label;
        so.FindProperty("cursor").objectReferenceValue = cursor.gameObject;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    private static TextMeshProUGUI CreateScreenText(string name, RectTransform parent, string content,
        TMP_FontAsset fontAsset, Material glowMaterial, Vector2 corner, Vector2 offset,
        TextAlignmentOptions alignment, float alpha)
    {
        var rect = NewUiObject(name, parent);
        rect.anchorMin = corner;
        rect.anchorMax = corner;
        rect.pivot = corner;
        rect.anchoredPosition = offset;
        rect.sizeDelta = new Vector2(800, ScreenTextSize * 1.5f);

        var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.font = fontAsset;
        text.fontSharedMaterial = glowMaterial;
        text.fontSize = ScreenTextSize;
        text.color = new Color(Phosphor.r, Phosphor.g, Phosphor.b, alpha);
        text.alignment = alignment;
        text.enableWordWrapping = false;
        text.raycastTarget = false;
        return text;
    }

    private static void CreateOverlay(Transform canvas, RectTransform screen, Material crtMaterial)
    {
        var overlay = NewUiObject(OverlayName, canvas);
        overlay.SetAsLastSibling();
        overlay.anchorMin = screen.anchorMin;
        overlay.anchorMax = screen.anchorMax;
        overlay.pivot = screen.pivot;
        overlay.anchoredPosition = screen.anchoredPosition;
        overlay.sizeDelta = screen.sizeDelta;

        var image = overlay.gameObject.AddComponent<Image>();
        image.material = crtMaterial;
        image.raycastTarget = false;

        var crt = overlay.gameObject.AddComponent<CrtScreen>();
        var so = new SerializedObject(crt);
        so.FindProperty("screenRect").objectReferenceValue = screen;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    private static RectTransform NewUiObject(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.layer = parent.gameObject.layer;
        var rect = (RectTransform)go.transform;
        rect.SetParent(parent, false);
        return rect;
    }

    // Mesmo procedimento do menu Assets > Create > TextMeshPro > Font Asset.
    private static TMP_FontAsset GetOrCreateFontAsset()
    {
        var existing = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontAssetPath);
        if (existing) return existing;

        var font = AssetDatabase.LoadAssetAtPath<Font>(FontPath);
        if (!font)
        {
            Debug.LogError($"[CRT] Fonte não encontrada em {FontPath}.");
            return null;
        }

        var fontAsset = TMP_FontAsset.CreateFontAsset(font, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024,
            AtlasPopulationMode.Dynamic, false);
        fontAsset.name = Path.GetFileNameWithoutExtension(FontAssetPath);
        AssetDatabase.CreateAsset(fontAsset, FontAssetPath);

        fontAsset.atlasTextures[0].name = fontAsset.name + " Atlas";
        AssetDatabase.AddObjectToAsset(fontAsset.atlasTextures[0], fontAsset);
        fontAsset.material.name = fontAsset.name + " Material";
        AssetDatabase.AddObjectToAsset(fontAsset.material, fontAsset);

        EditorUtility.SetDirty(fontAsset);
        AssetDatabase.SaveAssets();
        return fontAsset;
    }

    private static Material GetOrCreateGlowMaterial(TMP_FontAsset fontAsset)
    {
        if (!fontAsset) return null;

        var existing = AssetDatabase.LoadAssetAtPath<Material>(GlowMaterialPath);
        if (existing) return existing;

        var material = new Material(fontAsset.material) { name = Path.GetFileNameWithoutExtension(GlowMaterialPath) };
        material.EnableKeyword("GLOW_ON");
        material.SetColor("_GlowColor", new Color(Phosphor.r, Phosphor.g, Phosphor.b, 0.45f));
        material.SetFloat("_GlowOffset", 0f);
        material.SetFloat("_GlowInner", 0.05f);
        material.SetFloat("_GlowOuter", 0.35f);
        material.SetFloat("_GlowPower", 0.6f);
        AssetDatabase.CreateAsset(material, GlowMaterialPath);
        return material;
    }

    private static Material GetOrCreateCrtMaterial()
    {
        var existing = AssetDatabase.LoadAssetAtPath<Material>(CrtMaterialPath);
        if (existing) return existing;

        var shader = Shader.Find("UI/CrtScreen");
        if (!shader)
        {
            Debug.LogError("[CRT] Shader UI/CrtScreen não encontrado. Confira se Assets/Art/Shaders/UI-CrtScreen.shader compilou.");
            return null;
        }

        if (!AssetDatabase.IsValidFolder(CrtMaterialFolder)) AssetDatabase.CreateFolder("Assets/Materials", "UI");

        var material = new Material(shader) { name = "CrtScreen" };
        material.SetColor("_OffColor", ScreenColor);
        AssetDatabase.CreateAsset(material, CrtMaterialPath);
        return material;
    }
}
