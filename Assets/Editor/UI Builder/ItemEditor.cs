using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemEditor : EditorWindow
{
    //[SerializeField]
    //private VisualTreeAsset m_VisualTreeAsset = default;

    private ItemDataList_SO dataBase;
    private List<ItemDetails> itemList = new List<ItemDetails>();
    private VisualTreeAsset itemRowTemplate;

    //右侧物品详细信息
    private ScrollView itemDetailsSection;
    private ItemDetails activeItem;

    //右侧列表中的item的icon默认预览图片
    private Sprite defaultIcon;

    //右侧列表中的item的icon预览
    private VisualElement iconPreview;

    private ListView itemListView;

    //快捷键 % -> Ctrl & -> Alt #-> Shift
    [MenuItem("Tools/ItemEditor &s")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemEditor.uxml");
        visualTree.CloneTree(root);

        //other way to instantiate a VisualElement from UXML
        // VisualElement labelFromUXML = visualTree.Instantiate();
        // root.Add(labelFromUXML);

        //拿到模版数据
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemRowTemplate.uxml");
        //拿到默认的Icon图片
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        //变量赋值
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");
        iconPreview = itemDetailsSection.Q<VisualElement>("Icon");

        //获得按键
        root.Q<Button>("AddButton").clicked += OnAddItemClicked;
        root.Q<Button>("DeleteButton").clicked += OnDeleteClicked;

        //加载数据
        LoadDataBase();

        GenerateListView();
    }

    #region 按键事件
    private void OnDeleteClicked()
    {
        itemList.Remove(activeItem);
        itemListView.Rebuild();
        itemDetailsSection.visible = false;
    }

    private void OnAddItemClicked()
    {
        ItemDetails newItem = new ItemDetails();
        newItem.itemName = "New Item";
        newItem.itemID = 1001 + itemList.Count;

        itemList.Add(newItem);
        itemListView.Rebuild();
    }
    #endregion

    private void LoadDataBase()
    {
        //按类型搜索 t:ItemDataList_SO
        string[] dataArray = AssetDatabase.FindAssets("ItemDataList_SO");

        if (dataArray.Length > 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            dataBase = AssetDatabase.LoadAssetAtPath<ItemDataList_SO>(path);
        }
        itemList = dataBase.itemDetailsList;
        //如果不标记无法保存数据
        EditorUtility.SetDirty(dataBase);
    }


    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree();

        //element->每个元素 index -> 每个元素的索引
        Action<VisualElement, int> bindItem = (element, index) =>
        {
            if (index < itemList.Count)
            {
                if (itemList[index].itemIcon != null)
                {
                    element.Q<VisualElement>("Icon").style.backgroundImage = itemList[index].itemIcon.texture;
                }
                element.Q<Label>("Name").text = itemList[index] == null ? "No Item" : itemList[index].itemName;
            }
        };

        itemListView.fixedItemHeight = 60;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;
        itemListView.itemsSource = itemList;

        itemListView.onSelectionChange += OnListSelectionChange;

        //设置默认右侧面板不可见
        itemDetailsSection.visible = false;
    }

    private void OnListSelectionChange(IEnumerable<object> selectedItems)
    {
        activeItem = selectedItems.First() as ItemDetails;
        if (activeItem != null)
        {
            GetItemDetails();
            itemDetailsSection.visible = true;
        }
        else
        {
            Debug.Log("No item selected");
        }

    }


    private void GetItemDetails()
    {
        itemDetailsSection.MarkDirtyRepaint();

        // itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;
        // itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt =>
        // {
        //     activeItem.itemID = evt.newValue;
        // });

        //双向绑定
        SerializedObject serializedItem = new SerializedObject(dataBase);
        var itemTypePropertyName = serializedItem.FindProperty("itemDetailsList")
            .GetArrayElementAtIndex(itemList.IndexOf(activeItem))
            .FindPropertyRelative("itemID");

        var TextField = itemDetailsSection.Q<IntegerField>("ItemID");

        TextField.BindProperty(itemTypePropertyName);


        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild();
        });

        // 绑定枚举类型的字段(双向绑定)     //FIXME: 实现与SerializedObject的双向绑定
        // SerializedObject serializedItem = new SerializedObject(dataBase);
        var itemTypeProperty = serializedItem.FindProperty("itemDetailsList")
            .GetArrayElementAtIndex(itemList.IndexOf(activeItem))
            .FindPropertyRelative("itemType");

        var enumField = itemDetailsSection.Q<EnumField>("ItemType");
        enumField.Init(activeItem.itemType);

        enumField.BindProperty(itemTypeProperty); // 自动处理双向同步


        // itemDetailsSection.Q<EnumField>("ItemType").Init(activeItem.itemType);
        // itemDetailsSection.Q<EnumField>("ItemType").value = activeItem.itemType;
        // itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        // {
        //     activeItem.itemType = (ItemType)evt.newValue;
        // });

        iconPreview.style.backgroundImage = activeItem.itemIcon == null
                ? defaultIcon.texture : activeItem.itemIcon.texture;

        // 单独处理 ObjectField，因为它不是 BaseField<T> 的泛型
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;
            iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
            itemListView.Rebuild();
        });


        //世界图片
        itemDetailsSection.Q<ObjectField>("ItemSprite").value = activeItem.itemOnworldSprite;
        itemDetailsSection.Q<ObjectField>("ItemSprite").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemOnworldSprite = evt.newValue as Sprite;
        });

        itemDetailsSection.Q<TextField>("Description").value = activeItem.itemDescription;
        itemDetailsSection.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("ItemUseRadius").value = activeItem.itemUseRadius;
        itemDetailsSection.Q<IntegerField>("ItemUseRadius").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemUseRadius = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanPickedup").value = activeItem.canPickedUp;
        itemDetailsSection.Q<Toggle>("CanPickedup").RegisterValueChangedCallback(evt =>
        {
            activeItem.canPickedUp = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanDropped").value = activeItem.canDropped;
        itemDetailsSection.Q<Toggle>("CanDropped").RegisterValueChangedCallback(evt =>
        {
            activeItem.canDropped = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("CanCarried").value = activeItem.canCarried;
        itemDetailsSection.Q<Toggle>("CanCarried").RegisterValueChangedCallback(evt =>
        {
            activeItem.canCarried = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("Price").value = activeItem.itemPrice;
        itemDetailsSection.Q<IntegerField>("Price").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemPrice = evt.newValue;
        });

        itemDetailsSection.Q<Slider>("SellPercentage").value = activeItem.sellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellPercentage = evt.newValue;
        });


    }


    /// <summary>
    /// 通用序列化属性绑定方法
    /// </summary>
    /// <typeparam name="TField">UI字段类型</typeparam>
    /// <param name="propertyName">序列化属性名称</param>
    /// <param name="fieldName">UI字段名称</param>
    /// <param name="serializedObject">序列化对象</param>
    private void BindSerializedProperty<TField>(string propertyName, string fieldName, SerializedObject serializedObject)
        where TField : BaseField<object>
    {
        // 获取SerializedProperty
        var property = serializedObject.FindProperty("itemDetailsList")
            .GetArrayElementAtIndex(itemList.IndexOf(activeItem))
            .FindPropertyRelative(propertyName);

        // 获取UI字段
        var field = itemDetailsSection.Q<TField>(fieldName);

        if (field != null && property != null)
        {
            // 对于枚举字段需要特殊处理
            if (field is EnumField enumField)
            {
                enumField.Init(activeItem.itemType);
            }

            // 绑定属性
            field.BindProperty(property);
        }
        else
        {
            Debug.LogError($"绑定失败: {fieldName}");
        }

    }


        // // 使用通用方法绑定字段
        //     BindField<IntegerField, int>("ItemID",
        //         () => activeItem.itemID,
        //         value => activeItem.itemID = value);

        //     BindField<TextField, string>("ItemName",
        //         () => activeItem.itemName,
        //         value =>
        //         {
        //             activeItem.itemName = value;
        //             itemListView.Rebuild();
        //         });

        /// <summary>
        /// 通用字段绑定方法
        /// </summary>
        /// <typeparam name="TField">字段类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="name">字段名称</param>
        /// <param name="getter">获取值的方法</param>
        /// <param name="setter">设置值的方法</param>
        // private void BindField<TField, TValue>(string name,
        //     Func<TValue> getter,
        //     Action<TValue> setter) where TField : BaseField<TValue>
        // {
        //     var field = itemDatailsSection.Q<TField>(name);
        //     if (field != null)
        //     {
        //         field.value = getter();
        //         field.RegisterValueChangedCallback(evt => setter(evt.newValue));
        //     }
        // }

    }
