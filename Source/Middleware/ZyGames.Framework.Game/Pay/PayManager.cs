﻿using System;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Game.Pay
{
    /// <summary>
    /// 
    /// </summary>
    public class PayManager
    {
        private static PayOperator _operator;
        static PayManager()
        {
            _operator = new PayOperator();
        }

        /// <summary>
        /// 
        /// </summary>
        public static PayOperator Operator { get { return _operator; } }

        /// <summary>
        /// 获取个人充值未下发全部信息
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Server"></param>
        /// <param name="Account"></param>
        /// <returns></returns>
        public static OrderInfo[] getPayment(int game, int Server, string Account)
        {
            OrderFormBLL ordef = new OrderFormBLL();
            return ordef.GetList(game, Server, Account);
        }

        /// <summary>
        /// 91充值
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Server"></param>
        /// <param name="Account"></param>
        /// <param name="ServiceName"></param>
        /// <param name="orderNo"></param>
        public static void get91PayInfo(int game, int Server, string Account, string ServiceName, string orderNo, string RetailID)
        {
            //增加游戏名称避免出现游戏名称为空的现象 panx 2012-11-26
            string GameName = string.Empty;
            ServerInfo serverinfo = GetServerData(game, Server);
            if (serverinfo != null)
            {
                GameName = serverinfo.GameName;
            }

            OrderInfo orderInfo = new OrderInfo();
            orderInfo.OrderNO = orderNo;
            orderInfo.MerchandiseName = string.Empty;
            orderInfo.Currency = "CNY";
            orderInfo.Amount = 0;
            orderInfo.PassportID = Account;
            orderInfo.RetailID = RetailID;
            orderInfo.PayStatus = 1;
            orderInfo.GameID = game;
            orderInfo.ServerID = Server;
            orderInfo.GameName = GameName;
            orderInfo.ServerName = ServiceName;
            orderInfo.GameCoins = 0;
            orderInfo.SendState = 1;
            orderInfo.PayType = orderInfo.RetailID;
            orderInfo.Signature = "123456";
            OrderFormBLL obll = new OrderFormBLL();
            obll.Add91Pay(orderInfo, false);
        }



        /// <summary>
        /// appstroe充值
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Server"></param>
        /// <param name="Account"></param>
        /// <param name="Silver"></param>
        /// <param name="orderNo"></param>
        public static void AppStorePay(int game, int Server, string Account, int Silver, int Amount, string orderNo, string RetailID, string MemberMac)
        {
            try
            {
                string GameName = string.Empty;
                string ServerName = string.Empty;
                ServerInfo serverinfo = GetServerData(game, Server);
                if (serverinfo != null)
                {
                    GameName = serverinfo.GameName;
                    ServerName = serverinfo.Name;
                }

                OrderInfo orderInfo = new OrderInfo();
                orderInfo.OrderNO = orderNo;
                orderInfo.MerchandiseName = GameName;
                orderInfo.Currency = "CNY";
                orderInfo.Amount = Amount;
                orderInfo.PassportID = Account;
                orderInfo.RetailID = RetailID;
                orderInfo.PayStatus = 2;
                orderInfo.GameID = game;
                orderInfo.ServerID = Server;
                orderInfo.GameName = GameName;
                orderInfo.ServerName = ServerName;
                orderInfo.GameCoins = Silver;
                orderInfo.SendState = 1;
                orderInfo.PayType = "0004";
                orderInfo.Signature = "123456";
                orderInfo.DeviceID = MemberMac;
                OrderFormBLL obll = new OrderFormBLL();
                obll.Add(orderInfo);
                TraceLog.ReleaseWrite("AppStore充值完成");
            }
            catch (Exception ex)
            {
                TraceLog.ReleaseWriteFatal(ex.ToString());
            }
        }

        private static ServerInfo GetServerData(int gameID, int serverID)
        {
            OrderFormBLL ordrBLL = new OrderFormBLL();
            return ordrBLL.GetServerData(gameID, serverID);
        }


        public static void Abnormal(string OrderNO)
        {
            OrderFormBLL ordrBLL = new OrderFormBLL();
            ordrBLL.Updatestr(OrderNO);
        }

        /// <summary>
        /// 补订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="game"></param>
        /// <param name="server"></param>
        /// <param name="passport"></param>
        /// <returns></returns>
        public static bool ModifyOrder(string orderNo, int game, int server, string passport)
        {
            OrderFormBLL obll = new OrderFormBLL();
            return obll.UpdateBy91(new OrderInfo() { OrderNO = orderNo, GameID = game, ServerID = server, PassportID = passport }, false);
        }

        /// <summary>
        /// 更新订单支付成功状态
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public static bool PaySuccess(string orderNo, OrderInfo orderInfo)
        {
            OrderFormBLL obll = new OrderFormBLL();
            return obll.PaySuccess(orderNo, orderInfo);
        }

        /// <summary>
        /// 触控android订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="amount"></param>
        /// <param name="passportid"></param>
        /// <param name="serverID"></param>
        /// <param name="gameID"></param>
        /// <param name="gameConis"></param>
        /// <param name="deviceid"></param>
        /// <param name="RetailID"></param>
        public static void AddOrderInfo(string orderNo, decimal amount, string passportid, int serverID, int gameID, int gameConis, string deviceid, string RetailID)
        {
            try
            {
                string GameName = string.Empty;
                string ServerName = string.Empty;
                ServerInfo serverinfo = GetServerData(gameID, serverID);
                if (serverinfo != null)
                {
                    GameName = serverinfo.GameName;
                    ServerName = serverinfo.Name;
                }

                OrderInfo orderInfo = new OrderInfo();
                orderInfo.OrderNO = orderNo;
                orderInfo.MerchandiseName = GameName;
                orderInfo.Currency = "CNY";
                orderInfo.Amount = amount;
                orderInfo.PassportID = passportid;
                orderInfo.RetailID = RetailID;
                orderInfo.PayStatus = 1;
                orderInfo.GameID = gameID;
                orderInfo.ServerID = serverID;
                orderInfo.GameName = GameName;
                orderInfo.ServerName = ServerName;
                orderInfo.GameCoins = gameConis;
                orderInfo.SendState = 1;
                orderInfo.PayType = "6002";
                orderInfo.Signature = "123456";
                orderInfo.DeviceID = deviceid;
                OrderFormBLL obll = new OrderFormBLL();
                obll.Add91Pay(orderInfo, false);
                TraceLog.ReleaseWrite("触控android充值完成");
            }
            catch (Exception ex)
            {
                TraceLog.ReleaseWriteFatal(ex.ToString());
            }
        }

        public static bool AddOrder(OrderInfo orderInfo)
        {
            try
            {
                OrderFormBLL obll = new OrderFormBLL();
                obll.Add91Pay(orderInfo, false);
                return true;
            }
            catch (Exception ex)
            {
                TraceLog.ReleaseWriteFatal(ex.ToString());
                return false;
            }
        }
    }
}
