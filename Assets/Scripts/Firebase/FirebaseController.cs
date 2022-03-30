using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;

// this class handles the login authentication/user registeration with firebase
public class FirebaseController : MonoBehaviour
{
    // ui reference
    [Header("Main UI")]
    [SerializeField] private GameObject _mainMenu;

    // login UI
    [Header("Login UI")]
    [SerializeField] private TMP_InputField _emailLogin;
    [SerializeField] private TMP_InputField _passwordLogin;
    [SerializeField] private TMP_Text _loginError;
    [SerializeField] private TMP_Text _loginConfirmation;

    // register UI
    [Header("Register UI")]
    [SerializeField] private TMP_InputField _usernameRegister;
    [SerializeField] private TMP_InputField _emailRegister;
    [SerializeField] private TMP_InputField _passwordRegister;
    [SerializeField] private TMP_InputField _passwordConfirm;
    [SerializeField] private TMP_Text _registerError;

    // firebase
    [Header("Firebase Status")]
    [SerializeField] private DependencyStatus _dependencyStatus;
    private FirebaseAuth _authentication;
    private FirebaseUser _user;
    private DatabaseReference _db;

    //User Data variables
    [Header("PlayerData")]
    [SerializeField] private TMP_Text _username;
    [SerializeField] private TMP_Text _tiles;
    [SerializeField] private TMP_Text _kills;
    [SerializeField] private TMP_Text _treasure;
    [SerializeField] private GameObject _scoreboard;
    [SerializeField] private Transform _scoreData;

