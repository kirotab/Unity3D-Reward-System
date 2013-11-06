using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class SilverChest : BaseChest
    {
        public SilverChest()
            : base(BaseChest.SilverChestInterval, BaseChest.SilverChestRewardStep, BaseChest.SilverChestMaxReward)
        {
            //this.TimeInterval = BaseChest.SilverChestInterval;
            //this.RewardStep = BaseChest.SilverChestRewardStep;
        }
    }
}
