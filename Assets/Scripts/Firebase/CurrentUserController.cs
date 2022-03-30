using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

// this class is used as a middle man to transfer firebase user data between scenes
public class CurrentUserController : MonoBehaviour
{
    public static FirebaseUser _currentUser { get; set; }
    public static int tiles { get; set; }
    public static int kills { get; set; }
    public static int treasure { get; set; }
}
