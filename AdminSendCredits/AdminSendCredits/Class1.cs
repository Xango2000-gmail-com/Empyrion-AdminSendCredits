using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Eleon.Modding;
//using ProtoBuf;
using YamlDotNet.Serialization;


namespace AdminSendCredits
{
    public class MyEmpyrionMod : ModInterface
    {
        public static string ModVersion = "AdminSendCredits v0.1.0";
        public static string ModPath = "..\\Content\\Mods\\AdminSendCredits\\";
        internal static bool debug = true;
        internal static Dictionary<int, Storage.StorableData> SeqNrStorage = new Dictionary<int, Storage.StorableData> { };
        public int thisSeqNr = 2000;
        private SetupYaml.Root SetupYamlData = new SetupYaml.Root { };
        public ItemStack[] blankItemStack = new ItemStack[] { };
        private Dictionary<string, string> LastUseLog = new Dictionary<string, string> { };
        private Dictionary<int,string> Players = new Dictionary<int, string> { };

        //########################################################################################################################################################
        //################################################ This is where the actual Empyrion Modding API stuff Begins ############################################
        //########################################################################################################################################################
        public void Game_Start(ModGameAPI gameAPI)
        {
            Storage.GameAPI = gameAPI;
            if (debug) { File.WriteAllText(ModPath + "ERROR.txt", ""); }
            if (debug) { File.WriteAllText(ModPath + "debug.txt", ""); }
            SetupYamlData = SetupYaml.ReadYaml(ModPath + "Setup.yaml");
            CommonFunctions.Log("--------------------" + CommonFunctions.TimeStamp() + "----------------------------");
        }

