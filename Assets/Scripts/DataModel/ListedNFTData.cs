using System;
using Unity.VisualScripting;

[Serializable, Inspectable]

public class ListedNFTData
{
    [Inspectable] public int frameId;
    [Inspectable] public NFTData nft;
}
