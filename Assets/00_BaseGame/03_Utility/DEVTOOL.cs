
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DEVTOOL : MonoBehaviour
{
	public int amountIncreaseCoin;
	public int amountIncreaseStar;
	public int maxLevelUnlock;
	public bool isUnlimitHeart;

	[Button("Max Unlock Level", ButtonSizes.Large)]
	private void MaxUnlockLevel()
	{
		UseProfile.MaxUnlockedLevel =  maxLevelUnlock;
	}
	
	[Button("Increase Coin",ButtonSizes.Large)]
	private void IncreaseCoin()
	{
		UseProfile.Coin +=  amountIncreaseCoin;
	}
	[Button("Increase Star", ButtonSizes.Large)]
	private void IncreaseStar()
	{
		UseProfile.Star +=  amountIncreaseStar;
	}

	[Button("Reset Coin", ButtonSizes.Large)]
	private void ResetCoin()
	{
		UseProfile.Coin = 0;
	}


	[Button("ResetStar", ButtonSizes.Large)]
	private void ResetStar()
	{
		UseProfile.Star = 0;
	}

	[Button("Heart Unlimit", ButtonSizes.Large)]
	private void HeartUnlimit()
	{
		isUnlimitHeart = true;
		UseProfile.IsUnlimitedHeart = isUnlimitHeart;
	}


	[Button("Reset Day", ButtonSizes.Large)]
	private void ResetDay()
	{
		UseProfile.FirstTimeOpenGame = DateTime.Now.AddDays(-1);
	}

	[Button("Next Day", ButtonSizes.Large)]
	private void NextDay()
	{
		UseProfile.FirstTimeOpenGame = DateTime.Now.AddDays(1);
	}

	[Button("Next 2 Day", ButtonSizes.Large)]
	private void Next2Day()
	{
		UseProfile.FirstTimeOpenGame = DateTime.Now.AddDays(2);
	}

	[Button("Remove Ads", ButtonSizes.Large)]
	private void RemoveAds()
	{
		GameController.Instance.useProfile.IsRemoveAds = true;
	}

	[Button("Next Level",ButtonSizes.Large)]
	private void NextLevel()
	{
		if (UseProfile.CurrentLevel == UseProfile.MaxUnlockedLevel)
		{
			UseProfile.MaxUnlockedLevel++;
		}
		UseProfile.CurrentLevel++;
	}
}
