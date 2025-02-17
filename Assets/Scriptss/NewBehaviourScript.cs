//using System.Collections;
//using UnityEngine;
//using Firebase;
//using Firebase.Auth;
//using Firebase.Database;
//using TMPro;
//using System.Linq;
//using System;

//public class FirebaseManager : MonoBehaviour
//{
//    public UserInformation userDataUpdation;
//    public MetaPilotMenuManager menu;
//    [Header("Firebase")]
//    public DependencyStatus dependencyStatus;
//    public FirebaseAuth auth;
//    public FirebaseUser User;
//    public DatabaseReference DBreference;

//    //Login variables
//    [Header("Login")]
//    public TMP_InputField emailLoginField;
//    public TMP_InputField passwordLoginField;
//    public TMP_Text warningLoginText;
//    public TMP_Text confirmLoginText;

//    //Register variables
//    [Header("Register")]
//    public TMP_InputField usernameRegisterField;
//    public TMP_InputField emailRegisterField;
//    public TMP_InputField passwordRegisterField;
//    public TMP_InputField passwordRegisterVerifyField;
//    public TMP_Text warningRegisterText;

//    //User Data variables
//    [Header("UserData")]
//    public string usernameField;
//    public string averageRank;
//    public string totalGame;
//    public string successfulTrip;
//    public GameObject scoreElement;
//    public Transform scoreboardContent;
//    public GameObject scoreElement2;
//    public Transform scoreboardContent2;

//    public string usernameToStoreInDataFile;
//    public string username;
//    public int totalgame;
//    public int successfultrip;
//    public int averageR;

//    //public string username2;
//    //public int totalgame2;
//    //public int successfultrip2;
//    //public int averageR2;
//    public UserData currentUser;

//    public class UserData
//    {
//        public string userName;
//        public int averageRank;
//        public int successfulTrip;
//        public int totalGames;

//        internal void Initialize(string usernameField, int averageRank, int totalGame, int successfulTrip)
//        {
//            userName = usernameField;
//            this.averageRank = averageRank;
//            this.successfulTrip = successfulTrip;
//            this.totalGames = totalGame;
//        }
//    }

//    void Awake()
//    {
//        //Check that all of the necessary dependencies for Firebase are present on the system
//        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
//        //{
//        //    dependencyStatus = task.Result;
//        //    if (dependencyStatus == DependencyStatus.Available)
//        //    {
//        //        //If they are avalible Initialize Firebase
//        //        InitializeFirebase();
//        //    }
//        //    else
//        //    {
//        //        Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
//        //    }
//        //});
//        currentUser = new UserData();
//        InitializeFirebase();

//    }

//    private void InitializeFirebase()
//    {
//        Debug.Log("Setting up Firebase Auth");
//        //Set the authentication instance object
//        auth = FirebaseAuth.DefaultInstance;
//        Debug.Log("Setting up Firebase databbasee");
//        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
//        Debug.Log("Setting up Firebase databbasee again");
//    }
//    public void ClearLoginFeilds()
//    {
//        emailLoginField.text = "";
//        passwordLoginField.text = "";
//    }
//    public void ClearRegisterFeilds()
//    {
//        usernameRegisterField.text = "";
//        emailRegisterField.text = "";
//        passwordRegisterField.text = "";
//        passwordRegisterVerifyField.text = "";
//    }

//    //Function for the login button
//    public void LoginButton()
//    {
//        //Call the login coroutine passing the email and password
//        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
//    }
//    //Function for the register button
//    public void RegisterButton()
//    {
//        //Call the register coroutine passing the email, password, and username
//        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
//    }
//    //Function for the sign out button
//    public void SignOutButton()
//    {
//        auth.SignOut();
//        menu.loginPanelopen();
//        ClearRegisterFeilds();
//        ClearLoginFeilds();
//    }
//    //Function for the save button
//    public void SaveDataButton()
//    {
//        //  userDataUpdation.saveuserinfo();
//        StartCoroutine(UpdateUsernameAuth("test"));

