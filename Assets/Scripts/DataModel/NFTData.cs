using System;
using Unity.VisualScripting;

[Serializable, Inspectable]
public class NFTData
{
	[Inspectable] public int collectionId;
	[Inspectable] public int nftId;
	[Inspectable] public string nftName;
	[Inspectable] public string description;
	[Inspectable] public string metadata;
	[Inspectable] public ulong cost;
	[Inspectable] public Owner owner;
	[Inspectable] public Royalty royalty;
	[Inspectable] public bool equipped = false;

	public string GetFormattedCost()
	{
		string[] displayUnits = { "mS", "S", "KS", "MS", "BS", "TS" };
		var unitIndex = 0;
		var tempCost = cost;
		while (tempCost >= 1000 && unitIndex < displayUnits.Length - 1)
		{
			unitIndex++;
			tempCost /= 1000;
		}
		return tempCost + " " + displayUnits[unitIndex];
	}

	[Serializable, Inspectable]
    public class Owner
    {
		[Inspectable] public string AccountId;
    }

	[Serializable, Inspectable]
	public class Royalty
    {
		[Inspectable] public string recipient;
		[Inspectable] public float amount;
	}
}