    void Awake()
    {
        // make sure that dependencies are on system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            _dependencyStatus = task.Result;
            if (_dependencyStatus == DependencyStatus.Available)
            {
                // if they are initialize
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Firebase Dependencies Error: " + _dependencyStatus);
            }
        });
    }

    // sets the authentication to the default instance
    private void InitializeFirebase()
    {
        // set authentication to firebase default
        _authentication = FirebaseAuth.DefaultInstance;

        // set database to te firebase default root
        _db = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // clears all text in the login fields
    public void ClearLoginFeilds()
    {
        _emailLogin.text = "";
        _passwordLogin.text = "";
    }

    // clears text in the register fields
    public void ClearRegisterFeilds()
    {
        _usernameRegister.text = "";
        _emailRegister.text = "";
        _passwordRegister.text = "";
        _passwordConfirm.text = "";
    }

    // called by the login button in the UI
    public void LoginButton()
    {
        // start coroutine passing in the email and password
        StartCoroutine(Login(_emailLogin.text, _passwordLogin.text));
    }

    // called by the register button in the UI
    public void RegisterButton()
    {
        // register coroutine passing the email, password, and username
        StartCoroutine(Register(_emailRegister.text, _passwordRegister.text, _usernameRegister.text));
    }

    // called by the sign out button in the UI
    public void SignOutButton()
    {
        SignOut();
        ClearRegisterFeilds();
        ClearLoginFeilds();
    }

    // signs user out and sets temp user data to null
    public void SignOut()
    {
        _authentication.SignOut();

        // clear current user scene transfering var
        CurrentUserController._currentUser = null;

        Debug.Log("Signed Out");
    }

    // saves user data to the databse
    public void SaveToDatabase(int tiles, int kills, int treasure)
    {
        StartCoroutine(UpdateTiles(tiles));
        StartCoroutine(UpdateKills(kills));
        StartCoroutine(UpdateTreasure(treasure));
    }

    // called by the scoreboard button in the UI
    public void ScoreboardButton()
    {
        StartCoroutine(LoadScoreboardData());
    }

    // sets the user to current user from the temp var that was saved between scenes
    public void SetUser(FirebaseUser inputUuser)
    {
        _user = inputUuser;

        Debug.Log("_user set to: " + CurrentUserController._currentUser.DisplayName.ToString());
    }

    public void GetDbSnapshot()
    {
        StartCoroutine(DatabaseSnapshot());
    }

    // takes in email and password to check authentication with firebase
    private IEnumerator Login(string _email, string _password)
    {
        // firebase auth signin passing in the email and password
        var LoginTask = _authentication.SignInWithEmailAndPasswordAsync(_email, _password);

        // wait until login finishes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        // if it throws an error write to debug and set error message in UI
        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Error registering task with exception: {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Invalid Email or Password";
                    break;
                case AuthError.MissingPassword:
                    message = "Invalid Email or Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Invalid Email or Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email or Password";
                    break;
                case AuthError.UserNotFound:
                    message = "User not Found";
                    break;
            }

            // set UI messages for player
            _loginConfirmation.text = "";
            _loginError.text = message;
        }
        else
        {
            // get user login result
            _user = LoginTask.Result;
            
            // show user info in debug
            Debug.LogFormat("User signed in successfully: {0} ({1})", _user.DisplayName, _user.Email);

            // set UI messages for player
            _loginError.text = "";
            _loginConfirmation.text = "Logged In";

            // load user data to fill out player score stats
            StartCoroutine(LoadUserData());

            // after successful login wait 2 seconds then switch to
            // player data menu
            yield return new WaitForSeconds(2);

            // set username field to retrieved username
            _username.text = _user.DisplayName;
            
            // activate to player data menu
            _mainMenu.GetComponent<MainMenuController>().PlayerDataUI();

            _loginConfirmation.text = "";

            // clear input fields
            ClearLoginFeilds();
            ClearRegisterFeilds();

            CurrentUserController._currentUser = _user;
            Debug.Log("_currentUser: " + CurrentUserController._currentUser.DisplayName.ToString());
        }
    }

    // takes in user information to create firebase user profile
    private IEnumerator Register(string _email, string _password, string _username)
    {
        // if username is empty update error text
        if (_username == "")
        {
            _registerError.text = "Enter Username";
        }
        // if the password doesn't match confirm update error text
        else if (_passwordRegister.text != _passwordConfirm.text)
        {
            _registerError.text = "Password and Confirmation Do Not Match!";
        }
        // if everything looks good call firebase authentication
        else
        {
            // firebase authentication signin passing in email and password
            var RegisterTask = _authentication.CreateUserWithEmailAndPasswordAsync(_email, _password);

            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            // if it throws an error write to debug and set error message in UI
            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Error registering task with exception: {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }

                // set error message in the UI for the player
                _registerError.text = message;
            }
            else
            {
                // get created user result
                _user = RegisterTask.Result;

                if (_user != null)
                {
                    // create a user profile with username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    // firebase authentication updates user profile passing the profile with the username
                    var ProfileTask = _user.UpdateUserProfileAsync(profile);

                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    // add username to database
                    StartCoroutine(UpdateUsernameDatabase(_username));

                    // add 0 totals to new user's DB
                    StartCoroutine(UpdateTiles(0));
                    StartCoroutine(UpdateKills(0));
                    StartCoroutine(UpdateTreasure(0));


                    // if it throws an error write to debug and set error message in UI
                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Error registering task with exception: {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                        // update error text in UI for player
                        _registerError.text = "Set Username Failed!";
                    }
                    else
                    {
                        // return to survival login menu
                        _mainMenu.GetComponent<MainMenuController>().SurvivalLoginUI();

                        // reset error message in UI
                        _registerError.text = "";

                        // clear input fields
                        ClearLoginFeilds();
                        ClearRegisterFeilds();
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        // make profile and save the user name for it
        UserProfile profile = new UserProfile { DisplayName = _username };

        // update user profile in firebase
        var ProfileTask = _user.UpdateUserProfileAsync(profile);

        // wait until task finishes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Error registering task with exception: {ProfileTask.Exception}");
        }
        else
        {
            // send debug message
            Debug.Log("Auth Username Updated.");
        }
    }

    // updates the username in the firebase database
    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        // set logged in user name in the database
        var DBTask = _db.Child("users").Child(_user.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Error registering task with exception: {DBTask.Exception}");
        }
        else
        {
            // send debug message
            Debug.Log("Databse Username Updated.");
        }
    }

    // updates the player's tile count in the database
    private IEnumerator UpdateTiles(int _tiles)
    {
        // save logged in user's tiles traveled in database
        var DBTask = _db.Child("users").Child(_user.UserId).Child("tiles").SetValueAsync(_tiles);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Error registering task with exception: {DBTask.Exception}");
        }
        else
        {
            // send debug message
            Debug.Log("Databse Tiles Updated.");
        }
    }

    // updates the player's kill count in the database
    private IEnumerator UpdateKills(int _kills)
    {
        // save logged in user's kills in database
        var DBTask = _db.Child("users").Child(_user.UserId).Child("kills").SetValueAsync(_kills);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Error registering task with exception: {DBTask.Exception}");
        }
        else
        {
            // send debug message
            Debug.Log("Databse Kills Updated.");
        }
    }

    // updates the player's treasure count in the database
    private IEnumerator UpdateTreasure(int _treasure)
    {
        // save logged in user's treasure in database
        var DBTask = _db.Child("users").Child(_user.UserId).Child("treasure").SetValueAsync(_treasure);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Error registering task with exception: {DBTask.Exception}");
        }
        else
        {
            // send debug message
            Debug.Log("Databse Treasure Updated.");
        }
    }

    // loads player data from the db then populates it on player screen
    private IEnumerator LoadUserData()
    {
        // get data from the logged in player
        var DBTask = _db.Child("users").Child(_user.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Error registering task with exception: {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            // no current data so set UI to 0's
            _tiles.text = "0";
            _kills.text = "0";
            _treasure.text = "0";
        }
        else
        {
            // save snapshot of retrieved data
            DataSnapshot snapshot = DBTask.Result;

            _tiles.text = snapshot.Child("tiles").Value.ToString();
            _kills.text = snapshot.Child("kills").Value.ToString();
            _treasure.text = snapshot.Child("treasure").Value.ToString();
        }
    }

    private IEnumerator LoadScoreboardData()
    {
        // get data and sort by num tiles
        var DBTask = _db.Child("users").OrderByChild("tiles").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Error registering task with exception: {DBTask.Exception}");
        }
        else
        {
            // create snapshot of retrieved data
            DataSnapshot snapshot = DBTask.Result;

            // destroy all scoreboard highscore child objects
            foreach (Transform child in _scoreData.transform)
            {
                Destroy(child.gameObject);
            }

            // loop through all users, get user data, and send it to create new high score objects
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("username").Value.ToString();
                int tiles = int.Parse(childSnapshot.Child("tiles").Value.ToString());
                int kills = int.Parse(childSnapshot.Child("kills").Value.ToString());
                int treasure = int.Parse(childSnapshot.Child("treasure").Value.ToString());

                // instantiate new scoreboard objects
                GameObject scoreboardObj = Instantiate(_scoreboard, _scoreData);
                scoreboardObj.GetComponent<HighScore>().NewScoreElement(username, tiles, kills, treasure);
            }
        }
    }

    // takes a snapshot of the current user database and stores the data in the
    // current user controller vars for between scene storage and usage
    private IEnumerator DatabaseSnapshot()
    {
        // get data from the logged in player
        var DBTask = _db.Child("users").Child(_user.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Error registering task with exception: {DBTask.Exception}");
        }
        else
        {
            // save snapshot of retrieved data
            DataSnapshot snapshot = DBTask.Result;

            //tiles = int.Parse(snapshot.Child("tiles").Value.ToString());
            //kills = int.Parse(snapshot.Child("kills").Value.ToString());
            //treasure = int.Parse(snapshot.Child("treasure").Value.ToString());

            CurrentUserController.tiles = int.Parse(snapshot.Child("tiles").Value.ToString());
            CurrentUserController.kills = int.Parse(snapshot.Child("kills").Value.ToString());
            CurrentUserController.treasure = int.Parse(snapshot.Child("treasure").Value.ToString());
        }
    }
}
