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
            public int amount = 1;
            public string itemAssetId = "event.startEvent";
        }

        public class RizAccountOwnProduct
        {
            public int goodId = 0;
            public int purchaseCount = 1;
        }

        public class RizAccountProductAsset
        {
            public int amount = 1;
            public string type = "item";
            public string assetId = "event.startEvent";
        }

        public class RizAccountProductCost
        {
            public string type = "dot";
            public int amount = 1;
        }

        public class RizAccountProduct
        {
            public int id = 0;
            public List<RizAccountProductCost> costs = new List<RizAccountProductCost>();
            public int onSalePercent = 1;
            public List<RizAccountProductAsset> assets = new List<RizAccountProductAsset>();
            public int getLimit = 1;
            public string conditionType = "or";
            public List<int> preTask = new List<int> {};
        }

        public class RizAccount
        {
            public string _id = "";//某游戏公司直接把MongoDB里的整个Document转Json返回了，令人感慨，这是MongoDB的Object Id
            public string username = "";//事实上目前国际服设置页面里的名称直接用邮箱代替了，有点牛逼的哦
            public int coin = 114514;//对私服而言这个东西没什么鸟用 呃 难道你想盈利？
            public int dot = 1919810;//对私服而言这个东西也没有鸟用
            public string lastMadeCardId = null;//明显换卡系统砍剩下的东西，既然提到了Id，那我就先给安个string吧
            public List<RizAccountProduct> getProducts = new List<RizAccountProduct>();//可购买的内容
            public List<RizAccountOwnProduct> getOwnProducts = new List<RizAccountOwnProduct> { };//已购买内容
            public List<RizAccountItems> getItems = new List<RizAccountItems> { };//拥有的物品，我认为这个东西可能类似“背包”
            public List<RizAccountMyBest> myBest = new List<RizAccountMyBest>();//存成绩的，至于决定最终显示列表中的哪个，这是客户端干的事
            public List<String> unlockedLevels = new List<String>();//存放已解锁歌曲的地方
            public List<String> appearLevels = new List<String>();//存放显示的歌曲的地方
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





        public class ReRizReturnEncryptResponseWithSign
        {
            public string ResponseBody;
            public string ResponseHeaderSign;
        }
    }
}