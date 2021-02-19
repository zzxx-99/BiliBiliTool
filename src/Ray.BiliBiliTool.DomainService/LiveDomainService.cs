﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ray.BiliBiliTool.Agent.BiliBiliAgent.Interfaces;
using Ray.BiliBiliTool.Config.Options;
using Ray.BiliBiliTool.DomainService.Interfaces;

namespace Ray.BiliBiliTool.DomainService
{
    /// <summary>
    /// 直播
    /// </summary>
    public class LiveDomainService : ILiveDomainService
    {
        private readonly ILogger<LiveDomainService> _logger;
        private readonly ILiveApi _liveApi;
        private readonly DailyTaskOptions _dailyTaskOptions;

        public LiveDomainService(ILogger<LiveDomainService> logger,
            ILiveApi liveApi,
            IOptionsMonitor<DailyTaskOptions> dailyTaskOptions)
        {
            _logger = logger;
            _liveApi = liveApi;
            _dailyTaskOptions = dailyTaskOptions.CurrentValue;
        }

        /// <summary>
        /// 直播签到
        /// </summary>
        public void LiveSign()
        {
            var response = _liveApi.Sign()
                .GetAwaiter().GetResult();

            if (response.Code == 0)
            {
                _logger.LogInformation("直播签到成功，本次签到获得{text},{special}", response.Data.Text, response.Data.SpecialText);
            }
            else
            {
                _logger.LogInformation(response.Message);
            }
        }

        /// <summary>
        /// 直播中心银瓜子兑换B币
        /// </summary>
        /// <returns>兑换银瓜子后硬币余额</returns>
        public bool ExchangeSilver2Coin()
        {
            var result = false;

            if (_dailyTaskOptions.DayOfExchangeSilver2Coin == 0)
            {
                _logger.LogInformation("已配置为不进行兑换，跳过兑换任务");
                return false;
            }

            int targetDay = _dailyTaskOptions.DayOfExchangeSilver2Coin == -2
                ? DateTime.Today.Day
                : _dailyTaskOptions.DayOfExchangeSilver2Coin == -1
                    ? DateTime.Today.LastDayOfMonth().Day
                    : _dailyTaskOptions.DayOfExchangeSilver2Coin;

            if (DateTime.Today.Day != targetDay)
            {
                _logger.LogInformation("目标兑换日期为{targetDay}号，今天是{day}号，跳过兑换任务", targetDay, DateTime.Today.Day);
                return false;
            }

            var response = _liveApi.ExchangeSilver2Coin().GetAwaiter().GetResult();
            if (response.Code == 0)
            {
                result = true;
                _logger.LogInformation("银瓜子兑换硬币成功");
            }
            else
            {
                _logger.LogInformation("银瓜子兑换硬币失败，原因：{0}", response.Message);
            }

            var queryStatus = _liveApi.GetExchangeSilverStatus().GetAwaiter().GetResult();
            _logger.LogInformation("当前银瓜子余额: {0}", queryStatus.Data.Silver);

            return result;
        }
    }
}
