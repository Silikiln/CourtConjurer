using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Handles the main game logic for timing and rounds
/// </summary>
public class GameManager : MonoBehaviour {
    /// <summary>
    /// The GameObject that represents the entirety of the desk
    /// </summary>
    public static GameObject desk;
    /// <summary>
    /// All of the orders the player has completed for this round
    /// </summary>
    public static List<Order> completedOrders = new List<Order>();
    public static BookMarkedCreature bookedCreature = new BookMarkedCreature();
    /// <summary>
    /// Complete an order
    /// </summary>
    /// <param name="completedOrder">The order that was completed</param>
    public static void CompleteOrder(Order completedOrder)
    {
        completedOrders.Add(completedOrder);

        // Generate a new order
        Order.GenerateOrder();

        // Close the current ritual (ViewOrder)
        Ritual.CloseCurrentRitual();
    }

    // Parent GameObjects
    public GameObject bookMarkedCreaturePanel;
    public GameObject assignedDesk;
    public GameObject resultDisplay;
    public GameObject timeDisplay;

    // Timer variables in seconds
    public float countDownDuration = 3;
    public float roundDuration = 300;
    private float roundTimer;

    // TextMesh variables for displaying information
    private TextMesh textTimeCaption, textTimeCount, textOrdersCompleted;

    // Flags for current game state
    private bool isRoundOver;
    private bool isCountingDown;
    private bool isPaused;

    // Initialize the manager
    void Start()
    {
        // Load the creatures from external XML file
        Creature.LoadCreatures();

        // Set the desk object as a static object so it can be hidden globally
        desk = assignedDesk;

        //set panel
        bookedCreature.SetPanel(bookMarkedCreaturePanel);

        // Assign the TextMesh variables from the parents
        textTimeCaption = timeDisplay.transform.FindChild("Caption").GetComponent<TextMesh>();
        textTimeCount = timeDisplay.transform.FindChild("Count").GetComponent<TextMesh>();
        textOrdersCompleted = resultDisplay.transform.FindChild("Orders Completed").GetComponent<TextMesh>();

        LoadGame();
    }

    

    // Update is called once per frame
    void Update()
    {
        // Nothing to do if counting down
        if (isCountingDown)
            return;

        // Check for pause
        // TODO
        // Add check for if in ritual or not before doing this
        if (Input.GetKeyDown(KeyCode.Escape)) { }
            //TogglePause();

        // Actions for when the game is paused
        if (isPaused)
        {
            return;
        }

        // Actions for when the game is in the result screen
        if (isRoundOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
                LoadGame();
            return;
        }

        // Update the time spent in this round
        roundTimer += Time.deltaTime;
        textTimeCount.text = string.Format("{0:0.0}s", roundTimer);
        
        // Check if round has ended
        if (roundTimer >= roundDuration)
            EndGame();
    }

    /// <summary>
    /// Logic for each time the player enters or exits the pause screen
    /// </summary>
    void TogglePause() {
        isPaused = !isPaused;
    }

    /// <summary>
    /// Logic for when a round ends
    /// </summary>
    void EndGame()
    {
        Ritual.CloseCurrentRitual();
        desk.SetActive(false);
        timeDisplay.SetActive(false);

        isRoundOver = true;
        textOrdersCompleted.text = completedOrders.Count.ToString();
        resultDisplay.SetActive(true);
    }

    /// <summary>
    /// Fade text out
    /// </summary>
    /// <param name="textMesh">The text mesh to fade</param>
    /// <param name="fadeDuration">The duration of the fade</param>
    /// <returns></returns>
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

    /// <summary>
    /// Coroutine to start the countdown
    /// </summary>
    private IEnumerator CountDown()
    {
        isCountingDown = true;
        float countDownTimer = countDownDuration;

        textTimeCaption.text = "Starting in: ";
        textTimeCount.text = string.Format("{0:0.0}s", countDownTimer);
        timeDisplay.SetActive(true);
        yield return null;

        while (countDownTimer > 0)
        {
            countDownTimer -= Time.deltaTime;
            textTimeCount.text = string.Format("{0:0.0}s", countDownTimer);            

            yield return null;
        }

        // Game starting after this, initialize variables and reset timer
        GameManager.desk.SetActive(true);
        isCountingDown = false;
        this.roundTimer = 0;
        textTimeCaption.text = "Time Elapsed: ";
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void LoadGame()
    {
        // Disable desk and result displays
        GameManager.desk.SetActive(false);
        resultDisplay.SetActive(false);

        // Reset variables
        isRoundOver = false;
        completedOrders = new List<Order>();
        Order.GenerateOrder();

        StartCoroutine(CountDown());
    }

    public class BookMarkedCreature
    {
        private GameObject panel;
        private Creature currentBookmarkedCreature;
        private TextMesh creatureTitle, creatureType;
        private SpriteRenderer creatureImage;
        public void SetPanel(GameObject givenPanel)
        {
            this.panel = givenPanel;

            //grab the panel parts
            creatureImage = panel.transform.FindChild("CreatureImage").GetComponent<SpriteRenderer>();
            creatureTitle = panel.transform.FindChild("CreatureTitle").GetComponent<TextMesh>();
            creatureType = panel.transform.FindChild("CreatureType").GetComponent<TextMesh>();
        }

        public void NewBookedCreature(Creature newBookmarkedCreature)
        {
            currentBookmarkedCreature = newBookmarkedCreature;
            UpdatePanelDisplay();
        }

        private void UpdatePanelDisplay()
        {
            //set the panel parts based on the currentBookmarkedCreature
            creatureTitle.text = currentBookmarkedCreature.Title;
            creatureType.text = currentBookmarkedCreature.Type;
            creatureImage.sprite = currentBookmarkedCreature.FetchCreatureSprite();
            ShowPanel();
        }

        private void HidePanel()
        {
            foreach (Transform t in panel.transform)
                t.gameObject.SetActive(false);
        }
        private void ShowPanel()
        {
            foreach (Transform t in panel.transform)
                t.gameObject.SetActive(true);
        }
        public Creature getCreature()
        {
            return this.currentBookmarkedCreature;
        }
    }
}
