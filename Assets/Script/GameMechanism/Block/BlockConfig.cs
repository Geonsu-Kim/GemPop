using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="BlockConfig",menuName = "Block/BlockConfig")]
public class BlockConfig : ScriptableObject
{
    public Sprite basicBlockSprites;
    public Sprite[] itemBlockSprites;
    public Material[] blockMaterials;
}
