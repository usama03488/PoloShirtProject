using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Database;
using Firebase;
using TMPro;
using System.Linq;

public class AuthManager : MonoBehaviour
{
    //public TMP_InputField email;
    //public TMP_InputField password;
    public GameManager manager;
    FirebaseAuth auth;
    FirebaseUser newUser;
    FirebaseDatabase database;
    DatabaseReference Refrence;
    private string UserID;
    // Start is called before the first frame update
    void Start()
    {
        Refrence = FirebaseDatabase.DefaultInstance.RootReference;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Firebase.DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;
           
            }
            else
            {
                Debug.LogError("Could not resolve dependencies" + dependencyStatus);
            }
        });
        AddnewItem();
    }
    public IEnumerator  GetFavouritesList()
    {
        var GetTask = Refrence.Child("UserData").Child(UserID).Child("FavouriteShirts").GetValueAsync();
        yield return new WaitUntil(predicate: () => GetTask.IsCompleted);
        if (GetTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {GetTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = GetTask.Result;
            
            
            foreach(DataSnapshot datachild in snapshot.Children)
            {
                GameManager.Instance.PublishFavouritesList(datachild.Key);
                Debug.Log(datachild.Key);
            }
        }
    }
    public void GenerateList()
    {
        StartCoroutine(GetFavouritesList());
    }
    public void AddnewItem()
    {
        Shirts shirt = new Shirts();
     
        shirt.ColorName = "pink";
        //string jsondata = JsonUtility.ToJson(2,"CheeryRed");
        Debug.LogError("Refrence" + Refrence);
        Refrence.Child("ShirtsColor").Child("2").SetValueAsync("CherryRed");
        Refrence.Child("ShirtsColor").Child("3").SetValueAsync("Bloody Red");
        Refrence.Child("ShirtsColor").Child("4").SetValueAsync("Ruby Red");
        
        //Refrence.SetRawJsonValueAsync(jsondata).ContinueWith(task=>{
        //    if (task.IsCompleted)
        //    {
        //        Debug.LogWarning("task completed Sucessfully");
        //    }
        //}) ;

        //  newUser = new FirebaseUser("usamaaaa", "2");
    }
    public void AddtoFavourites(int Id)
    {
        Refrence.Child("UserData").Child(UserID).Child("FavouriteShirts").Child(Id.ToString()).SetValueAsync("Favourite");
    }
    public void AddUserData()
    {
        Refrence.Child("UserData").Child(newUser.UserId).Child("FavouriteShirts").SetValueAsync(" ");
        Refrence.Child("UserData").Child(newUser.UserId).Child("TotalFavouriteShirts").SetValueAsync("0");
    }
        public void SignUpClick()
    {
        StartCoroutine(OnSignUp());
    }
    public IEnumerator OnSignUp()
    {
        var taskresult = auth.CreateUserWithEmailAndPasswordAsync(manager.GetEmail(),manager.GetPassword());/*.ContinueWithOnMainThread(task =>*/
        //{
        //    if (task.IsCanceled)
        //    {
        //        Debug.LogWarning("SignUp fail");
        //        return;
        //    }
        //    if (task.IsFaulted)
        //    {
        //        Debug.LogWarning("SignUp fail because of fault");
        //        return;
        //    }
        //});
        yield return new WaitUntil(() => taskresult.IsCompleted);
        if (taskresult.Exception != null){
            FirebaseException exception = taskresult.Exception.GetBaseException() as FirebaseException;
            AuthError autherror = (AuthError)exception.ErrorCode;
            Debug.LogError("There are some exceptions in singing up"+autherror);

        }
        else
        {
          newUser= taskresult.Result.User;
            // newUser = taskresult.Result;
            AddUserData();
            manager.SucessfullySignedup();
            Debug.LogWarning("User signed Up sucessfully");
        }

    }
    public void OnLoginClicked()
    {
        StartCoroutine(LoginProcessing());
    }
    private IEnumerator LoginProcessing()
    {
        var LoginResult = auth.SignInWithEmailAndPasswordAsync(manager.GetEmail(), manager.GetPassword());
        yield return new WaitUntil(() => LoginResult.IsCompleted);
        if (LoginResult.Exception != null)
        {
            FirebaseException exception = LoginResult.Exception.GetBaseException() as FirebaseException;
            AuthError autherror = (AuthError)exception.ErrorCode;
            Debug.LogError("There are some exceptions in singing up" + autherror);
        }
        else
        {
            newUser = LoginResult.Result.User;
            UserID = newUser.UserId;
            manager.entreRoom.Invoke();
         //   StartCoroutine(GetFavouritesList());
            Debug.LogWarning("Sucessfully Login the app");
        }
    }
    public void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    void AuthStateChanged(object sender,System.EventArgs eventArgs)
    { 
        if(auth.CurrentUser!=newUser)
        {
            bool SignedIn = newUser != auth.CurrentUser && auth.CurrentUser != null;
            if (!SignedIn && newUser != null)
            {
                Debug.LogWarning("User Signed In");
            }
        }
    }
}
public class Shirts { 
   
    public string ColorName;
}

