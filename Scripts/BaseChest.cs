using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public abstract class BaseChest : MonoBehaviour, IChest
    {
        //Game Objects
        public Texture chestNameTexture;
        public GameObject camPos;

        //Constants and static
        public static int HandicapFactorBase = 20;
        public static int BonusFactorBase = 20;

        public static int GoldenChestRewardStep = 20;
        public static int SilverChestRewardStep = 25;
        public static int BronzeChestRewardStep = 30;

        public static int GoldenChestMaxReward = 80;
        public static int SilverChestMaxReward = 75;
        public static int BronzeChestMaxReward = 60;

        public static float GoldenChestInterval = 2 ;
        public static float SilverChestInterval = 5 ;
        public static float BronzeChestInterval = 10;

        //Fields
        #region Fields

        private float timeInterval;
        private float timeRemaining;
        private int currentReward;
        private int maxReward;
        private int intervalReward;
        private bool isHandicapApplied;
        private bool isBonusApplied;
        private int handicapFactorPercent;
        private int bonusFactorPercent;

        #endregion
        
        //Properties
        #region Properties

        public float TimeInterval
        {
            get { return timeInterval; }
            set { timeInterval = value; }
        }

        public float TimeRemaining
        {
            get { return timeRemaining; }
            set { timeRemaining = value; }
        }
        
        public int CurrentReward
        {
            get { return currentReward; }
            set { currentReward = value; }
        }

        public int MaxReward
        {
            get { return maxReward; }
            set { maxReward = value; }
        }

        public int RewardStep
        {
            get { return intervalReward; }
            set { intervalReward = value; }
        }

        public bool IsHandicapApplied
        {
            get { return isHandicapApplied; }
            set { isHandicapApplied = value; }
        }

        public bool IsBonusApplied
        {
            get { return isBonusApplied; }
            set { isBonusApplied = value; }
        }

        public int HandicapFactorPercent
        {
            get { return handicapFactorPercent; }
            set { handicapFactorPercent = value; }
        }

        public int BonusFactorPercent
        {
            get { return bonusFactorPercent; }
            set { bonusFactorPercent = value; }
        }
        #endregion

        //Constructors
        public BaseChest()
        {
            this.HandicapFactorPercent = BaseChest.HandicapFactorBase;
            this.BonusFactorPercent = BaseChest.BonusFactorBase;
            this.CurrentReward = 0;
        }

        public BaseChest(float interval, int rewardStep, int maxReward )
            : this()
        {
            this.TimeInterval = interval;
            this.RewardStep = rewardStep;
            this.MaxReward = maxReward;
            this.RestartInterval();
        }

        //Methods
        #region Methods

        public int CollectReward()
        {
            int reward = this.CurrentReward;
            this.CurrentReward = 0;
            this.RestartInterval();

            int MultiplyFactor = 100;
            if (this.IsBonusApplied)
            {
                MultiplyFactor += this.BonusFactorPercent;
            }
            if (this.IsHandicapApplied)
            {
                MultiplyFactor -= this.HandicapFactorPercent;
            }

            Debug.Log(string.Format("reward:{0}| multipl:{1}", reward, MultiplyFactor / 100f));
            reward = (int)(reward * (MultiplyFactor / 100f));

            return reward;
        }

        public void RestartInterval()
        {
            this.TimeRemaining = this.TimeInterval;
        }

        public void GenerateReward()
        {
            this.CurrentReward += this.RewardStep;
        }

        public void UpdateTimer()
        {
            if (this.TimeRemaining > 0 && this.CurrentReward < this.MaxReward)
            {
                this.TimeRemaining -= Time.deltaTime;

                //debug print
                //print(this.TimeRemaining);
                //Debug.Log(this.TimeRemaining);

                if (this.TimeRemaining <= 0)
                {
                    this.GenerateReward();
                    this.RestartInterval();
                }
            }
            else
            {
                //debug print
                //print("time remaining 0 or max reward is reached");
                //Debug.Log("time remaining 0 or max reward is reached");
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("chest reward " + this.CurrentReward);
            this.UpdateTimer();
        }

        #endregion
    }
}