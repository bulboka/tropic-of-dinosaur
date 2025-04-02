using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private Hand _hand;
    [SerializeField] private Body _body;
    [SerializeField] private AudioSource _music;
    [SerializeField] private StartUI _startUI;
    [SerializeField] private CameraController _camera;

    private static GameSession _instance;

    public static Hand Hand => _instance._hand;

    public static Body Body => _instance._body;

    public static CameraController Camera => _instance._camera;

    private void Start()
    {
        _instance = this;

        _body.Initialize();
        _hand.IsInputEnabled = false;
        _startUI.Show();
        _startUI.OnComplete += OnStartUIComplete;
    }

    private void OnStartUIComplete()
    {
        _startUI.OnComplete -= OnStartUIComplete;
        _hand.IsInputEnabled = true;
        _music.Play();
    }

    public static void SwitchBody(Body bodyPrefab)
    {
        var newBody = Instantiate(bodyPrefab);

        newBody.transform.position = _instance._hand.transform.position - newBody.Hand.transform.localPosition;
        newBody.Initialize();

        _instance._camera.SetTarget(newBody.Torso);

        newBody.Hand.SetView(_instance._hand.View);
        _instance._hand.Dispose();
        Destroy(_instance._hand.gameObject);
        _instance._hand = newBody.Hand;

        for (var i = 0; i < _instance._body.SkeletonRoot.childCount; i++)
        {
            var oldBodyPart = _instance._body.SkeletonRoot.GetChild(i);

            for (var j = 0; j < newBody.SkeletonRoot.childCount; j++)
            {
                var newBodyPart = newBody.SkeletonRoot.GetChild(j);

                if (newBodyPart.name == oldBodyPart.name)
                {
                    //newBodyPart.position = oldBodyPart.position;
                    newBodyPart.rotation = oldBodyPart.rotation;
                    break;
                }
            }
        }

        Destroy(_instance._body.gameObject);
        _instance._body = newBody;

        _instance._body.Hand.IsInputEnabled = true;
    }
}