//        //LoadScoreboardData();
//        //StartCoroutine(LoadScoreboardData());
//        StartCoroutine(UpdateUsernameDatabase(currentUser.userName));
//        StartCoroutine(UpdateAvergeRank(int.Parse(currentUser.averageRank.ToString())));
//        StartCoroutine(UpdateTotalGame(int.Parse(currentUser.totalGames.ToString())));
//        StartCoroutine(UpdatesuccessfulTrip(int.Parse(currentUser.successfulTrip.ToString())));
//    }

//    public void CreateNewUserInRealtimeDatabase(string _username)
//    {
//        //        userDataUpdation.saveuserinfo();
//        StartCoroutine(UpdateUsernameAuth("test"));
//        StartCoroutine(UpdateUsernameDatabase(_username));
//        StartCoroutine(UpdateAvergeRank(int.Parse("0")));
//        StartCoroutine(UpdateTotalGame(int.Parse("0")));
//        StartCoroutine(UpdatesuccessfulTrip(int.Parse("0")));
//    }


//    //Function for the scoreboard button
//    public void ScoreboardButton()
//    {
//        StartCoroutine(LoadScoreboardData());

//    }

//    public void ScoreboardButton2()
//    {
//        StartCoroutine(LoadScoreboardDataAfterGameEnd());

//    }

//    private IEnumerator Login(string email, string password)
//    {
//        //Call the Firebase auth signin function passing the email and password
//        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);

//        //Wait until the task completes
//        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

//        if (LoginTask.Exception != null)
//        {
//            //If there are errors handle them
//            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
//            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
//            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

//            string message = "Login Failed!";
//            switch (errorCode)
//            {
//                case AuthError.MissingEmail:
//                    message = "Missing Email";
//                    break;
//                case AuthError.MissingPassword:
//                    message = "Missing Password";
//                    break;
//                case AuthError.WrongPassword:
//                    message = "Wrong Password";
//                    break;
//                case AuthError.InvalidEmail:
//                    message = "Invalid Email";
//                    break;
//                case AuthError.UserNotFound:
//                    message = "Account does not exist";
//                    break;
//            }
//            warningLoginText.text = message;
//        }
//        else
//        {
//            //User is now logged in
//            //Now get the result
//            User = LoginTask.Result;
//            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
//            Debug.Log(User);
//            warningLoginText.text = "";
//            confirmLoginText.text = "Logged In";
//            StartCoroutine(LoadUserData());
//            usernameToStoreInDataFile = _email;

//            yield return new WaitForSeconds(2);

//            // usernameField = User.DisplayName;
//            menu.InstructionCanvason(); // Change to instruction canvas
//            confirmLoginText.text = "";
//            ClearLoginFeilds();
//            ClearRegisterFeilds();
//        }
//    }

//    private IEnumerator Register(string email, string password, string _username)
//    {
//        if (_username == "")
//        {
//            //If the username field is blank show a warning
//            warningRegisterText.text = "Missing Username";
//        }
//        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
//        {
//            //If the password does not match show a warning
//            warningRegisterText.text = "Password Does Not Match!";
//        }
//        else
//        {
//            //Call the Firebase auth signin function passing the email and password
//            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
//            //Wait until the task completes
//            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

//            if (RegisterTask.Exception != null)
//            {
//                //If there are errors handle them
//                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
//                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
//                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

//                string message = "Register Failed!";
//                switch (errorCode)
//                {
//                    case AuthError.MissingEmail:
//                        message = "Missing Email";
//                        break;
//                    case AuthError.MissingPassword:
//                        message = "Missing Password";
//                        break;
//                    case AuthError.WeakPassword:
//                        message = "Weak Password";
//                        break;
//                    case AuthError.EmailAlreadyInUse:
//                        message = "Email Already In Use";
//                        break;
//                }
//                warningRegisterText.text = message;
//            }
//            else
//            {
//                //User has now been created
//                //Now get the result
//                User = RegisterTask.Result;

//                if (User != null)
//                {
//                    //Create a user profile and set the username
//                    UserProfile profile = new UserProfile { DisplayName = _username };

//                    //Call the Firebase auth update user profile function passing the profile with the username
//                    var ProfileTask = User.UpdateUserProfileAsync(profile);
//                    //Wait until the task completes
//                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

