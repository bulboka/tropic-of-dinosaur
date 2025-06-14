using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Age { Baby, Child, Adult, Fat, Old }

public class GameSession : MonoBehaviour
{
    [SerializeField] private Hand _hand;
    [SerializeField] private Body _body;
    [SerializeField] private AudioSource _music;
    [SerializeField] private StartUI _startUI;
    [SerializeField] private CameraController _camera;
    [SerializeField] private bool _useCheatStartLocator;
    [SerializeField] private Transform _cheatStartLocator;
    [SerializeField] private ChickenHeartsManager _chickenHeartsManager;
    [SerializeField] private SkullManager _skullManager;
    [SerializeField] private PauseUI _pauseUI;

    private static GameSession _instance;
    private bool _isTimeCheatActive;
    private readonly List<HandView> _usedHandViewPrefabs = new();
    private readonly List<StickyObject> _stuckObjects = new();
    private Age _age;
    private bool _isGameOver;

    public static Hand Hand => _instance._hand;

    public static Body Body => _instance._body;

    public static CameraController Camera => _instance._camera;

    public static List<HandView> UsedHandViewPrefabs => _instance._usedHandViewPrefabs;

    public static List<StickyObject> StuckObjects => _instance._stuckObjects;

    public static ChickenHeartsManager ChickenHeartsManager => _instance._chickenHeartsManager;

    public static SkullManager SkullManager => _instance._skullManager;

    public static Age Age => _instance._age;

    public static bool IsGameOver
    {
        get => _instance._isGameOver;
        set => _instance._isGameOver = value;
    }

    private void Start()
    {
        _instance = this;

#if DEV
        if (_useCheatStartLocator)
        {
            _body.transform.position = _cheatStartLocator.position;
        }
#endif

        _body.Initialize();
        _hand.IsInputEnabled = false;

#if DEV
        if (_useCheatStartLocator)
        {
            var nearestSwitchBodyTrigger =
                FindObjectsByType<SwitchBodyTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                    .Where(trigger =>
                        _cheatStartLocator.position.x - trigger.transform.position.x > 0).OrderBy(trigger =>
                        (trigger.transform.position - _cheatStartLocator.position).sqrMagnitude).FirstOrDefault();

            if (nearestSwitchBodyTrigger != null)
            {
                SwitchBody(nearestSwitchBodyTrigger.BodyPrefab, true);
            }

            var nearestSwitchHandTrigger =
                FindObjectsByType<SwitchHandViewTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                    .Where(trigger =>
                        _cheatStartLocator.position.x - trigger.transform.position.x > 0).OrderBy(trigger =>
                        (trigger.transform.position - _cheatStartLocator.position).sqrMagnitude).FirstOrDefault();

            if (nearestSwitchHandTrigger != null)
            {
                nearestSwitchHandTrigger.SwitchHandView();
            }
        }
#endif

        _startUI.Show();
        _startUI.OnComplete += OnStartUIComplete;
        _chickenHeartsManager.Initialize();

#if !UNITY_EDITOR && !UNITY_WEBGL
        Screen.SetResolution(1600, 1200, FullScreenMode.FullScreenWindow);
#endif
    }

    private void OnStartUIComplete()
    {
        _startUI.OnComplete -= OnStartUIComplete;

#if !UNITY_EDITOR && UNITY_WEBGL
        GoFullscreen();
#endif

        _hand.IsInputEnabled = true;
        _music.Play();
    }

    public static void SwitchBody(Body bodyPrefab, bool fast = false)
    {
        /*foreach (var stuckObject in StuckObjects)
        {
            stuckObject.Unstick();
        }

        StuckObjects.Clear();*/

        var newBody = Instantiate(bodyPrefab);

        _instance._age = newBody.Age;
        _instance._body.StartTransition(newBody, fast);

        // ---------------- The second, currently working variant -------------------

        /*newBody.transform.position = _instance._hand.transform.position - newBody.Hand.transform.localPosition;
        newBody.Initialize();

        _instance._camera.SetTarget(newBody.Torso.transform);

        newBody.Hand.CopyParamsFrom(_instance._hand);
        newBody.Hand.SetView(_instance._hand.View);
        _instance._hand.Dispose();
        Destroy(_instance._hand.gameObject);
        _instance._hand = newBody.Hand;

        ProcessBone(newBody.RootBone, newBody.SkeletonRoot, _instance._body.SkeletonRoot);*/

        // ---------------- The first variant, totally obsolete -------------------

        /*for (var i = 0; i < _instance._body.SkeletonRoot.childCount; i++)
        {
            var oldBodyPart = _instance._body.SkeletonRoot.GetChild(i);

            for (var j = 0; j < newBody.SkeletonRoot.childCount; j++)
            {
                var newBodyPart = newBody.SkeletonRoot.GetChild(j);

                if (newBodyPart.name == oldBodyPart.name)
                {
                    newBodyPart.position = oldBodyPart.position;
                    newBodyPart.rotation = oldBodyPart.rotation;
                    break;
                }
            }
        }*/

        /*Destroy(_instance._body.gameObject);
        _instance._body = newBody;

        _instance._body.Hand.IsInputEnabled = true;*/
    }

    private static void ProcessBone(Transform newBone, Transform newBonesContainer, Transform oldBonesContainer)
    {
        var oldBone = oldBonesContainer.Find(newBone.name);

        if (oldBone == default)
        {
            return;
        }

        newBone.rotation = oldBone.rotation;

        var newBoneBody = newBone.GetComponent<Rigidbody2D>();
        var oldBoneBody = oldBone.GetComponent<Rigidbody2D>();

        if (oldBoneBody != null && newBoneBody != null)
        {
            newBoneBody.rotation = oldBoneBody.rotation;
        }

        var joint = newBone.GetComponent<HingeJoint2D>();

        if (joint == null)
        {
            newBone.position = oldBone.position;
            newBoneBody.position = oldBoneBody.position;
        }
        else
        {
            var connectedAnchorWorld =
                joint.connectedBody.transform.TransformPoint(joint.connectedAnchor);
            var anchorWorld = newBone.TransformPoint(joint.anchor);
            newBone.position = connectedAnchorWorld/* + (newBonesContainer.position - anchorWorld)*/;
            newBoneBody.position = connectedAnchorWorld;
        }

        var hingedBones = new List<Transform>();

        for (var i = 0; i < newBonesContainer.childCount; i++)
        {
            var bone = newBonesContainer.GetChild(i);
            var boneJoint = bone.GetComponent<HingeJoint2D>();

            if (boneJoint != null && boneJoint.connectedBody.gameObject == newBone.gameObject)
            {
                ProcessBone(bone, newBonesContainer, oldBonesContainer);
            }
        }
    }

    private void Update()
    {
#if DEV
        if (Input.GetKeyDown(KeyCode.T))
        {
            _isTimeCheatActive = !_isTimeCheatActive;
            Time.timeScale = _isTimeCheatActive ? 10f : 1f;
        }
#endif

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseUI.gameObject.activeSelf)
            {
                _pauseUI.Resume();
            }
            else if (!_startUI.gameObject.activeSelf)
            {
                _pauseUI.Show();
            }
        }
    }

#if UNITY_WEBGL
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            GoFullscreen();
        }
    }
#endif

    private void GoFullscreen()
    {
        //Screen.SetResolution(1600, 1200, FullScreenMode.FullScreenWindow);
        Screen.fullScreen = true;

        if (!_startUI.gameObject.activeSelf && !_pauseUI.gameObject.activeSelf)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
