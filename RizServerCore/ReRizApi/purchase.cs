using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RizServerCoreSharp.Classes;

namespace RizServerCoreSharp.ReRizApi
{
    public static class Purchase
    {
        private static List<RizNewLevelInfo> new_tracks = new List<RizNewLevelInfo>();
        private static List<RizAccountItems> new_items = new List<RizAccountItems>();

        public static ReRizReturnEncryptResponseWithSign PurchaseMain(string header_token, string requestbody)
        {
            RizPurchaseRequest req = JsonConvert.DeserializeObject<RizPurchaseRequest>(requestbody);

            string token_email = Classes.TokenGenerator.CheckToken(header_token);
            if (token_email == null)
            {
                return Tools.ReRizTools.BuildEncryptMessage("token_error");
            }

            Osp_DB.SearchFilter SearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_RizUserAccountObject", null, null);
            var SearchResult = Classes.DBMain.SearchWithFilter(GlobalConfig.DBConfig.JsonName, SearchFilter);
            foreach (var item in SearchResult)
            {
                var itemobj = JsonConvert.DeserializeObject<Classes.RizAccount>(item.obj.ToString());
                if (itemobj.username == token_email)
                {
                    var can_purchase = false;
                    RizAccountProduct target_product;
                    int target_need_dot = 0;
                    int target_need_coin = 0;

                    foreach (var product in Classes.RizAccountBase.getProducts)
                    {
                        if(product.id == req.goodId)
                        {
                            target_product = product;
                            foreach (var cost in product.costs)
                            {
                                if(cost.type == "dot")
                                {
                                    if (itemobj.dot >= cost.amount)
                                    {
                                        can_purchase = true;
                                        target_need_dot = cost.amount;
                                    }
                                }
                                else if(cost.type == "coin")
                                {
                                    if(itemobj.coin >= cost.amount)
                                    {
                                        can_purchase = true;
                                        target_need_coin = cost.amount;
                                    }
                                }
                            }
                            if (can_purchase)
                            {
                                //优先扣除dot
                                var NewObjectToModify = itemobj;
                                if (NewObjectToModify.dot >= target_need_dot)
                                {
                                    NewObjectToModify.dot -= target_need_dot;
                                }
                                else
                                {
                                    NewObjectToModify.coin -= target_need_coin;
                                }
                                bool Player_Buyed_This_Before = false;
                                foreach (var ownProduct in NewObjectToModify.getOwnProducts)
                                {
                                    if(ownProduct.goodId == req.goodId)
                                    {
                                        Player_Buyed_This_Before = true;
                                        ownProduct.purchaseCount ++;
                                    }
                                }
                                if (!Player_Buyed_This_Before)
                                {
                                    NewObjectToModify.getOwnProducts.Add(new RizAccountOwnProduct
                                    {
                                        goodId = req.goodId,
                                        purchaseCount = 1
                                    });
                                }
                                
                                foreach (var asset in target_product.assets)
                                {
                                    if(asset.type == "track")
                                    {
                                        if (!NewObjectToModify.appearLevels.Contains(asset.assetId))
                                        {
                                            NewObjectToModify.appearLevels.Add(asset.assetId);
                                        }
                                        NewObjectToModify.unlockedLevels.Add(asset.assetId);
                                        new_tracks.Add(new RizNewLevelInfo
                                        {
                                            trackAssetId = asset.assetId,
                                            level = "idk"
                                        });
                                    }
                                    else if(asset.type == "item")
                                    {
                                        NewObjectToModify.getItems.Add(new RizAccountItems
                                        {
                                            amount = asset.amount,
                                            itemAssetId = asset.assetId
                                        });
                                        new_items.Add(new RizAccountItems
                                        {
                                            amount = asset.amount,
                                            itemAssetId = asset.assetId
                                        });
                                    }
                                }
                                Osp_DB.SearchFilter ModifySearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_RizUserAccountObject", item.label, null);
                                Classes.DBMain.ModifyObject(GlobalConfig.DBConfig.JsonName, ModifySearchFilter, NewObjectToModify);
                                return Tools.ReRizTools.BuildEncryptMessage(JsonConvert.SerializeObject(new RizPurchaseResultInfo
                                {
                                    newCoins = NewObjectToModify.coin,
                                    newDots = NewObjectToModify.dot,
                                    newItems = new_items,
                                    newLevels = new_tracks
                                }));
                            }
                            else
                            {
                                return Tools.ReRizTools.BuildNoEncryptMessage("error_cant_purchase_because_dont_have_enough_costs");
                            }
                        }
                    }

                    return Tools.ReRizTools.BuildNoEncryptMessage("error_target_product_not_found");
                }
            }

            return Tools.ReRizTools.BuildEncryptMessage("token_error");
        }
    }
}
