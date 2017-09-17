using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    static float JUMP_SPEED = 1.6f;
    public enum ColorIndex { BLUE, YELLOW, RED, GREEN};
    private ColorIndex clrIndex = ColorIndex.BLUE;
    public Sprite[] playerSprites;
    public Image playerIcon;
    float speed = 0;
    [HideInInspector]
    public Vector3 highestDelta = Vector3.zero;
    private bool gameOver = false;
    void Start()
    {
        RandomColor(false);
    }

    private void RandomColor(bool diff = true, ColorChanger.ChangerType changerType = ColorChanger.ChangerType.All)
    {
        ColorIndex lastIndex = clrIndex;
        while (true)
        {
            if (changerType == ColorChanger.ChangerType.All)
            {
                clrIndex = (ColorIndex)Random.Range(0, 4);
            }
            else if (changerType == ColorChanger.ChangerType.BlueRed)
            {
                clrIndex = Random.Range(0, 2) == 0 ? ColorIndex.BLUE : ColorIndex.RED;
            }
            else if (changerType == ColorChanger.ChangerType.GreenYellow)
            {
                clrIndex = Random.Range(0, 2) == 0 ? ColorIndex.GREEN : ColorIndex.YELLOW;
            }
            else if (changerType == ColorChanger.ChangerType.ExceptYellow)
            {
                clrIndex = (ColorIndex)Random.Range(0, 4);
                if (clrIndex == ColorIndex.YELLOW) continue;
            }
            if (clrIndex != lastIndex || !diff) break;
        }
        playerIcon.sprite = playerSprites[(int)clrIndex];
    }

    void Update()
    {
        if (gameOver || MainController.instance.pausing) return;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            Sound.instance.Play(Sound.Others.Jump);
            speed = JUMP_SPEED;
        }
        transform.position = transform.position + 4f * Vector3.up * Time.deltaTime * speed;
        if (transform.localPosition.y > 0)
        {
            highestDelta += transform.localPosition;
            transform.localPosition = Vector3.zero;
        } 
        if (transform.localPosition.y < -400 && highestDelta == Vector3.zero)
        {
            transform.localPosition = new Vector3(0, -400);
        }
    }

    void FixedUpdate()
    {
        speed -= 0.09f;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (gameOver) return;
        if ((other.tag == "Red" && clrIndex != ColorIndex.RED)
            || (other.tag == "Green" && clrIndex != ColorIndex.GREEN)
            || (other.tag == "Blue" && clrIndex != ColorIndex.BLUE)
            || (other.tag == "Yellow" && clrIndex != ColorIndex.YELLOW))
        {
            //Game Over
            Sound.instance.Play(Sound.Others.Fail);
            gameOver = true;
            GetComponent<Animator>().SetTrigger("explode");
            BarrierRegion.instance.ShowPlayerEffect(transform.position);
            Timer.Schedule(this, 1f, () =>
            {
                MainController.instance.ShowEndDialog();
            });
            
        } 
        else if (other.tag == "ColorChanger")
        {
            ColorChanger.ChangerType changerType = other.GetComponent<ColorChanger>().type;
            other.GetComponent<ColorChanger>().DestroyChanger();
            RandomColor(true, changerType);
        }
        else if (other.tag == "Star")
        {
            Sound.instance.Play(Sound.Others.Score);
            Destroy(other.gameObject);
            BarrierRegion.instance.ShowStarEffect(other.transform.localPosition, other.transform.position);
            MainController.instance.Score++;
        }
    }

    
    
}
