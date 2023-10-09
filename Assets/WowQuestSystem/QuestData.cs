using System;
using Unity.VisualScripting;

[Serializable, Inspectable]
public class QuestData
{
	[Inspectable] public int questId;
	[Inspectable] public string questName;
	[Inspectable] public string questDescription;
	[Inspectable] public int itemId;
	[Inspectable] public int fulfillmentValue;
}
