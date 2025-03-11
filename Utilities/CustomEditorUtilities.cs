using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Maledictus.Utilities
{
    public static class CustomEditorUtilities
    {
        #region Sections

        public static VisualElement CreateSimpleSection(FlexDirection direction, Justify justifyContent = Justify.SpaceAround)
        {
            var element = new VisualElement()
            {
                style =
                {
                    flexDirection = direction,
                    justifyContent = justifyContent,

                    marginRight = 10,
                    marginLeft = 10,
                }
            };

            return element;
        }

        public static VisualElement CreateSectionTitle(string title, string subtitle = null, float fontSize = 20f, FontStyle fontStyle = FontStyle.Bold, Align alignSelf = Align.Center)
        {
            var element = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    flexGrow = 1,

                    alignSelf = Align.Center,
                }
            };

            var elementTitle = new Label(title)
            {
                style =
                {
                    fontSize = fontSize,
                    unityFontStyleAndWeight = fontStyle,
                    alignSelf = alignSelf,
                    marginTop = 15,
                }
            };

            element.Add(elementTitle);

            if (subtitle != null)
            {
                var elementSubtitle = new Label(subtitle)
                {
                    style =
                    {
                        alignSelf = alignSelf,
                        fontSize = fontSize * 0.5f,
                        unityFontStyleAndWeight = FontStyle.BoldAndItalic,
                    }
                };

                element.Add(elementSubtitle);
            }

            return element;
        }

        public static VisualElement CreateSection(VisualElement title, FlexDirection direction, float margin, float padding, float minWidth = 0, float maxWidth = 0)
        {
            var section = new VisualElement()
            {
                style =
                {
                    flexDirection = direction,
                    alignSelf = Align.Center,
                    marginBottom = margin,
                    marginRight = margin,
                    paddingTop = padding,
                    paddingBottom = padding,
                    paddingLeft = padding,
                    paddingRight = padding,
                }
            };

            if (minWidth > 0) section.style.minWidth = minWidth;
            if (maxWidth > 0) section.style.maxWidth = maxWidth;

            if (title != null) section.Add(title);

            return section;
        }

        #endregion

        #region Generic Field

        public static VisualElement CreateField(VisualElement elementValue, string label = null, float width = 0, bool readOnly = false)
        {
            VisualElement element = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 0,
                    justifyContent = Justify.SpaceBetween,
                    fontSize = 12,
                    marginBottom = 1f,
                    marginLeft = 3f,
                    marginRight = -2f,
                    marginTop = 1f,
                }
            };

            if (width > 0) element.style.minWidth = width;

            element.SetEnabled(!readOnly);

            if (label != null)
            {
                var elementLabel = new Label(label)
                {
                    style =
                    {
                        flexGrow = 1,
                        width = 100f,
                        alignSelf = Align.Center,
                        unityFontStyleAndWeight = FontStyle.Bold,
                    }
                };

                element.Add(elementLabel);
            }

            elementValue.style.flexGrow = 9;
            element.Add(elementValue);

            return element;
        }

        public static VisualElement CreateField(VisualElement elementValue, string label = null, bool readOnly = false)
        {
            VisualElement element = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1,
                    justifyContent = Justify.SpaceBetween,
                    fontSize = 12,
                    marginBottom = 1f,
                    marginLeft = 3f,
                    marginRight = -2f,
                    marginTop = 1f,
                }
            };

            element.SetEnabled(!readOnly);

            if (label != null)
            {
                var elementLabel = new Label(label)
                {
                    style =
                    {
                        flexGrow = 1,
                        unityFontStyleAndWeight = FontStyle.Bold,
                    }
                };

                element.Add(elementLabel);
            }

            element.Add(elementValue);

            return element;
        }

        #endregion

        #region Text Field

        public static VisualElement CreateTextField(SerializedProperty property, string label = null, float width = 0, bool readOnly = false)
        {
            var textField = new TextField();
            textField.BindProperty(property);

            if (width > 0) textField.style.maxWidth = width;

            textField.Q("unity-text-input").RegisterCallback<GeometryChangedEvent>(evt =>
            {
                textField.Q("unity-text-input").style.unityTextAlign = textField.resolvedStyle.width < 500
                    ? TextAnchor.MiddleLeft
                    : TextAnchor.MiddleRight;
            });

            return CreateField(textField, label, width, readOnly);
        }

        public static TextField CreateTextAreaField(SerializedProperty property, int height = 0)
        {
            var textField = new TextField();
            textField.BindProperty(property);
            textField.multiline = true;

            if (height > 0)
            {
                textField.style.height = height;
                textField.Q("unity-text-input").style.whiteSpace = WhiteSpace.Normal;
            }

            textField.Q("unity-text-input").style.paddingTop = 5;
            textField.Q("unity-text-input").style.paddingBottom = 5;
            textField.Q("unity-text-input").style.paddingLeft = 5;
            textField.Q("unity-text-input").style.paddingRight = 5;

            return textField;
        }

        #endregion

        #region Enum Field

        public static VisualElement CreateEnumField(SerializedProperty property, string label = null, float width = 0, bool readOnly = false)
        {
            var enumField = new EnumField();
            enumField.BindProperty(property);
            enumField.style.justifyContent = Justify.FlexEnd;

            if (width > 0) enumField.style.maxWidth = width;

            return CreateField(enumField, label, width, readOnly);
        }

        public static VisualElement CreateEnumField(SerializedProperty property, string label = null, bool readOnly = false)
        {
            var enumField = new EnumField();
            enumField.BindProperty(property);
            enumField.style.justifyContent = Justify.FlexEnd;

            return CreateField(enumField, label, readOnly);
        }

        #endregion

        #region Integer Field

        public static VisualElement CreateIntegerField(SerializedProperty property, string label = null, float width = 0, bool readOnly = false, int min = int.MinValue, int max = int.MaxValue)
        {
            var integerField = new IntegerField();
            integerField.BindProperty(property);

            integerField.RegisterValueChangedCallback(evt =>
            {
                integerField.value = Mathf.Clamp(evt.newValue, min, max);
            });

            integerField.Q("unity-text-input").RegisterCallback<GeometryChangedEvent>(evt =>
            {
                integerField.Q("unity-text-input").style.unityTextAlign = integerField.resolvedStyle.width < 500
                    ? TextAnchor.MiddleLeft
                    : TextAnchor.MiddleRight;
            });

            return CreateField(integerField, label, width, readOnly);
        }

        #endregion

        #region Image Field

        public static Image CreateImage(SerializedProperty property, float minWidth = 0, float minHeight = 0)
        {
            var image = new Image()
            {
                style =
                {
                    flexGrow = 0,
                    alignSelf = Align.Center,
                    minWidth = minWidth,
                    minHeight = minHeight,
                    maxWidth = minWidth * 2f,
                    maxHeight = minHeight * 2f,
                }
            };

            var sprite = property.objectReferenceValue as Sprite;
            Debug.Log($"Sprite: {sprite}");

            var currentSprite = property.objectReferenceValue as Sprite;

            if (currentSprite != null)
                image.image = currentSprite.texture;

            Debug.Log($"Sprite: {currentSprite.name}");
            return image;
        }

        #endregion

        #region Box Field

        public static Box CreateBoxContainer(SerializedProperty property = null, float minWidth = 0, float maxWidth = 0f)
        {
            var borderColor = new Color(0.13f, 0.13f, 0.13f, 0.2f);
            var backgroundColor = new Color(0.13f, 0.13f, 0.13f, 0.1f);

            var borderWidth = 0.5f;
            var borderRadius = 10f;
            var padding = 5f;

            var box = new Box()
            {
                style =
                {
                    flexGrow = 1,
                
                    alignSelf = Align.Center,
                
                    minWidth = minWidth,
                    maxWidth = maxWidth,
                
                    backgroundColor = backgroundColor,
                
                    borderBottomColor = borderColor,
                    borderLeftColor = borderColor,
                    borderRightColor = borderColor,
                    borderTopColor = borderColor,
                
                    borderBottomWidth = borderWidth,
                    borderLeftWidth = borderWidth,
                    borderRightWidth = borderWidth,
                    borderTopWidth = borderWidth,
                
                    borderBottomLeftRadius = borderRadius,
                    borderBottomRightRadius = borderRadius,
                    borderTopLeftRadius = borderRadius,
                    borderTopRightRadius = borderRadius,
                
                    paddingTop = padding,
                    paddingBottom = padding,
                    paddingLeft = padding,
                    paddingRight = padding,
                }
            };

            if(property != null)
            {
                var label = new Label($"\"{property.stringValue}\"")
                {
                    style =
                {
                    flexWrap = Wrap.Wrap,
                    color = new Color(1f, 1f, 1f, 0.6f),
                    whiteSpace = WhiteSpace.Normal,
                    unityTextAlign = TextAnchor.UpperCenter,
                    unityFontStyleAndWeight = FontStyle.Italic,
                }
                };

                box.Add(label);
            }

            return box;
        }

        #endregion

        #region Foldout Field

        public static VisualElement CreateFoldout(string title, string subtitle = null, VisualElement contentElement = null, bool expanded = true, int contentMarginLeft = 15)
        {
            //var element = CreateLabelBox();
            //element.style.minWidth = new StyleLength(StyleKeyword.Auto);
            //element.style.maxWidth = new StyleLength(StyleKeyword.Auto);

            var foldoutContainer = new VisualElement()
            {
                style =
                {
                    marginTop = 10f,
                    marginBottom = 10f,
                }
            };

            var headerContainer = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,

                    fontSize = 15f,
                    unityFontStyleAndWeight = FontStyle.BoldAndItalic,

                    paddingLeft = 5,
                    paddingTop = 3,
                    paddingBottom = 3,

                    cursor = new StyleCursor(),
                }
            };

            #region Header labels

            var leftHeader = CreateSimpleSection(FlexDirection.Row, Justify.FlexStart);
            leftHeader.style.flexGrow = 1;

            // Create an arrow icon (toggle symbol)
            var arrowLabel = new Label(expanded ? "▼" : "►")
            {
                style =
                {
                    fontSize = 12f,
                    unityFontStyleAndWeight = FontStyle.Bold,

                    alignSelf = Align.Center,

                    marginRight = 15f, // Space between arrow and title
                }
            };

            // Create the title label
            var titleLabel = new Label(title);

            leftHeader.Add(arrowLabel);
            leftHeader.Add(titleLabel);

            var rightHeader = CreateSimpleSection(FlexDirection.Row, Justify.FlexEnd);
            rightHeader.style.flexGrow = 9;

            var subtitleLabel = new Label(subtitle)
            {
                style =
                {
                    alignSelf = Align.FlexEnd,
                    width = 75f,
                }
            };

            rightHeader.Add(subtitleLabel);

            #endregion

            headerContainer.Add(leftHeader);
            headerContainer.Add(rightHeader);

            var contentContainer = new VisualElement();
            contentContainer.style.marginLeft = contentMarginLeft;
            contentContainer.style.display = expanded ? DisplayStyle.Flex : DisplayStyle.None;

            contentContainer.Add(contentElement);

            headerContainer.RegisterCallback<ClickEvent>(evt =>
            {
                bool isExpanded = contentContainer.style.display == DisplayStyle.Flex;
                contentContainer.style.display = isExpanded ? DisplayStyle.None : DisplayStyle.Flex;
                arrowLabel.text = isExpanded ? "►" : "▼";
            });

            foldoutContainer.Add(headerContainer);
            foldoutContainer.Add(contentContainer);

            return foldoutContainer;
            //element.Add(foldoutContainer);
            //return element;
        }

        #endregion

        #region Space

        public static VisualElement Space(float height = 0)
        {
            var element = new VisualElement();
            element.style.height = height;

            return element;
        }

        #endregion

        #region Toggle

        /// <summary>
        /// Creates a boolean field (toggle) bound to a serialized property.
        /// </summary>
        public static VisualElement CreateBooleanField(SerializedProperty property, string label = null, float width = 0, bool readOnly = false)
        {
            var toggle = new Toggle();
            toggle.BindProperty(property);

            if (width > 0) toggle.style.minWidth = width;

            // Return the container (VisualElement) that wraps the toggle
            return CreateField(toggle, label, width, readOnly);
        }

        #endregion
    }

    public static class EditorUtilities
    {
        public static void Label(string text, GUIStyle style)
        {
            EditorGUILayout.LabelField(text, style);
        }

        public static string TextField(string label, string text, float labelWidth = 50f)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            text = EditorGUILayout.TextField(text);
            EditorGUILayout.EndHorizontal();
            return text;
        }
        public static string DescriptionField(string label, string text)
        {
            Vertical(() =>
            {
                var labelStyle = new GUIStyle(EditorStyles.label)
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 14,
                    alignment = TextAnchor.MiddleRight,
                };

                Label(label, labelStyle);

                var textAreaStyle = new GUIStyle(EditorStyles.textArea)
                {
                    wordWrap = true,
                    padding = new RectOffset(10, 10, 10, 10),
                    fontStyle = FontStyle.Italic,
                    fontSize = 14,
                    alignment = TextAnchor.MiddleCenter,
                };

                text = EditorGUILayout.TextArea(text, textAreaStyle, GUILayout.ExpandWidth(true));
            });
            return text;
        }

        public static void Space(float width)
        {
            EditorGUILayout.Space(width);
        }

        public static Sprite SpriteField(string label, Sprite sprite)
        {
            return (Sprite)EditorGUILayout.ObjectField(label, sprite, typeof(Sprite), allowSceneObjects: false);
        }

        public static Sprite SpriteFieldNoLabel(Sprite sprite, float maxWidth, float maxHeight)
        {
            if (sprite != null)
            {
                // Get the sprite's original dimensions
                float spriteWidth = sprite.rect.width;
                float spriteHeight = sprite.rect.height;

                // Calculate the aspect ratio
                float aspectRatio = spriteWidth / spriteHeight;

                // Calculate the new dimensions while maintaining the aspect ratio
                float newWidth = maxWidth;
                float newHeight = maxWidth / aspectRatio;

                // If the calculated height exceeds the maxHeight, adjust the dimensions
                if (newHeight > maxHeight)
                {
                    newHeight = maxHeight;
                    newWidth = maxHeight * aspectRatio;
                }

                // Create the GUILayout options with the calculated dimensions
                var options = new GUILayoutOption[]
                {
                    GUILayout.Width(newWidth),
                    GUILayout.Height(newHeight),
                };

                // Display the ObjectField with the adjusted dimensions
                return (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), false, options);
            }
            else
            {
                // If the sprite is null, just use the maxWidth and maxHeight
                var options = new GUILayoutOption[]
                {
                    GUILayout.Width(maxWidth),
                    GUILayout.Height(maxHeight),
                };

                return (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), false, options);
            }
        }


        public static void ButtonField(string label, float width, System.Action callback)
        {
            if (GUILayout.Button(label, GUILayout.Width(width)))
                callback?.Invoke();
        }

        public static T EnumField<T>(string label, T selected, float labelWidth) where T : System.Enum
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            selected = (T)EditorGUILayout.EnumPopup(selected);
            EditorGUILayout.EndHorizontal();
            return selected;
        }

        public static int IntField(string label, int value)
        {
            return EditorGUILayout.IntField(label, value);
        }

        public static float FloatField(string label, float value)
        {
            return EditorGUILayout.FloatField(label, value);
        }

        public static bool FoldoutField(string label, bool foldout, System.Action content)
        {
            foldout = EditorGUILayout.Foldout(foldout, label);
            if (foldout)
            {
                EditorGUI.indentLevel++;
                content?.Invoke();
                EditorGUI.indentLevel--;
            }
            return foldout;
        }

        public static void Horizontal(System.Action content, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);
            content?.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        public static void Vertical(System.Action content, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(options);
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }
    }
}