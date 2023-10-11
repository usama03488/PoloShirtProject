
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShirtColors", menuName = "MyApp/ShirtColors")]
public class ShirtColor : ScriptableObject
{
    
    public List<Material> ShirtMaterials;
    public List<string> ColorCodes;
    public int Getlength()
    {
        return ShirtMaterials.Count - 1;
    }
 
}
