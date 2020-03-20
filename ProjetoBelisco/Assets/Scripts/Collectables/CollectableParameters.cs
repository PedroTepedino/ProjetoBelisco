using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CollectableData", menuName = "Collectables/CollectableData")]
public class CollectableParameters : SerializedScriptableObject
{
    [HorizontalGroup("Base", Width = 150)]

    [SerializeField]
    [AssetSelector(Paths = "Assets/Resources/Sprites/Colletables", DropdownTitle = "Sprites", FlattenTreeView = true )]
    [AssetsOnly]
    [PreviewField(Alignment = ObjectFieldAlignment.Left,Height = 100,AlignmentHasValue = true)]
    [HideLabel]
    [VerticalGroup("Base/Left")]
    private Sprite _sprite;

    [SerializeField]
    [BoxGroup("Base/Right", ShowLabel = false)]
    [LabelWidth(80)]
    [Required]
    private string _name;

    [SerializeField]
    [BoxGroup("Base/Right", ShowLabel = false)]
    [LabelWidth(80)]
    private BaseColletableEffect _effect;

    [SerializeField]
    private Collider2D _teste = new Collider2D();

    private enum ColliderType
    {

    }


}