        public void Game_Event(CmdId cmdId, ushort seqNr, object data)
        {
            try
            {
                switch (cmdId)
                {
                    case CmdId.Event_ChatMessage:
                        //Triggered when player says something in-game
                        ChatInfo Received_ChatInfo = (ChatInfo)data;
                        string msg = Received_ChatInfo.msg.ToLower();
                        if (msg == SetupYamlData.ReinitializeCommand) //Reinitialize
                        {
                            SetupYamlData = SetupYaml.ReadYaml(ModPath + "Setup.yaml");
                        }
                        else if (msg == SetupYamlData.AdminSendCreditsCommand.ToLower() + "?")
                        {
                            API.Chat("Player", Received_ChatInfo.playerId, "/asc [PlayerID] [CreditChange]");
                        }
                        else if (msg.StartsWith(SetupYamlData.AdminSendCreditsCommand.ToLower())) //Admin Send Credits command
                        {
                            try
                            {
                                Storage.StorableData function = new Storage.StorableData
                                {
                                    function = "AdminSendCredits",
                                    Match = Convert.ToString(Received_ChatInfo.playerId),
                                    Requested = "PlayerInfo",
                                    ChatInfo = Received_ChatInfo
                                };
                                thisSeqNr = API.PlayerInfo(Received_ChatInfo.playerId);
                                SeqNrStorage[thisSeqNr] = function;
                            }
                            catch
                            {
                                CommonFunctions.Debug("Fail 1");
                            }
                        }
                        else if (msg.StartsWith(SetupYamlData.FindPlayerCommand)) // Find
                        {
                            try
                            {
                                Storage.StorableData function = new Storage.StorableData
                                {
                                    function = "Find",
                                    Match = Convert.ToString(Received_ChatInfo.playerId),
                                    Requested = "PlayerInfo",
                                    ChatInfo = Received_ChatInfo
                                };
                                thisSeqNr = API.PlayerInfo(Received_ChatInfo.playerId);
                                SeqNrStorage[thisSeqNr] = function;
                            }
                            catch
                            {
                                //CommonFunctions.Debug("Fail 1");
                            }
                        }
                        else if (msg.StartsWith(SetupYamlData.PlayerLookupCommand)) // Plys Command
                        {
                            try
                            {
                                Storage.StorableData function = new Storage.StorableData
                                {
                                    function = "Plys",
                                    Match = Convert.ToString(Received_ChatInfo.playerId),
                                    Requested = "PlayerInfo",
                                    ChatInfo = Received_ChatInfo
                                };
                                thisSeqNr = API.PlayerInfo(Received_ChatInfo.playerId);
                                SeqNrStorage[thisSeqNr] = function;
                            }
                            catch
                            {
                                //CommonFunctions.Debug("Fail 1");
                            }
                        }
                        else if (msg.StartsWith(SetupYamlData.ServerSayCommand))// ServerSay Command
                        {
                            try
                            {
                                Storage.StorableData function = new Storage.StorableData
                                {
                                    function = "ServerSay",
                                    Match = Convert.ToString(Received_ChatInfo.playerId),
                                    Requested = "PlayerInfo",
                                    ChatInfo = Received_ChatInfo
                                };
                                thisSeqNr = API.PlayerInfo(Received_ChatInfo.playerId);
                                SeqNrStorage[thisSeqNr] = function;
                            }
                            catch
                            {
                                //CommonFunctions.Debug("Fail 1");
                            }
                        } 
                        else if (msg.StartsWith(SetupYamlData.AlertRedCommand)) // Alert Red Command
                        {
                            try
                            {
                                Storage.StorableData function = new Storage.StorableData
                                {
                                    function = "ServerAlertR",
                                    Match = Convert.ToString(Received_ChatInfo.playerId),
                                    Requested = "PlayerInfo",
                                    ChatInfo = Received_ChatInfo
                                };
                                thisSeqNr = API.PlayerInfo(Received_ChatInfo.playerId);
                                SeqNrStorage[thisSeqNr] = function;
                            }
                            catch
                            {
                                //CommonFunctions.Debug("ServerAlert Blue Fail");
                            }
                        }
                        else if (msg.StartsWith(SetupYamlData.AlertYellowCommand)) // Alert Yellow Command
                        {
                            try
                            {
                                Storage.StorableData function = new Storage.StorableData
                                {
                                    function = "ServerAlertY",
                                    Match = Convert.ToString(Received_ChatInfo.playerId),
                                    Requested = "PlayerInfo",
                                    ChatInfo = Received_ChatInfo
                                };
                                thisSeqNr = API.PlayerInfo(Received_ChatInfo.playerId);
                                SeqNrStorage[thisSeqNr] = function;
                            }
                            catch
                            {
                                //CommonFunctions.Debug("ServerAlert Blue Fail");
                            }
                        }
                        else if (msg.StartsWith(SetupYamlData.AlertDefaultCommand)) // Alert Default Command
                        {
                            try
                            {
                                Storage.StorableData function = new Storage.StorableData
                                {
                                    function = "ServerAlert",
                                    Match = Convert.ToString(Received_ChatInfo.playerId),
                                    Requested = "PlayerInfo",
                                    ChatInfo = Received_ChatInfo
                                };
                                thisSeqNr = API.PlayerInfo(Received_ChatInfo.playerId);
                                SeqNrStorage[thisSeqNr] = function;
                            }
                            catch
                            {
                                //CommonFunctions.Debug("ServerAlert Blue Fail");
                            }
                        }
                        break;


                    case CmdId.Event_Player_Connected:
                        //Triggered when a player logs on
                        Id Received_PlayerConnected = (Id)data;
                        Storage.StorableData thisfunction = new Storage.StorableData
                        {
                            function = "Logon",
                            Match = Convert.ToString(Received_PlayerConnected.id),
                            Requested = "PlayerInfo",
                            PlayerConnected = Received_PlayerConnected
                        };
                        thisSeqNr = API.PlayerInfo(Received_PlayerConnected.id);
                        SeqNrStorage[thisSeqNr] = thisfunction;

                        break;


                    case CmdId.Event_Player_Disconnected:
                        //Triggered when a player logs off
                        Id Received_PlayerDisconnected = (Id)data;
                        Players.Remove(Received_PlayerDisconnected.id);
                        break;


                    case CmdId.Event_Player_ChangedPlayfield:
                        //Triggered when a player changes playfield
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Player_ChangePlayfield, (ushort)CurrentSeqNr, new IdPlayfieldPositionRotation( [PlayerID], [Playfield Name], [PVector3 position], [PVector3 Rotation] ));
                        IdPlayfield Received_PlayerChangedPlayfield = (IdPlayfield)data;
                        break;


                    case CmdId.Event_Playfield_Loaded:
                        //Triggered when a player goes to a playfield that isnt currently loaded in memory
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Load_Playfield, (ushort)CurrentSeqNr, new PlayfieldLoad( [float nSecs], [string nPlayfield], [int nProcessId] ));
                        PlayfieldLoad Received_PlayfieldLoaded = (PlayfieldLoad)data;
                        break;


                    case CmdId.Event_Playfield_Unloaded:
                        //Triggered when there are no players left in a playfield
                        PlayfieldLoad Received_PlayfieldUnLoaded = (PlayfieldLoad)data;
                        break;


                    case CmdId.Event_Faction_Changed:
                        //Triggered when an Entity (player too?) changes faction
                        FactionChangeInfo Received_FactionChange = (FactionChangeInfo)data;
                        break;


                    case CmdId.Event_Statistics:
                        //Triggered on various game events like: Player Death, Entity Power on/off, Remove/Add Core
                        StatisticsParam Received_EventStatistics = (StatisticsParam)data;
                        break;


                    case CmdId.Event_Player_DisconnectedWaiting:
                        //Triggered When a player is having trouble logging into the server
                        Id Received_PlayerDisconnectedWaiting = (Id)data;
                        break;


                    case CmdId.Event_TraderNPCItemSold:
                        //Triggered when a player buys an item from a trader
                        TraderNPCItemSoldInfo Received_TraderNPCItemSold = (TraderNPCItemSoldInfo)data;
                        break;


                    case CmdId.Event_Player_List:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Player_List, (ushort)CurrentSeqNr, null));
                        IdList Received_PlayerList = (IdList)data;
                        break;


