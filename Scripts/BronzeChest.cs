using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class BronzeChest : BaseChest
    {
        public BronzeChest()
            : base(BaseChest.BronzeChestInterval, BaseChest.BronzeChestRewardStep, BaseChest.BronzeChestMaxReward)
        {
            //this.TimeInterval = BaseChest.BronzeChestInterval;
            //this.RewardStep = BaseChest.BronzeChestRewardStep;
        }
    }
}
