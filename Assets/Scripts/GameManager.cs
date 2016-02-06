using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static GameObject staticDesk;
    public static List<Order> completedOrders = new List<Order>();

    //public Objects
    public GameObject desk;
    public GameObject resultDisplay;
    public GameObject timeDisplay;

    //timer variables in seconds
    public float roundDuration = 300;
    public float countDownDuration = 3;

    private TextMesh textTimeCaption, textTimeCount, textOrdersCompleted;

    private float roundTimer;

    private bool endGameFlag;
    private bool isCountingDown;
    private bool isPaused;

    void Start()
    {
        Creature.LoadCreatures();
        staticDesk = desk;

        textTimeCaption = timeDisplay.transform.FindChild("Caption").GetComponent<TextMesh>();
        textTimeCount = timeDisplay.transform.FindChild("Count").GetComponent<TextMesh>();
        textOrdersCompleted = resultDisplay.transform.FindChild("Orders Completed").GetComponent<TextMesh>();

        this.LoadGame();
    }

    public static void SetDeskObjectsActive(bool active)
    {
        staticDesk.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCountingDown)
            return;

        // TODO
        // Add check for if in ritual or not before doing this
        if (Input.GetKeyDown(KeyCode.Escape)) { }
            //TogglePause();

        if (isPaused)
        {
            return;
        }

        if (endGameFlag)
        {
            if (Input.GetKeyDown(KeyCode.R))
                LoadGame();
            return;
        }

        roundTimer += Time.deltaTime;
        textTimeCount.text = string.Format("{0:0.0}s", roundTimer);
        
        if (roundTimer >= roundDuration)
            EndGame();
    }

    void TogglePause() {
        isPaused = !isPaused;
    }

    void EndGame()
    {
        Ritual.CloseCurrentRitual();
        endGameFlag = true;
        SetDeskObjectsActive(false);
        timeDisplay.SetActive(false);
        resultDisplay.SetActive(true);

        textOrdersCompleted.text = completedOrders.Count.ToString();
    }

    private IEnumerator TextFade(TextMesh textMesh, float fadeDuration)
    {
        float fadeTime = fadeDuration;
        while (fadeTime > 0)
        {
            //get the current color
            Color color = textMesh.color;

            //set the alpha and color
            color.a = fadeDuration / fadeTime; ;
            textMesh.color = color;

            //add time passed so far
            fadeTime -= Time.deltaTime;
            yield return null;
        }
        textMesh.text = "";
    }

    private IEnumerator CountDown()
    {
        float countDownTimer = countDownDuration;
        textTimeCaption.text = "Starting in: ";

        textTimeCount.text = string.Format("{0:0.0}s", countDownTimer);
        yield return null;

        while (countDownTimer > 0)
        {
            countDownTimer -= Time.deltaTime;
            textTimeCount.text = string.Format("{0:0.0}s", countDownTimer);            

            yield return null;
        }

        //game starts, enable actions and set timer to 0
        SetDeskObjectsActive(true);
        isCountingDown = false;
        this.roundTimer = 0;
        textTimeCaption.text = "Time Elapsed: ";
    }

    public static void OrderCompleted(Order completedOrder)
    {
        //update order variables
        completedOrders.Add(completedOrder);

        //generate a new order
        Order.GenerateOrder();
        Ritual.CloseCurrentRitual();
    }

    public void LoadGame()
    {
        //disable actions
        GameManager.SetDeskObjectsActive(false);
        resultDisplay.SetActive(false);

        //reset variables
        endGameFlag = false;
        isCountingDown = true;
        completedOrders = new List<Order>();

        timeDisplay.SetActive(true);

        Order.GenerateOrder();

        //start game countdown
        StartCoroutine(CountDown());
    }
}
