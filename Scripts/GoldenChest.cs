using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class GoldenChest : BaseChest
    {
        public GoldenChest()
            : base(BaseChest.GoldenChestInterval, BaseChest.GoldenChestRewardStep, BaseChest.GoldenChestMaxReward)
        {
            //this.TimeInterval = BaseChest.GoldenChestInterval;
            //this.RewardStep = BaseChest.GoldenChestRewardStep;
        }
    }
}
