using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Shirt : MonoBehaviour
{
    Vector3 position;
    Quaternion rotation;
    Transform initialtransform;
    public GrabMoveProvider leftmouse;
    public ShirtColor shirtcolors;
    public Renderer Shirtmaterial;
    public  bool grabbed = false;
    public Material dummymat;
    private int Shirt_Id = 0;
    private string Shirturl;
    public string GetShirtUrl()
    {
        return Shirturl;
    }
   // public GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
       
        initialtransform = transform;
        position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        rotation = new Quaternion(transform.rotation.x,transform.rotation.y,transform.rotation.z,transform.rotation.w);
       // pickRandomColor(GameManager.Instance.Get_ColorIndex());
       // pickRandomColor();
    }
    public void Set_id(int ID)
    {
        Shirt_Id = ID;
    }
    public int Get_id()
    {
        return Shirt_Id;
    }
    public bool Isgrabbing()
    {
        if (grabbed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void pickRandomColor(int index)
    {
        Debug.Log(index++);
        Set_id(index++);
        // int randomNumber = Random.Range(0, shirtcolors.Getlength());

        // Shirtmaterial.material = shirtcolors.ShirtMaterials[randomNumber];
        Material material1=new Material(dummymat);
    
        Shirtmaterial.material = material1;
        if (shirtcolors.ColorCodes.Count >index)
        {
            material1.color = Colors.FromHex(shirtcolors.ColorCodes[index]);
            Shirtmaterial.material.color = Colors.FromHex(shirtcolors.ColorCodes[index]);

        }
     
     //   Shirtmaterial.material.color=shirtcolors.ColorCodes[0]
    }
    // Update is called once per frame
    public static Color HexToColor(string hex)
    {
        
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        Debug.Log("color now " + r + g+b);
        return new Color(r / 255f, g / 255f, b / 255f);
    }
  
    public void Returnposition()
    {
        //Debug.Log("saved position"+position.position.y);
        //Debug.Log("current position"+transform.parent.position.y);
        //  Debug.Log("current rotation"+transform.parent.position.y);
        //transform.position = initialtransform.position;
        //transform.rotation = initialtransform.rotation;
     GameManager.Instance.StopHint();
       transform.position = position;
        transform.rotation = rotation;
        grabbed = false;
    }
    public void ThisShirtSelected()
    {
        GameManager.Instance.SetShirt_Id(this);
        StartCoroutine(GameManager.Instance.ShowHint());
        grabbed = true;
    }
}
