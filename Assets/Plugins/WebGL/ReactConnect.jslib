mergeInto(LibraryManager.library, {
  GetUserData: function (gameObjectName, functionName) {
    window.dispatchReactUnityEvent("GetUserData", UTF8ToString(gameObjectName), UTF8ToString(functionName));
  },

  GetUserOwnedNFTCollectionData: function (gameObjectName, functionName) {
    window.dispatchReactUnityEvent("GetUserOwnedNFT", UTF8ToString(gameObjectName), UTF8ToString(functionName));
  },

  GetListedNFTData: function (gameObjectName, functionName) {
    window.dispatchReactUnityEvent("GetListedNFT", UTF8ToString(gameObjectName), UTF8ToString(functionName));
  },

  DeListNFT: function (gameObjectName, functionName, frameID, collectionID, nftID) {
    console.log(`DeListNFT - JS Lib Log: ${UTF8ToString(gameObjectName)}, ${UTF8ToString(functionName)}, ${frameID}, ${collectionID}, ${nftID}`);
    window.dispatchReactUnityEvent("DeListNFT", UTF8ToString(gameObjectName), UTF8ToString(functionName), frameID, collectionID, nftID);
  },

  ListNFT: function (gameObjectName, functionName, frameID, collectionID, nftID, nftCost) {
    console.log(`ListNFT - JS Lib Log: ${UTF8ToString(gameObjectName)}, ${UTF8ToString(functionName)}, ${frameID}, ${collectionID}, ${nftID}, ${nftCost}`);
    window.dispatchReactUnityEvent("ListNFT", UTF8ToString(gameObjectName), UTF8ToString(functionName), frameID, collectionID, nftID, nftCost);
  },

  BuyListedNFT: function (gameObjectName, functionName, frameID, collectionID, nftID, nftCost) {
    console.log(`BuyListedNFT - JS Lib Log: ${UTF8ToString(gameObjectName)}, ${UTF8ToString(functionName)}, ${frameID}, ${collectionID}, ${nftID}, ${nftCost}`);
    window.dispatchReactUnityEvent("BuyListedNFT", UTF8ToString(gameObjectName), UTF8ToString(functionName), frameID, collectionID, nftID, nftCost);
  },

  GetQuestData: function (gameObjectName, functionName) {
    console.log("gameObjectName received is:" + UTF8ToString(gameObjectName));
    console.log("functionName received is:" + UTF8ToString(functionName));
    window.dispatchReactUnityEvent("RetrieveQuests", UTF8ToString(gameObjectName), UTF8ToString(functionName));
    console.log("Dispatched Event - GetQuestData");
  },

  CompleteQuest: function (questId, gameObjectName, functionName){
    console.log("JSLib: Attempting to call Complete quest for Quest " + questId + "with function name: " + functionName + " being called from Game Object " + gameObjectName);
    window.dispatchReactUnityEvent("CompleteQuest", questId, UTF8ToString(gameObjectName), UTF8ToString(functionName));
  },

  LinkBetweenWorlds: function(url){
    console.log("Calling Link Between Worlds to" + url);
    window.open(UTF8ToString(url),"_self");
  }
});
