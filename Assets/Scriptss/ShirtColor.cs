
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShirtColors", menuName = "MyApp/ShirtColors")]
public class ShirtColor : ScriptableObject
{
    
    public List<Material> ShirtMaterials;
    public List<string> ColorCodes;
    public List<string> ColorName;
    public int Getlength()
    {
        return ShirtMaterials.Count - 1;
    }
    public string GetColorName(int index)
    {
     //   int i = int.Parse(index);
        return ColorName[index - 1];
    }
 
}
