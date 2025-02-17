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
    public GameObject LogIn_SignUp_Panel;
        public GameObject RoomsList_Panel;
    public List<GameObject> RoomsList;
    public GameObject HintObject;
    [Header("During Sign up")]
    public TMP_Text text;
    public Button SignUP_btn;
    public Button Login_btn;
    public Button AlreadyRegistered_btn;
    public Button DontRegistered_btn;
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
    public InputActionProperty URLButtons;
    private Shirt SelectedSshirt_Id;
    //UI THINGS
    public GameObject ScrollView;
    public GameObject Content;
    public GameObject ItemPrefab;
    public GameObject noFavShirt;
    public GameObject CloselistBtn;
    public GameObject HintPanel;
    //Privates
    private int ColorIndex = 0;
    private string UserID;
  //  int a = 2;
  //  int b;
    public static GameManager Instance { get; private set; }

    public void SetShirt_Id(Shirt selected)
    {
        SelectedSshirt_Id = selected;
    }
    private void Awake()
    {
        //  b = a;
        //   a = 4;
        // Debug.LogError("vlaue of b is " + b);
        if (PlayerPrefs.GetFloat("FirstTime", 0) == 1)
        {
          //  Login_btn.gameObject.SetActive(true);
           // SignUP_btn.gameObject.SetActive(false);
            AlreadyRegistered();
            if(PlayerPrefs.GetString("Username","")!=" ")
            {
                email.transform.GetChild(0).gameObject.SetActive(false);
                Password.transform.GetChild(0).gameObject.SetActive(false);
                email.text = PlayerPrefs.GetString("Username", "");
                Password.text = PlayerPrefs.GetString("password", "");
                Debug.Log("password"+Password.text);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("FirstTime", 1);
            SignUP_btn.gameObject.SetActive(true);
        }
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
    public IEnumerator StartHint() {
        HintPanel.SetActive(true);
        yield return new WaitForSeconds(5f);

    }
    public void sendtoFavourites(InputAction.CallbackContext context)
    {
        bool isgrabbed=SelectedSshirt_Id.Isgrabbing(); 
        if (isgrabbed)
        {
            firebasemanager.AddtoFavourites(SelectedSshirt_Id.Get_id());
        }
    }
    public void OpenOpenSea(InputAction.CallbackContext context)
    {
        if (SelectedSshirt_Id.Isgrabbing())
        {
            Application.OpenURL("https://opensea.io/assets/ethereum/0x9ffc8b4d48605bd155b844d940aae0c4ad0d99de/1/");
        }
    }
    public void BuyShirt()
    {
        Debug.Log("BUTTON IS cLICKING");
        Application.OpenURL("https://opensea.io/assets/ethereum/0x9ffc8b4d48605bd155b844d940aae0c4ad0d99de/1/");
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
        URLButtons.action.started += OpenOpenSea;
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
        item.GetComponent<Button>().onClick.AddListener(BuyShirt);
        item.transform.GetChild(0).GetComponent<TMP_Text>().text= item.transform.GetChild(0).GetComponent<TMP_Text>().text +shirtcolors.GetColorName(int.Parse(id));
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
        AlreadyRegistered();
        PlayerPrefs.SetFloat("FirstTime", 1); 
       // SignUP_btn.gameObject.SetActive(false);
       // Login_btn.gameObject.SetActive(true);
      //  SignUP_btn.GetComponent<Image>().color = Color.red;
    }
    public void AlreadyRegistered()
    {
        SignUP_btn.gameObject.SetActive(false);
        Login_btn.gameObject.SetActive(true);
        DontRegistered_btn.gameObject.SetActive(true);
        AlreadyRegistered_btn.gameObject.SetActive(false);

    }
    public void RegisterSelf()
    {
        SignUP_btn.gameObject.SetActive(true);
        Login_btn.gameObject.SetActive(false);
        DontRegistered_btn.gameObject.SetActive(false);
        AlreadyRegistered_btn.gameObject.SetActive(true);
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
        LogIn_SignUp_Panel.SetActive(false);
        RoomsList_Panel.SetActive(true);
        //Canvas.SetActive(false);
    //    Room.SetActive(true);
       
        keyboard.gameObject.SetActive(false); 
    }
    public IEnumerator ShowHint()
    {
        HintObject.SetActive(true);
        HintPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
      //  HintPanel.SetActive(false);
    }
    public void StopHint()
    {
        StopCoroutine(nameof(ShowHint));
        HintPanel.SetActive(false);
    }
    public void ActiveRoom(int i)
    {
        Canvas.SetActive(false);
        RoomsList[i].SetActive(true);
        SpawnShirts();
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

