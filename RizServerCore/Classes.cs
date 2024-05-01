using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RizServerCoreSharp
{
    public class Classes
    {
        public static Osp_DB.DBMain DBMain = new Osp_DB.DBMain();
        public static Tools.TokenGenerator TokenGenerator = new Tools.TokenGenerator();
        public static GlobalConfig.DynamicConfigTemplate LoadedConfig = new GlobalConfig.DynamicConfigTemplate();
        public static Random RandomObject = new Random();
        public static Classes.RizAccount RizAccountBase = JsonConvert.DeserializeObject<Classes.RizAccount>(File.ReadAllText(Classes.LoadedConfig.resources_path + "/RizAccountBase.json"));

        public class RhythAccountCheckEmailRequest
        {
            public string email;
        }

        public class RhythAccountCheckEmailResponse
        {
            public int code;
            public string msg;
        }

        public class RhythAccountLoginRequest
        {
            public string email;
            public string password;
        }

        public class RhythAccountInsensitiveLoginRequest
        {
            public string email;
            public string token;
        }

        public class RhythAccountRegisterRequest
        {
            public string email;
            public string password;
            public string code;
        }

        public class RhythAccountSendEmailRequest
        {
            public string email;
            public string transaction;
        }

        public class RhythAccountResponseWithToken
        {
            public string ret;
            public string header_set_token;
        }

        public class RhythAccountAvatar
        {
            public string name = "testAvatar";
            public string portrait = "";
        }

        public class RhythAccount
        {
            public string email;
            public string password;
            public RhythAccountAvatar avatar;
        }

        public class RizAccountMyBest
        {
            public string trackAssetId;
            public string difficultyClassName;
            public int score;
            public double completeRate;
            public bool isFullCombo;
            public bool isClear;
        }

        public class RizAccountItems
        {
            public int amount;
            public string itemAssetId;
        }

        public class RizAccountOwnProduct
        {
            public int goodId;
            public int purchaseCount;
        }

        public class RizAccountProductCost
        {
            public string type;
            public int amount;
        }

        public class RizAccountProduct
        {
            public int id;
            public string content;
            public List<RizAccountProductCost> costs;
            public int onSalePercent;
            public int getLimit;
            public string preTask;
        }

        public class RizAccountRizcard
        {
            public RizAccountAvatarPos avatarPos;
            public string avatarId;
            public string bioId1;
            public string bioId2;
            public string backgroundId;
            public string layoutId;
            public DateTime createTime;
        }

        public class RizAccountAvatarPos
        {
            public double x;
            public double y;
            public double z;
        }

        public class RizAccountEvent
        {
            public int eventId;
            public List<RizAccountProduct> goods;
        }

        public class RizAccountOwnAchievement
        {
            public string achievementId;
            public DateTime getTime;
        }

        public class RizMailInfo
        {
            public string mailId;
            public string title;
            public string content;
            public DateTime receivedTime;
            public DateTime expiredTime;
            public bool read;
            public bool deleted;
            public List<RizMailAttachmentInfo> attachments;
        }

        public class RizMailAttachmentInfo
        {
            public string attachmentId;
            public string attachmentName;
            public int attachmentSize;
        }

        public class RizAccountOwnRizCards
        {
            public List<RizAccountRizcard> rizcards;
            public List<RizAccountRizcard> staticRizcards;
            public long readed;
        }

        public class RizAccount
        {
            public string _id;
            public string username;
            public int coin;
            public int dot;
            public RizAccountRizcard rizcard;
            public List<RizAccountEvent> getProducts;
            public List<RizAccountOwnProduct> getOwnProducts;
            public List<RizAccountItems> getItems;
            public List<RizAccountMyBest> myBest;
            public List<string> unlockedLevels;
            public List<string> appearLevels;
            public List<RizAccountOwnAchievement> getOwnAchievements;
            public RizAccountOwnRizCards ownRizcards;
            public List<RizAccountRizcard> staticRizcards;
            public List<RizMailInfo> mails;
            public long mailSyncId;
        }

        public class ReRizAfterPlayRequest
        {
            [JsonPropertyName("trackAssetId")]
            public string TrackAssetId { get; set; }

            [JsonPropertyName("difficultyClassName")]
            public string DifficultyClassName { get; set; }

            [JsonPropertyName("score")]
            public int Score { get; set; }

            [JsonPropertyName("completeRate")]
            public double CompleteRate { get; set; }

            [JsonPropertyName("maxPerfect")]
            public int MaxPerfect { get; set; }

            [JsonPropertyName("perfect")]
            public int Perfect { get; set; }

            [JsonPropertyName("miss")]
            public int Miss { get; set; }

            [JsonPropertyName("bad")]
            public int Bad { get; set; }

            [JsonPropertyName("early")]
            public int Early { get; set; }

            [JsonPropertyName("late")]
            public int Late { get; set; }

            [JsonPropertyName("comboScore")]
            public int ComboScore { get; set; }

            [JsonPropertyName("leftHp")]
            public double LeftHp { get; set; }
        }

        public class RizPurchaseRequest
        {
            public int goodId;
        }


        public class RizPurchaseResultInfo
        {
            // Token: 0x06000248 RID: 584 RVA: 0x00002050 File Offset: 0x00000250

            // Token: 0x04000256 RID: 598
            public List<RizNewLevelInfo> newLevels;

            // Token: 0x04000257 RID: 599
            public List<RizAccountItems> newItems;

            // Token: 0x04000258 RID: 600
            public int newDots;

            // Token: 0x04000259 RID: 601
            public int newCoins;
        }

        public class RizNewLevelInfo
        {
            // Token: 0x06000247 RID: 583 RVA: 0x00002050 File Offset: 0x00000250

            // Token: 0x04000254 RID: 596
            public string trackAssetId;

            // Token: 0x04000255 RID: 597
            public string level; //我不到啊 我也不知道这是啥
        }

        public class RizApiChangeUsernameRequest
        {
            public string username;
        }



        public class ReRizReturnEncryptResponseWithSign
        {
            public string ResponseBody;
            public string ResponseHeaderSign;
        }
    }
}