//                    if (ProfileTask.Exception != null)
//                    {
//                        //If there are errors handle them
//                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
//                        warningRegisterText.text = "Username Set Failed!";
//                    }
//                    else
//                    {
//                        //Username is now set
//                        //Now return to login screen
//                        menu.loginPanelopen();
//                        warningRegisterText.text = "";
//                        ClearRegisterFeilds();
//                        ClearLoginFeilds();
//                        usernameField = _username;
//                        CreateNewUserInRealtimeDatabase(_username);
//                    }
//                }
//            }
//        }
//    }

//    private IEnumerator UpdateUsernameAuth(string _username)
//    {
//        //Create a user profile and set the username
//        UserProfile profile = new UserProfile { DisplayName = _username };

//        //Call the Firebase auth update user profile function passing the profile with the username
//        var ProfileTask = User.UpdateUserProfileAsync(profile);
//        //Wait until the task completes
//        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

//        if (ProfileTask.Exception != null)
//        {
//            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
//        }
//        else
//        {
//            //Auth username is now updated
//        }
//    }

//    private IEnumerator UpdateUsernameDatabase(string _username)
//    {
//        //Set the currently logged in user username in the database
//        if (_username != null)
//        {


//            var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

//            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

//            if (DBTask.Exception != null)
//            {
//                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
//            }
//            else
//            {
//                //Database username is now updated
//            }
//        }
//    }

//    private IEnumerator UpdateAvergeRank(int _averageRank)
//    {
//        //Set the currently logged in user average rank
//        var DBTask = DBreference.Child("users").Child(User.UserId).Child("average rank").SetValueAsync(_averageRank);

//        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

//        if (DBTask.Exception != null)
//        {
//            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
//        }
//        else
//        {
//            //Xp is now updated
//        }
//    }

//    private IEnumerator UpdateTotalGame(int _TGame)
//    {
//        //Set the currently logged in user total game
//        var DBTask = DBreference.Child("users").Child(User.UserId).Child("total game").SetValueAsync(_TGame);

//        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

//        if (DBTask.Exception != null)
//        {
//            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
//        }
//        else
//        {
//            //total game are now updated
//        }
//    }

//    private IEnumerator UpdatesuccessfulTrip(int _successfulTrips)
//    {
//        //Set the currently logged in user successfulTrip
//        var DBTask = DBreference.Child("users").Child(User.UserId).Child("successfulTrip").SetValueAsync(_successfulTrips);

//        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

//        if (DBTask.Exception != null)
//        {
//            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
//        }
//        else
//        {
//            //successfulTrip are now updated
//        }
//    }

//    private IEnumerator LoadUserData()
//    {
//        //Get the currently logged in user data
//        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

//        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

//        if (DBTask.Exception != null)
//        {
//            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
//        }
//        else if (DBTask.Result.Value == null)
//        {
//            //No data exists yet
//            averageRank = "0";
//            totalGame = "0";
//            successfulTrip = "0";
//        }
//        else
//        {
//            //Data has been retrieved
//            DataSnapshot snapshot = DBTask.Result;
//            Debug.Log(snapshot);
//            // usernameField = User.DisplayName;

//            usernameField = snapshot.Child("username").Value.ToString();
//            averageRank = snapshot.Child("average rank").Value.ToString();
//            totalGame = snapshot.Child("total game").Value.ToString();
//            successfulTrip = snapshot.Child("successfulTrip").Value.ToString();

//            totalgame = int.Parse(snapshot.Child("total game").Value.ToString());
//            successfultrip = int.Parse(snapshot.Child("successfulTrip").Value.ToString());
//            averageR = int.Parse(snapshot.Child("average rank").Value.ToString());

//            currentUser.Initialize(usernameField, averageR, totalgame, successfultrip);
//        }
//    }

//    public IEnumerator LoadScoreboardData()
//    {
//        //Get all the users data ordered by total game amount
//        Debug.Log("Load ScoreBoard Data bfr DB Refrence");
//        var DBTask = DBreference.Child("users").OrderByChild("total game").GetValueAsync();
//        Debug.Log("Load ScoreBoard Data after DB Refrence");
//        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

