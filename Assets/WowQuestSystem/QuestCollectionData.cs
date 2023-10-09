using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable, Inspectable]
public class QuestCollectionData
{
    [Inspectable] public List<QuestData> quests = new();
}
