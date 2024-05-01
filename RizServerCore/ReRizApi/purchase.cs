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

        public static ReRizReturnEncryptResponseWithSign PurchaseMain(string header_token, string requestbody, string verify)
        {
            RizPurchaseRequest req = JsonConvert.DeserializeObject<RizPurchaseRequest>(requestbody);

            string token_email = Classes.TokenGenerator.CheckToken(header_token);
            if (token_email == null)
            {
                return Tools.ReRizTools.BuildEncryptMessage("token_error", verify);
            }

            Osp_DB.SearchFilter SearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_RizUserAccountObject", null, null);
            var SearchResult = Classes.DBMain.SearchWithFilter(GlobalConfig.DBConfig.JsonName, SearchFilter);
            foreach (var item in SearchResult)
            {
                var itemobj = JsonConvert.DeserializeObject<Classes.RizAccount>(item.obj.ToString());
                if (itemobj._id.Split(">")[2] == token_email)
                {
                    var can_purchase = false;
                    RizAccountProduct target_product;
                    int target_need_dot = 0;
                    int target_need_coin = 0;

                    foreach (var events in Classes.RizAccountBase.getProducts)
                    {
                        foreach (var product in events.goods)
                        {
                            if (product.id == req.goodId)
                            {
                                target_product = product;
                                foreach (var cost in product.costs)
                                {
                                    if (cost.type == "dot")
                                    {
                                        if (itemobj.dot >= cost.amount)
                                        {
                                            can_purchase = true;
                                            target_need_dot = cost.amount;
                                        }
                                    }
                                    else if (cost.type == "coin")
                                    {
                                        if (itemobj.coin >= cost.amount)
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
                                        if (ownProduct.goodId == req.goodId)
                                        {
                                            Player_Buyed_This_Before = true;
                                            ownProduct.purchaseCount++;
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

                                    if (target_product.content.Contains("track"))
                                    {
                                        if (!NewObjectToModify.appearLevels.Contains(target_product.content))
                                        {
                                            NewObjectToModify.appearLevels.Add(target_product.content);
                                        }
                                        NewObjectToModify.unlockedLevels.Add(target_product.content);
                                        new_tracks.Add(new RizNewLevelInfo
                                        {
                                            trackAssetId = target_product.content,
                                            level = "idk"
                                        });
                                    }
                                    else
                                    {
                                        NewObjectToModify.getItems.Add(new RizAccountItems
                                        {
                                            amount = 1,
                                            itemAssetId = target_product.content
                                        });
                                        new_items.Add(new RizAccountItems
                                        {
                                            amount = 1,
                                            itemAssetId = target_product.content
                                        });
                                    }

                                    Osp_DB.SearchFilter ModifySearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_RizUserAccountObject", item.label, null);
                                    Classes.DBMain.ModifyObject(GlobalConfig.DBConfig.JsonName, ModifySearchFilter, NewObjectToModify);
                                    return Tools.ReRizTools.BuildEncryptMessage(JsonConvert.SerializeObject(new RizPurchaseResultInfo
                                    {
                                        newCoins = NewObjectToModify.coin,
                                        newDots = NewObjectToModify.dot,
                                        newItems = new_items,
                                        newLevels = new_tracks
                                    }), verify);
                                }
                                else
                                {
                                    return Tools.ReRizTools.BuildNoEncryptMessage("error_cant_purchase_because_dont_have_enough_costs", verify);
                                }
                            }
                        }
                    }
                    return Tools.ReRizTools.BuildNoEncryptMessage("error_target_product_not_found", verify);
                }
            }

            return Tools.ReRizTools.BuildEncryptMessage("token_error", verify);
        }
    }
}
