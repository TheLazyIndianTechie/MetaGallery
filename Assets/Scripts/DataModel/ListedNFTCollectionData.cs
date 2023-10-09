using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable, Inspectable]
public class ListedNFTCollectionData
{
    [Inspectable] public List<ListedNFTData> listedNfts;
}