                    case CmdId.Event_Player_Info:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Player_Info, (ushort)CurrentSeqNr, new Id( [playerID] ));
                        PlayerInfo Received_PlayerInfo = (PlayerInfo)data;
                        if (SeqNrStorage.Keys.Contains(seqNr))
                        {
                            Storage.StorableData RetrievedData = SeqNrStorage[seqNr];
                            SeqNrStorage.Remove(seqNr);
                            if (RetrievedData.Requested == "PlayerInfo" && RetrievedData.function == "Logon" && Convert.ToString(Received_PlayerInfo.entityId) == RetrievedData.Match)
                            {
                                Players[Received_PlayerInfo.entityId] = Received_PlayerInfo.playerName;
                            }
                            else if (RetrievedData.Requested == "PlayerInfo" && RetrievedData.function == "AdminSendCredits" && Convert.ToString(Received_PlayerInfo.entityId) == RetrievedData.Match)
                            {
                                //CommonFunctions.Debug("Permission: " + Received_PlayerInfo.permission);
                                if (Received_PlayerInfo.permission > SetupYamlData.MinimumAdminLevel - 1)
                                {
                                    string[] msgArray = RetrievedData.ChatInfo.msg.Split(' ');
                                    if (msgArray.Count() == 3)
                                    {
                                        try
                                        {
                                            RetrievedData.function = "AdminSendCredits";
                                            RetrievedData.Match = msgArray[1];
                                            RetrievedData.Requested = "CreditQuerry1";
                                            RetrievedData.TriggerPlayer = Received_PlayerInfo;
                                            //thisSeqNr = API.Credits(Convert.ToInt32(msgArray[1]), Convert.ToInt32(msgArray[2]));
                                            thisSeqNr = API.CreditQuerry(Convert.ToInt32(msgArray[1]));
                                            SeqNrStorage[thisSeqNr] = RetrievedData;
                                        }
                                        catch
                                        {
                                            API.Chat("Player", Received_PlayerInfo.entityId, "ASC: Failed, please check your formatting.");
                                            API.Chat("Player", Received_PlayerInfo.entityId, "ASC: PlayerID= " + msgArray[1]);
                                            API.Chat("Player", Received_PlayerInfo.entityId, "ASC: CreditChange= " + msgArray[2]);
                                        }
                                    }
                                    else
                                    {
                                        API.Chat("Player", Received_PlayerInfo.entityId, "ASC: Invalid Number of Arguments");
                                    }
                                }
                            }
                            else if (RetrievedData.Requested == "PlayerInfo" && RetrievedData.function == "Plys" && Convert.ToString(Received_PlayerInfo.entityId) == RetrievedData.Match)
                            {
                                if (Received_PlayerInfo.permission > SetupYamlData.MinimumAdminLevel - 1)
                                {
                                    Dictionary<int, string> PlayersCopy = Players;
                                    API.Chat("Player", Received_PlayerInfo.entityId, "Players Online:");
                                    foreach (int player in PlayersCopy.Keys)
                                    {
                                        API.Chat("Player", Received_PlayerInfo.entityId, player + " = " + PlayersCopy[player]);
                                    }
                                }
                            }
                            else if (RetrievedData.Requested == "PlayerInfo" && RetrievedData.function == "Find" && Convert.ToString(Received_PlayerInfo.entityId) == RetrievedData.Match)
                            {
                                if (Received_PlayerInfo.permission > SetupYamlData.MinimumAdminLevel - 1)
                                {
                                    string[] msgArray = RetrievedData.ChatInfo.msg.Split(' ');
                                    
                                    RetrievedData.function = "FindConfirmed";
                                    RetrievedData.Match = msgArray[1];
                                    RetrievedData.Requested = "PlayerInfo";
                                    RetrievedData.TriggerPlayer = Received_PlayerInfo;

                                    try
                                    {
                                        thisSeqNr = API.PlayerInfo(Convert.ToInt32(msgArray[1]));
                                        SeqNrStorage[thisSeqNr] = RetrievedData;
                                    }
                                    catch
                                    {
                                        API.Chat("Player", Received_PlayerInfo.entityId, "Find: " + msgArray[1] + " is not a number");
                                    };
                                }
                            }
                            else if (RetrievedData.Requested == "PlayerInfo" && RetrievedData.function == "FindConfirmed" && Convert.ToString(Received_PlayerInfo.entityId) == RetrievedData.Match)
                            {
                                API.Chat("Player", RetrievedData.TriggerPlayer.entityId, "PlayerName: " + Received_PlayerInfo.playerName);
                                API.Chat("Player", RetrievedData.TriggerPlayer.entityId, "Playfield: " + Received_PlayerInfo.playfield);
                                API.Chat("Player", RetrievedData.TriggerPlayer.entityId, "Coordinates: " + Received_PlayerInfo.pos.x + ", " + Received_PlayerInfo.pos.y + ", " + Received_PlayerInfo.pos.z);
                            }
                            else if (RetrievedData.Requested == "PlayerInfo" && RetrievedData.function == "ServerSay" && Convert.ToString(Received_PlayerInfo.entityId) == RetrievedData.Match)
                            {
                                if (Received_PlayerInfo.permission > SetupYamlData.MinimumAdminLevel - 1)
                                {
                                    string[] msgArray = RetrievedData.ChatInfo.msg.Split(' ');
                                    string Message = CommonFunctions.ArrayToString(1, msgArray);
                                    API.Chat("Global", 1, Message);
                                    API.Alert(0, Message, "Blue", 10);
                                }
                            }
                            else if (RetrievedData.Requested == "PlayerInfo" && RetrievedData.function == "ServerAlertR" && Convert.ToString(Received_PlayerInfo.entityId) == RetrievedData.Match)
                            {
                                if (Received_PlayerInfo.permission > SetupYamlData.MinimumAdminLevel - 1)
                                {
                                    string[] msgArray = RetrievedData.ChatInfo.msg.Split(' ');
                                    string Message = CommonFunctions.ArrayToString(1, msgArray);
                                    API.Alert(0, Message, "Red", 10);
                                }
                            }
                            else if (RetrievedData.Requested == "PlayerInfo" && RetrievedData.function == "ServerAlertY" && Convert.ToString(Received_PlayerInfo.entityId) == RetrievedData.Match)
                            {
                                if (Received_PlayerInfo.permission > SetupYamlData.MinimumAdminLevel - 1)
                                {
                                    string[] msgArray = RetrievedData.ChatInfo.msg.Split(' ');
                                    string Message = CommonFunctions.ArrayToString(1, msgArray);
                                    API.Alert(0, Message, "Yellow", 10);
                                }
                            }
                            else if (RetrievedData.Requested == "PlayerInfo" && RetrievedData.function == "ServerAlert" && Convert.ToString(Received_PlayerInfo.entityId) == RetrievedData.Match)
                            {
                                if (Received_PlayerInfo.permission > SetupYamlData.MinimumAdminLevel - 1)
                                {
                                    string[] msgArray = RetrievedData.ChatInfo.msg.Split(' ');
                                    string Message = CommonFunctions.ArrayToString(1, msgArray);
                                    API.Alert(0, Message, "Blue", 10);
                                }
                            }
                        }
                        break;


                    case CmdId.Event_Player_Inventory:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Player_GetInventory, (ushort)CurrentSeqNr, new Id( [playerID] ));
                        Inventory Received_PlayerInventory = (Inventory)data;
                        break;


                    case CmdId.Event_Player_ItemExchange:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Player_ItemExchange, (ushort)CurrentSeqNr, new ItemExchangeInfo( [id], [title], [description], [buttontext], [ItemStack[]] ));
                        ItemExchangeInfo Received_ItemExchangeInfo = (ItemExchangeInfo)data;
                        break;


                    case CmdId.Event_DialogButtonIndex:
                        //All of This is a Guess
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_ShowDialog_SinglePlayer, (ushort)CurrentSeqNr, new IdMsgPrio( [int nId], [string nMsg], [byte nPrio], [float nTime] )); //for Prio: 0=Red, 1=Yellow, 2=Blue
                        //Save/Pos = 0, Close/Cancel/Neg = 1
                        IdAndIntValue Received_DialogButtonIndex = (IdAndIntValue)data;
                        break;


                    case CmdId.Event_Player_Credits:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Player_Credits, (ushort)CurrentSeqNr, new Id( [PlayerID] ));
                        IdCredits Received_PlayerCredits = (IdCredits)data;
                        if (SeqNrStorage.Keys.Contains(seqNr))
                        {
                            Storage.StorableData RetrievedData = SeqNrStorage[seqNr];
                            if (RetrievedData.Requested == "CreditQuerry1" && RetrievedData.function == "AdminSendCredits" && Convert.ToString(Received_PlayerCredits.id) == RetrievedData.Match)
                            {
                                string[] msgArray = RetrievedData.ChatInfo.msg.Split(' ');

                                RetrievedData.function = "AdminSendCredits";
                                RetrievedData.Match = Convert.ToString(Received_PlayerCredits.id);
                                RetrievedData.Requested = "CreditQuerry2";
                                RetrievedData.PlayerCredits = Received_PlayerCredits;
                                API.Credits(Convert.ToInt32(msgArray[1]), Convert.ToInt32(msgArray[2]));
                                thisSeqNr = API.CreditQuerry(Convert.ToInt32(msgArray[1]));
                                SeqNrStorage[thisSeqNr] = RetrievedData;
                            }
                            else if (RetrievedData.Requested == "CreditQuerry2" && RetrievedData.function == "AdminSendCredits" && Convert.ToString(Received_PlayerCredits.id) == RetrievedData.Match)
                            {
                                string[] msgArray = RetrievedData.ChatInfo.msg.Split(' ');
                                if (RetrievedData.PlayerCredits.credits + Convert.ToInt32(msgArray[2]) == Received_PlayerCredits.credits)
                                {
                                    SeqNrStorage.Remove(seqNr);
                                    API.Chat("Player", RetrievedData.TriggerPlayer.entityId, "ASC: Successfully sent " + Players[Convert.ToInt32(msgArray[1])] + " " + msgArray[2] + " Credits");
                                    string timestamp = CommonFunctions.TimeStamp();
                                    CommonFunctions.Log(timestamp + " " + RetrievedData.TriggerPlayer.entityId + " Sent " + msgArray[1] + " " + msgArray[2] + " Credits");
                                }
                                else
                                {
                                    CommonFunctions.Log("Credit Querry Failed, trying again");
                                    thisSeqNr = API.CreditQuerry(Convert.ToInt32(msgArray[1]));
                                    SeqNrStorage[thisSeqNr] = RetrievedData;
                                }
                            }
                        }
                        break;


                    case CmdId.Event_Player_GetAndRemoveInventory:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Player_GetAndRemoveInventory, (ushort)CurrentSeqNr, new Id( [playerID] ));
                        Inventory Received_PlayerGetRemoveInventory = (Inventory)data;
                        break;


                    case CmdId.Event_Playfield_List:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Playfield_List, (ushort)CurrentSeqNr, null));
                        PlayfieldList Received_PlayfieldList = (PlayfieldList)data;
                        break;


                    case CmdId.Event_Playfield_Stats:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Playfield_Stats, (ushort)CurrentSeqNr, new PString( [Playfield Name] ));
                        PlayfieldStats Received_PlayfieldStats = (PlayfieldStats)data;
                        break;


                    case CmdId.Event_Playfield_Entity_List:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Playfield_Entity_List, (ushort)CurrentSeqNr, new PString( [Playfield Name] ));
                        PlayfieldEntityList Received_PlayfieldEntityList = (PlayfieldEntityList)data;
                        break;


                    case CmdId.Event_Dedi_Stats:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Dedi_Stats, (ushort)CurrentSeqNr, null));
                        DediStats Received_DediStats = (DediStats)data;
                        break;


                    case CmdId.Event_GlobalStructure_List:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_GlobalStructure_List, (ushort)CurrentSeqNr, null));
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_GlobalStructure_Update, (ushort)CurrentSeqNr, new PString( [Playfield Name] ));
                        GlobalStructureList Received_GlobalStructureList = (GlobalStructureList)data;
                        //foreach (GlobalStructureInfo item in Structs.globalStructures[storedInfo[seqNr].PlayerInfo.playfield])
                        break;


                    case CmdId.Event_Entity_PosAndRot:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Entity_PosAndRot, (ushort)CurrentSeqNr, new Id( [EntityID] ));
                        IdPositionRotation Received_EntityPosRot = (IdPositionRotation)data;
                        break;


                    case CmdId.Event_Get_Factions:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Get_Factions, (ushort)CurrentSeqNr, new Id( [int] )); //Requests all factions from a certain Id onwards. If you want all factions use Id 1.
                        FactionInfoList Received_FactionInfoList = (FactionInfoList)data;
                        break;


                    case CmdId.Event_NewEntityId:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_NewEntityId, (ushort)CurrentSeqNr, null));
                        Id Request_NewEntityId = (Id)data;
                        break;


                    case CmdId.Event_Structure_BlockStatistics:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Structure_BlockStatistics, (ushort)CurrentSeqNr, new Id( [EntityID] ));
                        IdStructureBlockInfo Received_StructureBlockStatistics = (IdStructureBlockInfo)data;
                        break;


                    case CmdId.Event_AlliancesAll:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_AlliancesAll, (ushort)CurrentSeqNr, null));
                        AlliancesTable Received_AlliancesAll = (AlliancesTable)data;
                        break;


                    case CmdId.Event_AlliancesFaction:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_AlliancesFaction, (ushort)CurrentSeqNr, new AlliancesFaction( [int nFaction1Id], [int nFaction2Id], [bool nIsAllied] ));
                        AlliancesFaction Received_AlliancesFaction = (AlliancesFaction)data;
                        break;


                    case CmdId.Event_BannedPlayers:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_GetBannedPlayers, (ushort)CurrentSeqNr, null ));
                        BannedPlayerData Received_BannedPlayers = (BannedPlayerData)data;
                        break;


                    case CmdId.Event_GameEvent:
                        //Triggered by PDA Events
                        GameEventData Received_GameEvent = (GameEventData)data;
                        break;


                    case CmdId.Event_Ok:
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Player_SetInventory, (ushort)CurrentSeqNr, new Inventory(){ [changes to be made] });
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Player_AddItem, (ushort)CurrentSeqNr, new IdItemStack(){ [changes to be made] });
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Player_SetCredits, (ushort)CurrentSeqNr, new IdCredits( [PlayerID], [Double] ));
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Player_AddCredits, (ushort)CurrentSeqNr, new IdCredits( [PlayerID], [+/- Double] ));
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Blueprint_Finish, (ushort)CurrentSeqNr, new Id( [PlayerID] ));
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Blueprint_Resources, (ushort)CurrentSeqNr, new BlueprintResources( [PlayerID], [List<ItemStack>], [bool ReplaceExisting?] ));
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Entity_Teleport, (ushort)CurrentSeqNr, new IdPositionRotation( [EntityId OR PlayerID], [Pvector3 Position], [Pvector3 Rotation] ));
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Entity_ChangePlayfield , (ushort)CurrentSeqNr, new IdPlayfieldPositionRotation( [EntityId OR PlayerID], [Playfield],  [Pvector3 Position], [Pvector3 Rotation] ));
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Entity_Destroy, (ushort)CurrentSeqNr, new Id( [EntityID] ));
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Entity_Destroy2, (ushort)CurrentSeqNr, new IdPlayfield( [EntityID], [Playfield] ));
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Entity_SetName, (ushort)CurrentSeqNr, new Id( [EntityID] )); Wait, what? This one doesn't make sense. This is what the Wiki says though.
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Entity_Spawn, (ushort)CurrentSeqNr, new EntitySpawnInfo()); Doesn't make sense to me.
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_Structure_Touch, (ushort)CurrentSeqNr, new Id( [EntityID] ));
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_InGameMessage_SinglePlayer, (ushort)CurrentSeqNr, new IdMsgPrio( [int nId], [string nMsg], [byte nPrio], [float nTime] )); //for Prio: 0=Red, 1=Yellow, 2=Blue
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_InGameMessage_Faction, (ushort)CurrentSeqNr, new IdMsgPrio( [int nId], [string nMsg], [byte nPrio], [float nTime] )); //for Prio: 0=Red, 1=Yellow, 2=Blue
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_InGameMessage_AllPlayers, (ushort)CurrentSeqNr, new IdMsgPrio( [int nId], [string nMsg], [byte nPrio], [float nTime] )); //for Prio: 0=Red, 1=Yellow, 2=Blue
                        //Triggered by API mod request GameAPI.Game_Request(CmdId.Request_ConsoleCommand, (ushort)CurrentSeqNr, new PString( [Telnet Command] ));

                        //uh? Not Listed in Wiki... Received_ = ()data;
                        break;


                    case CmdId.Event_Error:
                        //Triggered when there is an error coming from the API
                        ErrorInfo Received_ErrorInfo = (ErrorInfo)data;
                        if (SeqNrStorage.Keys.Contains(seqNr))
                        {
                            CommonFunctions.LogFile("Debug.txt", "API Error:");
                            CommonFunctions.LogFile("Debug.txt", "ErrorType: " + Received_ErrorInfo.errorType);
                            CommonFunctions.LogFile("Debug.txt", "");
                        }
                        break;


                    case CmdId.Event_PdaStateChange:
                        //Triggered by PDA: chapter activated/deactivated/completed
                        PdaStateInfo Received_PdaStateChange = (PdaStateInfo)data;
                        break;


                    case CmdId.Event_ConsoleCommand:
                        //Triggered when a player uses a Console Command in-game
                        ConsoleCommandInfo Received_ConsoleCommandInfo = (ConsoleCommandInfo)data;
                        break;


                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.LogFile("ERROR.txt", "Message: " + ex.Message);
                CommonFunctions.LogFile("ERROR.txt", "Data: " + ex.Data);
                CommonFunctions.LogFile("ERROR.txt", "HelpLink: " + ex.HelpLink);
                CommonFunctions.LogFile("ERROR.txt", "InnerException: " + ex.InnerException);
                CommonFunctions.LogFile("ERROR.txt", "Source: " + ex.Source);
                CommonFunctions.LogFile("ERROR.txt", "StackTrace: " + ex.StackTrace);
                CommonFunctions.LogFile("ERROR.txt", "TargetSite: " + ex.TargetSite);
                CommonFunctions.LogFile("ERROR.txt", "");
            }
        }
        public void Game_Update()
        {
            //Triggered whenever Empyrion experiences "Downtime", roughly 75-100 times per second
        }
        public void Game_Exit()
        {
            //Triggered when the server is Shutting down. Does NOT pause the shutdown.
        }
    }
}