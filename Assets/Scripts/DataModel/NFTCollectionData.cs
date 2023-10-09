using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable, Inspectable]
public class NFTCollectionData
{
    [Inspectable] public List<NFTData> nfts;
}
