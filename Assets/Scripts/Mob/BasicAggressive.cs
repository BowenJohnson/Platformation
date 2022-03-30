//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BasicAggressive : BasicMob
//{
//    // *****
//    // Aggressive just wanders back and forth on its 
//    // platform when it reaches the edge it turns around
//    // if it sees the player it will charge and try to attack
//    // *****

//    // states for animation state machine
//    private enum MoveState { idle, walk, charge, attack };
//    private MoveState _currState;

//    // *****
//    // going to have similar characteristics as wanderer
//    // just need to add widget for a line of sight ray cast
//    // might be interesting to add a timer for the walk so 
//    // the mob will turn around at random times as well as
//    // the edge

//    // can use a ray for line of sight as it will change dir
//    // but use a circle collider for chasing

//    // basic mob base stats and extra stats for aggressive
//    private Animator _animator;
//    public Rigidbody2D _rigbody;
//    private RaycastHit2D _groundInfo;
//    private bool _movingRight = false;
//    private bool _walking = false;
//    private bool _falling;
//    [SerializeField] private ParticleSystem _particleSystem;
//    [SerializeField] private float _rayDistance;
//    public Transform groundDetection;

//    void Start()
//    {
//        _rigbody = GetComponent<Rigidbody2D>();
//        _animator = GetComponent<Animator>();
//        _currentHp = 1;
//        _maxHp = 1;
//        _basicDmg = 2;
//        _speed = 1;
//        _vertKnockbackForce = 2;
//        _horizKnockbackForce = 2;
//        _normalColor = Color.white;
//        _hurtColor = Color.red;
//        _rayDistance = 1;
//        _currState = MoveState.walk;
//        _falling = false;
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
