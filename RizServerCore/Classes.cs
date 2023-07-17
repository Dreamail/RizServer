using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp
{
    public class Classes
    {
        public static Osp_DB.DBMain DBMain = new Osp_DB.DBMain();
        public static Tools.TokenGenerator TokenGenerator = new Tools.TokenGenerator();
        public static GlobalConfig.DynamicConfigTemplate LoadedConfig = new GlobalConfig.DynamicConfigTemplate();

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


        
        public class RizAccountEncryptResponseWithSign
        {
            public string ResponseBody;
            public string ResponseHeaderSign;
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
    }
}