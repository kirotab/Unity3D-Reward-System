using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private List<string> chestsToPickFrom = new List<string>{ "Bronze", "Silver", "Golden"};

    //Consts
    private static float directionTolerance = 0.04f;
    private static float speedFactor = 5f;
    private static float smoothTime = 100f;

    //Syles consts
    private static int scrWidth = Screen.width;
    private static int scrHeight = Screen.height;
    //private static float resHRatio = ((float)(Screen.width)) / Screen.height;
    //private static float resVRatio = ((float)(Screen.height)) / Screen.width;
    private int btnWidth = GUIHorCoords(25f);
    private int btnHeight = GUIVerCoords(15f);
    private int btnVerticalPadding = GUIVerCoords(4f);

    //Game Objects and styles
    public GameObject SelectedChestGameObject;
    public GameObject PlayerCamera;
    public GUISkin myGUISkin;
    public GUISkin myGUISkinShinyArrows;
    public Texture bonusBtnTexture;
    public Texture handicapBtnTexture;
    public Texture collectBtnTexture;
    public Texture leftArrowBtn;
    public Texture rightArrowBtn;

    //Fields
    private Vector3 velocity;
    private int collectedCoins;
    private BaseChest selectedChest;
    private IList<BaseChest> allChests = new List<BaseChest>();
    private int selectedChestIndex;

    //Properties
    #region Properties

    public int CollectedCoins
    {
        get { return collectedCoins; }
        set { collectedCoins = value; }
    }

    public BaseChest SelectedChest
    {
        get
        {
            //BaseChest selChest = (this.SelectedChestGameObject.gameObject.GetComponent<BaseChest>()) as BaseChest;
            //return selChest;
            return this.selectedChest;
        }
        set
        {
            //BaseChest selChest = (value.gameObject.GetComponent<BaseChest>()) as BaseChest;
            //selectedChest = selChest;
            this.selectedChest = value;
        }
    }

    public bool HasHandicap
    {
        get { return this.SelectedChest.IsHandicapApplied; }
        set { this.SelectedChest.IsHandicapApplied = value; }
    }

    public bool HasBonus
    {
        get { return this.SelectedChest.IsBonusApplied; }
        set { this.SelectedChest.IsBonusApplied = value; }
    }

    public bool CanCollect
    {
        get
        {
            bool canCollectResult = false;
            if (this.SelectedChest != null)
            {
                if (this.SelectedChest.CurrentReward > 0)
                {
                    canCollectResult = true;
                }
                Vector3 oldPosition = this.PlayerCamera.transform.position;
                Vector3 newPosition = this.SelectedChest.camPos.transform.position;
                if ((newPosition - oldPosition).x > directionTolerance || (newPosition - oldPosition).x < -directionTolerance)
                {
                    canCollectResult = false;
                }
            }
            return canCollectResult;
        }
        //set { canCollect = value; }
    }

    public string SelectedChestType
    {
        get
        {
            if (this.SelectedChest != null)
            {
                if (this.SelectedChest is GoldenChest)
                {
                    return "Golden";
                }
                else if (this.SelectedChest is SilverChest)
                {
                    return "Silver";
                }
                else if (this.SelectedChest is BronzeChest)
                {
                    return "Bronze";
                }
                else
                {
                    return "None";
                }
            }
            return "None";
        }
    }

    #endregion

    //Constructors
    public Player()
    {
        //this.SelectedChest = null;
        this.collectedCoins = 0;
    }

    //Methods
    #region Methods
    
    //GUI Layout Helpers
    static int GUIHorCoords(float percent)
    {
        return Mathf.FloorToInt(scrWidth * (percent / 100f));
    }

    static int GUIVerCoords(float percent)
    {
        return Mathf.FloorToInt(scrHeight * (percent / 100f));
    }

    //Mechanics
    public void CollectReward()
    {
        this.CollectedCoins += this.SelectedChest.CollectReward();
    }

    private void ChangeChest(int index)
    {
        this.selectedChestIndex = index;

        this.SelectedChest = this.allChests[this.selectedChestIndex];
    }

    private void GetChests()
    {
        foreach (var chestType in this.chestsToPickFrom)
        {
            var chestObj = GameObject.Find("Chest" + chestType);
            if (chestObj != null)
            {
                var chest = chestObj.gameObject.GetComponent<BaseChest>() as BaseChest;
                if (chest != null)
                {
                    this.allChests.Add(chest);
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        this.GetChests();
        if (this.allChests.Count > 0)
        {
            this.selectedChestIndex = allChests.Count / 2;
            this.ChangeChest(selectedChestIndex);
            //this.SelectedChest = this.allChests[2];
        }
        else
        {
            Debug.Log("No chests found");
        }
        //this.SelectedChestGameObject = GameObject.Find("ChestSilver");
        //this.SelectedChest = this.SelectedChestGameObject.gameObject.GetComponent<BaseChest>() as BaseChest;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 oldPosition = this.PlayerCamera.transform.position;
        Vector3 newPosition = this.SelectedChest.camPos.transform.position;
        //this.PlayerCamera.transform.position = Vector3.Lerp(newPosition, oldPosition, Time.deltaTime * 100f);
        var direction = (newPosition - oldPosition).x;
        if (direction > directionTolerance)
        {
            velocity = Vector3.right * speedFactor;
        }
        else if (direction < -directionTolerance)
        {
            velocity = Vector3.left * speedFactor;
        }
        else
        {
            velocity = Vector3.zero;
        }
        this.PlayerCamera.transform.position = Vector3.SmoothDamp(oldPosition,newPosition,ref velocity, Time.deltaTime * smoothTime);
        //Debug.Log(this.SelectedChestGameObject.GetComponent<BaseChest>());
        //Debug.Log(this.SelectedChest);
    }
    
    void OnGUI()
    {
        GUI.skin = myGUISkin;
        var labelStyle = new GUIStyle("label");
        labelStyle.alignment = TextAnchor.LowerCenter;//TextAlignment.Center;

        //Bonuses Handicaps
        this.HasHandicap = GUI.Toggle(new Rect(GUIHorCoords(2f), btnVerticalPadding, btnWidth, btnHeight),
            this.HasHandicap, handicapBtnTexture, "button"); //"Handicap"
        this.HasBonus = GUI.Toggle(new Rect(GUIHorCoords(2f), btnVerticalPadding * 2 + btnHeight, btnWidth, btnHeight),
            this.HasBonus, bonusBtnTexture, "button");//"Bonus"
        //Collected Coins
        GUI.Label(new Rect(GUIHorCoords(35f), btnVerticalPadding, GUIHorCoords(30f), btnHeight / 1.5f),
            "Collected coins: " + this.CollectedCoins.ToString(), "button");
        GUI.Label(new Rect(GUIHorCoords(35f), btnVerticalPadding * 2 + btnHeight, GUIHorCoords(30f), btnHeight / 2),
            "Coins in the chest " + this.SelectedChest.CurrentReward.ToString(), labelStyle);
        
        //Collect and Remaining
        //Debug.Log(this.SelectedChest);
        if (this.SelectedChest != null)
        {
            GUI.Label(new Rect(GUIHorCoords(74f), btnVerticalPadding * 3, btnWidth, btnHeight / 2),
                string.Format("{0:00.00}m remains", this.SelectedChest.TimeRemaining), labelStyle);

            GUI.enabled = this.CanCollect;
            if (GUI.Button(new Rect(GUIHorCoords(74f), btnVerticalPadding * 2 + btnHeight, btnWidth, btnHeight),
                collectBtnTexture))//"Collect"
            {
                this.CollectReward();
            }
            GUI.enabled = true;
            GUI.Label(new Rect(GUIHorCoords(37.5f), GUIVerCoords(70f), btnWidth, btnHeight),
               this.SelectedChest.chestNameTexture, "button"); //this.SelectedChestType
        }

        GUI.skin = myGUISkinShinyArrows;

        if (this.selectedChestIndex > 0)
        {

            if (GUI.Button(new Rect(GUIHorCoords(10f), btnVerticalPadding * 4 + btnHeight * 2, btnWidth / 2.2f, btnHeight),
                "", "leftArrow"))//this.leftArrowBtn
            {
                this.ChangeChest(this.selectedChestIndex - 1);
                //Debug.Log("Left arrow pressed");
            }
        }
        if (this.selectedChestIndex < this.allChests.Count - 1)
        {
            if (GUI.Button(new Rect(GUIHorCoords(76f), btnVerticalPadding * 4 + btnHeight * 2, btnWidth / 2.2f, btnHeight),
                "", "rightArrow")) //this.rightArrowBtn
            {
                this.ChangeChest(this.selectedChestIndex + 1);
                //Debug.Log("Right arrow pressed");
            }
        }
    }

    #endregion
}