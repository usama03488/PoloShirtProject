using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpaceBear.VRUI;
using TMPro;
using VRKeyboard.Utils;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //public
    [SerializeField] AuthManager firebasemanager;
     [Header("During Login")]
   // public InputField email;
   // public InputField Password;
    public KeyboardManager keyboard;
    public TMP_InputField email;
    public TMP_InputField Password;
    [Header("Sucessfully Login")]
    public GameObject Canvas;
    public GameObject Room;

    [Header("During Sign up")]
    public TMP_Text text;
    public Button SignUP_btn;
    public delegate void SucessLogin();
    public SucessLogin entreRoom;
    public List<GameObject> Shirts;
    [Header("Shirts Color")]
    public GameObject ShirtPrefab;
    public List<Transform> spawnPositions;
    public Dictionary<string, int> ShirtsColor_Code= new Dictionary<string, int>();
    [Header("Walls Information")]
  //  public List<WallInformation> WallsFinfo;
    public float BetweenDistance;
    public List<WallInformation> WallsFinfo;
    public int Total_Rows=0;
    public int Total_Columns = 0;
    public ShirtColor shirtcolors;
    public InputActionProperty Abutton;
    private Shirt SelectedSshirt_Id;
    //UI THINGS
    public GameObject ScrollView;
    public GameObject Content;
    public GameObject ItemPrefab;
    public GameObject noFavShirt;
    public GameObject CloselistBtn;
    //Privates
    private int ColorIndex = 0;
    private string UserID;
    int a = 2;
    int b;
    public static GameManager Instance { get; private set; }

    public void SetShirt_Id(Shirt selected)
    {
        SelectedSshirt_Id = selected;
    }
    private void Awake()
    {
        b = a;
        a = 4;
        Debug.LogError("vlaue of b is " + b);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void sendtoFavourites(InputAction.CallbackContext context)
    {
        bool isgrabbed=SelectedSshirt_Id.Isgrabbing();
        if (isgrabbed)
        {
            firebasemanager.AddtoFavourites(SelectedSshirt_Id.Get_id());
        }
    }

    public void Increment_Colorindex()
    {
        ColorIndex++;
    }
    public int Get_ColorIndex()
    {
        return ColorIndex;
    }
    // Start is called before the first frame update
    void Start()
    {
        Abutton.action.started +=sendtoFavourites;
        //for (int a = 0; a < spawnPositions.Count; a++)
        //{
        //    Instantiate(ShirtPrefab, spawnPositions[a].position, spawnPositions[a].rotation);
        //    //   a.SetActive(true);
        //}

        entreRoom += SucessfullyLogin;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           firebasemanager.AddtoFavourites(1);
        }
    }
    public void OpenFavouritesList()
    {
        firebasemanager.GenerateList();
       
            ScrollView.SetActive(true);
            CloselistBtn.SetActive(true);
      
      
    }
    public void DisableError()
    {
        noFavShirt.SetActive(false);
    }
    public List<GameObject> GeneratedItems;
    public void PublishFavouritesList(string id)
    {
        
        GameObject item= Instantiate(ItemPrefab,Content.transform);
        GeneratedItems.Add(item);
        item.transform.GetChild(0).GetComponent<TMP_Text>().text= item.transform.GetChild(0).GetComponent<TMP_Text>().text + id;
       // item.transform.parent = Content.transform;
        item.transform.localScale = new Vector3(1, 1, 1);
      //  item.transform.position = new Vector3(96.69f, -12.085f, 0);
    }
    public void CloseList()
    {
        foreach(GameObject a in GeneratedItems)
        {
            Destroy(a);
        }
        GeneratedItems = new List<GameObject>();
    }
    public void SucessfullySignedup() {
        text.text = "Signed UP";
        SignUP_btn.GetComponent<Image>().color = Color.red;
    }
    public void EnterEmail()
    {
    
        keyboard.SetInputfield(email);
    }
    public void EnterPassword()
    {
   
        Debug.LogWarning("password enter func called");
        keyboard.SetInputfield(Password);
    }
    public string GetEmail()
    {
        return email.text;
    }
    public string GetPassword()
    {
        return Password.text;
    }
    public void SucessfullyLogin()
    {
        Canvas.SetActive(false);
        Room.SetActive(true);
        SpawnShirts();
        keyboard.gameObject.SetActive(false);
    }
    public void SpawnShirts()
    {
        int items = Total_Columns * Total_Rows;
        Vector3 starting_position;
        Quaternion rotation;

        for (int i = 0; i < WallsFinfo.Count; i++)
        {
            starting_position = spawnPositions[i].position;
            if (i !=0)
            {
                starting_position.z -= BetweenDistance;
                spawnPositions[i].position=new Vector3(starting_position.z,0,0);
            }
            rotation = spawnPositions[i].rotation;
            for (int x = 0; x < WallsFinfo[i].Rows; x++)
            {
                for (int j = 0; j < WallsFinfo[i].Columns; j++)
                {
                    GameObject shirtobj = Instantiate(ShirtPrefab, starting_position, rotation);
                    Shirt shirt = shirtobj.GetComponent<Shirt>();
                    shirt.pickRandomColor(ColorIndex);
                    if (i == 0)
                    {
                        starting_position.x += BetweenDistance;
                    }
                    else
                    {
                        starting_position.z -= BetweenDistance;

                    }
                  
                    Increment_Colorindex();

                }
                starting_position.y += BetweenDistance;
                if (i == 0)
                {
                    starting_position.x = spawnPositions[i].position.x;
                }
                else
                {
                    starting_position.z = spawnPositions[i].position.x;

                }
              
            }
        }
        //for (int a = 0; a < shirtcolors.ColorCodes.Count; a++)
        //{
        //    Instantiate(ShirtPrefab, spawnPositions[a].position, spawnPositions[a].rotation);
        //    //   a.SetActive(true);
        //}
    }
    [System.Serializable]
    public class WallInformation
    {
        public int Rows;
        public int Columns;
    }
}

