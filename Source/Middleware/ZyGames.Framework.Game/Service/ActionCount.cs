﻿using System;
using System.Collections.Generic;

namespace ZyGames.Framework.Game.Service
{
    /// <summary>
    /// 接口访问次数统计类
    /// </summary>
    public static class ActionCount
    {
        private static readonly object LockObj = new object();
        /// <summary>
        /// 当前日期
        /// </summary>
        private static DateTime curDate;
        /// <summary>
        /// 当日目前各接口访问情况统计
        /// </summary>
        private static Dictionary<int, ActionLog> dicActionInfo;

        /// <summary>
        /// 接口访问次数加1
        /// </summary>
        /// <param name="actionId">接口编号</param>
        /// <param name="aStat">访问状态</param>
        public static void ActionVisit(int actionId, GameStruct.LogActionStat aStat)
        {
            if (dicActionInfo == null)
            {
                lock (LockObj)
                {
                    if (dicActionInfo == null)
                    {
                        curDate = DateTime.Now.Date;
                        dicActionInfo = new Dictionary<int, ActionLog>();
                    }
                }
            }

            if (!dicActionInfo.ContainsKey(actionId))
            {
                lock (LockObj)
                {
                    if (!dicActionInfo.ContainsKey(actionId))
                    {
                        ActionLog tmpLog = new ActionLog(actionId, curDate);
                        dicActionInfo.Add(actionId, tmpLog);
                    }
                }
            }

            dicActionInfo[actionId].Visitor(aStat);

            if (curDate != DateTime.Now.Date)
            {
                //已经进入第二天，全部写入DB，并初始化数据
                lock (dicActionInfo)
                {
                    DateTime newDate = DateTime.Now.Date;
                    foreach (KeyValuePair<int, ActionLog> item in dicActionInfo)
                    {
                        item.Value.InsertDB(newDate);
                    }
                    curDate = newDate;
                }
            }
        }
    }

    /// <summary>
    /// 接口访问情况记录类
    /// </summary>
    public class ActionLog
    {
        private DateTime lastDbTime;
        /// <summary>
        /// 成功次数
        /// </summary>
        public int SucCount { get; protected set; }
        /// <summary>
        /// 失败次数
        /// </summary>
        public int FailCount { get; protected set; }
        /// <summary>
        /// 当前累计的访问次数
        /// </summary>
        public int TotalCount { get { return SucCount + FailCount; } }
        private int actionId;
        private DateTime curDate;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="_actionid">接口编号</param>
        /// <param name="_curDate">当前日期</param>
        public ActionLog(int _actionid, DateTime _curDate)
        {
            actionId = _actionid;
            curDate = _curDate;
            SucCount = 0;
            FailCount = 0;
            lastDbTime = DateTime.Now;
        }

        /// <summary>
        /// 累加访问次数
        /// </summary>
        /// <param name="aStat">接口访问是成功还是失败</param>
        public void Visitor(GameStruct.LogActionStat aStat)
        {
            if (aStat == GameStruct.LogActionStat.Sucess)
            {
                this.SucCount++;
            }
            else
            {
                this.FailCount++;
            }
            if (TotalCount >= 100)
            {
                this.InsertDB(DateTime.Now.Date);
            }
            else
            {
                if (lastDbTime.AddMinutes(5).CompareTo(DateTime.Now) <= 0)
                {
                    this.InsertDB(DateTime.Now.Date);
                }
            }
        }

        /// <summary>
        /// 更新数据到DB，并数据清零
        /// </summary>
        /// <param name="_curNewDate"></param>
        public void InsertDB(DateTime _curNewDate)
        {
            /*if (TotalCount > 0)
            {
               string sInsertSql = " insert into ActionLog(actionId, totalNum, SucNum, FailNum, DateValue, CountTime)values(@aActionid, @aTotalNum, @aSucNum, @aFailNum, @aDateValue, @aCurTime)";

                SqlParameter[] paramsAction = new SqlParameter[6];
                paramsAction[0] = SqlParamHelper.MakeInParam("@aActionid", SqlDbType.Int, 0, actionId);
                paramsAction[1] = SqlParamHelper.MakeInParam("@aTotalNum", SqlDbType.Int, 0, TotalCount);
                paramsAction[2] = SqlParamHelper.MakeInParam("@aSucNum", SqlDbType.Int, 0, this.SucCount);
                paramsAction[3] = SqlParamHelper.MakeInParam("@aFailNum", SqlDbType.Int, 0, this.FailCount);
                paramsAction[4] = SqlParamHelper.MakeInParam("@aDateValue", SqlDbType.DateTime, 0, this.curDate);
                paramsAction[5] = SqlParamHelper.MakeInParam("@aCurTime", SqlDbType.DateTime, 0, DateTime.Now);

                ActionMsmq.ActionMSMQ.Instance().SendSqlCmd(CommandType.Text, sInsertSql, paramsAction);

             
                SucCount = 0;
                FailCount = 0;
            }*/
            this.lastDbTime = DateTime.Now;
            curDate = _curNewDate;
        }
    }
}