//        if (DBTask.Exception != null)
//        {
//            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
//        }
//        else
//        {
//            //Data has been retrieved
//            DataSnapshot snapshot = DBTask.Result;

//            //Destroy any existing scoreboard elements
//            foreach (Transform child in scoreboardContent.transform)
//            {
//                Destroy(child.gameObject);
//            }

//            //Loop through every users UID
//            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
//            {
//                // username = User.DisplayName;
//                username = childSnapshot.Child("username").Value.ToString();
//                totalgame = int.Parse(childSnapshot.Child("total game").Value.ToString());
//                successfultrip = int.Parse(childSnapshot.Child("successfulTrip").Value.ToString());
//                averageR = int.Parse(childSnapshot.Child("average rank").Value.ToString());

//                //Instantiate new scoreboard elements
//                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
//                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, totalgame, successfultrip, averageR);
//            }

//            //Go to scoareboard screen
//            menu.ScoreboardON();
//        }

//    }




//    public IEnumerator LoadScoreboardDataAfterGameEnd()
//    {
//        //Get all the users data ordered by total game amount
//        Debug.Log("Load ScoreBoard Data bfr DB Refrence");
//        var DBTask = DBreference.Child("users").OrderByChild("total game").GetValueAsync();
//        Debug.Log("Load ScoreBoard Data after DB Refrence");
//        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

//        if (DBTask.Exception != null)
//        {
//            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
//        }
//        else
//        {
//            //Data has been retrieved
//            DataSnapshot snapshot = DBTask.Result;

//            //Destroy any existing scoreboard elements
//            foreach (Transform child in scoreboardContent2.transform)
//            {
//                Destroy(child.gameObject);
//            }

//            //Loop through every users UID
//            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
//            {
//                // username = User.DisplayName;
//                username = childSnapshot.Child("username").Value.ToString();
//                totalgame = int.Parse(childSnapshot.Child("total game").Value.ToString());
//                successfultrip = int.Parse(childSnapshot.Child("successfulTrip").Value.ToString());
//                averageR = int.Parse(childSnapshot.Child("average rank").Value.ToString());

//                //Instantiate new scoreboard elements
//                GameObject scoreboardElement2 = Instantiate(scoreElement2, scoreboardContent2);
//                scoreboardElement2.GetComponent<ScoreElement>().NewScoreElement(username, totalgame, successfultrip, averageR);
//            }

//            //Go to scoareboard screen
//            menu.ScoreboardON2();
//        }

//    }

//}



//public class MetaPilotFireBaseUpload : MonoBehaviour
//{
//    FirebaseStorage storage;

//    private void Awake()
//    {
//        _instance = this;
//    }
//    //private void Start()
//    //{
//    //    storage = FirebaseStorage.DefaultInstance;
//    //    Debug.Log("Firebase Storage Reference " + storage);
//    //}

//    private void Start()
//    {
//        storage = FirebaseStorage.DefaultInstance;
//        Debug.Log("Firebase Storage Reference " + storage);
//    }
//    public static MetaPilotFireBaseUpload _instance;

//    public static MetaPilotFireBaseUpload Instance
//    {
//        get
//        {
//            if (_instance == null)
//            {
//                _instance = GameObject.FindObjectOfType<MetaPilotFireBaseUpload>();
//            }

//            return _instance;
//        }
//    }

//    public void UploadFile(string filePath, string fileName)
//    {
//        StorageReference reference;

//        reference = storage.RootReference.Child(fileName);
//        //string localfile = Application.streamingAssetsPath + "/test.png";
//        reference.PutFileAsync(filePath).ContinueWith(task =>
//        {
//            if (task.IsCompleted)
//            {
//                Debug.Log("Done Storing File");
//            }
//        });
//    }

//    public void UploadBytes(byte[] bytes, string fileName)
//    {
//        StorageReference reference;
//        reference = storage.RootReference.Child(fileName);
//        //string localfile = Application.streamingAssetsPath + "/test.png";
//        reference.PutBytesAsync(bytes).ContinueWith(task =>
//        {
//            if (task.IsCompleted)
//            {
//                Debug.Log("Done Storing Image");
//            }
//        });
//    }
//}